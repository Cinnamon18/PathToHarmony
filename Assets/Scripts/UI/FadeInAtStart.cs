using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAtStart : MonoBehaviour {

	private float alpha = 1.2f;
	public float speed = 0.35f;

	public RawImage image;

	// Use this for initialization
	void Start () {
		image.color = new Color (0,0,0,1);
		image.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		alpha = alpha - (Time.deltaTime * speed);
		float a = alpha;
		

		image.color = new Color (0,0,0, alpha);

		if (alpha <= -0.1) {
			image.enabled = false;
		}
	}
}
