using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
	public class Level {
		//TODO
		public string mapFileName;
		public Character[] characters;
		public Dictionary<Character, int> playersPoints;
		//First index is "which player?", then each sub-array is the list of tiles that player can place units on in the pick phase
		public Dictionary<Character, List<Coord>> validPickTiles;

		public Level(string mapFileName, Character[] players, Dictionary<Character, int> playersPoints, Dictionary<Character, List<Coord>> validPickTiles) {
			this.mapFileName = mapFileName;
			this.characters = players;
			this.playersPoints = playersPoints;
			this.validPickTiles = validPickTiles;
		}
	}
}