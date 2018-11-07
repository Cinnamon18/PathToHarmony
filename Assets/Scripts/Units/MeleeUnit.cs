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
	public class MeleeUnit : Unit {

		//attributes specific to Melee
		private readonly DamageType damageType;
		private readonly int meleeAttackStrength;

		public MeleeUnit(
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
			float damage = this.meleeAttackStrength * (1f * (this as Unit).getHealth() / this.maxHealth);
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
			HashSet<Coord> attackZone = new HashSet<Coord>();
			attackZone.UnionWith(getAttackZone(myX, myY, battlefield, character));
			foreach (Coord coord in getValidMoves(myX, myY, battlefield)) {
				attackZone.UnionWith(getAttackZone(coord.x, coord.y, battlefield, character));
			}
			return attackZone;
		}
	}
}
