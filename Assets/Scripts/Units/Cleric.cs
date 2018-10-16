using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Gameplay;
using UnityEngine;

namespace Units {
	public class Cleric : HealerUnit {
		public Cleric() : base(ArmorType.Light, 100, MoveType.Unarmored, 4, DamageType.Heal, 50, Faction.Xingata) {

		}
	}
}