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

namespace Editors {
	public class LevelEditor : Editor<Unit> {
		public Unit previewUnit;
		//public Unit[,,] units;
		public string defaultMap;

		public Text loadMapText;
		public Text loadLevelText;

		[SerializeField]
		private Vector3Int initialDim;

		public GameObject knight;

		private string mapFilePath = "./Assets/Maps/";
		private string levelFilePath = "./Assets/Levels/";

		private Stack<Tile>[,] tiles;
		[SerializeField]
		private GameObject[] tilePrefabs;

		private Level level;
		private Battlefield battlefield;
		[SerializeField]
		private GameObject[] unitPrefabs;

		private ArrayList unitInfo;
		private string levelName = "testLevel";
		// Use this for initialization
		void Start() {
			//tiles = Serialization.DeserializeTilesStack(Serialization.ReadData(defaultMap, mapFilePath), tilePrefabs);
			unitInfo = new ArrayList();
			battlefield = new Battlefield();
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(defaultMap, mapFilePath), tilePrefabs);
			objs = new Unit[battlefield.map.GetLength(0), battlefield.map.GetLength(1), 5];
			Character[] characters = new[] {
				new Character("Alice", true, new PlayerAgent(battlefield, null, obj => Destroy(obj, 0) )),
				//new Character("The evil lord zxqv", false, new PlayerAgent(battlefield, null, obj => Destroy(obj, 0)))
				new Character("The evil lord zxqv", false, new simpleAgent(battlefield, null, obj => Destroy(obj, 0)))
				};
			List<Coord> alicePickTiles = new List<Coord> { new Coord(0, 0), new Coord(0, 1), new Coord(1, 0) };
			List<Coord> evilGuyPickTiles = new List<Coord> { new Coord(3, 7), new Coord(7, 4) };
			Dictionary<Character, List<Coord>> validPickTiles = new Dictionary<Character, List<Coord>>();
			level = new Level(defaultMap, characters, null, validPickTiles);
			validPickTiles[characters[0]] = alicePickTiles;
			validPickTiles[characters[1]] = evilGuyPickTiles;
		}

		public override void serialize() {
			if (levelName == null || levelName == "")
			{
				Debug.LogError("Can't save without a file name");
				return;
			}

			StringBuilder serialized = new StringBuilder(unitInfo.Count + ",");

			foreach (UnitInfo info in unitInfo)
			{
				if (info == null)
				{
					serialized.Append(",");
				}
				else
				{
					serialized.Append(info.serialize() + ",");
				}
			}

			Serialization.WriteData(serialized.ToString(), levelName, "./Assets/Levels/" + levelName + ".txt", true);
		}

		public override void deserialize() { }

		public void loadMap() {
			string file = loadMapText.text;
			tiles = Serialization.DeserializeTilesStack(Serialization.ReadData(file, mapFilePath), tilePrefabs);
		}

		public override void create(Vector3Int coord, Unit unit) {
			addUnit(UnitType.Knight, level.characters[0], coord.x, coord.y);
			//Tile newTile = newKnightObj.GetComponent<Tile>();
			//newTile.tileType = (TileType)(currentTile);
			//tiles[coord.x, coord.y, (coord.z + 1)] = newTile;
		}

		public override void remove(Vector3Int coord, Unit unit, RaycastHit hit) {

		}

		public override void updatePreview(float scroll) {

		}

		private void addUnit(UnitType unitType, Character character, int x, int y)
		{
			int index = (int)(unitType);

			GameObject newUnitGO = Instantiate(
				unitPrefabs[index],
				Util.GridToWorld(x, y, battlefield.map[x, y].Count + 1),
				unitPrefabs[index].gameObject.transform.rotation);

			Unit newUnit = newUnitGO.GetComponent<Unit>();
			UnitInfo info = new UnitInfo(unitType, true, new Vector3Int(x, y, battlefield.map[x, y].Count + 1));
			unitInfo.Add(info);
			//battlefield.addUnit(newUnit, character, x, y);
		}
	}
}