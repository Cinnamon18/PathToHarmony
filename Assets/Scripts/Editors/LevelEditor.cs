using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Gameplay;
using UnityEngine.UI;
using Constants;
using AI;
using System.Text;
using System.ComponentModel;
using TMPro;
using System.IO;
using System.Linq;

namespace Editors {
	public class LevelEditor : Editor<Unit> {
		public string defaultMap;

		public Text loadMapText;
		public Text loadLevelText;
		public Text saveLevelText;


		//faction info
		public TMP_Dropdown factionDropdown;
		private Faction currentFaction;

		//objective info
		public TMP_Dropdown objectiveDropdown;
		public GameObject goalModel;
		public Transform goalHolder;
		public Text goalText;
		private ObjectiveType currentObjective;
		private Dictionary<Vector2, GameObject> goalMap;
		private string goalInstructions = "Click spacebar to place goal (like VIP unit for Esort or position to defend for Defent.)  " +
			"Click alt to remove goal.";

		[SerializeField]
		private Transform unitsHolder;
		[SerializeField]
		private Transform tilesHolder;

		private string mapName;
		private string levelName;

		private GameObject[] tilePrefabs;
		//to create tile no matter order
		[SerializeField]
		private TileGenerator generator;

		private Battlefield battlefield;
		private int fieldHeight;
		[SerializeField]
		private GameObject[] unitPrefabs;
		private UnitInfo[,] unitsInfo;

		public Camera mainCamera;

		Faction playerFaction = Faction.Xingata;
		Faction enemyFaction = Faction.Corbita;

		Stack<Tile>[,] tiles { get; set; }
		// Use this for initialization
		protected void Start() {

			//use battlefield to help place units
			battlefield = new Battlefield();
			mapName = defaultMap;

			//get tile gamobjects from generator
			tilePrefabs = generator.getPrefabs();

			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(mapName, Paths.mapsPath()), generator, tilesHolder);
			//Hacky but Serialization holds how tall the last map loaded is
			//use to know how tall Unit array should be
			fieldHeight = Serialization.mapHeight + 1;
			objs = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1), fieldHeight];
			//unitsinfo is used to store units and whether they are player or enemy units
			unitsInfo = new UnitInfo[battlefield.map.GetLength(0), battlefield.map.GetLength(1)];

			//what type of unit is being placed
			currentFaction = Faction.Xingata;


			if (previewObj.Length != unitPrefabs.Length) {
				Debug.LogError("Must have equal number of prefab and preview objects.");
			}
			//Tell Editor type
			setEditorType();

			factionDropdown.onValueChanged.AddListener(delegate {
				//set current faction
				currentFaction = (Faction)factionDropdown.value;
			});


			goalMap = new Dictionary<Vector2, GameObject>();

			objectiveDropdown.onValueChanged.AddListener(delegate {
				//set current objective
				currentObjective = (ObjectiveType)objectiveDropdown.value;
				if (currentObjective == ObjectiveType.Elimination || currentObjective == ObjectiveType.Survival) {
					removeAllGoals();
				}
				setGoalText();
			});

			if (currentObjective == ObjectiveType.Elimination || currentObjective == ObjectiveType.Survival) {
				setGoalText();
			}

		}
		public override void serialize() {
			levelName = saveLevelText.text;
			if (levelName == null || levelName == "") {
				Debug.LogError("Can't save without a file name.");
				return;
			}

			StringBuilder serialized = new StringBuilder(mapName + ";");

			foreach (UnitInfo info in unitsInfo) {
				if (info != null) {
					serialized.Append(info.serialize() + ";");
				}
			}
			//Add objective type
			serialized.Append("*" + (int)currentObjective);
			if (currentObjective != ObjectiveType.Elimination && currentObjective != ObjectiveType.Survival) {
				//get position of goals to serialize
				List<Vector2> positions = goalMap.Select(d => d.Key).ToList();
				foreach (Vector2 pos in positions) {
					serialized.Append(";" + pos.x + "," + pos.y);
				}
			}
			Serialization.WriteData(serialized.ToString(), levelName, Paths.levelsPath(), overwriteData);
			resetTextBoxes();
		}

		//check for assigning goal position
		private void FixedUpdate() {
			if ((currentObjective != ObjectiveType.Elimination) && (currentObjective != ObjectiveType.Survival)) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, 1000.0f)) {
					Vector3Int objCoords = Util.WorldToGrid(hit.transform.position);
					int x = objCoords.x;
					int y = objCoords.y;

					//press space to assign goal
					if (Input.GetKeyDown(KeyCode.Space)) {
						addGoal(x, y);
					} else if (Input.GetButtonDown("AltSelect")) {
						removeGoal(x, y, hit);
					}

				}
			}
		}

		private void addGoal(int x, int y) {
			Vector2 goalPosition = new Vector2(x, y);
			int z = battlefield.map[x, y].Count + 1;
			//make sure not placing on top of unit or other goal
			if (unitsInfo[x, y] == null && !goalMap.ContainsKey(goalPosition)) {
				GameObject newGoal = Instantiate(
				goalModel,
				Util.GridToWorld(x, y, z),
				goalModel.gameObject.transform.rotation);
				newGoal.transform.parent = goalHolder;

				goalMap[goalPosition] = newGoal;
			}

		}

		private void removeGoal(int x, int y, RaycastHit hit) {
			Vector2 removeCoords = new Vector2(x, y);
			if (hit.collider.gameObject.tag.Equals("Goal")) {
				goalMap.Remove(removeCoords);
				Destroy(hit.collider.gameObject);
			}

		}

		private void removeAllGoals() {
			goalMap.Clear();
			foreach (Transform child in goalHolder) {
				GameObject.Destroy(child.gameObject);
			}
		}

		private void setGoalText() {
			if (currentObjective == ObjectiveType.Elimination || currentObjective == ObjectiveType.Survival) {
				goalText.text = "";
			} else {
				goalText.text = goalInstructions;
			}
		}

		public override void deserialize() {
			loadLevel();
			resetTextBoxes();
		}


		public void loadMap() {
			if (Serialization.ReadData(mapName, Paths.mapsPath()) != null) {
				removeAllUnits();
				eraseTiles();
				mapName = loadMapText.text;
				reloadMap();
				resetTextBoxes();
			} else {
				Debug.Log("Map filename does not exist");
			}

		}

		private void reloadMap() {

			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(mapName, Paths.mapsPath()), generator, tilesHolder);
			objs = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1), Serialization.mapHeight + 1];
			unitsInfo = new UnitInfo[battlefield.map.GetLength(0), battlefield.map.GetLength(1)];
			mainCamera.GetComponent<CameraController>().updateMaxPos(battlefield.map.GetLength(0), battlefield.map.GetLength(1));
		}

		public void loadLevel() {
			removeAllUnits();
			removeAllGoals();
			eraseTiles();

			LevelInfo levelInfo = Serialization.getLevel(loadLevelText.text);
			mapName = levelInfo.mapName;
			currentObjective = levelInfo.objective;

			//add goal for each vector in list
			List<Coord> goalPositions = levelInfo.goalPositions;
			foreach (Coord pos in goalPositions) {
				addGoal((int)pos.x, (int)pos.y);
			}

			//set dropdown from saved objective
			objectiveDropdown.value = (int)currentObjective;
			reloadMap();

			try {
				Stack<UnitInfo> stack = levelInfo.units;
				while (stack.Count != 0) {
					UnitInfo info = stack.Pop();
					currentFaction = info.getFaction();
					addUnit(info.getUnitType(), info.getCoord().x, info.getCoord().y);
				}
				//reset faction to match dropdown
				currentFaction = (Faction)factionDropdown.value;
			} catch (FileNotFoundException ex) {
				Debug.LogError("Level does not exist.");
				Debug.LogError(ex);
			}
		}

		public override void create(Vector3Int coord, Unit unit) {
			//prefab array needs to match unittype enum
			addUnit((UnitType)currentIndex, coord.x, coord.y);
		}

		public override void remove(Vector3Int coord, Unit unit, RaycastHit hit) {

			if (hit.collider.gameObject.tag.Equals("Unit")) {
				unitsInfo[coord.x, coord.y] = null;
				Destroy(hit.collider.gameObject);
			}
		}

		private void addUnit(UnitType unitType, int x, int y) {
			if (unitsInfo[x, y] == null) {
				int index = (int)(unitType);
				int z = battlefield.map[x, y].Count + 1;
				GameObject newUnitGO = Instantiate(
					unitPrefabs[index],
					Util.GridToWorld(x, y, z),
					unitPrefabs[index].gameObject.transform.rotation);
				newUnitGO.transform.parent = unitsHolder;
				Unit newUnit = newUnitGO.GetComponent<Unit>();

				newUnit.setFaction(currentFaction);

				UnitInfo info = new UnitInfo(unitType, currentFaction, new Vector3Int(x, y, z));
				unitsInfo[x, y] = info;
			} else {
				Debug.Log("Cannot place units on top of each other.");
			}

		}

		private void removeAllUnits() {

			foreach (UnitInfo info in unitsInfo) {
				if (info != null) {
					unitsInfo[info.getCoord().x, info.getCoord().y] = null;
				}
			}

			foreach (Transform child in unitsHolder) {
				Destroy(child.gameObject);
			}
		}

		private void eraseTiles() {
			foreach (Transform child in tilesHolder) {
				Destroy(child.gameObject);
			}

		}

		private void resetTextBoxes() {
			loadMapText.text = "";
			loadLevelText.text = "";
			saveLevelText.text = "";
		}

	}
}