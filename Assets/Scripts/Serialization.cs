using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using Constants;
using Gameplay;

public static class Serialization {
	public static void WriteMapData(string data, string fileName, bool overwriteFile) {
		string filePath = "./Assets/Maps/" + fileName + ".txt";
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

	public static string ReadMapData(string fileName) {
		string filePath = "./Assets/Maps/" + fileName + ".txt";

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

	//TODO: Decide on semantics for this
	public static Campaign deserializeCampaign(string fileName) {
		Campaign campaign = new Campaign();
		string serializedCampaign = Serialization.ReadMapData(fileName);
		string[] campaignArr = serializedCampaign.Split(',');
		campaign.name = campaignArr[0];

		//TODO: Finish deserialization routine

		return campaign;
	}
}
