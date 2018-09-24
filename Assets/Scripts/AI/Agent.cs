using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public abstract class Agent {
		protected Battlefield battlefield;
		public Level level;
		public Character character;
		public Action<UnityEngine.Object> Destroy;
		
		public Agent(Battlefield battlefield, Level level, Action<UnityEngine.Object> Destroy) {
			this.battlefield = battlefield;
			this.level = level;
			this.Destroy = Destroy;
		}

		//Returns the agent's move, based on the state of the battlefield
		public abstract Task<Move> getMove();

		protected Unit selectUnit() {

			List<Unit> agentsUnits = battlefield.charactersUnits[character];

			foreach (Unit unit in agentsUnits) {
				if (!unit.hasMovedThisTurn) {
					return unit;
				} else {
					Coord coord = battlefield.getUnitCoords(unit);
					if (!unit.getHasAttackedThisTurn() && unit.getTargets(coord.x, coord.y, battlefield, character).Count != 0) {
						return unit;
					}
				}
			}
			return null;
		}

		protected List<Coord> findNearestEnemies(Coord start) {
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

		protected List<Coord> findEnemiesWithinDistance(Coord start, int minDist) {
			int[,] moveDirs = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
			HashSet<Coord> visited = new HashSet<Coord>();
			Queue<Coord> moveQueue = new Queue<Coord>();
			moveQueue.Enqueue(start);
			visited.Add(start);

			List<Coord> enemies = new List<Coord> ();

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

		protected List<Coord> findAllEnemies() {
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

		// Return a list of Tiles from which a unit can attack any enemy unit
		protected List<Coord> getOffenseTiles(Coord unitCoord) {
			List<Coord> offenseTiles = new List<Coord>();
			Unit unit = coordToUnit(unitCoord);
			if (unit == null) {
				return offenseTiles;
			}

			List<Coord> moves = unit.getValidMoves(unitCoord.x, unitCoord.y, battlefield);
			foreach (Coord move in moves) {
				List<Coord> targets = unit.getTargets(move.x, move.y, battlefield, character);
				if (targets.Count != 0) {
					offenseTiles.Add(move);
				}
			}
			return offenseTiles;
		}

		// Return a list of Tiles from which a unit can attack a specific unit
		protected List<Coord> getOffenseTiles(Coord unitCoord, Coord enemy) {
			List<Coord> offenseTiles = new List<Coord>();
			Unit unit = coordToUnit(unitCoord);
			if (unit == null) {
				return offenseTiles;
			}

			List<Coord> moves = unit.getValidMoves(unitCoord.x, unitCoord.y, battlefield);
			foreach (Coord move in moves) {
				List<Coord> targets = unit.getTargets(move.x, move.y, battlefield, character);
				if (!targets.Contains(enemy)) {
					offenseTiles.Add(move);
				}
			}
			return offenseTiles;
		}

		protected Coord compareTargets(Unit unit, List<Coord> targets) {
			//Currently just checks health of all targets
			int minHealth = Int32.MaxValue;
			PriorityQueue<AIBattle> targetPQueue = new PriorityQueue<AIBattle>();
			foreach(Coord target in targets) {
				Unit enemy = coordToUnit(target);
				if (enemy != null) {
					Tile enemyTile = battlefield.map[target.x, target.y].Peek();
					targetPQueue.Enqueue(new AIBattle(unit, enemy, enemyTile, target));
				}
			}
			return targetPQueue.Dequeue().coord;
		}

		protected List<Coord> enemyAttackZone(List<Coord> enemies) {
			List<Coord> attackZone = new List<Coord>();
			foreach (Coord enemy in enemies) {
				Unit unit = coordToUnit(enemy);
				if (unit != null) {
					if (unit.getCharacter(battlefield) == character) {
						attackZone = unionCoords(attackZone, unit.getAttackZone(enemy.x, enemy.y, battlefield, unit.getCharacter(battlefield)));
					}
				}
			}
			return attackZone;
		}

		protected List<Coord> safeMoves(Coord unitCoord, List<Coord> dangerZone) {
			Unit unit = coordToUnit(unitCoord);
			if (unit != null) {
				List<Coord> moves = unit.getValidMoves(unitCoord.x, unitCoord.y, battlefield);
			return exceptCoords(moves, dangerZone);
			}
			return new List<Coord>();
		}

		//Use to check if multiple conditions are met (i.e. if the coord is a location that can be moved to and is from a list of desirable locations)
		protected List<Coord> intersectCoords(List<Coord> coords1, List<Coord> coords2) {
			HashSet<Coord> coordSet = new HashSet<Coord>(coords1);
			coordSet.IntersectWith(coords2);
			return new List<Coord>(coordSet);
		}

		//Use to remove undesirable coords (i.e. coords1 is a list of move options and coords2 is a list coords in enemy's attack range)
		protected List<Coord> exceptCoords(List<Coord> coords1, List<Coord> coords2) {
			HashSet<Coord> coordSet = new HashSet<Coord>(coords1);
			coordSet.ExceptWith(coords2);
			return new List<Coord>(coordSet);
		}

		protected List<Coord> unionCoords(List<Coord> coords1, List<Coord> coords2) {
			HashSet<Coord> coordSet = new HashSet<Coord>(coords1);
			coordSet.UnionWith(coords2);
			return new List<Coord>(coordSet);
		}

		protected int manhattanDistance(Coord coord1, Coord coord2) {
			return Math.Abs(coord2.x - coord1.x) + Math.Abs(coord2.y - coord1.y);
		}

		protected Unit coordToUnit(Coord coord) {
			IBattlefieldItem item = battlefield.battlefieldItemAt(coord.x, coord.y, 0);
			if (item is Unit) {
				return item as Unit;
			} else {
				return null;
			}
		}
	}
}