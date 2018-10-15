using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public class playerAgent : Agent {

		private List<Coord> moveOptions;
		private List<Coord> highlightedEnemyUnits;
		private List<GameObject> otherHighlightedObjects;
		private Unit highlightedFriendlyUnit;

		public playerAgent() : base() {
			otherHighlightedObjects = new List<GameObject>();
		}

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
					unhighlightAll();
					highlight(selectedTile.gameObject);
					otherHighlightedObjects.Add(selectedTile.gameObject);

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
							highlight(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject);
						}

						this.highlightedEnemyUnits = selectedUnit.getTargets(tileCoords.x, tileCoords.y, battlefield, this.character);
						foreach (Coord targetableUnit in highlightedEnemyUnits) {
							highlight(battlefield.units[targetableUnit.x, targetableUnit.y], 2);
							highlight(battlefield.map[targetableUnit.x, targetableUnit.y].Peek().gameObject, 2);
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
					unhighlightAll();

				} else if (selectedItem == null) {
					//Clicked on empty space, deselect
					unhighlightAll();
					currentMove.from = null;

				} else if (selectedItem is Unit) {
					Unit selectedUnit = selectedItem as Unit;

					if (highlightedFriendlyUnit == selectedUnit) {
						//clicked on the same unit, deselect
						unhighlightAll();
						currentMove.from = null;

					} else if (selectedUnit.getCharacter(battlefield) == this.character) {
						//Clicked on a friendly unit. Deselect the current one.
						unhighlightAll();
						currentMove.from = null;

					} else {
						//Clicked on a hostile unit! fight!
						if (highlightedEnemyUnits.Any(coord => coord.x == tileCoords.x && coord.y == tileCoords.y)) {
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

			//Wait for the mouse down event to un-fire. This avoids an infinite loop in the next condition.
			while (!Input.GetButtonUp("Select")) {
				await Task.Delay(1);
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
			objectToHighlight.AddComponent<cakeslice.Outline>();
			objectToHighlight.GetComponent<cakeslice.Outline>().color = colorIndex;
		}

		private void unhighlight(GameObject objectToHighlight) {
			if (objectToHighlight != null) {
				GameObject.Destroy(objectToHighlight.GetComponent<cakeslice.Outline>());
			}
		}

		private void unhighlightAll() {
			if (moveOptions == null) {
				//Someone accidentally called this twice in a row
				return;
			}

			foreach (Coord moveOption in moveOptions) {
				unhighlight(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject);
			}

			foreach (Coord highlightedEnemyUnit in highlightedEnemyUnits) {
				unhighlight(battlefield.units[highlightedEnemyUnit.x, highlightedEnemyUnit.y].gameObject);
				unhighlight(battlefield.map[highlightedEnemyUnit.x, highlightedEnemyUnit.y].Peek().gameObject);
			}

			foreach (GameObject obj in otherHighlightedObjects) {
				unhighlight(obj);
			}

			moveOptions.Clear();
			highlightedEnemyUnits.Clear();
			otherHighlightedObjects.Clear();
		}

	}
}