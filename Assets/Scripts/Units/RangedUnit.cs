using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using Gameplay;
using System;
using AI;
using System.Linq;
using UnityEngine.UI;
using Buffs;

namespace Units {
	public class RangedUnit : Unit {

		//attributes specific to Melee
		private readonly DamageType damageType;
		private readonly int rangedAttackStrength;

		public RangedUnit(ArmorType armorType, int maxHealth, MoveType moveType, int moveDistance, DamageType damageType, int rangedAttackStrength) : base(armorType, maxHealth, moveType, moveDistance) {
			this.damageType = damageType;
			this.rangedAttackStrength = rangedAttackStrength;
		}

		public override bool doBattleWith(Unit enemy, Tile enemyTile, Battlefield battlefield) {
			//TODO: create specific implementation for ranged units
			return false;

		}

		public override List<Coord> getAttackZone(int myX, int myY, Battlefield battlefield, Character character) {
			//TODO: create specific implementation for ranged units
			return null;
		}

	}
}
