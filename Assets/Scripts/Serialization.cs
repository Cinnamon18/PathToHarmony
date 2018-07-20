using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Serialization {
    public static void WriteData(string data, string fileName, bool overwriteFile) {
        fileName = "..\\Levels\\" + fileName;
        if (File.Exists(fileName) && !overwriteFile) {
            Debug.Log(fileName + " already exists.");
            return;
        }
		
        StreamWriter sr = File.CreateText(fileName);
        sr.WriteLine(data);
        sr.Close();
    }
}
