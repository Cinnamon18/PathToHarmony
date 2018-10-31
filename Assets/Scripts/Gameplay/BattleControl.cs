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

		[SerializeField]
		private Transform tilesHolder;
		[SerializeField]
		private TileGenerator generator;

		private string mapFilePath = Serialization.mapFilePath;
		private LevelInfo levelInfo;

		public Camera mainCamera;
		public Camera cutsceneCamera;

		private const float turnDelayMs = 150.0f;
		private BattleLoopStage battleStage;

		//Use this to keep one of the Update switch blocks from being called multiple times.
		private bool battleStageChanged;
		public int currentCharacter;
		public int playerCharacter;
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

		void Start() {
			playerCharacter = 0;
			battlefield = new Battlefield();
			currentCharacter = -1;
			battleStage = BattleLoopStage.Initial;
			halfTurnsElapsed = 0;

			turnPlayerText.enabled = false;
			turnChangeBackground.enabled = false;
			victoryImage.enabled = false;
			defeatImage.enabled = false;
			cutscene.hideVisualElements();
			useNormalCamera();

			getLevel();
			deserializeMap();
			deserializeLevel();


		}

		// Poor man's state machine. in retrospect i have no idea why i didn't use a proper one. oh well, next game.
		async void Update() {
			switch (battleStage) {
				case BattleLoopStage.Initial:
					advanceBattleStage();
					break;
				case BattleLoopStage.Pick:
					advanceBattleStage();
					break;
				case BattleLoopStage.BattleLoopStart:
					if (!battleStageChanged) {
						break;
					}
					battleStageChanged = false;
					//Check for cutscenes every start phase
					await runAppropriateCutscenes();

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

					await Task.Delay(1000);
					advanceBattleStage();

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

					//After patching the RTS bug, the getMove function will now return null if no move should be made.
					Move move = await level.characters[currentCharacter].getMove();

					if (move == null) {
						//A null move will be returned if the selection loop is interrupted by an ending turn.
						//Since no move occurs, nothing else needs to be done this turn (since nothing changed).
						break;
					}

					Unit ourUnit = battlefield.units[move.from.x, move.from.y];

					//The unit sometimes would no longer be in its expected position before the RTS bug was patched,
					//but this bug might no longer occur, so this check might be unnecessary. It doesn't hurt to leave it in,
					//since the behavior is the same: either this loop terminates due to a break, or due to an exception.
					if (ourUnit == null) {
						Debug.LogWarning("In BattleControl.update(), a move originated from a nonexistent unit, probably due to an ended turn.");
						break;
					}

					IBattlefieldItem selectedItem = battlefield.battlefieldItemAt(move.to.x, move.to.y);
					if (move.from.Equals(move.to)) {
						//This is the null move. just do nothing!
						ourUnit.hasMovedThisTurn = true;
						ourUnit.setHasAttackedThisTurn(true);
						ourUnit.greyOut();
					} else if (selectedItem is Tile) {
						//We selected a tile! lets move to it
						await moveUnit(ourUnit, move.to.x, move.to.y);

						if (ourUnit.getTargets(move.to.x, move.to.y, battlefield, level.characters[currentCharacter]).Count == 0) {
							ourUnit.greyOut();
						}

					} else if (selectedItem is Unit) {
						//Targeted a hostile unit! fight!
						Unit selectedUnit = selectedItem as Unit;

						await ourUnit.playAttackAnimation();
						bool defenderDefeated = ourUnit.doBattleWith(
							selectedUnit,
							battlefield.map[move.to.x, move.to.y].Peek(),
							battlefield);

						if (!defenderDefeated && (selectedItem is MeleeUnit) && (ourUnit is MeleeUnit)) {
							await selectedUnit.playAttackAnimation();
							//Counterattack applied only when both units are Melee
							selectedUnit.doBattleWith(
								ourUnit,
								battlefield.map[move.from.x, move.from.y].Peek(),
								battlefield);
						}

						ourUnit.setHasAttackedThisTurn(true);
						await Task.Delay(TimeSpan.FromMilliseconds(turnDelayMs));
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

					//Check cutscenes every end phase
					await runAppropriateCutscenes();

					foreach (Unit unit in battlefield.charactersUnits[level.characters[currentCharacter]]) {
						Coord coord = battlefield.getUnitCoords(unit);
						Tile tile = battlefield.map[coord.x, coord.y].Peek();
						checkTile(tile, unit);
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

		private void checkTile(Tile tile, Unit unit) {
			TileEffects effects = tile.tileEffects;
			switch (effects) {
				case TileEffects.Normal:
					break;
				case TileEffects.DOT:
					unit.setHealth(unit.getHealth() - 20);
					break;
				case TileEffects.Heal:
					unit.setHealth(unit.getHealth() + 20);
					break;
			}
		}

		private async void advanceCampaign() {
			victoryImage.enabled = true;
			await Task.Delay(TimeSpan.FromMilliseconds(6000));
			victoryImage.enabled = false;

			Persistance.campaign.levelIndex++;
			Persistance.saveProgress();
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

		private async Task moveUnit(Unit unit, int targetX, int targetY) {
			Coord unitCoords = battlefield.getUnitCoords(unit);
			battlefield.units[unitCoords.x, unitCoords.y] = null;
			battlefield.units[targetX, targetY] = unit;

			Vector3 startPos = unit.gameObject.transform.position;
			Vector3 endPos = Util.GridToWorld(
				new Vector3Int(targetX, targetY, battlefield.map[targetX, targetY].Count + 1)
			);

			float moveUnitProgress = 0.0f;
			while (moveUnitProgress < turnDelayMs) {
				//Slower at the start and end. a beautiful logistic curve. 
				float progressPercent = 1 / (1 + Mathf.Pow((float)(Math.E), -5 * ((moveUnitProgress / turnDelayMs) - 0.5f)));

				unit.gameObject.transform.position = Vector3.Lerp(startPos, endPos, progressPercent);
				await Task.Delay(10);
				moveUnitProgress += 10;
			}

			unit.gameObject.transform.position = endPos;

		}

		private async Task runAppropriateCutscenes() {
			foreach (String cutsceneID in level.cutsceneIDs) {
				if (Stages.testExecutionCondition(cutsceneID, battlefield, objective, halfTurnsElapsed)) {
					await runCutscene(cutsceneID);
					//I know this is bad practice, but it'll force the engine not to execute multiple cutscenes with the static resources
					break;
				}
			}
		}

		private async Task runCutscene(string cutsceneID) {
			cutscene.startCutscene(cutsceneID);
			useCutsceneCamera();

			//Wait for cutscene to finish.
			while (cutscene.isRunning) {
				//Unity, forgive me for not using coroutines. I am repentant. I understand my crimes and will not recommimt.
				await Task.Delay(100);
			}

			useNormalCamera();
		}

		private void useCutsceneCamera() {
			CameraController.inputEnabled = false;
			mainCamera.enabled = false;
			cutsceneCamera.enabled = true;
		}

		private void useNormalCamera() {
			CameraController.inputEnabled = true;
			cutsceneCamera.enabled = false;
			mainCamera.enabled = true;
		}


		private void advanceBattleStage() {
			battleStage = battleStage.NextPhase();
			battleStageChanged = true;
		}

		private void setBattleLoopStage(BattleLoopStage stage) {
			battleStage = stage;
			battleStageChanged = true;
		}


		private void deserializeMap() {
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(level.mapFileName, mapFilePath), generator, tilesHolder);

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

					if (info.getFaction() == Faction.Xingata) {
						addUnit(info.getUnitType(), level.characters[0], info.getCoord().x, info.getCoord().y, Faction.Xingata);
					} else {
						addUnit(info.getUnitType(), level.characters[1], info.getCoord().x, info.getCoord().y, info.getFaction());
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
					new Character("Alice", true, new PlayerAgent()),
					new Character("The evil lord zxqv", false, new SimpleAgent())
				};
				level = new Level("DemoMap2", "EasyVictory", characters, new string[] { });
				Persistance.campaign = new Campaign("test", 0, new[] { level });
				// cutscene.startCutscene("tutorialEnd");
				cutscene.hideVisualElements();
			}


			Persistance.loadProgress();
			level = Persistance.campaign.levels[Persistance.campaign.levelIndex];
			foreach (Character character in level.characters) {
				character.agent.battlefield = this.battlefield;
			}
		}

		public void skipTurn() {
			Agent agent = level.characters[currentCharacter].agent;
			if (currentCharacter == playerCharacter && battleStage == BattleLoopStage.ActionSelection) {
				if (agent is PlayerAgent) {
					((PlayerAgent)agent).unhighlightAll();
					((PlayerAgent)agent).currentMove=null;
					((PlayerAgent)agent).stopAwaiting = true;
					setBattleLoopStage(BattleLoopStage.EndTurn);
				}
			}
		}

	}
}