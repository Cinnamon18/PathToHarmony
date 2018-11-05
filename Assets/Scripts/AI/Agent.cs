using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public abstract class Agent {
		public Battlefield battlefield;
		public Character character;

		public Agent() {
			//The refrence to battlefield is assigned in BattleControl on load from persistance

			//the refrence to character is assigned in the character constructor
			//(because a character and agent are gaurenteed to have refrences to eachother)

			//I'm sorry I didn't mean to make these so tightly coupled but. stuff happens.
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

			List<Coord> enemies = new List<Coord>();
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

		// Replaced findEnemiesWithinDistance. Use with filterEnemies to achieve the same result.
		protected List<Coord> findUnitsWithinDistance(Coord start, int minDist) {
			int[,] moveDirs = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
			HashSet<Coord> visited = new HashSet<Coord>();
			Queue<Coord> moveQueue = new Queue<Coord>();
			moveQueue.Enqueue(start);
			visited.Add(start);

			List<Coord> units = new List<Coord> ();

			while (moveQueue.Count() > 0) {
				Coord curCoord = moveQueue.Dequeue();

				if (manhattanDistance(start, curCoord) > minDist) {
					return units;
				}

				IBattlefieldItem item = battlefield.battlefieldItemAt(curCoord.x, curCoord.y, 0);
				if (item is Unit) {
					Unit unit = item as Unit;
					units.Add(curCoord);
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
			return units;
		}

		// Replaced findAllEnemies. Use with filterEnemies to achieve the same result.
		protected List<Coord> findAllUnits() {
			List<Coord> units = new List<Coord>();
			for (int x = 0; x < battlefield.units.GetLength(0); x++) {
				for (int y = 0; y < battlefield.units.GetLength(1); y++) {
					if (battlefield.units[x,y] != null) {
						units.Add(new Coord(x,y));
					}
				}
			}
			return units;
		}

		#region Filter functions
		protected List<Coord> filterEnemies(List<Coord> units) {
			List<Coord> enemies = new List<Coord>();
			foreach (Coord coord in units) {
				Unit unit = battlefield.units[coord.x, coord.y];
				if (unit != null && unit.getCharacter(battlefield) != character) {
					enemies.Add(coord);
				}
			}
			return enemies;
		}

		protected List<Coord> filterAllies(List<Coord> units) {
			List<Coord> allies = new List<Coord>();
			foreach (Coord coord in units) {
				Unit unit = battlefield.units[coord.x, coord.y];
				if (unit != null && unit.getCharacter(battlefield) == character) {
					allies.Add(coord);
				}
			}
			return allies;
		}

		protected List<Coord> filterThreats(List<Coord> units, Coord myUnit, float score) {
			List<Coord> lowThreat = new List<Coord>();
			foreach (Coord enemy in units) {
				AIBattle battle = new AIBattle(battlefield.units[enemy.x, enemy.y], battlefield.units[myUnit.x, myUnit.y], battlefield.map[myUnit.x, myUnit.y].Peek(), myUnit);
				if (battle.score >= score) {
					lowThreat.Add(enemy);
				}
			}
			return lowThreat;
		}

		protected List<Coord> filterHasMove(List<Coord> units) {
			List<Coord> hasMove = new List<Coord>();
			foreach (Coord coord in units) {
				Unit unit = battlefield.units[coord.x, coord.y];
				if (unit != null && (!unit.hasMovedThisTurn || (!unit.getHasAttackedThisTurn() && unit.getTargets(coord.x, coord.y, battlefield, character).Count != 0))) {
					hasMove.Add(coord);
				}
			}
			return hasMove;
		}

		protected List<Coord> filterInjured(List<Coord> allies, float threshold) {
			List<Coord> injured = new List<Coord>();
			foreach (Coord coord in allies) {
				Unit unit = battlefield.units[coord.x, coord.y];
				if (unit.maxHealth / unit.getHealth() >= 1.0/threshold) {
					injured.Add(coord);
				}
			}
			return injured;
		}

		protected List<Coord> filterHealers(List<Coord> units) {
			List<Coord> healers = new List<Coord>();
			foreach (Coord coord in units) {
				Unit unit = battlefield.units[coord.x, coord.y];
				if (unit != null && unit is HealerUnit) {
					healers.Add(coord);
				}
			}
			return healers;
		}
		#endregion

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
			foreach (Coord target in targets) {
				Unit enemy = coordToUnit(target);
				if (enemy != null) {
					Tile enemyTile = battlefield.map[target.x, target.y].Peek();
					targetPQueue.Enqueue(new AIBattle(unit, enemy, enemyTile, target));
				}
			}
			return targetPQueue.Dequeue().coord;
		}

		protected HashSet<Coord> enemyAttackZone(List<Coord> enemies) {
			HashSet<Coord> attackZone = new HashSet<Coord>();
			foreach (Coord enemy in enemies) {
				Unit unit = coordToUnit(enemy);
				if (unit != null) {
					attackZone.UnionWith(unit.getTotalAttackZone(enemy.x, enemy.y, battlefield, unit.getCharacter(battlefield)));
				}
			}
			return attackZone;
		}

		protected HashSet<Coord> safeMoves(Coord unitCoord, HashSet<Coord> dangerZone) {
			Unit unit = coordToUnit(unitCoord);
			if (unit != null) {
				HashSet<Coord> moves = new HashSet<Coord>(unit.getValidMoves(unitCoord.x, unitCoord.y, battlefield));
				moves.ExceptWith(dangerZone);
				return moves;
			}
			return new HashSet<Coord>();
		}

		protected Coord nearestCoord(Coord myCoord, List<Coord> coords) {
			int minDist = Int32.MaxValue;
			Coord bestCoord = null;
			foreach(Coord coord in coords) {
				int dist = manhattanDistance(myCoord, coord);
				if (dist < minDist) {
					minDist = dist;
					bestCoord = coord;
				}
			}
			return bestCoord;
		}

		protected int sumDistances(Coord myCoord, List<Coord> coords) {
			int dist = 0;
			foreach(Coord coord in coords) {
				dist += manhattanDistance(myCoord, coord);
			}
			return dist;
		}

		protected Move flee(Coord unitCoord, Unit unit, List<Coord> healers) {
			return new Move();
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