using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {
	void Update() {
		Camera camera = Camera.main;
		if (camera != null) {
			transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
			transform.Rotate(new Vector3(0, 0, 90)); //One day i'll learn quaternions :p
		}
	}
}
