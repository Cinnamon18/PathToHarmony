using System;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public class AIBattle : IComparable<AIBattle> {
		// Contains a score representing the cost of a battle

		public float score;
		public Coord coord;

		public AIBattle(Unit unit, Unit enemy, Tile enemyTile, Coord enemyCoord, Battlefield battlefield) {
			if (unit is StatusUnit) {
				this.score = 1.0f;
			} else {
				Coord unitCoord = battlefield.getUnitCoords(unit);
				Coord targetCoord = battlefield.getUnitCoords(enemy);
				int unitHeight = battlefield.map[unitCoord.x, unitCoord.y].Count;
				int targetHeight = battlefield.map[unitCoord.x, unitCoord.y].Count;
				int damage = unit.battleDamage(enemy, enemyTile, unitHeight, targetHeight);
				this.score = damage / (float)enemy.getHealth();
				if (this.score > 1) {
					this.score = 1;
				}
			}
			this.coord = enemyCoord;
		}

		public int CompareTo(AIBattle otherMove) {
			return (int)((this.score - otherMove.score) * 100);
		}
	}
}

