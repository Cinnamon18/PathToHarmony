using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Gameplay;
using UnityEngine;
using Buffs;

namespace Units {
	public class Mage : StatusUnit {
		//TODO create a new debuff for this unit instead of using DamageBuff
		public Mage() : base(ArmorType.Medium, 100, MoveType.Medium, 4, new DamageBuff(0.1f), 3, Faction.Xingata) {

		}
	}
}