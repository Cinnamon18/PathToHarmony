using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Gameplay;
using UnityEngine;

namespace Units {
	public class Cleric : HealerUnit {
		public Cleric() : base(ArmorType.Unarmored, 100, MoveType.ArmoredInfantry, 4, DamageType.Heal, 30, Faction.Xingata) {

		}
	}
}