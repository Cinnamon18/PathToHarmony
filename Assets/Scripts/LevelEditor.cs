using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class LevelEditor : MonoBehaviour {

    //X, y, height (from the bottom)
    private Terrain[, ,] terrain;
    [SerializeField]
    private LineRenderer lineRenderer;
    private GameObject highlightedObject;

    // Use this for initialization
    void Start () {
        terrain = new Terrain[10, 10, 5];
        //Just for testing
        terrain[0, 0, 0] = new Terrain(TerrainType.None);
        terrain[1, 0, 0] = new Terrain(TerrainType.None);
        terrain[0, 1, 0] = new Terrain(TerrainType.None);
        terrain[1, 1, 0] = new Terrain(TerrainType.None);

        drawBorders();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f)) {
            Vector3 tileCoords = Util.WorldToGrid(hit.transform.position);
            //Terrain tile = hit.collider.gameObject.GetComponent<Terrain>();
            Terrain tile = terrain[(int)tileCoords.x, (int)tileCoords.y, (int)tileCoords.z];
            Debug.Log(tile.terrain); // ensure you picked right object

            if (Input.GetMouseButtonDown(0)) {
                
            }
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
}
