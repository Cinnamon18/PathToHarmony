using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Editors
{
	public class UnitInfo
	{
		private UnitType unitType;
		private bool isPlayerUnit;
		private Vector3Int unitCoord;

		public UnitInfo(UnitType type, bool player, Vector3Int coord)
		{
			unitType = type;
			isPlayerUnit = player;
			unitCoord = coord;
		}

		public string serialize()
		{
			return "" + ((int)(this.unitType));
		}
	}
}
