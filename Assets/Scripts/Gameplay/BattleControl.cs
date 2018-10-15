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
using Buffs;
using Editors;
using System.IO;
using Cutscenes.Stages;
using UnityEngine.SceneManagement;

namespace Gameplay {
	public class BattleControl : MonoBehaviour {

		//x, y, height (from the bottom)
		private Battlefield battlefield;
		private Level level;
		private GameObjective objective;
		[SerializeField]
		private GameObject[] tilePrefabs;
		[SerializeField]
		private GameObject[] unitPrefabs;
		public Camera mainCamera;
		public Camera cutsceneCamera;

		private BattleLoopStage battleStage;
		//Use this to keep one of the Update switch blocks from being called multiple times.
		private bool battleStageChanged;

		private int currentCharacter;
		private int playerCharacter;
		public int halfTurnsElapsed;

		[SerializeField]
		private Text turnPlayerText;
		[SerializeField]
		private Image turnChangeBackground;
		[SerializeField]
		private Image victoryImage;
		[SerializeField]
		private Image defeatImage;
		[SerializeField]
		private Stage cutscene;

		// Use this for initialization
		void Start() {
			//Actual constructor code. This should still be here after the demo :p
			playerCharacter = 0;
			battlefield = new Battlefield();
			currentCharacter = -1;
			battleStage = BattleLoopStage.Initial;
			halfTurnsElapsed = 0;

			turnPlayerText.enabled = false;
			turnChangeBackground.enabled = false;
			victoryImage.enabled = false;
			defeatImage.enabled = false;

			getLevel();
			deserializeMap();
			deserializeLevel();

			//TODO replace this with predicate based execution
			CameraController.inputEnabled = false;
			mainCamera.enabled = false;
			cutsceneCamera.enabled = true;
			if (level.cutsceneIDs.Length != 0) {
				cutscene.startCutscene(level.cutsceneIDs[0]);
			}
		}

		// Update is called once per frame
		async void Update() {
			switch (battleStage) {
				case BattleLoopStage.Initial:
					if (!cutscene.isRunning) {
						CameraController.inputEnabled = true;
						cutsceneCamera.enabled = false;
						mainCamera.enabled = true;
						advanceBattleStage();
					}
					break;
				case BattleLoopStage.Pick:
					advanceBattleStage();
					break;
				case BattleLoopStage.BattleLoopStart:
					advanceBattleStage();
					break;
				case BattleLoopStage.TurnChange:
					//If we've already entered this we're awaiting. Don't call it again every frame.
					if (!battleStageChanged) {
						break;
					}
					battleStageChanged = false;

					currentCharacter = (currentCharacter + 1) % level.characters.Length;
					turnPlayerText.text =
						level.characters[currentCharacter].name + "'s turn\n" +
						"Turns remaining:  " + (objective.maxHalfTurns - ((halfTurnsElapsed / 2) + 1));
					turnPlayerText.enabled = true;
					turnChangeBackground.enabled = true;
					Util.setTimeout(advanceBattleStage, 1000);

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
					//If we've already entered this we're awaiting. Don't call it again every frame.
					if (!battleStageChanged) {
						break;
					}
					battleStageChanged = false;

					//Character.getMove() is responsible for validation so we assume the move to be legal
					Move move = await level.characters[currentCharacter].getMove();
					Unit ourUnit = battlefield.units[move.from.x, move.from.y];
					IBattlefieldItem selectedItem = battlefield.battlefieldItemAt(move.to.x, move.to.y);

					if (selectedItem is Tile) {
						//We selected a tile! lets move to it
						moveUnit(ourUnit, move.to.x, move.to.y);

						if (ourUnit.getTargets(move.to.x, move.to.y, battlefield, level.characters[currentCharacter]).Count == 0) {
							ourUnit.greyOut();
						}

					} else if (selectedItem is Unit) {
						//Targeted a hostile unit! fight!
						Unit selectedUnit = selectedItem as Unit;

						bool defenderDefeated = ourUnit.doBattleWith(
							selectedUnit,
							battlefield.map[move.to.x, move.to.y].Peek(),
							battlefield);

						await Task.Delay(TimeSpan.FromMilliseconds(250));

						if (!defenderDefeated && (selectedItem is MeleeUnit) && (ourUnit is MeleeUnit)) {
							//Counterattack applied only when both units are Melee
							selectedUnit.doBattleWith(
								ourUnit,
								battlefield.map[move.from.x, move.from.y].Peek(),
								battlefield);
						}

						ourUnit.setHasAttackedThisTurn(true);
						await Task.Delay(TimeSpan.FromMilliseconds(250));
					} else {
						Debug.LogWarning("Item of unrecognized type clicked on.");
					}

					checkWinAndLose();

					//If all of our units have moved advance. Otherwise, go back to unit selection.
					ourUnit.hasMovedThisTurn = true;
					if (battlefield.charactersUnits[level.characters[currentCharacter]].All(unit => {
						//I know this looks inelegant but it avoid calling getUnitCoords if necessary
						if (!unit.hasMovedThisTurn) {
							return false;
						} else if (unit.getHasAttackedThisTurn()) {
							return true;
						} else {
							Coord coord = battlefield.getUnitCoords(unit);
							return unit.getTargets(coord.x, coord.y, battlefield, level.characters[currentCharacter]).Count == 0;
						}
					})) {
						advanceBattleStage();
					} else {
						setBattleLoopStage(BattleLoopStage.UnitSelection);
					}

					break;
				case BattleLoopStage.EndTurn:
					//If we've already entered this we're awaiting. Don't call it again every frame.
					if (!battleStageChanged) {
						break;
					}
					battleStageChanged = false;

					foreach (Unit unit in battlefield.charactersUnits[level.characters[currentCharacter]]) {
						unit.hasMovedThisTurn = false;
						unit.setHasAttackedThisTurn(false);
					}

					bool endGame = checkWinAndLose();
					if (!endGame) {
						advanceBattleStage();
					}

					halfTurnsElapsed++;

					break;
			}
		}

		private bool checkWinAndLose() {
			if (objective.isWinCondition(halfTurnsElapsed)) {
				advanceCampaign();
				return true;

			} else if (objective.isLoseCondition(halfTurnsElapsed)) {
				restartLevelDefeat();
				return true;
			} else {
				return false;
			}
		}

		private async void advanceCampaign() {
			victoryImage.enabled = true;
			await Task.Delay(TimeSpan.FromMilliseconds(6000));
			victoryImage.enabled = false;

			Persistance.campaign.levelIndex++;
			//Oh Boy i hope this works.
			SceneManager.LoadScene("DemoBattle");
		}

		private async void restartLevelDefeat() {
			defeatImage.enabled = true;
			await Task.Delay(TimeSpan.FromMilliseconds(6000));
			victoryImage.enabled = false;
			//TODO show ui to quit or retry

			//Restart the level, don't increment
			SceneManager.LoadScene("DemoBattle");
		}

		private void addUnit(UnitType unitType, Character character, int x, int y, Faction faction) {
			int index = (int)(unitType);
			GameObject newUnitGO = Instantiate(
				unitPrefabs[index],
				Util.GridToWorld(x, y, battlefield.map[x, y].Count + 1),
				unitPrefabs[index].gameObject.transform.rotation);

			Unit newUnit = newUnitGO.GetComponent<Unit>();
			newUnit.setFaction(faction);
			battlefield.addUnit(newUnit, character, x, y);
		}

		private void moveUnit(Unit unit, int targetX, int targetY) {
			Coord unitCoords = battlefield.getUnitCoords(unit);
			battlefield.units[unitCoords.x, unitCoords.y] = null;
			battlefield.units[targetX, targetY] = unit;
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
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(level.mapFileName, "Assets\\Maps\\"), tilePrefabs, null);
			battlefield.units = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1)];
		}

		private void deserializeLevel() {
			//Testing Level Deserialization
			LevelInfo levelInfo = Serialization.getLevel(level.levelFileName);



			//TODO: game objective will be serialized in the level editor data. assign it here, and do any other necessary reference assignment
			objective = new EliminationObjective(battlefield, level, level.characters[playerCharacter], 20);

			// Uncomment these for the escort objective
			// (objective as EscortObjective).vips.Add(battlefield.units[0,0]);
			// (objective as EscortObjective).vips.Add(battlefield.units[1,0]);
			// (objective as EscortObjective).vips.Add(battlefield.units[0,1]);

			// Uncomment these for the intercept objective
			// (objective as InterceptObjective).vips.Add(battlefield.units[3,7]);

			// objective = new CaptureObjective(battlefield, level, characters[playerCharacter], 20, new List<Coord>(new Coord[] {new Coord(1,1)}), 0);
			// objective = new DefendObjective(battlefield, level, characters[playerCharacter], 20, new List<Coord>(new Coord[] {new Coord(3,4), new Coord(1,1)}), 0);

			//For these objectives to work, you must also comment out the lines in the initial battle stage below
			// objective = new EscortObjective(battlefield, level, characters[playerCharacter], 20);
			// objective = new InterceptObjective(battlefield, level, characters[playerCharacter], 20);



			try {
				Stack<UnitInfo> stack = levelInfo.units;
				while (stack.Count != 0) {
					UnitInfo info = stack.Pop();
					if (info.getIsPlayer()) {
						addUnit(info.getUnitType(), level.characters[0], info.getCoord().x, info.getCoord().y, Faction.Xingata);
					} else {
						addUnit(info.getUnitType(), level.characters[1], info.getCoord().x, info.getCoord().y, Faction.Tsubin);
					}
				}
			} catch (FileNotFoundException ex) {
				Debug.Log("Incorrect level name" + ex.ToString());
			}
		}

		private void getLevel() {
			//This indicates the scene has been played from the editor, without first running MainMenu. This is a debug mode.
			if (Persistance.campaign == null && Application.isEditor) {
				Character[] characters = new[] {
					new Character("Alice", true, new playerAgent()),
					new Character("The evil lord zxqv", false, new simpleAgent())
				};
				level = new Level("DemoMap2", "TestLevel", characters, new string[] { });
				Persistance.campaign = new Campaign("test", 0, new[] { level });
				// cutscene.startCutscene("tutorialEnd");
				cutscene.hideVisualElements();
			}


			level = Persistance.campaign.levels[Persistance.campaign.levelIndex];
			foreach (Character character in level.characters) {
				character.agent.battlefield = this.battlefield;
			}
		}
	}
}