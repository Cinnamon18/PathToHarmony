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
	public class RangedUnit : Unit {

		//attributes specific to Melee
		private readonly DamageType damageType;
		private readonly int rangedAttackStrength;
		private readonly int range;

		public RangedUnit(
				ArmorType armorType,
				int maxHealth,
				MoveType moveType,
				int moveDistance,
				DamageType damageType,
				int rangedAttackStrength,
				int range,
				Faction faction
			) : base(armorType, maxHealth, moveType, moveDistance, faction) {

			this.damageType = damageType;
			this.rangedAttackStrength = rangedAttackStrength;
			this.range = range;
		}

		public override int battleDamage(Unit enemy, Tile enemyTile) {
			//TODO: create specific implementation for ranged units
			float damage = this.rangedAttackStrength * (1f * (this as Unit).getHealth() / this.maxHealth);
			damage = damage * ((100 - this.damageType.DamageReduction(enemy.armor)) / 100.0f);
			damage = damage * ((100 - enemyTile.tileType.DefenseBonus()) / 100.0f);

			List<Buff> damageBuffs = getBuffsOfType(BuffType.Damage);
			foreach (Buff buff in damageBuffs) {
				damage *= (buff as DamageBuff).getDamageBonus();
			}

			return (int)(Mathf.Ceil(damage));
		}

		public override async Task<bool> doBattleWith(Unit enemy, Tile enemyTile, Battlefield battlefield) {
			await playAttackAnimation();

			int damage = this.battleDamage(enemy, enemyTile);
			//Damage rounds up
			await enemy.changeHealth(-damage, true);

			if (enemy.getHealth() <= 0) {
				enemy.defeated(battlefield);
				return true;
			} else {
				return false;
			}

		}

		public override List<Coord> getAttackZone(int myX, int myY, Battlefield battlefield, Character character) {
			//TODO: create specific implementation for ranged units
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
