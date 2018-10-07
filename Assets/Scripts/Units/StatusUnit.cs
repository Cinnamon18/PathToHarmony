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
	public class StatusUnit : Unit {

		//attributes specific to Melee
		private readonly DamageType damageType;
		private readonly int statusAttackStrength;

		public StatusUnit(
				ArmorType armorType,
				int maxHealth,
				MoveType moveType,
				int moveDistance,
				DamageType damageType,
				int statusAttackStrength,
				Faction faction
			) : base(armorType, maxHealth, moveType, moveDistance, faction) {
			
			this.damageType = damageType;
			this.statusAttackStrength = statusAttackStrength;
		}

		public override int battleDamage(Unit enemy, Tile enemyTIle) {
			//TODO: create specific implementation for status units
			return 0;
		}

		public override bool doBattleWith(Unit enemy, Tile enemyTile, Battlefield battlefield) {
			//TODO: create specific implementation for status units
			return false;
		}

		public override List<Coord> getAttackZone(int myX, int myY, Battlefield battlefield, Character character) {
			//TODO: create specific implementation for status units
			return null;
		}

		public override HashSet<Coord> getTotalAttackZone(int myX, int myY, Battlefield battlefield, Character character) {
			return new HashSet<Coord>(getAttackZone(myX, myY, battlefield, character));
		}

	}
}
