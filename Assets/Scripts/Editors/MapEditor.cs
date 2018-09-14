using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constants;
using System.Text;
using System;
using System.Linq;
using Gameplay;

namespace Editors {
	public class MapEditor : Editor {

		//x, y, height (from the bottom)
		[SerializeField]
		private Vector3Int initialDim;
		//Oof so I realized after the fact that a 2D stack would be a better way to do this. However, it's abstracted by the serialization
		//layer, so this is perfectly funcitonal atm.... #TODO
		private Tile[,,] tiles;
		[SerializeField]
		private LineRenderer lineRenderer;
		[SerializeField]
		private GameObject previewTile;
		[SerializeField]
		private GameObject[] tilePrefabs;
		private int currentTile = 0;
		private string mapName;
		private bool overwriteData = false;

		public Text loadFileText;
		public Text loadDimText;


		// Use this for initialization
		void Start() {
			tiles = new Tile[initialDim.x, initialDim.y, initialDim.z];

			makeYellowBaseTiles();

			drawBorders();
		}

		void Update() {
			//Creation and deletion of tiles
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000.0f)) {
				Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
				//Tile tile = hit.collider.gameObject.GetComponent<Tile>();
				Tile tile = tiles[tileCoords.x, tileCoords.y, tileCoords.z];
				if (Input.GetButtonDown("Select")) {
					createTile(tileCoords, tile);
				} else if (Input.GetButtonDown("AltSelect")) {
					removeTile(tileCoords, tile, hit);
				}
			}

			updateTile(Input.GetAxis("MouseScrollWheel"));
		}

		public override void serialize() { }

		public override void deserialize() { }

		public void removeTile(Vector3Int tileCoords, Tile tile, RaycastHit hit) {
			//Remove tile. 
			bool removingBottom = tileCoords.z == 0;
			bool removingTop = tileCoords.z == tiles.GetLength(2) - 1;
			bool isTileAbove = false;
			if (!removingTop) {
				isTileAbove = tiles[tileCoords.x, tileCoords.y, (tileCoords.z + 1)] != null;
			}

			//You can't remove the bottom one, or if there's a tile above you
			if (!removingBottom && (removingTop || !isTileAbove)) {
				//refactor idea: 2d array of stacks
				tiles[tileCoords.x, tileCoords.y, (tileCoords.z)] = null;
				Destroy(hit.collider.gameObject);
			} else {
				//give the user some feedback that this is a badness
				Sfx.playSound("Bad noise");
				tile.vibrateUnhappily();
			}
		}

		public void createTile(Vector3Int tileCoords, Tile tile) {
			if (tileCoords.z == tiles.GetLength(2) - 1) {
				Sfx.playSound("Bad noise");
				tile.vibrateUnhappily();
			} else {
				GameObject newTileObj = Instantiate(tilePrefabs[currentTile], tile.gameObject.transform.position + new Vector3(0, Util.GridHeight, 0), tile.gameObject.transform.rotation);
				Tile newTile = newTileObj.GetComponent<Tile>();
				newTile.tileType = (TileType)(currentTile);
				tiles[tileCoords.x, tileCoords.y, (tileCoords.z + 1)] = newTile;
			}
		}

		private void updateTile(float scroll) {
			if (scroll != 0) {
				if (scroll < 0) {
					currentTile--;
				} else if (scroll > 0) {
					currentTile++;
				}

				//Why can't we all just agree on what % means? This makes it "warp back around". My gut says there's a more elegant way to do this, but....
				currentTile = currentTile < 0 ? currentTile + tilePrefabs.Length : currentTile % tilePrefabs.Length;

				GameObject oldPreviewTile = previewTile;
				previewTile = Instantiate(tilePrefabs[currentTile], oldPreviewTile.transform.position, oldPreviewTile.transform.rotation);
				previewTile.AddComponent<RotateGently>();
				Destroy(oldPreviewTile);
			}
		}

		public void updateSize(int x, int y, int z) {
			initialDim = new Vector3Int(x, y, z);
			eraseTiles();
			Start();


			// The code following was my noble attempt at doing this method in a way that didn't erase the whole board.
			// I saved it so that if I come back to it I'll have something to go off of. But if you're not me, and you
			// want to take a crack at making it work feel free to start over.
			// 
			// It would've made for much more convenient prototyping... but alas... the demo is due in a few weeks so
			// this is getting put off into the #TODO void. Sorry abt that map makers!


			//This method isn't especially readable... sorry abt that... It changes the board size.
			//Wow I really hate looking at this method. I sure hope no one ever has to maintain it.
			//Before we do anything, if we're shrinking the map delete the game objects we don't need
			//We have to do this maxDim nonsense because you might shrink in one dimension but increase in another.
			// Vector3Int maxDim = new Vector3Int(
			// 	Math.Max(x, tiles.GetLength(0)),
			// 	Math.Max(y, tiles.GetLength(1)),
			// 	Math.Max(z, tiles.GetLength(2)));
			// for (int i = 0; i < maxDim.x; i++) {
			// 	for (int ii = 0; ii < maxDim.y; ii++) {
			// 		for (int iii = 0; iii < maxDim.z; iii++) {
			// 			if (i > x || i > y || iii > z) {
			// 				if (tiles[i, ii, iii] != null) {
			// 					Destroy(tiles[i, ii, iii].gameObject);
			// 				}
			// 			}
			// 		}
			// 	}
			// }

			// //Copy the remaining ones over
			// Tile[,,] newTiles = new Tile[x, y, z];
			// for (int i = 0; i < x; i++) {
			// 	for (int ii = 0; ii < y; ii++) {
			// 		for (int iii = 0; iii < z; iii++) {
			// 			if (i < tiles.GetLength(0) && ii < tiles.GetLength(1) && iii < tiles.GetLength(2)) {
			// 				//Prevent out of bounds on "tiles" if dimensions are being increased
			// 				newTiles[i, ii, iii] = tiles[i, ii, iii];
			// 			} else if (iii == 0) {
			// 				//If size is being increased, fill in the bottom row with none tiles
			// 				//Mmm, juicy code duplication.
			// 				GameObject newTile = Instantiate(tilePrefabs[(int)(TileType.None)],
			// 					Util.GridToWorld(i, ii, 0),
			// 					tilePrefabs[(int)(TileType.None)].gameObject.transform.rotation);
			// 				Tile newTileTile = newTile.GetComponent<Tile>();
			// 				newTileTile.tile = TileType.None;
			// 				newTiles[i, ii, 0] = newTileTile;
			// 			}
			// 		}
			// 	}
			// }
			// tiles = newTiles;
			// drawBorders();
		}

		public void makeYellowBaseTiles() {
			for (int x = 0; x < tiles.GetLength(0); x++) {
				for (int y = 0; y < tiles.GetLength(1); y++) {
					GameObject newTile = Instantiate(tilePrefabs[(int)(TileType.None)], Util.GridToWorld(x, y, 0), tilePrefabs[currentTile].transform.rotation);
					tiles[x, y, 0] = newTile.GetComponent<Tile>();
				}
			}
		}

		public void incrementTile() {
			updateTile(1);
		}

		public void decrementTile() {
			updateTile(-1);
		}

		public void updateMapName(String newName) {
			this.mapName = newName;
		}

		public void updateOverwriteMode(bool state) {
			this.overwriteData = state;
		}


		public void deserializeTiles() {
			eraseTiles();
			updateMapName(loadFileText.text);
			tiles = Serialization.DeserializeTiles(Serialization.ReadMapData(mapName), tilePrefabs);
			makeYellowBaseTiles();
		}

		private void eraseTiles() {
			if (tiles != null) {
				foreach (Tile tile in tiles) {
					if (tile != null) {
						Destroy(tile.gameObject);
					}
				}
				tiles = null;
			}

		}

		public void drawBorders() {
			//Just a pinch more readable
			int w = tiles.GetLength(0);
			int l = tiles.GetLength(1);
			int h = tiles.GetLength(2);

			Vector3 offset = Util.GridToWorld(-0.5f, -0.5f, -0.8f);

			Vector3[] positions = new Vector3[] {
			Util.GridToWorld(0, 0, 0),
			Util.GridToWorld(0, 0, h),
			Util.GridToWorld(w, 0, h),
			Util.GridToWorld(w, 0, 0),
			Util.GridToWorld(0, 0, 0),
			Util.GridToWorld(0, l, 0),
			Util.GridToWorld(0, l, h),
			Util.GridToWorld(0, 0, h),
			Util.GridToWorld(0, l, h),
			Util.GridToWorld(w, l, h),
			Util.GridToWorld(w, l, 0),
			Util.GridToWorld(0, l, 0),
			Util.GridToWorld(w, l, 0),
			Util.GridToWorld(w, 0, 0),
			Util.GridToWorld(w, 0, h),
			Util.GridToWorld(w, l, h)
		};

			positions = positions.Select((pos) => {
				return pos + offset;
			}).ToArray();

			lineRenderer.SetPositions(positions);
		}

		public void serializeTiles() {
			if (mapName == null || mapName == "") {
				Debug.LogError("Can't save without a file name");
				return;
			}

			StringBuilder serialized = new StringBuilder(tiles.GetLength(0) + "," + tiles.GetLength(1) + "," + tiles.GetLength(2) + ",");
			Tile[] flattenedTile = Util.Flatten3DArray(tiles);

			foreach (Tile tile in flattenedTile) {
				if (tile == null) {
					serialized.Append(",");
				} else {
					serialized.Append(tile.serialize() + ",");
				}
			}

			Serialization.WriteMapData(serialized.ToString(), mapName, overwriteData);
		}

		public void updateSizeUI() {
			String newSize = loadDimText.text;
			int[] dimensions = newSize.Split(',').Select((dimension) => {
				//If you're used to java this might look weird. In c# you can explicitly pass by refrence with the keyword "out". Neat, isn't it?
				int num = 1;
				if (!Int32.TryParse(dimension, out num)) {
					Debug.LogError("Cannot parse provided dimension. Using default of 1");
				}
				return num;
			}).ToArray();
			updateSize(dimensions[0], dimensions[1], dimensions[2]);
		}
	}
}