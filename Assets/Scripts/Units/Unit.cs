using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using Gameplay;

namespace Units {
	public abstract class Unit : IBattlefieldItem {
		private const int DEFAULT_DAMAGE = 10;
		private const int DEFAULT_MOVE = 4;

		private readonly ArmorType armor;
		private readonly WeaponType weapon;
		private bool hasMovedThisTurn;
		//How many tiles this unit can move per turn turn
		private int numMoveTiles { get; set; }

		public Unit(ArmorType armorType, WeaponType weaponType, int numMoveTiles) {
			armor = armorType;
			weapon = weaponType;
			this.numMoveTiles = numMoveTiles;
			hasMovedThisTurn = false;
		}

		//For now this will use a simple percolation algorithm using a visited set instead of a disjoint set approach
		//We can get away with this because there's only one "flow" source point (the unit)
		public List<UnitMove> getValidMoves(int myX, int myY, Battlefield battlefield) {
			HashSet<UnitMove> visited = new HashSet<UnitMove>();

			//Look breadth first to use the heuristic of "straighter path is probably faster"
			//this'll help to cut out bad trees earlier too.
			Queue<UnitMove> moveQueue = new Queue<UnitMove>();
			moveQueue.Enqueue(new UnitMove(myX, myY));
			while (moveQueue.Count > 0) {
				UnitMove currentMove = moveQueue.Dequeue();

				//do all four directions
				List<UnitMove> validMoves = new List<UnitMove>();
				//TODO: populate validMoves

				foreach (UnitMove move in validMoves) {
					if (!visited.Contains(move)) {
						moveQueue.Enqueue(move);
					}
				}
			}

			List<UnitMove> visitedList = new List<UnitMove>();
			foreach (UnitMove move in visited) {
				visitedList.Add(move);
			}
			return visitedList;
		}
	}
}