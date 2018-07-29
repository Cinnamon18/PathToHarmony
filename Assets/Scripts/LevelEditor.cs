using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using System.Text;
using System;
using System.Linq;
using Gameplay;

public class LevelEditor : MonoBehaviour {

	//x, y, height (from the bottom)
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


	// Use this for initialization
	void Start() {
		tiles = new Tile[10, 10, 5];

		//Just for testing
		for (int x = 0; x < tiles.GetLength(0); x++) {
			for (int y = 0; y < tiles.GetLength(1); y++) {
				GameObject newTile = Instantiate(tilePrefabs[currentTile], Util.GridToWorld(new Vector3Int(x, y, 0)), tilePrefabs[currentTile].transform.rotation);
				tiles[x, y, 0] = newTile.GetComponent<Tile>();
			}
		}

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
			GameObject newTile = Instantiate(tilePrefabs[currentTile], tile.gameObject.transform.position + new Vector3(0, Util.GridHeight, 0), tile.gameObject.transform.rotation);
			Tile newTileTile = newTile.GetComponent<Tile>();
			//This doesn't really feel right but, uh. It's how it's gonna be, at least for now. tilePrefabs[x].tile is always TileType.None and idk why :(
			newTileTile.tile = (TileType)(currentTile);
			tiles[tileCoords.x, tileCoords.y, (tileCoords.z + 1)] = newTile.GetComponent<Tile>();
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
		Tile[,,] newTile = new Tile[x, y, z];
		for (int i = 0; i < tiles.Length; i++) {
			for (int ii = 0; ii < tiles.GetLength(1); ii++) {
				for (int iii = 0; iii < tiles.GetLength(2); iii++) {
					newTile[i, ii, iii] = tiles[i, ii, iii];
				}
			}
		}
		tiles = newTile;
		drawBorders();
	}

	public void updateMapName(String newName) {
		this.mapName = newName;
	}

	public void updateOverwriteMode(bool state) {
		this.overwriteData = state;
	}

	public void drawBorders() {
		float height = Util.GridHeight * tiles.GetLength(2);
		float length = Util.GridWidth * tiles.Length;
		float zero = 0;
		Vector3[] positions = {
			new Vector3(zero, zero, zero),
			new Vector3(zero, zero, height),
			new Vector3(length, length, height),
			new Vector3(length, length, zero),
			new Vector3(zero, zero, zero),
			new Vector3(-length, length, zero),
			new Vector3(-length, length, height),
			new Vector3(zero, zero, height),
			new Vector3(-length, length, height),
			new Vector3(zero, Mathf.Sqrt(2) * length, height),
			new Vector3(zero, Mathf.Sqrt(2) * length, zero),
			new Vector3(-length, length, zero),
			new Vector3(zero, Mathf.Sqrt(2) * length, zero),
			new Vector3(length, length, zero),
			new Vector3(length, length, height),
			new Vector3(zero, Mathf.Sqrt(2) * length, height)
		};

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

		Serialization.WriteData(serialized.ToString(), mapName, overwriteData);
	}

	public void deserializeTiles() {
		tiles = Serialization.DeserializeTiles(Serialization.ReadData(mapName), tilePrefabs);
	}
}
