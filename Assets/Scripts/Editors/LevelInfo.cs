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
		public ObjectiveType objective;
		public Vector2 goalPosition;

		public LevelInfo(Stack<UnitInfo> units, string name, ObjectiveType objective) {
			this.units = units;
			this.mapName = name;
			this.objective = objective;
		}

		public void setPosition(Vector2 pos)
		{
			goalPosition = pos;
		}
	}
}