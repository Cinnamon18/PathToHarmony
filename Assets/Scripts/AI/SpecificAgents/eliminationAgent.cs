using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public class eliminationAgent : Agent {

		public eliminationAgent() : base() { }

		public override async Task<Move> getMove() {

			List<Coord> units = findAllUnits();
			List<Coord> enemies = filterEnemies(units);
			List<Coord> allies = filterAllies(units);

			List<Coord> available = filterHasMove(allies);

			Coord unitCoord = new Coord(0,0);
			Unit curUnit;
			foreach (Coord coord in available) {
				Unit unit = battlefield.units[coord.x, coord.y];
				if (unit is HealerUnit) {
					unitCoord = coord;
					curUnit = unit;
					break;
				} else if (unit is MeleeUnit)  {
					unitCoord = coord;
					curUnit = unit;
				} else if (!(curUnit is MeleeUnit)){
					unitCoord = coord;
					curUnit = unit;
				}
			}

			if (curUnit is HealerUnit) {
				if (curUnit.getHealth() < curUnit.maxHealth * 0.4) {
					// TODO
					// Flee
				} else {
					HashSet<Coord> attackZone = curUnit.getTotalAttackZone(unitCoord.x, unitCoord.y, battlefield, character);
					attackZone.IntersectWith(allies);
					List<Coord> targets = new List<Coord>(attackZone);
					if (targets.Count > 0) {
						// TODO
						// Choose best target
					} else {
						
					}
				}
			} else if (curUnit is MeleeUnit) {
				if (curUnit.getHealth() < curUnit.maxHealth * 0.4) {
					// TODO
					// Flee
				} else {
					HashSet<Coord> attackZone = curUnit.getTotalAttackZone(unitCoord.x, unitCoord.y, battlefield, character);
					attackZone.IntersectWith(enemies);
					List<Coord> targets = new List<Coord>(attackZone);
					if (targets.Count > 0) {
						// TODO
						// Choose best target
					} else {
						targets = findNearestEnemeis(unitCoord);
						if (targets.Count == 0) {
							return new Move(unitCoord, unitCoord);
						}
						int minDIst = Int32.MaxValue;
						foreach (Coord coord in curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield)) {
							int dist = this.manhattanDistance(coord, targetCoord);
							if (dist < minDist) {
								minDist = dist;
								bestCoord = coord;
							}
						}
						return new Move(unitCoord, bestCoord);
					}
				}
			} else if (curUnit is RangedUnit) {
				if (curUnit.getHealth() < curUnit.maxHealth * 0.4) {
					// TODO
					// Flee
				} else {

				}
			} else if (curUnit is StatusUnit) {
				if (curUnit.getHealth() < curUnit.maxHealth * 0.4) {
					// TODO
					// Flee
				} else {

				}
			}
			// Unit unit = selectUnit();
			// Coord unitCoord = battlefield.getUnitCoords(unit);	

			// List<Coord> targetCoords = findNearestEnemies(unitCoord);

			// List<Coord> targets = unit.getTargets(unitCoord.x, unitCoord.y, battlefield, character);

			// int minDist = Int32.MaxValue;
			// Coord bestCoord = null;
			// foreach (Coord targetCoord in targetCoords){
			// 	if (targets.Any(t => t.x == targetCoord.x && t.y == targetCoord.y)) {
			// 		return new Move(unitCoord.x, unitCoord.y, targetCoord.x, targetCoord.y);
			// 	}
				
			// 	foreach (Coord coord in unit.getValidMoves(unitCoord.x, unitCoord.y, battlefield)) {
			// 		int dist = this.manhattanDistance(coord, targetCoord);
			// 		if (dist < minDist) {
			// 			minDist = dist;
			// 			bestCoord = coord;
			// 		}
			// 	}
			// }
			
			//Just so the player can keep track of what's happening
			await Task.Delay(300);

			return new Move();
		}

	}
}
