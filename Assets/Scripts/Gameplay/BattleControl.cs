using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Constants;
using UnityEngine.UI;
using System;

namespace Gameplay {
	public class BattleControl : MonoBehaviour {

		//x, y, height (from the bottom)
		private Battlefield battlefield;
		private GameObject highlightedObject;
		private Level level;
		[SerializeField]
		private GameObject[] tilePrefabs;

		private int currentPlayer;
		[SerializeField]
		private Text turnPlayerText;

		// Use this for initialization
		void Start() {

			//Just for testing because we don't have any way to set the campaign yet:
			Character[] characters = new[] { new Character("Alice"), new Character("Just some guy") };
			Vector2Int[] aliceMoves = new[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0) };
			Vector2Int[] evilGuyMoves = new[] { new Vector2Int(8, 4), new Vector2Int(8, 5) };
			Vector2Int[][] validPickTiles = new[] { aliceMoves, evilGuyMoves };
			Level level = new Level("DemoMap", characters, new[] { 1000, 1000 }, validPickTiles);
			Campaign testCampaign = new Campaign("test", 0, new[] { level });
			Persistance.campaign = testCampaign;


			battlefield = new Battlefield();
			getLevel();
			deserializeMap();
		}

		// Update is called once per frame
		void Update() {
			
			//Player input
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0f)) {
				Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
				IBattlefieldItem tile = battlefield.map[tileCoords.x, tileCoords.y, tileCoords.z];

				//LMB
				if (Input.GetButtonDown("Select")) {
					highlightItem(hit);
				}
			}
		}

		private void advanceTurnPlayer() {
			currentPlayer = (currentPlayer + 1) % level.players.Length;
			this.turnPlayerText.text = level.players[currentPlayer] + "'s turn";
		}

		private void highlightItem(RaycastHit hit) {
			//Deselect the old object
			if (highlightedObject != null) {
				Util.unhighlightObject(highlightedObject);
			}

			//Deselect the currently selected one one if we're clicking on it
			if (highlightedObject == hit.collider.gameObject) {
				Util.unhighlightObject(highlightedObject);
				highlightedObject = null;
			} else {
				//Select the new one
				highlightedObject = hit.collider.gameObject;
				Util.highlightObject(highlightedObject);
			}
		}

		private void deserializeMap() {
			battlefield.map = Serialization.DeserializeTiles(Serialization.ReadData(level.mapFileName), tilePrefabs);
			foreach (IBattlefieldItem tileMaybe in battlefield.map) {
				Tile tile = tileMaybe as Tile;
				if (tile != null) {
					if (tile.tileType == TileType.None) {
						Destroy(tile.gameObject);
						tile = null;
					}
				}
			}
		}

		private void getLevel() {
			level = Persistance.campaign.levels[Persistance.campaign.levelIndex];
		}

	}
}