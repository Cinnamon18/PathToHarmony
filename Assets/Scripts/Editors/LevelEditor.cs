using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Gameplay;
using UnityEngine.UI;
using Constants;
using AI;
using System.Collections;
using System.Text;
using System.ComponentModel;
using TMPro;
using System.IO;

namespace Editors {
	public class LevelEditor : Editor<Unit> {
		public string defaultMap;

		public Text loadMapText;
		public Text loadLevelText;
		public Text saveLevelText;
		
		public TMPro.TMP_Dropdown dropdown;
		private bool isPlayer;

		public Transform unitsHolder;

		private string mapName;
		private string levelName;

		private Stack<Tile>[,] tiles;
		[SerializeField]
		private GameObject[] tilePrefabs;

		private Battlefield battlefield;
		[SerializeField]
		private GameObject[] unitPrefabs;
		private UnitInfo[,] unitsInfo;

		Faction playerFaction = Faction.Xingata;
		Faction enemyFaction = Faction.Corbita;
		
		// Use this for initialization
		void Start() {
			//use battlefield to help place units
			battlefield = new Battlefield();
			mapName = defaultMap;
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(mapName, mapFilePath), tilePrefabs);
			//objs are unit prefabs
			//MIGHT be problem with setting z to 5 in tall maps, might need way to store map max height
			objs = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1), 5];
			//unitsinfo is used to store units and whether they are player or enemy units
			unitsInfo = new UnitInfo[battlefield.map.GetLength(0), battlefield.map.GetLength(1)];
			isPlayer = true;
			if (previewObj.Length != unitPrefabs.Length)
			{
				Debug.LogError("Must have equal number of prefab and preview objects.");
			}
		
		}

		private void LateUpdate()
		{
			
			dropdown.onValueChanged.AddListener(delegate {
				//player when 0
				isPlayer = (dropdown.value == 0);
			});
		
		}

		public override void serialize() {
			levelName = saveLevelText.text;
			if (levelName == null || levelName == "")
			{
				Debug.LogError("Can't save without a file name");
				return;
			}

			StringBuilder serialized = new StringBuilder(mapName + ";" );

			foreach (UnitInfo info in unitsInfo)
			{
				if (info == null)
				{
					serialized.Append(";");
				}
				else
				{
					serialized.Append(info.serialize() + ";" );
				}
			}

			Serialization.WriteData(serialized.ToString(), levelName, levelFilePath, overwriteData);
		}

		public override void deserialize() {

		}


		public void loadMap() {
			mapName = loadMapText.text;
			reloadMap();
			removeAllUnits();
		}
		private void reloadMap()
		{
			tiles = Serialization.DeserializeTilesStack(Serialization.ReadData(mapName, mapFilePath), tilePrefabs);
		}

		public void loadLevel()
		{
			removeAllUnits();

			LevelInfo levelInfo = Serialization.getLevel(loadLevelText.text);
			mapName = levelInfo.mapName;
			reloadMap();
			
			try
			{
				Stack<UnitInfo> stack = levelInfo.units;
				while (stack.Count != 0)
				{
					UnitInfo info = stack.Pop();
					isPlayer = info.getIsPlayer();
					addUnit(info.getUnitType(), info.getCoord().x, info.getCoord().y);
				}
				//reset bool to match dropdown
				isPlayer = dropdown.value == 0;
			} catch (FileNotFoundException ex)
			{
				Debug.Log("Incorrect file name at line 188 TestLevelEditor");
			}
		}

		public override void create(Vector3Int coord, Unit unit) {
			addUnit((UnitType) currentIndex, coord.x, coord.y);
		}

		public override void remove(Vector3Int coord, Unit unit, RaycastHit hit) {
			
			if (hit.collider.gameObject.tag.Equals("Unit"))
			{
				unitsInfo[coord.x, coord.y] = null;
				Destroy(hit.collider.gameObject);
			}	
			
		}

		private void addUnit(UnitType unitType,int x, int y)
		{
			if (unitsInfo[x, y] == null)
			{
				int index = (int)(unitType);
				int z = battlefield.map[x, y].Count + 1;
				GameObject newUnitGO = Instantiate(
					unitPrefabs[index],
					Util.GridToWorld(x, y, z),
					unitPrefabs[index].gameObject.transform.rotation);
				newUnitGO.transform.parent = unitsHolder;
				Unit newUnit = newUnitGO.GetComponent<Unit>();
				Faction currentFaction;
				//set material to differentiate between player and enemy
				//TODO: will change later when user can choose unit of a particular faction
				if (isPlayer)
				{
					currentFaction = playerFaction;
				}
				else
				{
					currentFaction = enemyFaction;
				}
				newUnit.setFaction(currentFaction);

				UnitInfo info = new UnitInfo(unitType, isPlayer, new Vector3Int(x, y, z));
				unitsInfo[x, y] = info;
			}
			else
			{
				Debug.Log("Cannot place units on top of each other.");
			}
			
		}

		private void removeAllUnits()
		{
			foreach(UnitInfo info in unitsInfo)
			{
				if (info != null)
				{
					unitsInfo[info.getCoord().x, info.getCoord().y] = null;
				}
			}

			foreach(Transform child in unitsHolder){
				Destroy(child.gameObject);
			}
		}

	}
}