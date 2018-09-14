using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Gameplay;
using UnityEngine.UI;

namespace Editors {
	public class LevelEditor : Editor {
		public Unit previewUnit;
		public Unit[,,] units;
		public string defaultMap;

		public Text loadMapText;
		public Text loadLevelText;

		private Stack<Tile>[,] tiles;
		[SerializeField]
		private GameObject[] tilePrefabs;
		// Use this for initialization
		void Start() {
			tiles = Serialization.DeserializeTilesStack(Serialization.ReadMapData(defaultMap), tilePrefabs);
		}

		// Update is called once per frame
		void Update() {
			//Creation and deletion of tiles
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0f)) {
				Vector3Int unitCoords = Util.WorldToGrid(hit.transform.position);
				//Tile tile = hit.collider.gameObject.GetComponent<Tile>();
				Unit unit = units[unitCoords.x, unitCoords.y, unitCoords.z];
				if (Input.GetButtonDown("Select")) {
					createUnit(unitCoords, unit);
				} else if (Input.GetButtonDown("AltSelect")) {
					removeUnit(unitCoords, unit, hit);
				}
			}

			updatePreview(Input.GetAxis("MouseScrollWheel"));
		}


		public override void serialize() { }

		public override void deserialize() { }

		public void loadMap() {
			tiles = Serialization.DeserializeTilesStack(Serialization.ReadMapData(loadMapText.text), tilePrefabs);
		}

		private void createUnit(Vector3Int coord, Unit unit) {

		}

		private void removeUnit(Vector3Int coord, Unit unit, RaycastHit hit) {

		}

		private void updatePreview(float scroll) {

		}
	}
}