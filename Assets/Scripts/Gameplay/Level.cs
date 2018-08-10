using UnityEngine;

namespace Gameplay {
	public class Level {
		//TODO
		public string mapFileName;
		public Character[] players;
		public int[] playersPoints;
		//First index is "which player?", then each sub-array is the list of tiles that player can place units on in the pick phase
		public Vector2Int[][] validPickTiles;

		public Level(string mapFileName, Character[] players, int[] playersPoints, Vector2Int[][] validPickTiles) {
			this.mapFileName = mapFileName;
			this.players = players;
			this.playersPoints = playersPoints;
			this.validPickTiles = validPickTiles;
		}
	}
}