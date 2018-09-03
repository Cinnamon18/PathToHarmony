using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Constants;
using UnityEngine.UI;
using System;
using System.Linq;
using Cutscenes;
using System.Threading.Tasks;
using TMPro;

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
		private List<Coord> moveOptions;
		private List<Unit> highlightedEnemyUnits;
		private Unit highlightedFriendlyUnit;

		private int currentCharacter;
		private int playerCharacter;

		//Just a refrence to the cutscene prefab
		[SerializeField]
		private Cutscene cutscene;
		[SerializeField]
		private TextMeshProUGUI turnPlayerText;
		[SerializeField]
		private Image turnChangeBackground;
		[SerializeField]
		private Image victoryImage;
		[SerializeField]
		private Image defeatImage;

		// Use this for initialization
		void Start() {
			//Just for testing because we don't have any way to set the campaign yet:
			Character[] characters = new[] { new Character("Alice"), new Character("The evil lord zxqv") };
			Vector2Int[] aliceMoves = new[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0) };
			Vector2Int[] evilGuyMoves = new[] { new Vector2Int(3, 7), new Vector2Int(7, 4) };
			Vector2Int[][] validPickTiles = new[] { aliceMoves, evilGuyMoves };
			Level level = new Level("DemoMap", characters, new[] { 1000, 1000 }, validPickTiles);
			Campaign testCampaign = new Campaign("test", 0, new[] { level });
			Persistance.campaign = testCampaign;

			//This will be encoded in the campaign
			CutsceneCharacter blair = CutsceneCharacter.blair;
			CutsceneCharacter juniper = CutsceneCharacter.juniper;
			CutsceneScript script = new CutsceneScript(new List<CutsceneScriptLine> {
				new CutsceneScriptLine(CutsceneAction.SetBackground, background: CutsceneBackground.Academy),
				new CutsceneScriptLine(CutsceneAction.SetCharacter, character: blair, side: Side.Left),
				new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "My name is <r>Blair</r>!"),
				new CutsceneScriptLine(CutsceneAction.SetCharacter, character: juniper, side: Side.Right),
				new CutsceneScriptLine(CutsceneAction.SayDialogue, character: juniper, dialogue: "and I'm <w>Juniper</w>."),
				new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "There's a <color=red>third</color> major character, <w><r>Bruno</r></w>. He would've been here, but he got tied up with paperwork"),
				new CutsceneScriptLine(CutsceneAction.SayDialogue, character: juniper, dialogue: "Which is to say we ran out of budget"),
				new CutsceneScriptLine(CutsceneAction.SayDialogue, character: juniper, dialogue: "Anyways, I hope you enjoy this slick as h*ck demo"),
				new CutsceneScriptLine(CutsceneAction.TransitionOut, side: Side.Right),
				new CutsceneScriptLine(CutsceneAction.TransitionOut, side: Side.Left)
			});
			cutscene.setup(new CutsceneCharacter[] { blair, juniper }, script);

			//Actual constructor code. This should still be here after the demo :p
			playerCharacter = 0;
			battlefield = new Battlefield();
			currentCharacter = -1;
			battleStage = BattleLoopStage.Initial;

			turnPlayerText.enabled = false;
			turnChangeBackground.enabled = false;
			victoryImage.enabled = false;
			defeatImage.enabled = false;
			getLevel();
			deserializeMap();
		}

		// Update is called once per frame
		async void Update() {
			switch (battleStage) {
				case BattleLoopStage.Initial:
					if (!cutscene.inProgress) {
						advanceBattleStage();
					}
					break;
				case BattleLoopStage.Pick:
					//TODO This is temp just for testing until pick phase gets built. 
					addUnit(UnitType.Knight, level.characters[0], 0, 0);
					addUnit(UnitType.Knight, level.characters[0], 1, 0);
					addUnit(UnitType.Knight, level.characters[0], 0, 1);
					addUnit(UnitType.Knight, level.characters[1], 3, 7);
					addUnit(UnitType.Knight, level.characters[1], 4, 7);
					foreach (Unit unit in battlefield.charactersUnits[level.characters[1]]) {
						Renderer rend = unit.gameObject.GetComponent<Renderer>();
						rend.material.shader = Shader.Find("_Color");
						rend.material.SetColor("_Color", Color.green);
						rend.material.shader = Shader.Find("Specular");
						rend.material.SetColor("_SpecColor", Color.green);
					}

					advanceBattleStage();
					break;
				case BattleLoopStage.BattleLoopStart:
					advanceBattleStage();
					break;
				case BattleLoopStage.TurnChange:
					//There's probably a less fragile way of doing this. It's just to make sure this call only happens once per turn loop.
					if (!turnPlayerText.enabled) {
						currentCharacter = (currentCharacter + 1) % level.characters.Length;
						turnPlayerText.text = level.characters[currentCharacter].name + "'s turn";
						turnPlayerText.enabled = true;
						turnChangeBackground.enabled = true;
						Util.setTimeout(advanceBattleStage, 1000);
					}
					break;
				case BattleLoopStage.TurnChangeEnd:
					turnPlayerText.enabled = false;
					turnChangeBackground.enabled = false;
					advanceBattleStage();
					break;
				case BattleLoopStage.UnitSelection:
					//Player input. LMB
					if (Input.GetButtonDown("Select")) {
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
							} else if (selectedItem is Unit) {
								Unit selectedUnit = selectedItem as Unit;
								if (selectedUnit.getCharacter(battlefield) == level.characters[currentCharacter] && !selectedUnit.hasMovedThisTurn) {
									//Selected friendly unit. show move options.
									highlightSingleObject(selectedUnit.gameObject, 1);
									this.highlightedFriendlyUnit = selectedUnit;

									moveOptions = selectedUnit.getValidMoves(tileCoords.x, tileCoords.y, battlefield);
									foreach (Coord moveOption in moveOptions) {
										highlightMultipleObjects(battlefield.map[moveOption.x, moveOption.y].Peek().gameObject);
									}

									this.highlightedEnemyUnits = selectedUnit.getTargets(tileCoords.x, tileCoords.y, battlefield, level.characters[currentCharacter]);
									foreach (Unit targetableUnit in highlightedEnemyUnits) {
										highlightMultipleObjects(targetableUnit.gameObject, 2);
									}

									advanceBattleStage();
								} else {
									//Selected enemy unit. Show unit and its move options.
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
								//We selected a tile! lets move to it
								if (moveOptions.Any(move => (move.x == tileCoords.x && move.y == tileCoords.y))) {
									moveUnit(highlightedFriendlyUnit, tileCoords);
									deselectMoveOptions();

									highlightedFriendlyUnit.hasMovedThisTurn = true;

									if (battlefield.charactersUnits[level.characters[currentCharacter]].All(unit => unit.hasMovedThisTurn)) {
										advanceBattleStage();
									} else {
										this.battleStage = BattleLoopStage.UnitSelection;
									}
								}
							} else if (highlightedFriendlyUnit == selectedItem || selectedItem == null) {
								//Decided not to move that unit afterall, deselect it
								deselectMoveOptions();
								battleStage = BattleLoopStage.UnitSelection;
							} else if (selectedItem is Unit) {
								Unit selectedUnit = selectedItem as Unit;
								if (selectedUnit.getCharacter(battlefield) == level.characters[currentCharacter]) {
									//Clicked on a friendly unit. Deselect the current one.
									deselectMoveOptions();
									battleStage = BattleLoopStage.UnitSelection;
								} else {
									//Clicked on a hostile unit! fight!
									if (highlightedEnemyUnits.Contains(selectedUnit)) {
										bool defenderDefeated = highlightedFriendlyUnit.doBattleWith(
											selectedUnit,
											battlefield.map[tileCoords.x, tileCoords.y].Peek(),
											battlefield);

										await Task.Delay(TimeSpan.FromMilliseconds(250));

										if (defenderDefeated) {
											highlightedEnemyUnits.RemoveAll(units => units == null);
										} else {
											//Counterattack
											Coord unitCoords = battlefield.getUnitCoords(highlightedFriendlyUnit);
											bool attackerDefeated = selectedUnit.doBattleWith(
												highlightedFriendlyUnit,
												battlefield.map[unitCoords.x, unitCoords.y].Peek(),
												battlefield);
										}
										checkWinAndLose();

										highlightedFriendlyUnit.hasMovedThisTurn = true;
										await Task.Delay(TimeSpan.FromMilliseconds(250));
										deselectMoveOptions();

										if (battlefield.charactersUnits[level.characters[currentCharacter]].All(unit => unit.hasMovedThisTurn)) {
											advanceBattleStage();
										} else {
											this.battleStage = BattleLoopStage.UnitSelection;
										}
									}
								}
							} else {
								Debug.LogWarning("Item of unrecognized type clicked on.");
							}
						}
					}

					break;
				case BattleLoopStage.EndTurn:
					foreach (Unit unit in battlefield.charactersUnits[level.characters[currentCharacter]]) {
						unit.hasMovedThisTurn = false;
					}

					checkWinAndLose();
					advanceBattleStage();
					break;
			}
		}

		private async void checkWinAndLose() {
			if (winCondition()) {
				//TODO: advance campaign
				victoryImage.enabled = true;

				await Task.Delay(TimeSpan.FromMilliseconds(6000));

				victoryImage.enabled = false;

				//This will be encoded in the campaign
				CutsceneCharacter blair = CutsceneCharacter.blair;
				CutsceneScript script = new CutsceneScript(new List<CutsceneScriptLine> {
					new CutsceneScriptLine(CutsceneAction.SetBackground, background: CutsceneBackground.None),
					new CutsceneScriptLine(CutsceneAction.SetCharacter, character: blair, side: Side.Left),
					new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "That sure was a intense battle huh?"),
					new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "Oh no! it looks like the evil lord zxqv is getting away. Does this qualify as a plot hook?"),
				});
				Cutscene endCutscene = Instantiate(cutscene);
				endCutscene.setup(new CutsceneCharacter[] { blair }, script, cutscene);

			} else if (loseCondition()) {
				defeatImage.enabled = true;
			}
		}

		//Returns true if the human player has won, false otherwise
		private bool winCondition() {
			foreach (Character character in battlefield.charactersUnits.Keys) {
				if (character != level.characters[playerCharacter]) {
					if (battlefield.charactersUnits[character].Count() != 0) {
						return false;
					}
				}
			}
			return true;
		}

		//returns true if the human player has lost, false otherwise
		private bool loseCondition() {
			if (battlefield.charactersUnits[level.characters[playerCharacter]].Count() == 0) {
				return true;
			}
			return false;
		}

		private void moveUnit(Unit unit, Vector3Int target) {
			Coord unitCoords = battlefield.getUnitCoords(unit);
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
		}

		private void unhighlightMultipleObjects(GameObject objectToHighlight) {
			if (objectToHighlight != null) {
				Destroy(objectToHighlight.GetComponent<cakeslice.Outline>());
			}
		}

		//Convenience
		private void advanceBattleStage() {
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