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
using System.Threading.Tasks;

namespace Units {
	public class StatusUnit : Unit {

		//attributes specific to Melee
		private readonly Buff buffType;
		private readonly int range;


		public StatusUnit(
				ArmorType armorType,
				int maxHealth,
				MoveType moveType,
				int moveDistance,
				Buff buffType,
				int range,
				Faction faction
			) : base(armorType, maxHealth, moveType, moveDistance, faction) {

			this.buffType = buffType;
			this.range = range;
		}

		public override int battleDamage(Unit enemy, Tile enemyTile) {
			return 0; //Status units dont deal any direct damage
		}

		public override async Task<bool> doBattleWith(Unit enemy, Tile enemyTile, Battlefield battlefield) {
			await playAttackAnimation();

			// only add if the enemy does not already have this buff
			if (enemy.getBuffsOfClass(this.buffType).Count == 0) {
				enemy.addBuff(this.buffType);
			}
			return false;
		}

		public override List<Coord> getAttackZone(int myX, int myY, Battlefield battlefield, Character character) {
			List<Coord> validTargets = new List<Coord>();

			//	Create a diamond field around myX and myY.
			for (int y = -range; y <= range; y++) {             // For all valid y...
				int XAxis = range - Math.Abs(y);
				for (int x = -XAxis; x <= XAxis; x++) {         // and valid x...
					bool onTheMap = !(x + myX < 0 || y + myY < 0 || x + myX >= battlefield.map.GetLength(0) || y + myY >= battlefield.map.GetLength(1));
					if (onTheMap) {
						Coord inRange = new Coord(x + myX, y + myY);
						validTargets.Add(inRange);
					}
				}
			}
			return validTargets;
		}

		public override HashSet<Coord> getTotalAttackZone(int myX, int myY, Battlefield battlefield, Character character) {
			return new HashSet<Coord>(getAttackZone(myX, myY, battlefield, character));
		}

	}
}
