using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Constants;
using UnityEngine.UI;
using System;
using System.Linq;

namespace Gameplay {
	public class BattleControl : MonoBehaviour {

		//x, y, height (from the bottom)
		private Battlefield battlefield;
		private Level level;
		[SerializeField]
		private GameObject[] tilePrefabs;
		[SerializeField]
		private GameObject[] unitPrefabs;

		private GameObject highlightedObject;
		private BattleLoopStage battleStage;
		private List<UnitMove> moveOptions;
		private Unit highlightedUnit;

		private int currentPlayer;
		[SerializeField]
		private Text turnPlayerText;

		// Use this for initialization
		void Start() {
			//Just for testing because we don't have any way to set the campaign yet:
			Character[] characters = new[] { new Character("Alice"), new Character("Just some guy") };
			Vector2Int[] aliceMoves = new[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0) };
			Vector2Int[] evilGuyMoves = new[] { new Vector2Int(3, 7), new Vector2Int(7, 4) };
			Vector2Int[][] validPickTiles = new[] { aliceMoves, evilGuyMoves };
			Level level = new Level("DemoMap", characters, new[] { 1000, 1000 }, validPickTiles);
			Campaign testCampaign = new Campaign("test", 0, new[] { level });
			Persistance.campaign = testCampaign;


			battlefield = new Battlefield();
			currentPlayer = -1;
			battleStage = BattleLoopStage.Initial;
			turnPlayerText.enabled = false;
			getLevel();
			deserializeMap();
		}

		// Update is called once per frame
		void Update() {
			switch (battleStage) {
				case BattleLoopStage.Initial:
					advancePhase();
					break;
				case BattleLoopStage.Pick:
					//This is temp just for testing until I build pick phase.
					addUnit(UnitType.Knight, level.players[0], 0, 0);
					addUnit(UnitType.Knight, level.players[0], 1, 0);
					addUnit(UnitType.Knight, level.players[0], 0, 1);
					addUnit(UnitType.Knight, level.players[1], 3, 7);
					addUnit(UnitType.Knight, level.players[1], 4, 7);

					advancePhase();
					break;
				case BattleLoopStage.BattleLoopStart:
					advancePhase();
					break;
				case BattleLoopStage.TurnChange:
					//There's probably a less fragile way of doing this. It's just to make sure this call only happens once per turn loop.
					if (!turnPlayerText.enabled) {
						currentPlayer = (currentPlayer + 1) % level.players.Length;
						turnPlayerText.text = level.players[currentPlayer].name + "'s turn";
						turnPlayerText.enabled = true;
						Util.setTimeout(advancePhase, 1000);
					}
					break;
				case BattleLoopStage.TurnChangeEnd:
					turnPlayerText.enabled = false;
					advancePhase();
					break;
				case BattleLoopStage.UnitSelection:
					//Player input. LMB
					if (Input.GetButtonDown("Select")) {
						RaycastHit hit;
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						if (Physics.Raycast(ray, out hit, 1000.0f)) {
							Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
							IBattlefieldItem selectedItem = battlefield.battlefieldItemAt(tileCoords.x, tileCoords.y, tileCoords.z);
							//TODO: Show info about tile if a tile is clicked
							if (selectedItem is Tile) {
								Tile selectedTile = selectedItem as Tile;
								highlightSingleObject(selectedTile.gameObject);
								advancePhase();
							} else if (selectedItem is Unit) {
								Unit selectedUnit = selectedItem as Unit;
								if (selectedUnit.getCharacter(battlefield) == level.players[currentPlayer]) {
									highlightSingleObject(selectedUnit.gameObject, 1);
									this.highlightedUnit = selectedUnit;

									List<UnitMove> validMoves = selectedUnit.getValidMoves(tileCoords.x, tileCoords.y, battlefield);
									this.moveOptions = validMoves;
									foreach (UnitMove moveOptions in moveOptions) {
										highlightMultipleObjects(battlefield.map[moveOptions.x, moveOptions.y].Peek().gameObject);
									}

									advancePhase();
								} else {
									//TODO: highlight enemy's valid move tiles.
								}
							} else if (selectedItem == null) {
								//Clicked on empty space! Nbd, don't do anything.
								Debug.Log("Clicked on empty space");
							} else {
								Debug.LogWarning("Item of unrecognized type clicked on.");
							}
						}
					}


					//If player has selected a move:
					//Play the animation
					break;
				case BattleLoopStage.ActionSelection:
					if (Input.GetButtonDown("Select")) {
						RaycastHit hit;
						Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
						if (Physics.Raycast(ray, out hit, 1000.0f)) {
							Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
							IBattlefieldItem selectedItem = battlefield.battlefieldItemAt(tileCoords.x, tileCoords.y, tileCoords.z);
							if (selectedItem is Tile) {
								//Check that it's a valid tile. .Contains() uses refrence :(
								if (moveOptions.Any(move => (move.x == tileCoords.x && move.y == tileCoords.y))) {
									moveUnit(highlightedUnit, tileCoords);
									deselectMoveOptions();
									advancePhase();
								}
							} else if (highlightedUnit == selectedItem || selectedItem == null) {
								deselectMoveOptions();
							} else if (selectedItem is Unit) {
								Unit selectedUnit = selectedItem as Unit;
								if (selectedUnit.getCharacter(battlefield) == level.players[currentPlayer]) {
									deselectMoveOptions();
								} else {

									moveUnit(highlightedUnit, tileCoords + new Vector3Int(1, 0, 0));

									highlightedUnit.doBattleWith(selectedUnit, battlefield.map[tileCoords.x, tileCoords.y].Peek());
									//If we didnt' defeat the enemy unit in battle, they get a counter attack
									if (selectedUnit != null) {
										Vector3Int unitCoords = battlefield.getUnitCoords(highlightedUnit);
										selectedUnit.doBattleWith(highlightedUnit, battlefield.map[unitCoords.x, unitCoords.y].Peek());
									}
									deselectMoveOptions();
									advancePhase();
								}
							} else {
								Debug.LogWarning("Item of unrecognized type clicked on.");
							}
						}
					}

					break;
				case BattleLoopStage.EndTurn:
					advancePhase();
					break;
			}
		}

		private void moveUnit(Unit unit, Vector3Int target) {
			Vector3Int unitCoords = battlefield.getUnitCoords(unit);
			battlefield.units[target.x, target.y] = unit;
			battlefield.units[unitCoords.x, unitCoords.y] = null;
			unit.gameObject.transform.position = Util.GridToWorld(target + new Vector3Int(0, 0, 1));
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
			// Material[] materials = objectToHighlight.GetComponent<Renderer>().materials;
			// foreach (Material material in materials) {
			//Set the main Color of the Material to green
			// material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
			// material.SetColor("_Color", Color.green);

			// }
		}

		private void unhighlightMultipleObjects(GameObject objectToHighlight) {
			Destroy(objectToHighlight.GetComponent<cakeslice.Outline>());
			// Material[] materials = objectToHighlight.GetComponent<Renderer>().materials;
			// foreach (Material material in materials) {
			//Set the main Color of the Material to green
			// material.shader = Shader.Find("Standard");
			// material.SetColor("_Color", Color.blue);
			// }
		}

		//Convenience
		private void advancePhase() {
			battleStage = battleStage.NextPhase();
		}

		private void deserializeMap() {
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(level.mapFileName), tilePrefabs);
			battlefield.units = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1)];
		}

		private void getLevel() {
			level = Persistance.campaign.levels[Persistance.campaign.levelIndex];
		}

		private void deselectMoveOptions() {
			foreach (UnitMove moveOption in moveOptions) {
				unhighlightMultipleObjects(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject);
			}

			highlightSingleObject(highlightedObject);
			moveOptions = null;
			highlightedObject = null;
		}

		private void addUnit(UnitType unitType, Character character, int x, int y) {
			int index = (int)(unitType);
			GameObject newUnitGO = Instantiate(
				unitPrefabs[index],
				Util.GridToWorld(x, y, battlefield.map[x, y].Count + 1),
				unitPrefabs[index].gameObject.transform.rotation);

			Unit newUnit = newUnitGO.GetComponent<Unit>();
			battlefield.addUnit(newUnit, character, x, y);
		}

	}
}