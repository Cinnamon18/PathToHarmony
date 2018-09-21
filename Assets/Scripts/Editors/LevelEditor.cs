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

namespace Editors {
	public class LevelEditor : Editor<Unit> {
		public Unit previewUnit;
		public string defaultMap;

		public Text loadMapText;
		public Text loadLevelText;
		public Text saveLevelText;
		
		public TMPro.TMP_Dropdown dropdown;
		private bool isPlayer;

		[SerializeField]
		private Vector3Int initialDim;

		public GameObject knight;

		private string mapFilePath = Serialization.mapFilePath;
		private string levelFilePath = Serialization.levelFilePath;

		private Stack<Tile>[,] tiles;
		[SerializeField]
		private GameObject[] tilePrefabs;

		private Level level;
		private Battlefield battlefield;
		[SerializeField]
		private GameObject[] unitPrefabs;
		private UnitInfo[,,] unitsInfo;

		public string levelName;
		public string mapName;


		
		// Use this for initialization
		void Start() {
			//height doesn't matter for units, battlefield takes care of it
			battlefield = new Battlefield();
			mapName = defaultMap;
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(mapName, mapFilePath), tilePrefabs);
			//objs are unit prefabs
			objs = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1),5];
			//unitsinfo is used to store units and whether they are player or enemy units
			unitsInfo = new UnitInfo[battlefield.map.GetLength(0), battlefield.map.GetLength(1), 5];
			level = new Level(defaultMap, null, null, null);
		
		}

		private void Update()
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

			Serialization.WriteData(serialized.ToString(), levelName, levelFilePath, true);
		}

		public override void deserialize() {

		}


		public void loadMap() {
			string file = loadMapText.text;
			tiles = Serialization.DeserializeTilesStack(Serialization.ReadData(file, defaultMap), tilePrefabs);
		}

		public override void create(Vector3Int coord, Unit unit) {
			addUnit(UnitType.Knight, coord.x, coord.y);
		}

		public override void remove(Vector3Int coord, Unit unit, RaycastHit hit) {
			
			Debug.Log(hit.collider.gameObject.tag);
			if (hit.collider.gameObject.tag.Equals("Unit"))
			{
				unitsInfo[coord.x, coord.y, coord.z] = null;
				Destroy(hit.collider.gameObject);
			}
			
			
		}

		public override void updatePreview(float scroll) {

		}

		private void addUnit(UnitType unitType,int x, int y)
		{
			int index = (int)(unitType);
			int z = battlefield.map[x, y].Count + 1;
			GameObject newUnitGO = Instantiate(
				unitPrefabs[index],
				Util.GridToWorld(x, y, z),
				unitPrefabs[index].gameObject.transform.rotation);

			Unit newUnit = newUnitGO.GetComponent<Unit>();
			UnitInfo info = new UnitInfo(unitType, isPlayer, new Vector3Int(x,y,z));
			unitsInfo[x, y, z] = info;
		}

	}
}