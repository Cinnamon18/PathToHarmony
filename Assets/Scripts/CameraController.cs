using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float speed = 15;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W)) {
            gameObject.transform.Translate(new Vector3(1, 0, 1) * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A)) {
            gameObject.transform.Translate(new Vector3(-1, 0, 1) * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S)) {
            gameObject.transform.Translate(new Vector3(-1, 0, -1) * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D)) {
            gameObject.transform.Translate(new Vector3(1, 0, -1) * speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            //Snap to grid
            float xDistance, zDistance;
            float xPos = gameObject.transform.position.x;
            float zPos = gameObject.transform.position.z;
            if (xPos % Util.GridWidth > Util.GridWidth / 2) {
                xDistance = Util.GridWidth - (xPos % Util.GridWidth);
            } else {
                xDistance = -(xPos % Util.GridWidth);
            }
            if (zPos % Util.GridWidth > Util.GridWidth / 2) {
                zDistance = Util.GridWidth - (zPos % Util.GridWidth);
            } else {
                zDistance = -(zPos % Util.GridWidth);
            }
            gameObject.transform.Translate(xDistance, 0, zDistance, Space.World);
        }
    }
}
