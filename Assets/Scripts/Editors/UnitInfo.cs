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
		private Vector3Int unitPos;

		public UnitInfo(UnitType type, bool player, Vector3Int pos)
		{
			unitType = type;
			isPlayerUnit = player;
			unitPos = pos;
		}

		public string serialize()
		{
			if (isPlayerUnit)
			{
				return ((int)(this.unitType)) + "," + 1 + "," + unitPos.x + "," + unitPos.y + "," + unitPos.z; ;
			}
			else
			{
				return ((int)(this.unitType)) + "," + 0 + "," + unitPos.x + "," + unitPos.y + "," + unitPos.z;
			}

		}

		public UnitType getUnitType()
		{
			return unitType;
		}

		public bool getIsPlayer()
		{
			return isPlayerUnit;
		}

		public Vector3Int getCoord()
		{
			return unitPos;
		}
	}
}