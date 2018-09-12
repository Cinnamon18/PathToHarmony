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
	public class MeleeUnit : Unit {

		//attributes specific to Melee
		private readonly DamageType damageType;
		private readonly int meleeAttackStrength;
	

	
		public MeleeUnit(ArmorType armorType, int maxHealth, MoveType moveType, int moveDistance, DamageType damageType, int meleeAttackStrength) : base(armorType, maxHealth, moveType,moveDistance) {
           this.damageType = damageType;
		   this.meleeAttackStrength = meleeAttackStrength;
        }

		public override bool doBattleWith(Unit enemy, Tile enemyTile, Battlefield battlefield) {
			float damage = this.meleeAttackStrength * (1f * (this as Unit).health / this.maxHealth);
			damage = damage * ((100 - this.damageType.DamageReduction(enemy.armor)) / 100.0f);
			damage = damage * ((100 - enemyTile.tileType.DefenseBonus()) / 100.0f);

			List<Buff> damageBuffs = buffs.FindAll( buff => buff.GetType() == typeof(DamageBuff));
			foreach (Buff buff in damageBuffs) {
				damage = damage *= (buff as DamageBuff).getDamageBonus();
			}

			//Damage rounds up
			enemy.health -= (int)(Mathf.Ceil(damage));
			enemy.healthBar.fillAmount = 1f * enemy.health / enemy.maxHealth;

			if (enemy.health <= 0) {
				enemy.defeated(battlefield);
				return true;
			} else {
				return false;
			}

		}

		public override List<Unit> getTargets(int myX, int myY, Battlefield battlefield, Character character) {

			List<Unit> targets = new List<Unit>();

			for (int x = myX - 1; x <= myX + 1;x++) {
				for (int y = myY - 1; y <= myY + 1; y++) {
					try {
						Unit targetUnit = battlefield.units[x, y];
						if(targetUnit != null && targetUnit.getCharacter(battlefield) != character) {
							targets.Add(targetUnit);
						}
					} catch (IndexOutOfRangeException) {
					//do nothing, we just tried to check for a tile at a location outside the bounds of the map
					}
				}
			}
			return targets;
		}

	}
}
