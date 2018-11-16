using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScene : MonoBehaviour {

	void Start() {
		Audio.playSound("MainTheme1", true, true);
	}

	[SerializeField]
	private FadeOutTransition fade;
	// Use this for initialization
	public void goToMainMenu() {
		fade.fadeToScene("Title");
	}
}
