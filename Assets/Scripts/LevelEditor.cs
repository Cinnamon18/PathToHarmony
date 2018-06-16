using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using System.Text;
using System;

public class LevelEditor : MonoBehaviour {

    //x, y, height (from the bottom)
    private Terrain[, ,] terrain;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private GameObject previewTile;
    [SerializeField]
    private GameObject[] tilePrefabs;
    private int currentTile = 0;
    private string mapName;
    private bool overwriteData;
    

    // Use this for initialization
    void Start () {
        terrain = new Terrain[10, 10, 5];
        //Just for testing
        for (int x = 0; x < terrain.GetLength(0); x++) {
            for (int y = 0; y < terrain.GetLength(1); y++) {
                GameObject newTile = Instantiate(tilePrefabs[currentTile], Util.GridToWorld(new Vector3(x, y, 0)), tilePrefabs[currentTile].transform.rotation);
                terrain[x, y, 0] = newTile.GetComponent<Terrain>();
            }
        }
        mapName = "testMap";
        overwriteData = true;

        drawBorders();
	}
	
	// Update is called once per frame
	void Update () {
        //Creation and deletion of tiles
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f)) {
            Vector3 tileCoords = Util.WorldToGrid(hit.transform.position);
            //Terrain tile = hit.collider.gameObject.GetComponent<Terrain>();
            Terrain tile = terrain[(int)tileCoords.x, (int)tileCoords.y, (int)tileCoords.z];
            if (Input.GetMouseButtonDown(0)) {
                //Create tile
                if (tileCoords.z == terrain.GetLength(2) - 1) {
                    Sfx.playSound("Bad noise");
                } else {
                    GameObject newTile = Instantiate(tilePrefabs[currentTile], tile.gameObject.transform.position + new Vector3(0, Util.GridHeight, 0), tile.gameObject.transform.rotation);
                    terrain[(int)tileCoords.x, (int)tileCoords.y, (int)(tileCoords.z + 1)] = newTile.GetComponent<Terrain>();
                }
            } else if (Input.GetMouseButtonDown(1)) {
                //Remove tile. 
                bool removingBottom = tileCoords.z == 0;
                bool removingTop = tileCoords.z == terrain.GetLength(2) - 1;
                bool isTileAbove = false;
                if (!removingTop) {
                    isTileAbove = terrain[(int)tileCoords.x, (int)tileCoords.y, (int)(tileCoords.z + 1)] != null;
                }

                //You can't remove the bottom one, or if there's a tile above you
                if (!removingBottom && (removingTop || !isTileAbove)) {
                    //pointless reimplementation idea: 2d array of stacks
                    terrain[(int)tileCoords.x, (int)tileCoords.y, (int)(tileCoords.z)] = null;
                    Destroy(hit.collider.gameObject);
                } else {
                    //give the user some feedback that this is a badness
                    Sfx.playSound("Bad noise");
                    tile.vibrateUnhappily();
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                serializeTerrain();
            }
        }

        updateTile(Input.GetAxis("Mouse ScrollWheel"));
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
        Terrain[, ,] newTerrain = new Terrain[x, y, z];
        for (int i = 0; i < terrain.Length; i++) {
            for (int ii = 0; ii < terrain.GetLength(1); ii++) {
                for (int iii = 0; iii < terrain.GetLength(2); iii++) {
                    newTerrain[i, ii, iii] = terrain[i, ii, iii];
                }
            }
        }
        terrain = newTerrain;
        drawBorders();
    }

    public void add(int x, int y, int z, Terrain data) {
        terrain[x, y, z] = data;
    }

    public Terrain remove(int x, int y, int z) {
        Terrain removed = terrain[x, y, z];
        terrain[x, y, z] = null;
        return removed;
    }

    public void drawBorders() {
        float height = Util.GridHeight * terrain.GetLength(2);
        float length = Util.GridWidth * terrain.Length;
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

    public void serializeTerrain() {
        StringBuilder serialized = new StringBuilder(terrain.GetLength(0) + "," + terrain.GetLength(1) + "," + terrain.GetLength(2) + ",");
        Terrain[] flattenedTerrain = new Terrain[terrain.GetLength(0) * terrain.GetLength(1) * terrain.GetLength(2)];
        for(int x = 0; x < terrain.GetLength(0); x++) {
            for(int y = 0; y < terrain.GetLength(1); y++) {
                for(int z = 0; z < terrain.GetLength(2); z++) {
                    flattenedTerrain[z + terrain.GetLength(1) * y + (x * terrain.GetLength(2) * terrain.GetLength(1))] = terrain[x,y,z];
                }
            }
        }

        for(int x = 0; x < flattenedTerrain.Length; x++) {
            serialized.Append(flattenedTerrain[x]);
        }

        Serialization.WriteData(serialized.ToString(), mapName, overwriteData);
    }
}
