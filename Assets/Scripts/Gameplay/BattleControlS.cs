using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;

public class BattleControlS : MonoBehaviour {

    //TODO swap const for the one that you can set once, i think const is compile time fixed
    public const int mapXSize = 10;
    public const int mapYSize = 10;
    public const int mapZSize = 5;
    //x, y, height (from the bottom)
    private Terrain[,,] terrain;
    private Unit[,,] units;
    private GameObject highlightedObject;


    // Use this for initialization
    void Start () {
		units = new Unit[mapXSize, mapYSize, mapZSize];
        terrain = new Terrain[mapXSize, mapYSize, mapZSize];
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f)) {
            Vector3Int tileCoords = Util.WorldToGrid(hit.transform.position);
            //Terrain tile = hit.collider.gameObject.GetComponent<Terrain>();
            Terrain tile = terrain[tileCoords.x, tileCoords.y, tileCoords.z];
            Debug.Log(tile.terrain); // ensure you picked right object

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
