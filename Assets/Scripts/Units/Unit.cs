using System.Collections.Generic;
using UnityEngine;
using Constants;
using Gameplay;
using System;
using AI;
using System.Linq;
using UnityEngine.UI;
using Buffs;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

namespace Units {
	public abstract class Unit : MonoBehaviour, IBattlefieldItem {

		private const int minIdleTime = 15 * 1000;
		private const int maxIdleTime = 60 * 1000;
		private const int idleDelay = 100;

		public readonly ArmorType armor;
		private readonly MoveType moveType;
		public readonly int maxHealth;
		private Faction faction;
		private int health;
		private List<Buff> buffs;
		public bool hasMovedThisTurn;
		private bool hasAttackedThisTurn;

		private int numMoveTiles { get; set; }

		[SerializeField]
		public Material[] factionMaterials;
		[SerializeField]
		public BuffUIManager buffUIManager;
		[SerializeField]
		public UnitHealthUIManager healthUIManager;
		public string attackSoundEffect;

		public Unit(ArmorType armorType, int maxHealth, MoveType moveType, int moveDistance, Faction faction) {
			armor = armorType;
			this.moveType = moveType;
			this.faction = faction;
			buffs = new List<Buff>();

			this.maxHealth = maxHealth;
			this.health = maxHealth;
			hasMovedThisTurn = false;
			hasAttackedThisTurn = false;
			this.numMoveTiles = moveDistance;
		}

		void Start() {
			// startIdleAnimation();
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

		//return damage that would result from a battle without inflicting it. Useful for AI
		public abstract int battleDamage(Unit enemy, Tile enemyTile);

		//returns true if the enemy was destroyed by battle
		public abstract Task<bool> doBattleWith(Unit enemy, Tile enemyTile, Battlefield battlefield);

		//Added for use by AI
		public abstract List<Coord> getAttackZone(int myX, int myY, Battlefield battlefield, Character character);


		public abstract HashSet<Coord> getTotalAttackZone(int myX, int myY, Battlefield battlefield, Character character);

		//returns a list of targetable units
		public virtual List<Coord> getTargets(int myX, int myY, Battlefield battlefield, Character character) {

			List<Coord> targets = new List<Coord>();
			List<Coord> tiles = getAttackZone(myX, myY, battlefield, character);

			foreach (Coord tile in tiles) {
				Unit targetUnit = battlefield.units[tile.x, tile.y];
				if (targetUnit != null && targetUnit.getCharacter(battlefield) != character) {
					targets.Add(tile);
				}
			}
			return targets;
		}

		public void defeated(Battlefield battlefield) {
			Destroy(this.gameObject);
			battlefield.charactersUnits[this.getCharacter(battlefield)].Remove(this);
		}


		//For now this will use a simple approach using a visited set instead of a disjoint set approach
		//We can get away with this because there's only one "flow" source point (the unit).
		public List<Coord> getValidMoves(int myX, int myY, Battlefield battlefield) {
			if (hasMovedThisTurn) {
				return new List<Coord>();
			}

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
						if (targetTile.Count != 0) {
							float movePointsExpended = currentMove.weight + targetTile.Peek().tileType.Cost(this.moveType);
							Coord targetMove = new Coord(targetX, targetY);
							AIMove targetMoveAI = new AIMove(targetX, targetY, movePointsExpended);

							if (movePointsExpended <= this.numMoveTiles && !visited.Contains(targetMove)) {
								//If it's empty, we can move to it and on it
								if (battlefield.units[targetX, targetY] == null) {
									visited.Add(targetMove);
									movePQueue.Enqueue(targetMoveAI);
								} else if (battlefield.units[targetX, targetY].getCharacter(battlefield) == this.getCharacter(battlefield)) {
									//If it's our unit, we can move through it, but not on it
									movePQueue.Enqueue(targetMoveAI);
								} else {
									//If it's a hostile unit, we can't move to or through it.
								}
							}
						}

					}
				}
			}

			return visited.ToList();
		}

		public void greyOut() {
			foreach (GameObject model in this.getModels()) {
				if (model != null) {
					model.GetComponent<Renderer>().material.shader = Shader.Find("Grayscale");
				} else {
					Debug.Log("Unit destroyed.  Cannot apply shader");
				}

			}
		}

		public void unGreyOut() {
			foreach (GameObject model in this.getModels()) {
				model.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
			}
		}

		public void setFaction(Faction faction) {
			this.faction = faction;
			this.healthUIManager.setMaterial(factionMaterials[(int)(this.faction)], health);
		}

		public async Task setHealth(int newHealth, bool playAnimation = false) {
			int oldHealth = this.health;
			this.health = Mathf.Clamp(newHealth, 0, maxHealth);
			await healthUIManager.setHealth(newHealth, oldHealth, playAnimation);
		}

		public async Task changeHealth(int change, bool playAnimation = false) {
			this.health += change;
			this.health = Mathf.Clamp(health, 0, maxHealth);
			await healthUIManager.setHealth(health, health - change, playAnimation);
		}

		public int getHealth() {
			return this.health;
		}

		public void setHasAttackedThisTurn(bool hasAttackedThisTurn) {
			this.hasAttackedThisTurn = hasAttackedThisTurn;
			if (hasAttackedThisTurn) {
				greyOut();
			} else {
				unGreyOut();
			}
		}

		public bool getHasAttackedThisTurn() {
			return hasAttackedThisTurn;
		}

		public List<GameObject> getModels() {
			return this.healthUIManager.getModels();
		}

		public List<Animator> getAnimators() {
			return this.healthUIManager.getAnimators();
		}

		public void addBuff(Buff buff) {
			buffs.Add(buff);
			buffUIManager.addBuff(buff);
		}

		public void removeBuff(Buff buff) {
			buffs.Remove(buff);
			buffUIManager.removeBuff(buff);
		}

		//Removes all buffs of given type
		public void removeBuff(BuffType buffType) {
			List<Buff> removeBuffs = buffs.FindAll(buff => buff.buffType == buffType);
			foreach (Buff buff in removeBuffs) {
				removeBuff(buff);
			}
		}

		//Technically these two are different, as different classes could (but should not) have different enum types.
		public List<Buff> getBuffsOfType(BuffType buffType) {
			return getBuffs(buff => buff.buffType == buffType);
		}

		public List<Buff> getBuffsOfClass(Buff otherBuff) {
			return getBuffs(myBuff => myBuff.GetType() == otherBuff.GetType());
		}

		public List<Buff> getBuffs(Predicate<Buff> predicate) {
			return buffs.FindAll(predicate);
		}

		public async Task playAttackAnimation() {
			List<Animator> animators = healthUIManager.getAnimators();

			foreach (Animator animator in animators) {
				animator.SetTrigger("Attack");
			}

			float animLenght = animators[0].GetCurrentAnimatorStateInfo(0).length;
			//Play the sound effect halfway through. this is closer to the "hit" portion of the animation.
			await Task.Delay((int)(animLenght * 500));
			Audio.playSfx(attackSoundEffect);
			await Task.Delay((int)(animLenght * 500));
		}

		public async void startIdleAnimation() {
			while (true) {
				await Task.Delay(Random.Range(minIdleTime, maxIdleTime));

				//Re get these every time b/c they might be destroyed
				try {
					List<Animator> animators = healthUIManager.getAnimators();
					foreach (Animator animator in animators) {
						animator.SetTrigger("StartIdle");
						await Task.Delay(idleDelay);
					}
				} catch (MissingReferenceException e) {
					//This prevents annoying errors when run from the editor.
					Debug.LogWarning("Editor quit with idle animation thread still running");
				}
			}
		}
	}
}