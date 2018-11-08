using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

namespace Units {
	public class Archer : RangedUnit {
		public Archer() : base(ArmorType.Medium, 100, MoveType.LightInfantry, 4, DamageType.Pierce, 30, 3, Faction.Xingata) {

		}
	}
}
