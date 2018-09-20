using System;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public class AIBattle : IComparable<AIBattle> {
		// Contains a score representing the cost of a battle

		public float score;

		public AIBattle(Unit unit, Unit enemy, Tile enemyTile) {
			int damage = unit.battleDamage(enemy, enemyTile);
			this.score = damage / (float) enemy.getHealth();
			if (this.score > 1) {
				this.score = 1;
			}
		}

		public int CompareTo(AIBattle otherMove) {
			return (int)((this.score - otherMove.score)*100);
		}
	}
}

