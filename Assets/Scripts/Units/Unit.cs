using System.Collections.Generic;
using UnityEngine;
using Constants;
using Gameplay;
using System;
using AI;
using System.Linq;
using UnityEngine.UI;

namespace Units {
	public abstract class Unit : MonoBehaviour, IBattlefieldItem {
		private const int DEFAULT_DAMAGE = 10;
		private const int DEFAULT_MOVE = 4;
		private const int DEFAULT_HEALTH = 100;

		private readonly ArmorType armor;
		private readonly WeaponType weapon;
		private readonly MoveType moveType;
		private readonly UnitType unitType;
		private readonly int maxHealth;
		private int health;
		private bool hasMovedThisTurn;
		//How many tiles this unit can move per turn turn
		private int numMoveTiles { get; set; }

		[SerializeField]
		private Image healthBar;

		public Unit(ArmorType armorType, WeaponType weaponType, MoveType moveType, UnitType unitType) {
			armor = armorType;
			weapon = weaponType;
			this.moveType = moveType;
			this.unitType = unitType;

			maxHealth = DEFAULT_HEALTH;
			health = maxHealth;
			hasMovedThisTurn = false;
			this.numMoveTiles = unitType.unitMoveDistance();
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
		public void doBattleWith(Unit enemy, Tile enemyTile) {
			float damage = this.weapon.baseDamage * ( 1f * this.health / this.maxHealth);
			damage = damage * ((100 - this.weapon.damageType.DamageReduction(enemy.armor)) / 100.0f);
			damage = damage * ((100 - enemyTile.tileType.DefenseBonus()) / 100.0f);

			//Damage rounds up
			enemy.health -= (int)(Mathf.Ceil(damage));
			enemy.healthBar.fillAmount = 1f * this.health / this.maxHealth;
			Debug.Log("battle happened! " + damage + " damage dealt, leaving the target with " + enemy.health + " health.");
			
			if (enemy.health <= 0) {
				enemy.defeated();
			}
		}

		public void defeated() {
			Destroy(this.gameObject);
		}

		/*
		For now this will use a simple percolation algorithm using a visited set instead of a disjoint set approach
		We can get away with this because there's only one "flow" source point (the unit).
		 */
		public List<UnitMove> getValidMoves(int myX, int myY, Battlefield battlefield) {

			HashSet<UnitMove> visited = new HashSet<UnitMove>();
			PriorityQueue<AIUnitMove> movePQueue = new PriorityQueue<AIUnitMove>();
			movePQueue.Enqueue(new AIUnitMove(myX, myY, 0));
			while (movePQueue.Count() > 0) {
				AIUnitMove currentMove = movePQueue.Dequeue();
				//check all four directions
				int[,] moveDirs = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

				for (int x = 0; x < moveDirs.GetLength(0); x++) {
					int targetX = currentMove.x + moveDirs[x, 0];
					int targetY = currentMove.y + moveDirs[x, 1];
					if (targetX < battlefield.map.GetLength(0) && targetY < battlefield.map.GetLength(1) && targetX >= 0 && targetY >= 0) {
						Stack<Tile> targetTile = battlefield.map[targetX, targetY];

						int movePointsExpended = currentMove.movePoints + targetTile.Peek().tileType.Cost(this.moveType);
						UnitMove targetMove = new UnitMove(targetX, targetY);
						AIUnitMove targetMoveAI = new AIUnitMove(targetX, targetY, movePointsExpended);

						if (movePointsExpended <= unitType.unitMoveDistance()) {
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