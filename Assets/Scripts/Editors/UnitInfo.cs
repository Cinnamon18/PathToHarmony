using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Editors {
	public class UnitInfo {
		private UnitType unitType;
		private Faction unitFaction;
		private Vector3Int unitPos;

		public UnitInfo(UnitType type, Faction faction, Vector3Int pos) {
			unitType = type;
			unitFaction = faction;
			unitPos = pos;
		}

		public string serialize() {

			return ((int)(this.unitType)) + "," + (int)(this.unitFaction) + "," + unitPos.x + "," + unitPos.y + "," + unitPos.z;
	
		}

		public UnitType getUnitType() {
			return unitType;
		}

		public Faction getFaction()
		{
			return unitFaction;
		}

		public Vector3Int getCoord() {
			return unitPos;
		}
	}
}