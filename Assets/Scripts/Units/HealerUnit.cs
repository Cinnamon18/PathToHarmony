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
	public class HealerUnit : Unit {

		//attributes specific to Melee
		private readonly DamageType damageType;
		private readonly int meleeAttackStrength;

		public HealerUnit(
				ArmorType armorType,
				int maxHealth,
				MoveType moveType,
				int moveDistance,
				DamageType damageType,
				int meleeAttackStrength,
				Faction faction
			) : base(armorType, maxHealth, moveType, moveDistance, faction) {

			this.damageType = damageType;
			this.meleeAttackStrength = meleeAttackStrength;
		}

		public override int battleDamage(Unit enemy, Tile enemyTile) {
			float healing = -1 * this.meleeAttackStrength * (1f * (this as Unit).getHealth() / this.maxHealth);
			// damage = damage * ((100 - this.damageType.DamageReduction(enemy.armor)) / 100.0f);
			// damage = damage * ((100 - enemyTile.tileType.DefenseBonus()) / 100.0f);

			// List<Buff> damageBuffs = getBuffsOfType(BuffType.Damage);
			// foreach (Buff buff in damageBuffs) {
			// damage *= (buff as DamageBuff).getDamageBonus();
			// }

			return (int)(Mathf.Floor(healing));
		}

		public override async Task<bool> doBattleWith(Unit enemy, Tile enemyTile, Battlefield battlefield) {
			await playAttackAnimation();

			int damage = this.battleDamage(enemy, enemyTile);
			//Damage rounds up
			await enemy.changeHealth(-damage, true);

			return false;
		}

		//returns a list of targetable allies
		public override List<Coord> getTargets(int myX, int myY, Battlefield battlefield, Character character) {

			List<Coord> targets = new List<Coord>();
			List<Coord> tiles = getAttackZone(myX, myY, battlefield, character);

			foreach (Coord tile in tiles) {
				Unit targetUnit = battlefield.units[tile.x, tile.y];
				if (
					targetUnit != null &&
					targetUnit.getCharacter(battlefield) == character &&
					targetUnit.getHealth() != targetUnit.maxHealth) {

					targets.Add(tile);
				}
			}
			return targets;
		}

		public override List<Coord> getAttackZone(int myX, int myY, Battlefield battlefield, Character character) {

			List<Coord> targets = new List<Coord>();
			Coord[] tiles = new Coord[] { new Coord(myX + 1, myY), new Coord(myX - 1, myY), new Coord(myX, myY + 1), new Coord(myX, myY - 1) };
			foreach (Coord tile in tiles) {
				if (!(tile.x < 0 || tile.y < 0 || tile.x >= battlefield.map.GetLength(0) || tile.y >= battlefield.map.GetLength(1))) {
					targets.Add(tile);
				}
			}
			return targets;
		}

		public override HashSet<Coord> getTotalAttackZone(int myX, int myY, Battlefield battlefield, Character character) {
			return new HashSet<Coord>(getAttackZone(myX, myY, battlefield, character));
		}
	}
}
