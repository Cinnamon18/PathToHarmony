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
		private BattleLoopStage battleStage;

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
			battleStage = BattleLoopStage.Initial;
			turnPlayerText.enabled = false;
			getLevel();
			deserializeMap();
		}

		// Update is called once per frame
		void Update() {
			switch (battleStage) {
				case BattleLoopStage.Initial:
					advancePhase();
					break;
				case BattleLoopStage.Pick:
					//This is temp just for testing until I build pick phase.
					battlefield.addUnit(new Knight(), level.players[0], 0, 0);
					battlefield.addUnit(new Knight(), level.players[0], 1, 0);
					battlefield.addUnit(new Knight(), level.players[0], 0, 1);
					battlefield.addUnit(new Knight(), level.players[1], 8, 4);
					battlefield.addUnit(new Knight(), level.players[1], 8, 5);
					//TODO make game objects

					advancePhase();
					break;
				case BattleLoopStage.BattleLoopStart:
					advancePhase();
					break;
				case BattleLoopStage.TurnChange:
					//There's probably a less fragile way of doing this. It's just to make sure this call only happens once per turn loop.
					if (!turnPlayerText.enabled) {
						currentPlayer = (currentPlayer + 1) % level.players.Length;
						turnPlayerText.text = level.players[currentPlayer].name + "'s turn";
						turnPlayerText.enabled = true;
						Util.setTimeout(() => {
							advancePhase();
						}, 1000);
					}
					break;
				case BattleLoopStage.TurnChangeEnd:
					turnPlayerText.enabled = false;
					advancePhase();
					break;
				case BattleLoopStage.MoveSelection:
					//TODO: get player selection

					//Player input
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(ray, out hit, 1000.0f)) {
						Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
						//TODO: adjust this to allow players to select tiles...
						Unit unit = battlefield.units[tileCoords.x, tileCoords.y];

						//LMB
						if (Input.GetButtonDown("Select")) {
							highlightItem(hit);
						}
					}


					//If player has selected a move:
					//Play the animation
					advancePhase();
					break;
				case BattleLoopStage.EndTurn:
					advancePhase();
					break;
			}
		}

		private void highlightItem(RaycastHit hit) {
			//Deselect the old object
			if (highlightedObject != null) {
				unhighlightObject(highlightedObject);
			}

			//Deselect the currently selected one one if we're clicking on it
			if (highlightedObject == hit.collider.gameObject) {
				unhighlightObject(highlightedObject);
				highlightedObject = null;
			} else {
				//Select the new one
				highlightedObject = hit.collider.gameObject;
				highlightObject(highlightedObject);
			}
		}

		private void deserializeMap() {
			battlefield.map = Serialization.DeserializeTilesStack(Serialization.ReadData(level.mapFileName), tilePrefabs);
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

		//TODO: Find a way to programatically create a highlight effect. Shader??
		private void highlightObject(GameObject objectToHighlight) {
			Material[] materials = objectToHighlight.GetComponent<Renderer>().materials;
			foreach (Material material in materials) {
				//Set the main Color of the Material to green
				material.shader = Shader.Find("_Color");
				material.SetColor("_Color", Color.green);

				//Find the Specular shader and change its Color to red
				material.shader = Shader.Find("Specular");
				material.SetColor("_SpecColor", Color.red);
			}
		}

		private void unhighlightObject(GameObject objectToHighlight) {
			Material[] materials = objectToHighlight.GetComponent<Renderer>().materials;
			foreach (Material material in materials) {
				//Set the main Color of the Material to green
				material.shader = Shader.Find("_Color");
				material.SetColor("_Color", Color.blue);

				//Find the Specular shader and change its Color to red
				material.shader = Shader.Find("Specular");
				material.SetColor("_SpecColor", Color.red);
			}
		}

		//Convenience
		private void advancePhase() {
			battleStage = battleStage.NextPhase();
		}

		private void getLevel() {
			level = Persistance.campaign.levels[Persistance.campaign.levelIndex];
		}

	}
}