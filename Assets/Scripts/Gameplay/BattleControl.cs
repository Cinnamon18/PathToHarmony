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

		private string mapFilePath = Serialization.mapFilePath;

		private LevelInfo levelInfo;

		private BattleLoopStage battleStage;
		//Use this to keep one of the Update switch blocks from being called multiple times.
		private bool battleStageChanged;

		private int currentCharacter;
		private int playerCharacter;
		public int halfTurnsElapsed;

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
			halfTurnsElapsed = 0;

			turnPlayerText.enabled = false;
			turnChangeBackground.enabled = false;
			victoryImage.enabled = false;
			defeatImage.enabled = false;

			//Changed to generate different levels
			levelInfo = Serialization.getLevel("DemoLevel");

			//Just for testing because we don't have any way to set the campaign yet:
			Character[] characters = new[] {
				new Character("Alice", true, new PlayerAgent(battlefield, null, obj => Destroy(obj, 0) )),
				//new Character("The evil lord zxqv", false, new PlayerAgent(battlefield, null, obj => Destroy(obj, 0)))
				new Character("The evil lord zxqv", false, new simpleAgent(battlefield, null, obj => Destroy(obj, 0)))
				};
			List<Coord> alicePickTiles = new List<Coord> { new Coord(0, 0), new Coord(0, 1), new Coord(1, 0) };
			List<Coord> evilGuyPickTiles = new List<Coord> { new Coord(3, 7), new Coord(7, 4) };
			Dictionary<Character, List<Coord>> validPickTiles = new Dictionary<Character, List<Coord>>();
			validPickTiles[characters[0]] = alicePickTiles;
			validPickTiles[characters[1]] = evilGuyPickTiles;
			//gets mapname from levelinfo
			Level level = new Level(levelInfo.mapName, characters, null, validPickTiles);
			objective = new EliminationObjective(battlefield, level, characters[playerCharacter], 20);
			// objective = new CaptureObjective(battlefield, level, characters[playerCharacter], 20, new List<Coord>(new Coord[] {new Coord(1,1)}), 0);
			// objective = new DefendObjective(battlefield, level, characters[playerCharacter], 20, new List<Coord>(new Coord[] {new Coord(3,4), new Coord(1,1)}), 0);

			//For these objectives to work, you must also comment out the lines in the initial battle stage below
			// objective = new EscortObjective(battlefield, level, characters[playerCharacter], 20);
			// objective = new InterceptObjective(battlefield, level, characters[playerCharacter], 20);

			characters[0].agent.level = level;
			characters[1].agent.level = level;

			Campaign testCampaign = new Campaign("test", 0, new[] { level });
			Persistance.campaign = testCampaign;

			//This will be encoded in the campaign. Somewhere.
			CutsceneCharacter blair = CutsceneCharacter.blair;
			CutsceneCharacter juniper = CutsceneCharacter.juniper;
			CutsceneScript script = new CutsceneScript(new List<CutsceneScriptLine>
			{
				//new CutsceneScriptLine(CutsceneAction.SetBackground, background: CutsceneBackground.Academy),
				//new CutsceneScriptLine(CutsceneAction.SetCharacter, character: blair, side: CutsceneSide.Left),
				//new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "My name is Blair!"),
				//new CutsceneScriptLine(CutsceneAction.SetCharacter, character: juniper, side: CutsceneSide.Right),
				//new CutsceneScriptLine(CutsceneAction.SayDialogue, character: juniper, dialogue: "and I'm Juniper."),
				// new CutsceneScriptLine(CutsceneAction.SayDialogue, character: blair, dialogue: "There's a third major character, Bruno. He would've been here, but he got tied up with paperwork"),
				// new CutsceneScriptLine(CutsceneAction.SayDialogue, character: juniper, dialogue: "Which is to say we ran out of budget"),
				// new CutsceneScriptLine(CutsceneAction.SayDialogue, character: juniper, dialogue: "Anyways, I hope you enjoy this slick as h*ck demo"),
				// new CutsceneScriptLine(CutsceneAction.TransitionOut, side: CutsceneSide.Right),
				//new CutsceneScriptLine(CutsceneAction.TransitionOut, side: CutsceneSide.Left)
			});
		
			cutscene.setup(script);
			cutscene.playScene();

			getLevel();
			deserializeMap();
		}

		// Update is called once per frame
		async void Update() {
			switch (battleStage) {
				case BattleLoopStage.Initial:
					if (!cutscene.inProgress) {

						//Testing Level Deserialization
						try
						{
							Stack<UnitInfo> stack = levelInfo.units;
							while (stack.Count != 0)
							{
								UnitInfo info = stack.Pop();
								if (info.getIsPlayer())
								{
									addUnit(info.getUnitType(), level.characters[0], info.getCoord().x, info.getCoord().y, Faction.Xingata);
								}
								else
								{
									addUnit(info.getUnitType(), level.characters[1], info.getCoord().x, info.getCoord().y, Faction.Tsubin);
								}

							}
						}
						catch (FileNotFoundException ex)
						{
							Debug.Log("Incorrect level name" + ex.ToString());
						}
						
						//TODO This is temp just for testing until level editor deserialization. 
						addUnit(UnitType.Knight, level.characters[0], 0, 0, Faction.Xingata);
						addUnit(UnitType.Knight, level.characters[0], 1, 0, Faction.Xingata);
						addUnit(UnitType.Knight, level.characters[0], 0, 1, Faction.Xingata);
						addUnit(UnitType.Knight, level.characters[1], 3, 7, Faction.Tsubin);
						addUnit(UnitType.Knight, level.characters[1], 4, 7, Faction.Tsubin);

						addUnit(UnitType.Mage, level.characters[0], 1, 1, Faction.Xingata);

						// Uncomment these for the escort objective
						// (objective as EscortObjective).vips.Add(battlefield.units[0,0]);
						// (objective as EscortObjective).vips.Add(battlefield.units[1,0]);
						// (objective as EscortObjective).vips.Add(battlefield.units[0,1]);

						// Uncomment these for the intercept objective
						// (objective as InterceptObjective).vips.Add(battlefield.units[3,7]);

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
				defeatImage.enabled = true;
				return true;
			} else {
				return false;
			}
		}

		private async void advanceCampaign() {
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

			//TODO: actually advance campaign
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
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(level.mapFileName, mapFilePath), tilePrefabs, tilesHolder);
			battlefield.units = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1)];
		}

		private void getLevel() {
			level = Persistance.campaign.levels[Persistance.campaign.levelIndex];
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
	}
}