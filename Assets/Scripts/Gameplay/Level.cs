using System.Collections.Generic;
using Cutscenes.Stages;
using UnityEngine;

namespace Gameplay {
	public class Level {
		//TODO
		public string mapFileName;
		public Character[] characters;
		public string levelFileName;
		public Cutscene[] cutscenes;

		public Level(
			string mapFileName,
			string levelFileName,
			Character[] players,
			Cutscene[] cutscenes) {

			this.mapFileName = mapFileName;
			this.characters = players;
			this.levelFileName = levelFileName;
			this.cutscenes = cutscenes;

		}
	}
}