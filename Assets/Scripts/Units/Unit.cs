using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using Gameplay;
using System;
using AI;

namespace Units {
	public abstract class Unit : MonoBehaviour, IBattlefieldItem {
		private const int DEFAULT_DAMAGE = 10;
		private const int DEFAULT_MOVE = 4;

		private readonly ArmorType armor;
		private readonly WeaponType weapon;
		private readonly MoveType moveType;
		private bool hasMovedThisTurn;
		//How many tiles this unit can move per turn turn
		private int numMoveTiles { get; set; }

		public Unit(ArmorType armorType, WeaponType weaponType, MoveType moveType, int numMoveTiles) {
			armor = armorType;
			weapon = weaponType;
			this.moveType = moveType;
			this.numMoveTiles = numMoveTiles;
			hasMovedThisTurn = false;
		}

		void Start() {
		}
		void Update() {

		}

		public abstract int unitMoveDistance();

		public static int unitMoveDistance(Type unitType) {
			int unitTypeIndex = 0;

			if (unitType == typeof(Knight)) {
				unitTypeIndex = 0;
			} else {
				throw new TypeUnloadedException("Type " + unitType + " not recognized as a valid type for units");
			}

			return ConstantTables.UnitMoveDistance[unitTypeIndex];
		}

		//For now this will use a simple percolation algorithm using a visited set instead of a disjoint set approach
		//We can get away with this because there's only one "flow" source point (the unit)
		public List<AIUnitMove> getValidMoves(int myX, int myY, Battlefield battlefield) {
			HashSet<AIUnitMove> visited = new HashSet<AIUnitMove>();

			//Look breadth first to use the heuristic of "straighter path is probably faster"
			//this'll help to cut out bad trees earlier too.
			Queue<AIUnitMove> moveQueue = new Queue<AIUnitMove>();
			moveQueue.Enqueue(new AIUnitMove(myX, myY, unitMoveDistance()));
			while (moveQueue.Count > 0) {
				AIUnitMove currentMove = moveQueue.Dequeue();

				//do all four directions
				List<AIUnitMove> validMoves = new List<AIUnitMove>();
				int[,] moveDirs = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
				for (int x = 0; x < moveDirs.GetLength(0); x++) {
					Stack<Tile> targetTile = battlefield.map[currentMove.x + moveDirs[x, 0], currentMove.y + moveDirs[x, 1]];
					int remainingMovePoints = currentMove.movePointsLeft - targetTile.Peek().tileType.Cost(this.moveType);
					if (remainingMovePoints > 0) {
						//TODO if the move is already in the visited set for cheaper, don't add it.
						validMoves.Add(new AIUnitMove(currentMove.x, currentMove.y, remainingMovePoints));
					}
				}

				foreach (AIUnitMove move in validMoves) {
					if (!visited.Contains(move)) {
						moveQueue.Enqueue(move);
					}
				}
			}

			List<AIUnitMove> visitedList = new List<AIUnitMove>();
			foreach (AIUnitMove move in visited) {
				visitedList.Add(move);
			}
			return visitedList;
		}
	}
}