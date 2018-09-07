using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float speed = 15;
    public float panSpeed = 4f;
    public float panBorderThickness = 10f;
    public Vector3 panLimit;
    Vector3 forward, right;
    // Use this for initialization
    void Start() {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

	// Update is called once per frame
	void Update() {
        //TODO: Consider scrolling when the mouse is on the edge of the screen
        //W  / S
        Vector3 pos = gameObject.transform.position;
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos += forward * panSpeed * 2 * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.z, panLimit.z);
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos += forward * panSpeed * 2 * Time.deltaTime * -1;
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.z, panLimit.z);
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos += right * panSpeed * 2 * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.z, panLimit.z);
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos += right * panSpeed * 2 * Time.deltaTime * -1;
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.z, panLimit.z);
        }
        gameObject.transform.position = pos;
        if (Input.GetAxis("Vertical") != 0) {
			gameObject.transform.Translate(new Vector3(1, 0, 1) * speed * Time.deltaTime * Input.GetAxis("Vertical"), Space.World);
		}
        
        //A / D
        if (Input.GetAxis("Horizontal") != 0) {
			gameObject.transform.Translate(new Vector3(-1, 0, 1) * speed * Time.deltaTime * Input.GetAxis("Horizontal"), Space.World);
		}
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
