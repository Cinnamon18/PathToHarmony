using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
}
