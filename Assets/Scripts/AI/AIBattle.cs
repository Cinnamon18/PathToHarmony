using System;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public class AIBattle : IComparable<AIBattle> {
		// Contains a score representing the cost of a battle

		public float score;
		public Coord coord;

		public AIBattle(Unit unit, Unit enemy, Tile enemyTile, Coord enemyCoord) {
			if (unit is StatusUnit) {
				this.score = 1.0f;
			} else {
				int damage = unit.battleDamage(enemy, enemyTile);
				this.score = damage / (float) enemy.getHealth();
				if (this.score > 1) {
					this.score = 1;
				}
			}
			this.coord = enemyCoord;
		}

		public int CompareTo(AIBattle otherMove) {
			return (int)((this.score - otherMove.score)*100);
		}
	}
}

