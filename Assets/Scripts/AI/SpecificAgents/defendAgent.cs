using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;
using Constants;

namespace AI {
	public class DefendAgent : Agent {

		public Coord capturePoint;
		public List<Unit> VIPs;

		//Capture points may not be known at instantiation time. They must be assigned at some point before getMove is called.
		public DefendAgent() : this(new Coord(0,0)) {}

		public DefendAgent(Coord capturePoint) : base() {
			this.capturePoint = capturePoint;
			this.VIPs = new List<Unit>();
		}

		public override async Task<Move> getMove() {

			//Get all unit categories
			List<Coord> units = findAllUnits();
			List<Coord> enemies = filterEnemies(units);
			List<Coord> allies = filterAllies(units);

			List<Coord> available = filterHasMove(allies);

			// Select a unit based on type
			Coord unitCoord = new Coord(0,0);
			Unit curUnit = null;
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

			
			if (curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield).Count == 0 && curUnit.getTargets(unitCoord.x, unitCoord.y, battlefield, character).Count == 0) {
				return new Move(unitCoord, unitCoord);
			}

			if (VIPs.Contains(curUnit)) {
				// Debug.Log("VIP");
				// Evade
				HashSet<Coord> dangerZone = enemyAttackZone(enemies);
				HashSet<Coord> safeZone = safeMoves(unitCoord, dangerZone);
				if (safeZone.Count > 0) {
					int bestScore = Int32.MaxValue;
					Coord bestCoord = null;
					foreach (Coord coord in safeZone) {
						int distScore = sumDistances(coord, allies);
						if (distScore < bestScore) {
							bestScore = distScore;
							bestCoord = coord;
						}
					}
					return new Move(unitCoord, bestCoord);
				} else {
					int bestScore = 0;
					Coord bestCoord = null;
					List<Coord> moves = curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield);
					moves.Add(unitCoord);
					foreach (Coord coord in moves) {
						int distScore = sumDistances(coord, enemies);
						if (distScore > bestScore) {
							bestScore = distScore;
							bestCoord = coord;
						}
					}
					return new Move(unitCoord, bestCoord);
				}
			}

			//Decide action based on type
			if (curUnit is HealerUnit) {
				Coord bestTarget = null;
				float bestScore = 0;
				if (curUnit.hasMovedThisTurn) {
					foreach(Coord target in curUnit.getTargets(unitCoord.x, unitCoord.y, battlefield, character)) {
						Unit targetUnit = battlefield.units[target.x, target.y];
						float score = targetUnit.maxHealth / targetUnit.getHealth();
						if (score > bestScore) {
							bestTarget = target;
							bestScore = score;
						}
					}
					return new Move(unitCoord, bestTarget);
				}
				if (curUnit.getHealth() < curUnit.maxHealth * -0.4) {
					// TODO
					// Flee
				} else {
					List<Coord> injured = filterInjured(allies, 0.6f);
					if (injured.Count > 0) {
						HashSet<Coord> attackZone = curUnit.getTotalAttackZone(unitCoord.x, unitCoord.y, battlefield, character);
						attackZone.IntersectWith(injured);
						List<Coord> targets = new List<Coord>(attackZone);
						if (targets.Count > 0) {
							bestTarget = null;
							bestScore = 0;
							foreach (Coord target in targets) {
								Unit targetUnit = battlefield.units[target.x, target.y];
								float score = targetUnit.maxHealth / targetUnit.getHealth();
								if (score > bestScore) {
									bestTarget = target;
									bestScore = score;
								}
							}
							// If unit has already moved heal best target
							if (curUnit.hasMovedThisTurn) {
								return new Move(unitCoord, bestTarget);
							}
							// Else choose best tile to move to
							HashSet<Coord> adjacentTiles = new HashSet<Coord>();
							adjacentTiles.Add(new Coord(bestTarget.x + 1, bestTarget.y));
							adjacentTiles.Add(new Coord(bestTarget.x - 1, bestTarget.y));
							adjacentTiles.Add(new Coord(bestTarget.x, bestTarget.y + 1));
							adjacentTiles.Add(new Coord(bestTarget.x, bestTarget.y - 1));
							adjacentTiles.IntersectWith(curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield));
							
							// Hardcoded hack for time efficiency
							if (manhattanDistance(unitCoord, bestTarget) == 1) {
								adjacentTiles.Add(unitCoord);
							}
							Coord bestCoord = null;
							int tileDef = Int32.MinValue;
							foreach (Coord coord in adjacentTiles) {
								if (tileDef < ConstantTables.TileDefense[(int)battlefield.map[coord.x, coord.y].Peek().tileType]){
									tileDef = ConstantTables.TileDefense[(int)battlefield.map[coord.x, coord.y].Peek().tileType];
									bestCoord = coord;
								}
							}
							if (unitCoord.Equals(bestCoord)) {
								return new Move(unitCoord, bestTarget);
							}
							return new Move(unitCoord, bestCoord);
						} else {
							// Find nearest injured and move to them
							bestTarget = nearestCoord(unitCoord, injured);
							Coord bestCoord = nearestCoord(bestTarget, curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield));
							return new Move(unitCoord, bestCoord);
						}
					} else {
						// Evade around the capture point
						HashSet<Coord> dangerZone = enemyAttackZone(enemies);
						HashSet<Coord> safeZone = safeMoves(unitCoord, dangerZone);
						if (safeZone.Count > 0) {
							bestScore = Int32.MaxValue;
							Coord bestCoord = null;
							foreach (Coord coord in safeZone) {
								int distScore = manhattanDistance(capturePoint, coord);
								if (distScore < bestScore) {
									bestScore = distScore;
									bestCoord = coord;
								}
							}
							return new Move(unitCoord, bestCoord);
						} else {
							bestScore = 0;
							Coord bestCoord = null;
							List<Coord> moves = curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield);
							moves.Add(unitCoord);
							foreach (Coord coord in moves) {
								int distScore = manhattanDistance(capturePoint, coord);
								if (distScore > bestScore) {
									bestScore = distScore;
									bestCoord = coord;
								}
							}
							return new Move(unitCoord, bestCoord);
						}
					}
					
				}
			} else if (curUnit is MeleeUnit) {
				if (curUnit.getHealth() < curUnit.maxHealth * -0.4) {
					// TODO
					// Flee
				} else {
					// Seek an enemy to attack if health is high
					HashSet<Coord> attackZone = curUnit.getTotalAttackZone(unitCoord.x, unitCoord.y, battlefield, character);
					attackZone.IntersectWith(enemies);
					List<Coord> targets = new List<Coord>(attackZone);
					float bestScore = 0;
					Coord bestTarget = null;
					if (targets.Count > 0) {
						// If targets are in range find best
						foreach (Coord target in targets) {
							Tile enemyTile = battlefield.map[target.x, target.y].Peek();
							Unit enemy = battlefield.units[target.x, target.y];
							AIBattle battle = new AIBattle(curUnit, enemy, enemyTile, target, battlefield);
							if (battle.score > bestScore) {
								bestScore = battle.score;
								bestTarget = target;
							}
						}
						// If unit has already moved attack best target
						if (curUnit.hasMovedThisTurn) {
							return new Move(unitCoord, bestTarget);
						}
						// Else choose best tile to move to
						HashSet<Coord> adjacentTiles = new HashSet<Coord>();
						adjacentTiles.Add(new Coord(bestTarget.x + 1, bestTarget.y));
						adjacentTiles.Add(new Coord(bestTarget.x - 1, bestTarget.y));
						adjacentTiles.Add(new Coord(bestTarget.x, bestTarget.y + 1));
						adjacentTiles.Add(new Coord(bestTarget.x, bestTarget.y - 1));
						adjacentTiles.IntersectWith(curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield));
						
						// Hardcoded hack for time efficiency
						if (manhattanDistance(unitCoord, bestTarget) == 1) {
							adjacentTiles.Add(unitCoord);
						}
						Coord bestCoord = null;
						int tileDef = Int32.MinValue;
						foreach (Coord coord in adjacentTiles) {
							if (tileDef < ConstantTables.TileDefense[(int)battlefield.map[coord.x, coord.y].Peek().tileType]){
								tileDef = ConstantTables.TileDefense[(int)battlefield.map[coord.x, coord.y].Peek().tileType];
								bestCoord = coord;
							}
						}
						if (unitCoord.Equals(bestCoord)) {
							return new Move(unitCoord, bestTarget);
						}
						return new Move(unitCoord, bestCoord);
					} else {
						// Move towards defence point
						List<Coord> moves = curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield);
						moves.Add(unitCoord);
						Coord bestCoord = nearestCoord(capturePoint, moves);
						return new Move(unitCoord, bestCoord);
					}
				}
			} else if (curUnit is RangedUnit) {
				if (curUnit.getHealth() < curUnit.maxHealth * -0.4) {
					// TODO
					// Flee
				} else {
					// Seek an enemy to attack if health is high
					HashSet<Coord> attackZone = curUnit.getTotalAttackZone(unitCoord.x, unitCoord.y, battlefield, character);
					attackZone.IntersectWith(enemies);
					List<Coord> targets = new List<Coord>(attackZone);
					float bestScore = 0;
					Coord bestTarget = null;
					if (targets.Count > 0) {
						// If targets are in range find best
						foreach (Coord target in targets) {
							Tile enemyTile = battlefield.map[target.x, target.y].Peek();
							Unit enemy = battlefield.units[target.x, target.y];
							AIBattle battle = new AIBattle(curUnit, enemy, enemyTile, target, battlefield);
							if (battle.score > bestScore) {
								bestScore = battle.score;
								bestTarget = target;
							}
						}
						return new Move(unitCoord, bestTarget);
					} else {
						// Move towards defence point
						List<Coord> moves = curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield);
						moves.Add(unitCoord);
						Coord bestCoord = nearestCoord(capturePoint, moves);
						return new Move(unitCoord, bestCoord);
					}
				}
			} else if (curUnit is StatusUnit) {
				if (curUnit.getHealth() < curUnit.maxHealth * -0.4) {
					// TODO
					// Flee
				} else {
					// Seek an enemy to attack if health is high
					HashSet<Coord> attackZone = curUnit.getTotalAttackZone(unitCoord.x, unitCoord.y, battlefield, character);
					attackZone.IntersectWith(enemies);
					List<Coord> targets = new List<Coord>(attackZone);
					float bestScore = 0;
					Coord bestTarget = null;
					if (targets.Count > 0) {
						// If targets are in range find best
						foreach (Coord target in targets) {
							Tile enemyTile = battlefield.map[target.x, target.y].Peek();
							Unit enemy = battlefield.units[target.x, target.y];
							AIBattle battle = new AIBattle(curUnit, enemy, enemyTile, target, battlefield);
							if (battle.score > bestScore) {
								bestScore = battle.score;
								bestTarget = target;
							}
						}
						return new Move(unitCoord, bestTarget);
					} else {
						// Move towards defence point
						List<Coord> moves = curUnit.getValidMoves(unitCoord.x, unitCoord.y, battlefield);
						moves.Add(unitCoord);
						Coord bestCoord = nearestCoord(capturePoint, moves);
						return new Move(unitCoord, bestCoord);
					}
				}
			}
			
			//Just so the player can keep track of what's happening
			await Task.Delay(300);

			return new Move();
		}

	}
}
