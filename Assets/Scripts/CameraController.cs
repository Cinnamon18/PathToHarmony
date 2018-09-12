using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
	/**
	 * The camera controls are as follows:
	 * Arrows/WASD/Shift+IJKL: Pan Camera
	 * IJKL/Shift+Arrows/Shift+WASD: Rotate Camera
	 * RMB + drag: Pan Camera
	 * MMB + drag: Rotate Camera
	 * E: Reset to default position
	 * Move mouse to edges of screen: Pan Camera
	 * Scroll wheel: control zoom
	 * R/F: control zoom
	 */
	//These are just private so they do not clutter the component box, but these can be made public and adjusted if desired.
	private float translateSpeedKeys = 15;  //The rate at which holding down the movement keys adjusts the linear momentum.
	private float rotateSpeedKeys = 15;  //The rate at which holding down the movement keys (while also holding the shift key) adjusts the linear momentum.
	private float zoomSpeedExp = 2.5f;	//The rate at which the camera exponentially zooms to achieve the desired distance.
	private float zoomSpeedLin = 1;		//The rate at which the camera linearally zooms to achieve the desired distance.
	private float zoomControlMultiplier = 40;   //The rate at which scrolling the mouse changes the zoom level of the camera.
	private float zoomControlMultiplierKeys = 1;
	private float minZoomDistance = -100;	//The maximum distance allowed for the camera zoom.
	private float maxZoomDistance = -20;	//The minimum distance allowed for the camera zoom.
	private float rotMomentumDecayExp = 3;		//The rate at which the camera exponentially slows down its rotation.
	private float rotMomentumDecayLin = 0.5f;   //The rate at which the camera linerally slows down its rotation.
	private float linMomentumDecayExp = 2;		//The rate at which the camera exponentially slows down its translation.
	private float linMomentumDecayLin = 1;		//The rate at which the camera linearally slows down its translation.
	private float rotateSpeed = 0.05f;      //The rate at which dragging the mouse while holding RMB adjusts the camera's rotational momentum.
	private float translateSpeed = 0.01f;		//The rate at which dragging the mouse while holding MMB adjusts the camera's translational momentum.
	public bool invertCameraX = false;	//Whether to invert the x axis when rotating the camera.
	public bool invertCameraY = false;  //Whether to invert the y axis when rotating the camera.
	public bool invertDragPan = false;  //Whether to invert the mouse dragging controls when sliding the camera around.
	private float translateSpeedBorder = 4f;			//The rate at which the camera slides due to positioning the mouse around the edges of the screen.
	private float panBorderThickness = 10f;	//The size of the border where the mouse will scroll.

	private Vector3 lastMousePos = Vector3.zero;	//The previous position of the mouse, used to calculate dragging.
	private Vector2 rotationMomentum = Vector2.zero;    //The rotational momentum of the camera, used to fluidly rotate the camera.
	private Vector2 linearMomentum = Vector2.zero;		//The translational momentum of the camera, used to fluidly move the camera.
	private float maintainedDistance = 1;	//The distance intended to be maintained between the camera and the terrain, adjusted via mouse wheel.
	private float lastPlaneViewed = 0;		//The y-coordinate of the tile being looked at. This is used to prevent things like trees and players from messing with the zoom.

	// Use this for initialization
	void Start() {
		maintainedDistance = (minZoomDistance + maxZoomDistance) / 2f;
		resetCamera();
	}

	// Update is called once per frame
	void Update() {

		//Reset everything by pressing the E button
		if (Input.GetAxis("InteractE") > 0) {
			resetCamera();
		}

		//BEGINNING OF SETUP SECTION
		//Before any input is received, the position of the camera is rewritten in terms of the point it is looking at and the distance between the camera and the point.

		RaycastHit hit;
		//This block tests if the camera is looking at a tile and readjusts the height of the current tile being looked at
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		if (Physics.Raycast(ray, out hit, 1000.0f)) {
			lastPlaneViewed = Util.WorldToGrid(hit.transform.position).z * 4;
		}
		float distance = Mathf.Abs(lastPlaneViewed - gameObject.transform.position.y) / (gameObject.transform.rotation * Vector3.forward).y;	//The distance between the camera and focus.
		Vector3 focus = gameObject.transform.position - (gameObject.transform.rotation * Vector3.forward) * distance; //The exact point being looked at.

		//END OF SETUP SECTION, BEGINNING OF INPUT SECTION

		//Mouse panning
		if (Input.mousePosition.y >= Screen.height - panBorderThickness) {
			linearMomentum.y += translateSpeedBorder * Time.deltaTime;
		}
		if (Input.mousePosition.y <= panBorderThickness) {
			linearMomentum.y -= translateSpeedBorder * Time.deltaTime;
		}
		if (Input.mousePosition.x >= Screen.width - panBorderThickness) {
			linearMomentum.x += translateSpeedBorder * Time.deltaTime;
		}
		if (Input.mousePosition.x <= panBorderThickness) {
			linearMomentum.x -= translateSpeedBorder * Time.deltaTime;
		}

		//W / S
		if (Input.GetAxis("Vertical") != 0) {
			if (Input.GetAxis("AltModifier") > 0) {
				rotationMomentum.y -= rotateSpeedKeys * Time.deltaTime * Input.GetAxis("Vertical");
			} else {
				linearMomentum.y += translateSpeedKeys * Time.deltaTime * Input.GetAxis("Vertical");
			}
		}
		//A / D
		if (Input.GetAxis("Horizontal") != 0) {
			if (Input.GetAxis("AltModifier") > 0) {
				rotationMomentum.x += rotateSpeedKeys * Time.deltaTime * Input.GetAxis("Horizontal");
			} else {
				linearMomentum.x -= translateSpeedKeys * Time.deltaTime * Input.GetAxis("Horizontal");
			}
		}
		//I / K
		if (Input.GetAxis("RotateVertical") != 0) {
			if (Input.GetAxis("AltModifier") > 0) {
				linearMomentum.y += translateSpeedKeys * Time.deltaTime * Input.GetAxis("RotateVertical");
			} else {
				rotationMomentum.y -= rotateSpeedKeys * Time.deltaTime * Input.GetAxis("RotateVertical");
			}
		}
		//J / L
		if (Input.GetAxis("RotateHorizontal") != 0) {
			if (Input.GetAxis("AltModifier") > 0) {
				linearMomentum.x += translateSpeedKeys * Time.deltaTime * Input.GetAxis("RotateHorizontal");
			} else {
				rotationMomentum.x -= rotateSpeedKeys * Time.deltaTime * Input.GetAxis("RotateHorizontal");
			}
		}

		//This block sets the linear and rotation momenta, changing them on mouse movement and slowing them down gradually.
		//Rotational momentum is adjusted by holding RMB and dragging, while translational is adjusted by holding MMB and dragging.
		if (Input.GetAxis("AltSelect") > 0) {
			if (Input.GetAxis("AltModifier") > 0) {
				rotationMomentum += new Vector2((Input.mousePosition.x - lastMousePos.x) * rotateSpeed, (Input.mousePosition.y - lastMousePos.y) * rotateSpeed);
			} else {
				linearMomentum += new Vector2((Input.mousePosition.x - lastMousePos.x) * translateSpeed, (Input.mousePosition.y - lastMousePos.y) * translateSpeed) * (invertDragPan ? 1 : -1);
			}
		}
		if (Mathf.Abs(linearMomentum.x) > (linMomentumDecayLin + linMomentumDecayExp * linearMomentum.x) * Time.deltaTime) {
			linearMomentum.x -= (linMomentumDecayLin + linMomentumDecayExp * linearMomentum.x * Mathf.Sign(linearMomentum.x)) * Time.deltaTime * Mathf.Sign(linearMomentum.x);
		} else {
			linearMomentum.x = 0;
		}
		if (Mathf.Abs(linearMomentum.y) > (linMomentumDecayLin + linMomentumDecayExp * linearMomentum.y) * Time.deltaTime) {
			linearMomentum.y -= (linMomentumDecayLin + linMomentumDecayExp * linearMomentum.y * Mathf.Sign(linearMomentum.y)) * Time.deltaTime * Mathf.Sign(linearMomentum.y);
		} else {
			linearMomentum.y = 0;
		}
		if (Input.GetAxis("MiddleSelect") > 0) {
			if (Input.GetAxis("AltModifier") > 0) {
				linearMomentum += new Vector2((Input.mousePosition.x - lastMousePos.x) * translateSpeed, (Input.mousePosition.y - lastMousePos.y) * translateSpeed) * (invertDragPan ? 1 : -1);
			} else {
				rotationMomentum += new Vector2((Input.mousePosition.x - lastMousePos.x) * rotateSpeed, (Input.mousePosition.y - lastMousePos.y) * rotateSpeed);
			}
		}
		if (Mathf.Abs(rotationMomentum.x) > (rotMomentumDecayLin + rotMomentumDecayExp * rotationMomentum.x) * Time.deltaTime) {
			rotationMomentum.x -= (rotMomentumDecayLin + rotMomentumDecayExp * rotationMomentum.x * Mathf.Sign(rotationMomentum.x)) * Time.deltaTime * Mathf.Sign(rotationMomentum.x);
		} else {
			rotationMomentum.x = 0;
		}
		if (Mathf.Abs(rotationMomentum.y) > (rotMomentumDecayLin + rotMomentumDecayExp * rotationMomentum.y) * Time.deltaTime) {
			rotationMomentum.y -= (rotMomentumDecayLin + rotMomentumDecayExp * rotationMomentum.y * Mathf.Sign(rotationMomentum.y)) * Time.deltaTime * Mathf.Sign(rotationMomentum.y);
		} else {
			rotationMomentum.y = 0;
		}
		lastMousePos = Input.mousePosition;

		//This has the mouse wheel control the current zoom distance.
		if (Input.GetAxis("MouseScrollWheel") != 0) {
			maintainedDistance += Input.GetAxis("MouseScrollWheel") * zoomControlMultiplier;
			maintainedDistance = Mathf.Clamp(maintainedDistance, minZoomDistance, maxZoomDistance);
		}

		//R / F
		if (Input.GetAxis("ButtonScroll") != 0) {
			maintainedDistance += Input.GetAxis("ButtonScroll") * zoomControlMultiplierKeys;
			maintainedDistance = Mathf.Clamp(maintainedDistance, minZoomDistance, maxZoomDistance);
		}

		//END OF INPUT SECTION, BEGINNING OF MOVEMENT SECTION

		//First, the focus is moved according to any translation inputs.
		focus += Quaternion.Euler(0,gameObject.transform.rotation.eulerAngles.y,0) * new Vector3(linearMomentum.x, 0, linearMomentum.y);
		var unclampedFocus = focus;
		focus = new Vector3(Mathf.Clamp(focus.x, -5, 10 * (Util.GridWidth) - 15), focus.y, Mathf.Clamp(focus.z, -5, 10 * (Util.GridWidth) - 15));
		if (!focus.Equals(unclampedFocus)) {
			linearMomentum = Vector2.zero;
		}

		//Next, the distance is increased or decreased, depending on the current intended distance (controlled by scrolling).
		//The difference between the real and ideal distances (denoted D) decreases at a rate of E*D+L, where E is the exponential decay factor and L is the linear decay factor.
		distance += zoomSpeedExp * Time.deltaTime * (maintainedDistance - distance);
		if (Mathf.Abs(maintainedDistance - distance) > zoomSpeedLin * Time.deltaTime) {
			distance += zoomSpeedLin * Mathf.Sign(maintainedDistance - distance) * Time.deltaTime;
		} else {
			distance = maintainedDistance;
		}

		//Lastly, the camera is rotated, and the camera's position is reestablished using the new focus, distance, and direction.
		Quaternion newDir = gameObject.transform.rotation;
		newDir.eulerAngles += new Vector3(-rotationMomentum.y * (invertCameraY ? -1 : 1 ), rotationMomentum.x * (invertCameraX ? -1 : 1), 0);
		float prevX = newDir.eulerAngles.x;
		newDir.eulerAngles = new Vector3(Mathf.Clamp(newDir.eulerAngles.x, 15, 75), newDir.eulerAngles.y, newDir.eulerAngles.z);
		if(prevX != newDir.eulerAngles.x) {
			rotationMomentum.y = 0;
		}
		gameObject.transform.position = focus + ((newDir) * Vector3.forward) * distance;
		gameObject.transform.rotation = newDir;
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

	public void resetCamera() {
		gameObject.transform.position = new Vector3(-1, 1, -1).normalized * maintainedDistance * -1;
		gameObject.transform.rotation = Quaternion.Euler(45, 45, 0);
	}
}
