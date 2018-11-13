using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAtStart : MonoBehaviour {

	private float alpha = 1.0f;
	public float speed = 1f;

	public RawImage image;

	// Use this for initialization
	void Start() {
		image.color = new Color(0, 0, 0, 1);
		image.enabled = true;
	}

	// Update is called once per frame
	void Update() {
		alpha -= (Time.deltaTime * speed);

		image.color = new Color(0, 0, 0, Mathf.Log(alpha + 1, 2));
		// image.color = new Color(0, 0, 0, alpha);

		if (alpha < 0) {
			image.enabled = false;
			Destroy(this.gameObject);
		}
	}
}
