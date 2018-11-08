using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseMenu : MonoBehaviour {

	[SerializeField]
	Canvas pauseCanvas;
	[SerializeField]
	private FadeOutTransition fade;

	// Use this for initialization
	void Start() {
		pauseCanvas.enabled = false;
	}

	public void showMenu() {
		pauseCanvas.enabled = true;
	}

	public void resumeGame() {
		pauseCanvas.enabled = false;
	}

	public void mainMenu() {
		Persistence.saveProgress();
		fade.fadeToScene("Title");
	}

	public void quitGame() {
		Persistence.saveProgress();
		Application.Quit();
	}
}
