using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Paths{

	public static string mapsPath() {
		return streamingPath() + "Maps/";
	}

	public static string levelsPath() {
		return  streamingPath() + "Levels/";
	}

	public static string streamingPath() {
		return Application.streamingAssetsPath + "/";
	}
}
