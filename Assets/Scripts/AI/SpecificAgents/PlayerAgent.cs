using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public class PlayerAgent : Agent {

		private List<Coord> moveOptions;
		private List<Coord> targetableUnits;
		private List<GameObject> otherHighlightedObjects;
		private Unit highlightedFriendlyUnit;

        public Move currentMove;


        private const int INPUT_LOOP_DELAY = 10;

    

		public bool stopAwaiting = false;

        public PlayerAgent() : base() {
			otherHighlightedObjects = new List<GameObject>();
		}

		//Either returns a valid move or returns null if no move should be made.
		public override async Task<Move> getMove() {
			//Note: stopAwaiting will cause this entire phase to end.
			currentMove = new Move();
			while (!stopAwaiting && currentMove.from == null || currentMove.to == null) {
				currentMove = new Move();

				while (!stopAwaiting && currentMove.from == null) {
					//Await player input. But. Unity doesn't really support async await.
					//I'm feeling kinda dumb for not just learning coroutines. Next time!
					while (!stopAwaiting && !Input.GetButtonDown("Select")) {
						await Task.Delay(INPUT_LOOP_DELAY);
					}
					if (stopAwaiting) {
						stopAwaiting = false;
						return null;
					}
					getSelectionPhase(currentMove);

					//Wait for the mouse down event to un-fire. This avoids an infinite loop in the next condition.
					while (Input.GetButtonDown("Select")) {
						await Task.Delay(INPUT_LOOP_DELAY);
					}
				}

				if (stopAwaiting) {
					stopAwaiting = false;
					return null;
				}

				//Part 2
				while (!stopAwaiting && currentMove.to == null && currentMove.from != null) {
					//Await player input.
					while (!stopAwaiting && !Input.GetButtonDown("Select")) {
						await Task.Delay(INPUT_LOOP_DELAY);
					}
					if (stopAwaiting) {
						stopAwaiting = false;
						return null;
					}
					getMovePhase(currentMove);
					//Wait for the mouse down event to un-fire. This avoids an infinite loop in the next condition.
					while (Input.GetButtonDown("Select")) {
						await Task.Delay(INPUT_LOOP_DELAY);
					}
				}
			}
			if (stopAwaiting) {
				stopAwaiting = false;
				return null;
			}
			return currentMove;
		}

		public void getSelectionPhase(Move currentMove) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0f)) {
				Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
				IBattlefieldItem selectedItem = battlefield.battlefieldItemAt(tileCoords.x, tileCoords.y, tileCoords.z);

				if (selectedItem is Tile) {
					//Selected a tile, show info abt that tile
					//TODO: Show info about tile if a tile is clicked
					Tile selectedTile = selectedItem as Tile;
					unhighlightAll();
					highlight(selectedTile.gameObject);

					//This method will get called again because we didn't find a valid selection
				} else if (selectedItem is Unit) {
					Unit selectedUnit = selectedItem as Unit;

					if (selectedUnit.getCharacter(battlefield) == this.character &&
						(!selectedUnit.hasMovedThisTurn || (!selectedUnit.getHasAttackedThisTurn() &&
						selectedUnit.getTargets(tileCoords.x, tileCoords.y, battlefield, character).Count != 0))) {

						//Selected friendly unit. Valid selection, store selection coords.
						currentMove.from = new Coord(tileCoords.x, tileCoords.y);

						//show move options
						unhighlightAll();
						highlight(selectedUnit, 1);

						this.highlightedFriendlyUnit = selectedUnit;

						moveOptions = selectedUnit.getValidMoves(tileCoords.x, tileCoords.y, battlefield);

						foreach (Coord moveOption in moveOptions) {
							highlight(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject, 1);
						}

						this.targetableUnits = selectedUnit.getTargets(tileCoords.x, tileCoords.y, battlefield, this.character);

						foreach (Coord targetableUnit in targetableUnits) {
							int colorIndex = selectedUnit is Cleric ? 3 : 2; 
							highlight(battlefield.units[targetableUnit.x, targetableUnit.y], colorIndex);
							highlight(battlefield.map[targetableUnit.x, targetableUnit.y].Peek().gameObject, colorIndex);
						}

					} else {
                        //Selected enemy unit. Show unit and its move options.
						unhighlightAll();
						highlight(selectedUnit, 2);

						moveOptions = selectedUnit.getValidMoves(tileCoords.x, tileCoords.y, battlefield);

						foreach (Coord moveOption in moveOptions) {
							highlight(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject, 2);
						}

						this.targetableUnits = selectedUnit.getTargets(tileCoords.x, tileCoords.y, battlefield, selectedUnit.getCharacter(battlefield));

						foreach (Coord targetableUnit in targetableUnits) {
							int colorIndex = selectedUnit is Cleric ? 3 : 2; 
							highlight(battlefield.units[targetableUnit.x, targetableUnit.y], colorIndex);
							highlight(battlefield.map[targetableUnit.x, targetableUnit.y].Peek().gameObject, colorIndex);
						}
                    }

				} else if (selectedItem == null) {
					//Clicked on empty space! Nbd, don't do anything.
					Debug.Log("Clicked on empty space");
				} else {
					Debug.LogWarning("Item of unrecognized type clicked on.");
				}
			}

		}

		public void getMovePhase(Move currentMove) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0f)) {
				Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
				IBattlefieldItem selectedItem = battlefield.battlefieldItemAt(tileCoords.x, tileCoords.y, tileCoords.z);
				if (selectedItem is Tile) {
					//We selected a tile! lets move to it
					if (moveOptions.Any(move => (move.x == tileCoords.x && move.y == tileCoords.y))) {
						currentMove.to = new Coord(tileCoords.x, tileCoords.y);
					} else {
						//Clicked on invalid tile, restart.
						currentMove.from = null;
					}
					unhighlightAll();

				} else if (selectedItem == null) {
					//Clicked on empty space, deselect
					unhighlightAll();
					currentMove.from = null;

				} else if (selectedItem is Unit) {
					Unit selectedUnit = selectedItem as Unit;

					if (highlightedFriendlyUnit == selectedUnit) {
						//clicked on the same unit, return the "do nothing" move
						unhighlightAll();
						currentMove.to = new Coord(tileCoords.x, tileCoords.y);

					} else if (selectedUnit.getCharacter(battlefield) == this.character && !(highlightedFriendlyUnit is Cleric)) {
						//Clicked on a friendly unit. Deselect the current one.
						unhighlightAll();
						currentMove.from = null;

					} else {
						//Clicked on a hostile unit! fight!
						if (targetableUnits.Any(coord => coord.x == tileCoords.x && coord.y == tileCoords.y)) {
							currentMove.to = new Coord(tileCoords.x, tileCoords.y);
						} else {
							//Clicked on invalid enemy unit, restart.
							currentMove.from = null;
						}
						unhighlightAll();
					}
				} else {
					Debug.LogWarning("Item of unrecognized type clicked on.");
				}
			}
		}

		private void highlight(Unit unit, int colorIndex = 0) {
			foreach (GameObject obj in unit.getModels()) {
				highlight(obj, colorIndex);
				otherHighlightedObjects.Add(obj);
			}
		}

		private void highlight(List<GameObject> objList, int colorIndex = 0) {
			foreach (GameObject obj in objList) {
				highlight(obj, colorIndex);
			}
		}

		private void highlight(GameObject objectToHighlight, int colorIndex = 0) {
			objectToHighlight.AddComponent<cakeslice.Outline>().color = colorIndex;
			otherHighlightedObjects.Add(objectToHighlight);
		}

		private void unhighlight(GameObject objectToHighlight) {
			if (objectToHighlight != null) {
				//mmm defensive programming
				cakeslice.Outline[] outlines = objectToHighlight.GetComponents<cakeslice.Outline>();
				foreach(cakeslice.Outline outline in outlines) {
					GameObject.Destroy(outline);
				}
			}
		}

		public void unhighlightAll() {
			if (moveOptions == null) {
				//Someone accidentally called this twice in a row
				return;
			}

			foreach (Coord moveOption in moveOptions) {
				unhighlight(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject);
			}

			foreach (Coord highlightedEnemyUnit in targetableUnits) {
				unhighlight(battlefield.units[highlightedEnemyUnit.x, highlightedEnemyUnit.y].gameObject);
				unhighlight(battlefield.map[highlightedEnemyUnit.x, highlightedEnemyUnit.y].Peek().gameObject);
			}

			foreach (GameObject obj in otherHighlightedObjects) {
				unhighlight(obj);
			}

			moveOptions.Clear();
			targetableUnits.Clear();
			otherHighlightedObjects.Clear();
		}

	}
}