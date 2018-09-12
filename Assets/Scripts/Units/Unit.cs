using System.Collections.Generic;
using UnityEngine;
using Constants;
using Gameplay;
using System;
using AI;
using System.Linq;
using UnityEngine.UI;
using Buffs;

namespace Units {
	public abstract class Unit : MonoBehaviour, IBattlefieldItem {

		public readonly ArmorType armor;
		private readonly MoveType moveType;
		public readonly int maxHealth;
		public int health;
		public List<Buff> buffs;
		public bool hasMovedThisTurn;

		private int numMoveTiles { get; set; }

		[SerializeField]
		public Image healthBar;

		public Unit(ArmorType armorType, int maxHealth, MoveType moveType, int moveDistance) {
			armor = armorType;
			this.moveType = moveType;
			buffs = new List<Buff>();

			this.maxHealth = maxHealth;
			this.health = maxHealth;
			hasMovedThisTurn = false;
			this.numMoveTiles = moveDistance;
		}

		void Start() {

		}
		void Update() {

		}

		public Character getCharacter(Battlefield battlefield) {
			Character myCharacter = null;
			foreach (Character character in battlefield.charactersUnits.Keys) {
				if (battlefield.charactersUnits[character].Contains(this)) {
					myCharacter = character;
				}
			}
			return myCharacter;
		}

		//returns true if the enemy was destroyed by battle
		public abstract bool doBattleWith(Unit enemy, Tile enemyTile, Battlefield battlefield);

		//returns a list of targetable units
		public abstract List<Unit> getTargets(int myX, int myY, Battlefield battlefield, Character character);

		public void defeated(Battlefield battlefield) {
			Destroy(this.gameObject);
			battlefield.charactersUnits[this.getCharacter(battlefield)].Remove(this);
		}

		
		//For now this will use a simple percolation algorithm using a visited set instead of a disjoint set approach
		//We can get away with this because there's only one "flow" source point (the unit).
		public List<Coord> getValidMoves(int myX, int myY, Battlefield battlefield) {

			HashSet<Coord> visited = new HashSet<Coord>();
			PriorityQueue<AIMove> movePQueue = new PriorityQueue<AIMove>();
			movePQueue.Enqueue(new AIMove(myX, myY, 0));
			while (movePQueue.Count() > 0) {
				AIMove currentMove = movePQueue.Dequeue();
				//check all four directions
				int[,] moveDirs = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

				for (int x = 0; x < moveDirs.GetLength(0); x++) {
					int targetX = currentMove.x + moveDirs[x, 0];
					int targetY = currentMove.y + moveDirs[x, 1];
					if (targetX < battlefield.map.GetLength(0) && targetY < battlefield.map.GetLength(1) && targetX >= 0 && targetY >= 0) {
						Stack<Tile> targetTile = battlefield.map[targetX, targetY];

						int movePointsExpended = currentMove.weight + targetTile.Peek().tileType.Cost(this.moveType);
						Coord targetMove = new Coord(targetX, targetY);
						AIMove targetMoveAI = new AIMove(targetX, targetY, movePointsExpended);

						if (movePointsExpended <= this.numMoveTiles) {
							if (!visited.Contains(targetMove)) {
								visited.Add(targetMove);
								movePQueue.Enqueue(targetMoveAI);
							}
						}
					}
				}
			}

			return visited.ToList();
		}
	}
}