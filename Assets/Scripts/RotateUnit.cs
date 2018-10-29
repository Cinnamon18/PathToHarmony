using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUnit : MonoBehaviour {

    [SerializeField]
    private float speed = 20f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
