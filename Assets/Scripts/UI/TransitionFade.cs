using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionFade : MonoBehaviour {


	float fadeSpeed = 0.1f;
	public RawImage image;

	private float timer;

	private string destination; //the scene to be loaded after this object fades
	private float alpha = 0;

	private float speed = 0.7f;


	// Use this for initialization
	void Start () {
		timer = 0;
		image.color = new Color(0,0,0,0);
		image.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		image.color = new Color(0,0,0, alpha);
		if (timer > 0) {
			timer = timer - Time.deltaTime * speed;
			alpha = alpha + Time.deltaTime * speed;
			Util.Log(timer);
		}

		if (timer <= 0) {
			SceneManager.LoadScene(destination);
		}
	}

	public void fadeToScene(string sceneName) {
		destination = sceneName;

		alpha = 0;
		image.enabled = true;
		timer = 1;
	}
}
