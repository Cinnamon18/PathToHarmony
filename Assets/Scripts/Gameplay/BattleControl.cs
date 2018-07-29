using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;

namespace Gameplay {
	public class BattleControl : MonoBehaviour {

		//x, y, height (from the bottom)
		private Battlefield battlefield;
		private GameObject highlightedObject;
		public string mapName { get; set; }
		[SerializeField]
		private GameObject[] tilePrefabs;

		public void deserializeTerrain() {
			battlefield.map = Serialization.DeserializeTerrain(Serialization.ReadData(mapName), tilePrefabs);
		}

		// Use this for initialization
		void Start() {
			//Just for testing
			this.mapName = "asdf";

			
			deserializeTerrain();
		}

		// Update is called once per frame
		void Update() {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0f)) {
				Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
				IBattlefieldItem tile = battlefield.map[tileCoords.x, tileCoords.y, tileCoords.z];
				Util.Log(tile);

				//LMB
				if (Input.GetButtonDown("Select")) {
					if (highlightedObject != null) {
						//Deselect the old object
						Util.unhighlightObject(highlightedObject);
					}

					if (highlightedObject == hit.collider.gameObject) {
						//Deselect the currently selected one one if we're clicking on it
						Util.unhighlightObject(highlightedObject);
						highlightedObject = null;
					} else {
						//Select the new one
						highlightedObject = hit.collider.gameObject;
						Util.highlightObject(highlightedObject);
					}
				}
			}
		}
	}
}