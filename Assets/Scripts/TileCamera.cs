using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCamera : MonoBehaviour {
    private Camera tileCamera;
    [SerializeField]
    private float height = 0.2f;

	// Use this for initialization
	void Start () {
        tileCamera = gameObject.GetComponent<Camera>();
        int dist = (int)(Screen.height * height);
        tileCamera.pixelRect = new Rect(Screen.width - dist, Screen.height - dist, dist, dist);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
