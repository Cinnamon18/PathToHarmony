using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using Constants;

public static class Serialization {
	public static void WriteData(string data, string fileName, bool overwriteFile) {
		string filePath = "./Assets/Levels/" + fileName + ".txt";
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

	public static string ReadData(string fileName) {
		string filePath = "Assets/Levels/" + fileName + ".txt";

		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(filePath);
		string data = reader.ReadToEnd();
		reader.Close();
		Util.Log("read file at " + filePath + "successfully!");
		return data;
	}


	//Basically, give it the data as encoded by LevelEditor.serializeTerrain
	public static Terrain[,,] DeserializeTerrain(string terrainRaw, GameObject[] tilePrefabs) {
		//Parse the saved data. If there's nothing there, indicate that by -1
		int[] data = terrainRaw.Split(',').Select((datum) => {
			int num = -1;
			if (!Int32.TryParse(datum, out num)) {
				num = -1;
			}
			return num;
		}).ToArray();
		Terrain[,,] parsedTerrains = new Terrain[data[0], data[1], data[2]];
		data = data.Skip(3).ToArray();

		//Reconstruct the map
		for (int x = 0; x < parsedTerrains.GetLength(0); x++) {
			for (int y = 0; y < parsedTerrains.GetLength(1); y++) {
				for (int z = 0; z < parsedTerrains.GetLength(2); z++) {
					int flatIndex = x + parsedTerrains.GetLength(1) * (y + parsedTerrains.GetLength(0) * z);
					if (data[flatIndex] != -1) {
						Terrain terrain = GameObject.Instantiate(tilePrefabs[data[flatIndex]],
							Util.GridToWorld(new Vector3Int(x, y, z)),
							tilePrefabs[data[flatIndex]].transform.rotation)
							.AddComponent<Terrain>();
						terrain.terrain = (TerrainType)(data[flatIndex]);
						parsedTerrains[x, y, z] = terrain;
					}
				}
			}
		}

		return parsedTerrains;
	}
}
