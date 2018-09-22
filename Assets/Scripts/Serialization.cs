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

public static class Serialization {
	public static string mapFilePath = "./Assets/Maps/";
	public static string levelFilePath = "./Assets/Levels/";
	//This is used for LevelEditor so the obj[,,,] array know how tall it should be
	//And can place units now matter how tall the map is.
	//Gotta be better way but works for now.
	public static int mapHeight;

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
		string filePath = path + fileName + ".txt";

		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(filePath);
		string data = reader.ReadToEnd();
		reader.Close();
		Util.Log("read file at " + filePath + "successfully!");
		return data;
	}


	//Basically, give it the data as encoded by MapEditor.serializeTile
	public static Tile[,,] DeserializeTiles(string tileRaw, GameObject[] tilePrefabs) {
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
					//If its an actual gameplay tile
					if (data[flatIndex] != -1 && data[flatIndex] != 0) {
						Tile tileObject = GameObject.Instantiate(tilePrefabs[data[flatIndex]],
							Util.GridToWorld(x, y, z),
							tilePrefabs[data[flatIndex]].transform.rotation)
							.GetComponent<Tile>();
						tileObject.tileType = (TileType)(data[flatIndex]);
						parsedTiles[x, y, z] = tileObject;
					}
				}
			}
		}

		return parsedTiles;
	}

	//I know this is a hacky way of doing this, but it'll work.... for now....  #TODO
	public static Stack<Tile>[,] DeserializeTilesStack(string tileRaw, GameObject[] tilePrefabs) {
		Tile[,,] parsedTiles = DeserializeTiles(tileRaw, tilePrefabs);
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

	public static LevelInfo getLevel(string levelName)
	{
		string levelString = ReadData(levelName, levelFilePath);
		Queue<string> levelData = new Queue<string>();
		foreach (string str in levelString.Split(';'))
		{
			if (!(str.Equals("") | str.Equals(null)))
			{
				levelData.Enqueue(str);
			}
		}
		Stack<UnitInfo> units = new Stack<UnitInfo>();
		String mapname = DeserializeUnits(levelData, units);
		return new LevelInfo(units, mapname);

	}

	public static string DeserializeUnits(Queue<String> queue, Stack<UnitInfo> units)
	{
		string mapName = queue.Dequeue();
		while (queue.Count != 0)
		{
			string unitStr = queue.Dequeue();
			int[] data = unitStr.Split(',').Select((datum) => {
				int num = -1;
				if (!Int32.TryParse(datum, out num))
				{
					num = -1;
				}
				return num;
			}).ToArray();
			if(data.Length == 5)
			{
				//get store Unittype
				UnitType type = (UnitType)data[0];
				//see if player unit
				bool isPlayerUnit = false;

				if (data[1] == 1)
				{
					isPlayerUnit = true;
				}
				units.Push(new UnitInfo(type, isPlayerUnit, new Vector3Int(data[2], data[3], data[4])));
			}
			
			
		}
		return mapName;
		
	}

	//TODO: Decide on semantics for this
	public static Campaign deserializeCampaign(string fileName) {
		Campaign campaign = new Campaign();
		string serializedCampaign = Serialization.ReadData(fileName, mapFilePath);
		string[] campaignArr = serializedCampaign.Split(',');
		campaign.name = campaignArr[0];

		//TODO: Finish deserialization routine

		return campaign;
	}

}
