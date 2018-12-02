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
		public Mage() : base(ArmorType.Medium, 100, MoveType.LightInfantry, 4, new DamageBuff(0.7f), 3, Faction.Xingata) {

		}
	}
}