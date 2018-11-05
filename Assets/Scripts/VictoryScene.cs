using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScene : MonoBehaviour {

	// Use this for initialization
	public void goToMainMenu() {
		SceneManager.LoadScene("Title");
	}
}
