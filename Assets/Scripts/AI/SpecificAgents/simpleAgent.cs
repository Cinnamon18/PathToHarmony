using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public class simpleAgent : Agent {

		public simpleAgent(Battlefield battlefield, Level level, Action<UnityEngine.Object> Destroy) : base(battlefield, level, Destroy) { }

		public override async Task<Move> getMove() {
			Unit unit = selectUnit();
			Coord unitCoord = battlefield.getUnitCoords(unit);	

			List<Coord> targetCoords = findNearestEnemies(unitCoord);

			List<Coord> targets = unit.getTargets(unitCoord.x, unitCoord.y, battlefield, character);

			int minDist = Int32.MaxValue;
			Coord bestCoord = null;
			foreach (Coord targetCoord in targetCoords){
				if (targets.Any(t => t.x == targetCoord.x && t.y == targetCoord.y)) {
					return new Move(unitCoord.x, unitCoord.y, targetCoord.x, targetCoord.y);
				}
				
				foreach (Coord coord in unit.getValidMoves(unitCoord.x, unitCoord.y, battlefield)) {
					int dist = this.manhattanDistance(coord, targetCoord);
					if (dist < minDist) {
						minDist = dist;
						bestCoord = coord;
					}
				}
			}
			
			//Just so the player can keep track of what's happening
			await Task.Delay(300);

			return new Move(unitCoord.x, unitCoord.y, bestCoord.x, bestCoord.y);
		}

		private Unit selectUnit() {

			List<Unit> agentsUnits = battlefield.charactersUnits[character];
			Unit unit = null;

			foreach (Unit u in agentsUnits) {
				if (!u.hasMovedThisTurn) {
					unit = u;
					break;
				}
			}
			return unit;
		}

		private Coord findNearestEnemy(Coord start) {

			int[,] moveDirs = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
			HashSet<Coord> visited = new HashSet<Coord>();
			Queue<Coord> moveQueue = new Queue<Coord>();
			moveQueue.Enqueue(start);
			visited.Add(start);

			while (moveQueue.Count() > 0) {
				Coord curCoord = moveQueue.Dequeue();
				IBattlefieldItem item = battlefield.battlefieldItemAt(curCoord.x, curCoord.y, 0);
				if (item is Unit) {
					Unit unit = item as Unit;
					if (!battlefield.charactersUnits[character].Contains(unit)) {
						return curCoord;
					}
				}

				for (int x = 0; x < moveDirs.GetLength(0); x++) {
					Coord nextCoord = new Coord(curCoord.x + moveDirs[x, 0], curCoord.y + moveDirs[x, 1]);
					if (nextCoord.x >= 0 && nextCoord.y >= 0 && nextCoord.x < battlefield.map.GetLength(0) && nextCoord.y < battlefield.map.GetLength(1)) {
						if (!visited.Contains(nextCoord)) {
							visited.Add(nextCoord);
							moveQueue.Enqueue(nextCoord);
						}
					}
				}
			}
			return start;
		}

		private List<Coord> findNearestEnemies(Coord start) {
			int[,] moveDirs = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
			HashSet<Coord> visited = new HashSet<Coord>();
			Queue<Coord> moveQueue = new Queue<Coord>();
			moveQueue.Enqueue(start);
			visited.Add(start);

			List<Coord> enemies = new List<Coord> ();
			int minDist = Int32.MaxValue;

			while (moveQueue.Count() > 0) {
				Coord curCoord = moveQueue.Dequeue();

				if (manhattanDistance(start, curCoord) > minDist) {
					return enemies;
				}

				IBattlefieldItem item = battlefield.battlefieldItemAt(curCoord.x, curCoord.y, 0);
				if (item is Unit) {
					Unit unit = item as Unit;
					if (!battlefield.charactersUnits[character].Contains(unit)) {
						enemies.Add(curCoord);
						if (minDist == Int32.MaxValue) {
							minDist = manhattanDistance(start, curCoord);
						}
					}
				}

				for (int x = 0; x < moveDirs.GetLength(0); x++) {
					Coord nextCoord = new Coord(curCoord.x + moveDirs[x, 0], curCoord.y + moveDirs[x, 1]);
					if (nextCoord.x >= 0 && nextCoord.y >= 0 && nextCoord.x < battlefield.map.GetLength(0) && nextCoord.y < battlefield.map.GetLength(1)) {
						if (!visited.Contains(nextCoord)) {
							visited.Add(nextCoord);
							moveQueue.Enqueue(nextCoord);
						}
					}
				}
			}
			return enemies;
		}

		private List<Coord> findAllEnemies() {
			List<Coord> enemies = new List<Coord>();
			for (int x = 0; x < battlefield.units.GetLength(0); x++) {
				for (int y = 0; y < battlefield.units.GetLength(1); y++) {
					if (battlefield.units[x,y] != null && battlefield.units[x,y].getCharacter(battlefield) != character) {
						enemies.Add(new Coord(x,y));
					}
				}
			}
			return enemies;
		}

		private Coord bestTarget(List<Coord> targets) {
			//Currently just checks health of all targets
			int minHealth = Int32.MaxValue;
			Coord best = null;
			foreach(Coord target in targets) {
				IBattlefieldItem item = battlefield.battlefieldItemAt(target.x, target.y, 0);
				if (item is Unit) {
					Unit unit = item as Unit;
					if (unit.health < minHealth) {
						minHealth = unit.health;
						best = target;
					}
				}
			}
			return best;
		}

		private int manhattanDistance(Coord coord1, Coord coord2) {
			return Math.Abs(coord2.x - coord1.x) + Math.Abs(coord2.y - coord1.y);
		}
	}
}
