using System.Collections;
using System.Collections.Generic;
using Cutscenes.Stages;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	Canvas optionsCanvas;
	[SerializeField]
	Canvas creditsCanvas;

	// Use this for initialization
	void Start() {
		optionsCanvas.enabled = false;
		creditsCanvas.enabled = false;
		Stages.setupCutscenes();
	}

	public void playGame() {
		SceneManager.LoadScene("DemoBattle");
	}

	public void showOptions() {
		optionsCanvas.enabled = true;
	}

	public void hideOptions() {
		optionsCanvas.enabled = false;
	}

	public void showCredits() {
		creditsCanvas.enabled = true;
	}

	public void hideCredits() {
		creditsCanvas.enabled = false;
	}

	public void quitGame() {
		Application.Quit();
	}
}
