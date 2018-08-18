using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Gameplay;
using UnityEngine;

namespace Units {
	public class Knight : Unit {
		public Knight() : base(ArmorType.Medium, WeaponType.KnightWeapon, MoveType.Medium, knightMoveDistance()) {

		}

		//I don't love this solution but it's the best I could come up with.
		override public int unitMoveDistance() {
			return Unit.unitMoveDistance(this.GetType());
		}

		public static int knightMoveDistance() {
			return Unit.unitMoveDistance(typeof(Knight));
		}
	}
}