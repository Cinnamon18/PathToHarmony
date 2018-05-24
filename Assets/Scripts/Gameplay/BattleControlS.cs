using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;

public class BattleControlS : MonoBehaviour {

    public const int mapXSize = 10;
    public const int mapYSize = 10;
    public Unit[,] units = new Unit[mapXSize, mapYSize];
    public Terrain[,] terrain = new Terrain[mapXSize, mapYSize];

	// Use this for initialization
	void Start () {
		
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
