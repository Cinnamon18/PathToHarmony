using Editors;
using Gameplay;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Editors {
	public class LevelInfo {
		public Stack<UnitInfo> units;
		public string mapName;
		//public Stack<Tile>[,] tiles;

		public LevelInfo(Stack<UnitInfo> units, string name) {
			this.units = units;
			this.mapName = name;
		}
	}
}