using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
	public class Level {
		//TODO
		public string mapFileName;
		public Character[] characters;
		public string levelFileName;
		public string[] cutsceneIDs;

		public Level(
			string mapFileName,
			string levelFileName,
			Character[] players,
			string[] cutsceneIDs) {

			this.mapFileName = mapFileName;
			this.characters = players;
			this.levelFileName = levelFileName;
			this.cutsceneIDs = cutsceneIDs;

		}
	}
}