using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCamera : MonoBehaviour {
    private Camera camera;
    [SerializeField]
    private float height = 0.2f;

	// Use this for initialization
	void Start () {
        camera = gameObject.GetComponent<Camera>();
        int dist = (int)(Screen.height * height);
        camera.pixelRect = new Rect(Screen.width - dist, Screen.height - dist, dist, dist);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
