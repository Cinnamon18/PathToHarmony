using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	[SerializeField]
	private Canvas pauseCanvas;
	[SerializeField]
	private Canvas controls;
	[SerializeField]
	private FadeOutTransition fade;

	// Use this for initialization
	void Start() {
		controls.enabled = false;
		pauseCanvas.enabled = false;
	}

	void Update() {
		if (Input.GetButtonDown("Cancel")) {
			if (pauseCanvas.enabled == false) {
				showMenu();
			} else {
				resumeGame();
			}
		}
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

	public void showControls() {
		controls.enabled = true;
	}

	public void hideControls() {
		controls.enabled = false;
	}
}
