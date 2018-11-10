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
using Random = UnityEngine.Random;

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
		[SerializeField]
		private FadeOutTransition fade;


		private LevelInfo levelInfo;

		public Camera mainCamera;
		public Camera cutsceneCamera;

		private const float turnDelayMs = 150.0f;
		private BattleLoopStage battleStage;

		//Use this to keep one of the Update switch blocks from being called multiple times.
		private bool battleStageChanged = true;
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
		private GameObject cutsceneCanvasPrefab;
		[SerializeField]
		private Button skipButton;
		[SerializeField]
		private GameObject vipCrownPrefab;

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

			useNormalCamera();

			getLevel();
			deserializeMap();
			deserializeLevel();

			mainCamera.GetComponent<CameraController>().updateMaxPos(battlefield.map.GetLength(0), battlefield.map.GetLength(1));
		}

		// Poor man's state machine. in retrospect i have no idea why i didn't use a proper one. oh well, next game.
		async void Update() {
			switch (battleStage) {
				case BattleLoopStage.Initial:
					if (!battleStageChanged) {
						break;
					}
					battleStageChanged = false;

					playBgm();

					turnPlayerText.text =
						"Battle objective:\n" +
						objective.getName();
					turnPlayerText.enabled = true;
					turnChangeBackground.enabled = true;
					await Task.Delay(3000);

					advanceBattleStage();
					break;
				case BattleLoopStage.Pick:
					advanceBattleStage();
					turnPlayerText.enabled = false;
					turnChangeBackground.enabled = false;

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
						"Turns remaining:  " + (objective.maxHalfTurns - halfTurnsElapsed);
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
						await moveUnit(ourUnit, move.to);

						if (ourUnit.getTargets(move.to.x, move.to.y, battlefield, level.characters[currentCharacter]).Count == 0) {
							ourUnit.greyOut();
							ourUnit.setHasAttackedThisTurn(true);
						}

					} else if (selectedItem is Unit) {
						//Targeted a hostile unit! fight!
						Unit selectedUnit = selectedItem as Unit;

						await rotateUnit(ourUnit, battlefield.getUnitCoords(selectedUnit));
						bool defenderDefeated = await ourUnit.doBattleWith(
							selectedUnit,
							battlefield.map[move.to.x, move.to.y].Peek(),
							battlefield);

						if (!defenderDefeated && (selectedItem is MeleeUnit) && (ourUnit is MeleeUnit)) {
							await rotateUnit(selectedUnit, battlefield.getUnitCoords(ourUnit));
							//Counterattack applied only when both units are Melee
							await selectedUnit.doBattleWith(
								ourUnit,
								battlefield.map[move.from.x, move.from.y].Peek(),
								battlefield);
						}

						//Re-grey model if needed... I'm regretting my desire to make the health ui manager stateless :p
						if (ourUnit is HealerUnit) {
							if (selectedUnit.hasMovedThisTurn || selectedUnit.getHasAttackedThisTurn()) {
								// selectedUnit.getTargets(move.to.x, move.to.y, battlefield, level.characters[currentCharacter]).Count == 0) {
								selectedUnit.greyOut();
							}
						}

						ourUnit.setHasAttackedThisTurn(true);
						// await Task.Delay(TimeSpan.FromMilliseconds(turnDelayMs));
					} else {
						Debug.LogWarning("Item of unrecognized type clicked on.");
					}

					// checkWinAndLose();

					ourUnit.hasMovedThisTurn = true;

					await runAppropriateCutscenes();

					//If all of our units have moved advance. Otherwise, go back to unit selection.
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
						await checkTile(tile, unit);
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
			if (objective.isWinCondition(halfTurnsElapsed) || Input.GetKey(KeyCode.P)) {
				advanceCampaign();
				return true;

			} else if (objective.isLoseCondition(halfTurnsElapsed)) {
				restartLevelDefeat();
				return true;
			} else {
				return false;
			}
		}

		private async Task checkTile(Tile tile, Unit unit) {
			TileEffects effects = tile.tileEffects;
			switch (effects) {
				case TileEffects.Normal:
					break;
				case TileEffects.DOT:
					await unit.changeHealth(-20, true);
					break;
				case TileEffects.Heal:
					await unit.changeHealth(20, true);
					break;
			}
		}

		private async void advanceCampaign() {
			victoryImage.enabled = true;
			await Task.Delay(TimeSpan.FromMilliseconds(6000));
			victoryImage.enabled = false;

			Persistence.campaign.levelIndex++;

			await runAppropriateCutscenes(true);

			//check for end of campaign
			if (Persistence.campaign.levelIndex >= Persistence.campaign.levels.Count()) {
				fade.fadeToScene("VictoryScene");
			} else {
				Persistence.saveProgress();
				//Oh Boy im glad this works.
				fade.fadeToScene("DemoBattle");
			}
		}

		private async void restartLevelDefeat() {
			defeatImage.enabled = true;
			await Task.Delay(TimeSpan.FromMilliseconds(6000));
			victoryImage.enabled = false;
			//TODO show ui to quit or retry

			//Restart the level, don't increment
			fade.fadeToScene("DemoBattle");
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

		private async Task moveUnit(Unit unit, Coord target) {
			Coord unitCoords = battlefield.getUnitCoords(unit);
			battlefield.units[unitCoords.x, unitCoords.y] = null;
			battlefield.units[target.x, target.y] = unit;

			Vector3 startPos = unit.gameObject.transform.position;
			Vector3 endPos = Util.GridToWorld(
				new Vector3Int(target.x, target.y, battlefield.map[target.x, target.y].Count + 1)
			);

			//Rotate to face target
			await rotateUnit(unit, target);

			//interpolate. 
			float moveUnitProgress = 0.0f;
			while (moveUnitProgress < turnDelayMs) {
				//Slower at the start and end. a beautiful logistic curve. 
				float progressPercent = 1 / (1 + Mathf.Pow((float)(Math.E), -4 * ((moveUnitProgress / turnDelayMs) - 0.5f)));

				unit.gameObject.transform.position = Vector3.Lerp(startPos, endPos, progressPercent);
				await Task.Delay(10);
				moveUnitProgress += 10;
			}
			//Just in case....
			unit.gameObject.transform.position = endPos;

			//Yes yes i know this is terrible.
			if (unit is RangedUnit) {
				unit.setHasAttackedThisTurn(true);
			}
		}

		private async Task rotateUnit(Unit unit, Coord target) {
			Quaternion startPos = unit.gameObject.transform.rotation;
			Vector3 relPos = unit.transform.position;
			relPos -= (new Vector3(0, unit.transform.position.y, 0) + Util.GridToWorld(target));//change this to actual coords
			Quaternion endPos = Quaternion.LookRotation(relPos, Vector3.up);

			//Help 3d math is hard
			endPos *= Quaternion.Euler(0, 180, 0);

			float moveUnitProgress = 0.0f;
			while (moveUnitProgress < turnDelayMs) {
				//Slower at the start and end. a beautiful logistic curve. 
				float progressPercent = 1 / (1 + Mathf.Pow((float)(Math.E), -4 * ((moveUnitProgress / turnDelayMs) - 0.5f)));

				unit.gameObject.transform.rotation = Quaternion.Lerp(startPos, endPos, progressPercent);

				await Task.Delay(10);
				moveUnitProgress += 10;
			}

			//Just in case....
			unit.gameObject.transform.rotation = endPos;
		}

		private async Task runAppropriateCutscenes(bool afterVictoryImage = false) {
			foreach (Cutscene cutscene in level.cutscenes) {
				if (cutscene.executionCondition(new ExecutionInfo(battlefield, objective, halfTurnsElapsed, battleStage, afterVictoryImage))) {
					await runCutscene(cutscene);
					//I know this is bad practice, but it'll force the engine not to execute multiple cutscenes with the static resources
					break;
				}
			}
		}

		private async Task runCutscene(Cutscene cutscene) {

			//Hacky solution to cutscene canvas not being designed for multiple different cutscenes being played.
			Stage cutsceneCanvas = Instantiate(
				cutsceneCanvasPrefab,
				cutsceneCanvasPrefab.transform.position,
				cutsceneCanvasPrefab.transform.rotation,
				this.transform)
				.GetComponent<Stage>();
			skipButton.onClick.AddListener(() => cutsceneCanvas.skipCutscene());
			cutsceneCanvas.skipButton = skipButton.gameObject;
			cutsceneCanvas.startCutscene(cutscene);
			useCutsceneCamera();

			//Wait for cutscene to finish.
			while (cutsceneCanvas.isRunning) {
				//Unity, forgive me for not using coroutines. I am repentant. I understand my crimes and will not recommimt.
				await Task.Delay(100);
			}

			//Even though the cutscene can never be accessed again, this is a whole lot cleaner.
			Destroy(cutsceneCanvas);
			skipButton.onClick.RemoveAllListeners();

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

		private void playBgm() {
			string bgm = Audio.battleBgm[Random.Range(0, Audio.battleBgm.Length - 1)];
			Audio.playSound(bgm, true, true);
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
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(level.mapFileName, Paths.mapsPath()), generator, tilesHolder);

			battlefield.units = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1)];
		}

		private void deserializeLevel() {
			//get all level info for units, objectives and map name
			LevelInfo levelInfo = Serialization.getLevel(level.levelFileName);

			//add all units
			//default enemy faction
			Faction enemyFaction = Faction.Velgari;
			try {
				Stack<UnitInfo> stack = levelInfo.units;
				while (stack.Count != 0) {
					UnitInfo info = stack.Pop();

					if (info.getFaction() == Faction.Xingata) {
						addUnit(info.getUnitType(), level.characters[0], info.getCoord().x, info.getCoord().y, Faction.Xingata);
					} else {
						addUnit(info.getUnitType(), level.characters[1], info.getCoord().x, info.getCoord().y, info.getFaction());
						enemyFaction = info.getFaction();
					}
				}
			} catch (FileNotFoundException ex) {
				Debug.Log("Incorrect level name" + ex.ToString());
			}


			//get all goal info
			List<Coord> goalPositions = levelInfo.goalPositions;
			switch (levelInfo.objective) {
				case ObjectiveType.Elimination:
					objective = new EliminationObjective(battlefield, level, level.characters[playerCharacter], 25);
					break;
				case ObjectiveType.Escort:
					objective = new EscortObjective(battlefield, level, level.characters[playerCharacter], 15);
					//add vips
					foreach (Coord pos in goalPositions) {
						addUnit(UnitType.Knight, level.characters[0], pos.x, pos.y, Faction.Xingata);
						Unit unit = battlefield.units[pos.x, pos.y];
						(objective as EscortObjective).vips.Add(unit);
						Instantiate(vipCrownPrefab, unit.transform.position + new Vector3(0, 3, 0), vipCrownPrefab.transform.rotation, unit.transform);
					}
					break;
				case ObjectiveType.Intercept:
					objective = new InterceptObjective(battlefield, level, level.characters[playerCharacter], 20);
					foreach (Coord pos in goalPositions) {
						addUnit(UnitType.Knight, level.characters[1], pos.x, pos.y, enemyFaction);
						Unit unit = battlefield.units[pos.x, pos.y];
						(objective as InterceptObjective).vips.Add(unit);
						Instantiate(vipCrownPrefab, unit.transform.position + new Vector3(0, 3, 0), vipCrownPrefab.transform.rotation, unit.transform);
					}

					break;
				case ObjectiveType.Capture:
					objective = new CaptureObjective(battlefield, level, level.characters[playerCharacter], 20, goalPositions, 2);
					foreach (Coord pos in goalPositions) {
						Instantiate(vipCrownPrefab,
							battlefield.map[pos.x, pos.y].Peek().transform.position + new Vector3(0, 3, 0),
							vipCrownPrefab.transform.rotation,
							battlefield.map[pos.x, pos.y].Peek().transform);
					}
					break;
				case ObjectiveType.Defend:
					objective = new DefendObjective(battlefield, level, level.characters[playerCharacter], 20, goalPositions, 2);
					foreach (Coord pos in goalPositions) {
						Instantiate(vipCrownPrefab,
							battlefield.map[pos.x, pos.y].Peek().transform.position + new Vector3(0, 3, 0),
							vipCrownPrefab.transform.rotation,
							battlefield.map[pos.x, pos.y].Peek().transform);
					}
					break;
				case ObjectiveType.Survival:
					objective = new SurvivalObjective(battlefield, level, level.characters[playerCharacter], 10);
					break;
				default:
					objective = new EliminationObjective(battlefield, level, level.characters[playerCharacter], 20);
					break;
			}

		}

		private void getLevel() {
			//This indicates the scene has been played from the editor, without first running MainMenu. This is a debug mode.
			if (Persistence.campaign == null && Application.isEditor) {
				Character[] characters = new[] {
					new Character("Alice", true, new PlayerAgent()),
					new Character("The evil lord zxqv", false, new EliminationAgent())
				};

				level = new Level("DemoMap2", "AITest", characters, new Cutscene[] { });
				Persistence.campaign = new Campaign("test", 0, new[] { level });
				// cutscene.startCutscene("tutorialEnd");
			}
			//  else {
			// 	Persistance.loadProgress();
			// }

			level = Persistence.campaign.levels[Persistence.campaign.levelIndex];
			foreach (Character character in level.characters) {
				character.agent.battlefield = this.battlefield;
			}
		}

		public void skipTurn() {
			Agent agent = level.characters[currentCharacter].agent;
			if (currentCharacter == playerCharacter && battleStage == BattleLoopStage.ActionSelection) {
				if (agent is PlayerAgent) {
					((PlayerAgent)agent).unhighlightAll();
					((PlayerAgent)agent).currentMove = null;
					((PlayerAgent)agent).stopAwaiting = true;
					setBattleLoopStage(BattleLoopStage.EndTurn);
				}
			}
		}

	}
}