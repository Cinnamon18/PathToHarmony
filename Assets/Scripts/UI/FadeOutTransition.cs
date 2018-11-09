using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOutTransition : MonoBehaviour {

	float fadeSpeed = 0.1f;
	public RawImage image;

	private float timer = 1;

	private string destination; //the scene to be loaded after this object fades
	private float alpha = 0;

	public float speed = 1.2f;
	private bool countdown = false;


	// Use this for initialization
	void Start () {
		image.color = new Color(0,0,0,0);
		image.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		image.color = new Color(0,0,0, Mathf.Pow(alpha, 2));
		if (countdown) {
			timer = timer - Time.deltaTime * speed;
			alpha = alpha + Time.deltaTime * speed;
			
		}

		if (countdown && alpha >= 1.05) {
			SceneManager.LoadScene(destination);
		}
	}

	public void fadeToScene(string sceneName) {
		Util.Log("Begine fade");


		destination = sceneName;
		countdown = true;
		image.enabled = true;

	}
}
