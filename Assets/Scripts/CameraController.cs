using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float speed = 15;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		//TODO: Consider scrolling when the mouse is on the edge of the screen
		//W  / S
		if (Input.GetAxis("Vertical") != 0) {
			gameObject.transform.Translate(new Vector3(1, 0, 1) * speed * Time.deltaTime * Input.GetAxis("Vertical"), Space.World);
		}
		//A / D
		if (Input.GetAxis("Horizontal") != 0) {
			gameObject.transform.Translate(new Vector3(-1, 0, 1) * speed * Time.deltaTime * Input.GetAxis("Horizontal"), Space.World);
		}
		//S
		// if (Input.GetAxis("Vertical") < 0) {
		// 	gameObject.transform.Translate(new Vector3(-1, 0, -1) * speed * Time.deltaTime, Space.World);
		// }
		//D
		// if (Input.GetAxis("Horizontal") > 0) {
		// 	gameObject.transform.Translate(new Vector3(1, 0, -1) * speed * Time.deltaTime, Space.World);
		// }
		//E
		if (Input.GetAxis("InteractE") > 0) {
			snapToGrid();
		}
	}

	public void snapToGrid() {
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
