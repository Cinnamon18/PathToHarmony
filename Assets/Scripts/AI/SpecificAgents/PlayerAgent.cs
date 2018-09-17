using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public class PlayerAgent : Agent {

		private GameObject highlightedObject;
		private List<Coord> moveOptions;
		private List<Coord> highlightedEnemyUnits;
		private Unit highlightedFriendlyUnit;

		public PlayerAgent(Battlefield battlefield, Level level, Action<UnityEngine.Object> Destroy) : base(battlefield, level, Destroy) { }

		public override async Task<Move> getMove() {
			Move currentMove = new Move();
			while (currentMove.from == null || currentMove.to == null) {
				currentMove = new Move();
				while (currentMove.from == null) {
					await getSelectionPhase(currentMove);
				}

				while (currentMove.to == null && currentMove.from != null) {
					await getMovePhase(currentMove);
				}
			}

			return currentMove;
		}


		public async Task getSelectionPhase(Move currentMove) {
			//Await player input. But. Unity doesn't really support async await.
			//I'm feeling kinda dumb for not just learning coroutines. Next time!
			while (!Input.GetButtonDown("Select")) {
				await Task.Delay(1);
			}

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0f)) {
				Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
				IBattlefieldItem selectedItem = battlefield.battlefieldItemAt(tileCoords.x, tileCoords.y, tileCoords.z);

				if (selectedItem is Tile) {
					//Selected a tile, show info abt that tile
					//TODO: Show info about tile if a tile is clicked
					Tile selectedTile = selectedItem as Tile;
					highlightSingleObject(selectedTile.gameObject);

					//This method will get called again because we didn't find a valid selection
				} else if (selectedItem is Unit) {
					Unit selectedUnit = selectedItem as Unit;

					if (selectedUnit.getCharacter(battlefield) == this.character && !selectedUnit.hasMovedThisTurn) {
						//Selected friendly unit. Valid selection, store selection coords.
						currentMove.from = new Coord(tileCoords.x, tileCoords.y);

						//show move options
						highlightSingleObject(selectedUnit.gameObject, 1);
						this.highlightedFriendlyUnit = selectedUnit;

						moveOptions = selectedUnit.getValidMoves(tileCoords.x, tileCoords.y, battlefield);
						foreach (Coord moveOption in moveOptions) {
							highlightMultipleObjects(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject);
						}

						this.highlightedEnemyUnits = selectedUnit.getTargets(tileCoords.x, tileCoords.y, battlefield, this.character);
						foreach (Coord targetableUnit in highlightedEnemyUnits) {
							highlightMultipleObjects(battlefield.units[targetableUnit.x, targetableUnit.y].gameObject, 2);
							highlightMultipleObjects(battlefield.map[targetableUnit.x, targetableUnit.y].Peek().gameObject, 2);
						}

					} else {
						//Selected enemy unit. Show unit and its move options.
						//TODO: highlight enemy's valid move tiles. don't assign currentMove.from.x or from.y, bc not valid advancement criteria
					}

				} else if (selectedItem == null) {
					//Clicked on empty space! Nbd, don't do anything.
					Debug.Log("Clicked on empty space");
				} else {
					Debug.LogWarning("Item of unrecognized type clicked on.");
				}
			}

			//Wait for the mouse down event to un-fire. This avoids an infinite loop in the next condition.
			while (!Input.GetButtonUp("Select")) {
				await Task.Delay(1);
			}

		}

		public async Task getMovePhase(Move currentMove) {
			//Await player input.
			while (!Input.GetButtonDown("Select")) {
				await Task.Delay(1);
			}

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
					deselectMoveOptions();

				} else if (selectedItem == null) {
					//Clicked on empty space, deselect
					deselectMoveOptions();
					currentMove.from = null;

				} else if (selectedItem is Unit) {
					Unit selectedUnit = selectedItem as Unit;

					if (highlightedFriendlyUnit == selectedUnit) {
						//clicked on the same unit, deselect
						deselectMoveOptions();
						currentMove.from = null;

					} else if (selectedUnit.getCharacter(battlefield) == this.character) {
						//Clicked on a friendly unit. Deselect the current one.
						deselectMoveOptions();
						currentMove.from = null;

					} else {
						//Clicked on a hostile unit! fight!
						if (highlightedEnemyUnits.Any(coord => coord.x == tileCoords.x && coord.y == tileCoords.y)) {
							currentMove.to = new Coord(tileCoords.x, tileCoords.y);
						} else {
							//Clicked on invalid enemy unit, restart.
							currentMove.from = null;
						}
						deselectMoveOptions();
					}
				} else {
					Debug.LogWarning("Item of unrecognized type clicked on.");
				}
			}

			//Wait for the mouse down event to un-fire. This avoids an infinite loop in the next condition.
			while (!Input.GetButtonUp("Select")) {
				await Task.Delay(1);
			}
		}

		private void highlightSingleObject(GameObject objectToHighlight, int colorIndex = 0) {
			//Deselect the old object
			if (highlightedObject != null) {
				unhighlightMultipleObjects(highlightedObject);
			}

			//Deselect the currently selected one one if we're clicking on it
			if (highlightedObject == objectToHighlight) {
				unhighlightMultipleObjects(highlightedObject);
				highlightedObject = null;
			} else {
				//Select the new one
				highlightedObject = objectToHighlight;
				highlightMultipleObjects(highlightedObject, colorIndex);
			}
		}

		private void highlightMultipleObjects(GameObject objectToHighlight, int colorIndex = 0) {
			objectToHighlight.AddComponent<cakeslice.Outline>();
			objectToHighlight.GetComponent<cakeslice.Outline>().color = colorIndex;
		}

		private void unhighlightMultipleObjects(GameObject objectToHighlight) {
			if (objectToHighlight != null) {
				Destroy(objectToHighlight.GetComponent<cakeslice.Outline>());
			}
		}

		private void deselectMoveOptions() {
			if (moveOptions == null) {
				//Someone accidentally called this twice in a row
				return;
			}

			foreach (Coord moveOption in moveOptions) {
				unhighlightMultipleObjects(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject);
			}

			foreach (Coord highlightedEnemyUnit in highlightedEnemyUnits) {
				unhighlightMultipleObjects(battlefield.units[highlightedEnemyUnit.x, highlightedEnemyUnit.y].gameObject);
				unhighlightMultipleObjects(battlefield.map[highlightedEnemyUnit.x, highlightedEnemyUnit.y].Peek().gameObject);

			}

			highlightSingleObject(highlightedObject);
			moveOptions = null;
			highlightedEnemyUnits = null;
			highlightedObject = null;
		}

	}
}