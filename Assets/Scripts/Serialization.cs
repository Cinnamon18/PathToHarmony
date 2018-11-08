using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using Constants;
using Gameplay;
using Units;
using Editors;
using Random = UnityEngine.Random;

public static class Serialization {

	//This is used for LevelEditor so the obj[,,,] array know how tall it should be
	//And can place units now matter how tall the map is.
	//Gotta be better way but works for now.
	public static int mapHeight;

	private const float flavorChance = 0.2f;
	private const float flavorMaxPos = 4.0f;

	public static void WriteData(string data, string fileName, string path, bool overwriteFile) {
		string filePath = path + fileName + ".txt";
		if (File.Exists(filePath)) {
			if (!overwriteFile) {
				Debug.LogError(filePath + " already exists. Aborting");
				return;
			} else {
				Debug.LogWarning("Overwriting saved file: " + filePath);
			}
		}

		StreamWriter sr = File.CreateText(filePath);
		sr.WriteLine(data);
		sr.Close();
		Util.Log("Saved file " + filePath + " successfully!");
	}

	public static string ReadData(string fileName, string path) {
		try {
			string filePath = path + fileName + ".txt";

			//Read the text from directly from the test.txt file
			StreamReader reader = new StreamReader(filePath);
			string data = reader.ReadToEnd();
			reader.Close();
			Util.Log("read file at " + filePath + "successfully!");
			return data;
		} catch (FileNotFoundException ex) {
			Debug.LogError("File name " + path + fileName + ".txt" + " entered does not exist.");
			return null;
		}

	}


	//Basically, give it the data as encoded by MapEditor.serializeTile
	public static Tile[,,] DeserializeTiles(string tileRaw, TileGenerator generator, Transform tileHolder) {
		//Parse the saved data. If there's nothing there, indicate that by -1
		int[] data = tileRaw.Split(',').Select((datum) => {
			int num = -1;
			if (!Int32.TryParse(datum, out num)) {
				num = -1;
			}
			return num;
		}).ToArray();
		Tile[,,] parsedTiles = new Tile[data[0], data[1], data[2]];
		data = data.Skip(3).ToArray();

		//Reconstruct the map
		for (int x = 0; x < parsedTiles.GetLength(0); x++) {
			for (int y = 0; y < parsedTiles.GetLength(1); y++) {
				for (int z = 0; z < parsedTiles.GetLength(2); z++) {
					int flatIndex = x + parsedTiles.GetLength(1) * (y + parsedTiles.GetLength(0) * z);
					//If its an actual gameplay tile, deserialize it
					if (data[flatIndex] != -1 && data[flatIndex] != 0) {

						int tileTypeInt = data[flatIndex];

						TileType type = (TileType)tileTypeInt;
						GameObject newTile = generator.getTileByType((TileType)tileTypeInt);

						if (newTile != null) {
							//typePrefabs was old way of doing this
							Tile tile = GameObject.Instantiate(newTile,
								Util.GridToWorld(x, y, z),
								Quaternion.Euler(-90, 0, 0))
								.GetComponent<Tile>();

							//make child of singe transform for easier deletion
							tile.transform.parent = tileHolder;
							tile.tileType = (TileType)(tileTypeInt);

							parsedTiles[x, y, z] = tile;

							//Make it taste better
							addFlavor(new Vector3Int(x, y, z), tile);
						} else {
							Debug.Log("Null tile");
						}

					}
				}
			}
		}
		return parsedTiles;
	}

	//I know this is a hacky way of doing this, but it'll work.... for now....  #TODO
	public static Stack<Tile>[,] DeserializeTilesStack(string tileRaw, TileGenerator generator, Transform tilesHolder) {
		Tile[,,] parsedTiles = DeserializeTiles(tileRaw, generator, tilesHolder);
		mapHeight = parsedTiles.GetLength(2);
		Stack<Tile>[,] stackedTiles = new Stack<Tile>[parsedTiles.GetLength(0), parsedTiles.GetLength(1)];
		for (int x = 0; x < parsedTiles.GetLength(0); x++) {
			for (int y = 0; y < parsedTiles.GetLength(1); y++) {
				stackedTiles[x, y] = new Stack<Tile>();
				for (int z = 0; z < parsedTiles.GetLength(2); z++) {
					if (parsedTiles[x, y, z] != null) {
						stackedTiles[x, y].Push(parsedTiles[x, y, z]);
					}
				}
			}
		}
		return stackedTiles;
	}

	public static void addFlavor(Vector3Int tileCoords, Tile tile) {

		if (Random.Range(0.0f, 1.0f) < flavorChance) {
			Vector3 flavorPos = Util.GridToWorld(tileCoords) + new Vector3(
				Random.Range(-flavorMaxPos, flavorMaxPos),
				1,//Honestly I'm not sure why it needs to be elevated by 1, but it does
				Random.Range(-flavorMaxPos, flavorMaxPos));
			GameObject[] flavorOptions = tile.getFlavorPrefabs();

			if (flavorOptions.Length > 0) {
				GameObject flavorObject = flavorOptions[Random.Range(0, flavorOptions.Length)];
				GameObject.Instantiate(flavorObject, flavorPos, Quaternion.Euler(-90, Random.Range(0, 360), 0));
			}
		}
	}

	public static LevelInfo getLevel(string levelName) {
		string levelString = ReadData(levelName, Paths.levelsPath());

		//separate level in from objective info
		string[] seperate = levelString.Split('*');
		ObjectiveType objective;
		List<Coord> goalPosList = new List<Coord>();
		if (seperate.Length == 2) {
			levelString = seperate[0];

			String objectiveString = seperate[1];

			//get objective and position of relevant tile/unit
			string[] goalInfo = objectiveString.Split(';');
			int objInt = Convert.ToInt32(goalInfo[0]);
			objective = (ObjectiveType)objInt;

			if (goalInfo.Length >= 2) {
				//get all Vector2 positions for goals
				for (int i = 1; i < goalInfo.Length; i++) {
					string posStr = goalInfo[i];
					string[] goalPosition = posStr.Split(',');
					int x = Convert.ToInt32(goalPosition[0]);
					int y = Convert.ToInt32(goalPosition[1]);
					Coord position = new Coord(x, y);
					goalPosList.Add(position);
				}
			}

		} else {
			//default for old way of serializing level
			objective = ObjectiveType.Elimination;

		}


		if (levelString != null) {

			Queue<string> levelData = new Queue<string>();
			foreach (string str in levelString.Split(';')) {
				if (!(str.Equals("") | str.Equals(null))) {
					levelData.Enqueue(str);
				}
			}
			Stack<UnitInfo> units = new Stack<UnitInfo>();
			String mapname = DeserializeUnits(levelData, units);

			//Package level data
			LevelInfo levelInfo = new LevelInfo(units, mapname, objective);
			levelInfo.setGoalPositions(goalPosList);

			return levelInfo;
		}
		return null;

	}

	public static string DeserializeUnits(Queue<String> queue, Stack<UnitInfo> units) {
		string mapName = queue.Dequeue();
		while (queue.Count != 0) {
			string unitStr = queue.Dequeue();
			int[] data = unitStr.Split(',').Select((datum) => {
				int num = -1;
				if (!Int32.TryParse(datum, out num)) {
					num = -1;
				}
				return num;
			}).ToArray();
			if (data.Length == 5) {
				//get store Unittype
				UnitType type = (UnitType)data[0];
				//get stored Faction
				Faction faction = (Faction)data[1];
				units.Push(new UnitInfo(type, faction, new Vector3Int(data[2], data[3], data[4])));
			}

		}
		return mapName;

	}

	//TODO: Decide on semantics for this
	public static Campaign deserializeCampaign(string fileName) {
		Campaign campaign = new Campaign();
		string serializedCampaign = Serialization.ReadData(fileName, Paths.mapsPath());
		string[] campaignArr = serializedCampaign.Split(',');
		campaign.name = campaignArr[0];

		//TODO: Finish deserialization routine

		return campaign;
	}



}