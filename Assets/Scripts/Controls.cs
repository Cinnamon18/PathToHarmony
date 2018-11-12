using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour {
	[SerializeField]
	private FadeOutTransition fade;
	[SerializeField]
	private Text anyKeyToContinue;

	public float blinkSpeed = 0.33f;
	private float currentAlpha = 0f;
	private bool increasing = true; 
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			fade.fadeToScene("Prologue");
		}

		if (increasing) {
			currentAlpha += (Time.deltaTime * blinkSpeed);
		} else {
			currentAlpha -= (Time.deltaTime * blinkSpeed);
		}

		if(currentAlpha > 1) {
			increasing = false;
		}

		if(currentAlpha < 0) {
			increasing = true;
		}

		Color temp = anyKeyToContinue.color;
		temp.a = currentAlpha;
		anyKeyToContinue.color = temp;
	}
}
