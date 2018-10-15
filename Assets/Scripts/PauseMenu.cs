using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	[SerializeField]
	Canvas pauseCanvas;

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
		Persistance.save();
		SceneManager.LoadScene("Title");
	}

	public void quitGame() {
		Persistance.save();
		Application.Quit();
	}
}
