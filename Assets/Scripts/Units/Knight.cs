using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Gameplay;
using UnityEngine;

namespace Units {
	public class Knight : Unit {
		public Knight() : base(ArmorType.Medium, WeaponType.KnightWeapon, MoveType.Medium, UnitType.Knight) {

		}
	}
}