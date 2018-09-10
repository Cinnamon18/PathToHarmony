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
using AI;

namespace Gameplay {
	public class BattleControl : MonoBehaviour {

		//x, y, height (from the bottom)
		private Battlefield battlefield;
		private Level level;
		[SerializeField]
		private GameObject[] tilePrefabs;
		[SerializeField]
		private GameObject[] unitPrefabs;

		private BattleLoopStage battleStage;
		//Use this to keep one of the Update switch blocks from being called multiple times.
		private bool battleStageChanged;

		private int currentCharacter;
		private int playerCharacter;

		//Just a refrence to the cutscene prefab
		[SerializeField]
		private Cutscene cutscene;
		[SerializeField]
		private Text turnPlayerText;
		[SerializeField]
		private Image turnChangeBackground;
		[SerializeField]
		private Image victoryImage;
		[SerializeField]
		private Image defeatImage;

		// Use this for initialization
		void Start() {
			//Actual constructor code. This should still be here after the demo :p
			playerCharacter = 0;
			battlefield = new Battlefield();
			currentCharacter = -1;
			battleStage = BattleLoopStage.Initial;

			turnPlayerText.enabled = false;
			turnChangeBackground.enabled = false;
			victoryImage.enabled = false;
			defeatImage.enabled = false;


			//Just for testing because we don't have any way to set the campaign yet:
			Character[] characters = new[] {
				new Character("Alice", true, new PlayerAgent(battlefield, null, obj => Destroy(obj, 0) )),
				new Character("The evil lord zxqv", false, new PlayerAgent(battlefield, null, obj => Destroy(obj, 0)))
				};
			List<Coord> alicePickTiles = new List<Coord> { new Coord(0, 0), new Coord(0, 1), new Coord(1, 0) };
			List<Coord> evilGuyPickTiles = new List<Coord> { new Coord(3, 7), new Coord(7, 4) };
			Dictionary<Character, List<Coord>> validPickTiles = new Dictionary<Character, List<Coord>>();
			validPickTiles[characters[0]] = alicePickTiles;
			validPickTiles[characters[1]] = evilGuyPickTiles;
			Level level = new Level("DemoMap", characters, null, validPickTiles);
			characters[0].agent.level = level;
			characters[1].agent.level = level;

			Campaign testCampaign = new Campaign("test", 0, new[] { level });
			Persistance.campaign = testCampaign;

			//This will be encoded in the campaign. Somewhere.
			CutsceneCharacter blair = CutsceneCharacter.blair;
			CutsceneCharacter juniper = CutsceneCharacter.juniper;
			CutsceneScript script = new CutsceneScript(new List<CutsceneScriptLine>
			{
				// new CutsceneScriptLine(CutsceneAction.SetBackground, background: CutsceneBackground.Academy),
				// new CutsceneScriptLine(CutsceneAction.SetCharacter, character: blair, side: CutsceneSide.Left),
				// new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "My name is Blair!"),
				// new CutsceneScriptLine(CutsceneAction.SetCharacter, character: juniper, side: CutsceneSide.Right),
				// new CutsceneScriptLine(CutsceneAction.SayDialogue, character: juniper, dialogue: "and I'm Juniper."),
				// new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "There's a third major character, Bruno. He would've been here, but he got tied up with paperwork"),
				// new CutsceneScriptLine(CutsceneAction.SayDialogue, character: juniper, dialogue: "Which is to say we ran out of budget"),
				// new CutsceneScriptLine(CutsceneAction.SayDialogue, character: juniper, dialogue: "Anyways, I hope you enjoy this slick as h*ck demo"),
				// new CutsceneScriptLine(CutsceneAction.TransitionOut, side: CutsceneSide.Right),
				// new CutsceneScriptLine(CutsceneAction.TransitionOut, side: CutsceneSide.Left)
			});
			cutscene.setup(script);


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
					advanceBattleStage();
					break;
				case BattleLoopStage.ActionSelection:
					//If we've already entered this we're awaiting. Don't call it again this frame.
					if (!battleStageChanged) {
						break;
					}
					battleStageChanged = false;

					//Character.getMove() is responsible for validation so we assume the move to be legal
					Move move = await level.characters[currentCharacter].getMove();
					Unit ourUnit = battlefield.units[move.fromX, move.fromY];
					IBattlefieldItem selectedItem = battlefield.battlefieldItemAt(move.toX, move.toY);

					if (selectedItem is Tile) {
						//We selected a tile! lets move to it
						moveUnit(ourUnit, move.toX, move.toY);

					} else if (selectedItem is Unit) {
						//Targeted a hostile unit! fight!
						Unit selectedUnit = selectedItem as Unit;

						bool defenderDefeated = ourUnit.doBattleWith(
							selectedUnit,
							battlefield.map[move.toX, move.toY].Peek(),
							battlefield);

						await Task.Delay(TimeSpan.FromMilliseconds(250));

						if (!defenderDefeated) {
							//Counterattack
							selectedUnit.doBattleWith(
								ourUnit,
								battlefield.map[move.fromX, move.fromY].Peek(),
								battlefield);
						}

						await Task.Delay(TimeSpan.FromMilliseconds(250));
					} else {
						Debug.LogWarning("Item of unrecognized type clicked on.");
					}

					checkWinAndLose();

					//If all of our units have moved advance. Otherwise, go back to unit selection.
					ourUnit.hasMovedThisTurn = true;
					if (battlefield.charactersUnits[level.characters[currentCharacter]].All(unit => unit.hasMovedThisTurn)) {
						advanceBattleStage();
					} else {
						setBattleLoopStage(BattleLoopStage.UnitSelection);
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
					new CutsceneScriptLine(CutsceneAction.SetCharacter, character: blair, side: CutsceneSide.Left),
					new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "That sure was a intense battle huh?"),
					new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "Oh no! it looks like the evil lord zxqv is getting away. Does this qualify as a plot hook?"),
				});
				Cutscene endCutscene = Instantiate(cutscene);
				endCutscene.setup(script, cutscene);

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

		private void moveUnit(Unit unit, int targetX, int targetY) {
			Coord unitCoords = battlefield.getUnitCoords(unit);
			battlefield.units[targetX, targetY] = unit;
			battlefield.units[unitCoords.x, unitCoords.y] = null;
			unit.gameObject.transform.position = Util.GridToWorld(
				new Vector3Int(targetX, targetY, battlefield.map[targetX, targetY].Count + 1)
			);
		}

		//Convenience
		private void advanceBattleStage() {
			battleStage = battleStage.NextPhase();
			battleStageChanged = true;
		}

		private void setBattleLoopStage(BattleLoopStage stage) {
			battleStage = stage;
			battleStageChanged = true;
		}


		private void deserializeMap() {
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(level.mapFileName), tilePrefabs);
			battlefield.units = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1)];
		}

		private void getLevel() {
			level = Persistance.campaign.levels[Persistance.campaign.levelIndex];
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