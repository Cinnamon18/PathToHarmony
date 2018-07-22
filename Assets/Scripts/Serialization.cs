using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Serialization {
	public static void WriteData(string data, string fileName, bool overwriteFile) {
		fileName = "./Assets/Levels/" + fileName + ".txt";
		if (File.Exists(fileName)) {
			if (!overwriteFile) {
			Debug.LogError(fileName + " already exists. Aborting");
			return;
			} else {
				Debug.LogWarning("Overwriting saved file: " +  fileName);
			}
		}

		StreamWriter sr = File.CreateText(fileName);
		sr.WriteLine(data);
		sr.Close();
		Util.Log("Saved file " + fileName + " successfully!");
	}
}
