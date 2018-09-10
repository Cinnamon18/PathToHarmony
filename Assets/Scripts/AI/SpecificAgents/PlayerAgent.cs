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
		private List<Unit> highlightedEnemyUnits;
		private Unit highlightedFriendlyUnit;

		public PlayerAgent(Battlefield battlefield, Level level, Action<UnityEngine.Object> Destroy) : base(battlefield, level, Destroy) { }

		public override async Task<Move> getMove() {
			Move currentMove = new Move(-1, -1, -1, -1);
			while (!currentMove.fromDefined() || !currentMove.toDefined()) {
				while (!currentMove.fromDefined()) {
					currentMove = await getSelectionPhase(currentMove);
				}

				//Wait for the mouse down event to un-fire. This avoid an infinite loop in the next condition.
				while (Input.GetButtonDown("Select")) {
					await Task.Delay(10);
				}

				while (!currentMove.toDefined()) {
					currentMove = await getMovePhase(currentMove);
				}

			}

			return currentMove;
		}


		public async Task<Move> getSelectionPhase(Move currentMove) {
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
						currentMove.fromX = tileCoords.x;
						currentMove.fromY = tileCoords.y;

						//show move options
						highlightSingleObject(selectedUnit.gameObject, 1);
						this.highlightedFriendlyUnit = selectedUnit;

						moveOptions = selectedUnit.getValidMoves(tileCoords.x, tileCoords.y, battlefield);
						foreach (Coord moveOption in moveOptions) {
							highlightMultipleObjects(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject);
						}

						this.highlightedEnemyUnits = selectedUnit.getTargets(tileCoords.x, tileCoords.y, battlefield, this.character);
						foreach (Unit targetableUnit in highlightedEnemyUnits) {
							highlightMultipleObjects(targetableUnit.gameObject, 2);
						}

					} else {
						//Selected enemy unit. Show unit and its move options.
						//TODO: highlight enemy's valid move tiles. don't assign currentMove.fromX or fromY, bc not valid advancement criteria
					}

				} else if (selectedItem == null) {
					//Clicked on empty space! Nbd, don't do anything.
					Debug.Log("Clicked on empty space");

				} else {
					Debug.LogWarning("Item of unrecognized type clicked on.");
				}
			}

			return currentMove;
		}

		public async Task<Move> getMovePhase(Move currentMove) {
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

						currentMove.toX = tileCoords.x;
						currentMove.toY = tileCoords.y;

						deselectMoveOptions();
					}

				} else if (selectedItem == null) {
					//Clicked on empty space, deselect
					deselectMoveOptions();
					currentMove = goBackToSelectionPhase();

				} else if (selectedItem is Unit) {
					Unit selectedUnit = selectedItem as Unit;

					if (highlightedFriendlyUnit == selectedUnit) {
						//clicked on the same unit, deselect
						deselectMoveOptions();
						currentMove = goBackToSelectionPhase();

					} else if (selectedUnit.getCharacter(battlefield) == this.character) {
						//Clicked on a friendly unit. Deselect the current one.
						deselectMoveOptions();
						currentMove = goBackToSelectionPhase();

					} else {
						//Clicked on a hostile unit! fight!
						deselectMoveOptions();
						currentMove.toX = tileCoords.x;
						currentMove.toY = tileCoords.y;
					}
				} else {
					Debug.LogWarning("Item of unrecognized type clicked on.");
				}
			}

			return currentMove;
		}


		//Bluhhh so ik this is a bad way of doing this, but I don't want to be recursing.
		//I'm not that afraid of overflows or anything, it just seems like a real smell for something that should be iterative.
		private Move goBackToSelectionPhase() {
			return new Move(-1, -1, -1, -1);
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

			foreach (Unit highlightedEnemyUnit in highlightedEnemyUnits) {
				unhighlightMultipleObjects(highlightedEnemyUnit.gameObject);
			}

			highlightSingleObject(highlightedObject);
			moveOptions = null;
			highlightedEnemyUnits = null;
			highlightedObject = null;
		}

	}
}