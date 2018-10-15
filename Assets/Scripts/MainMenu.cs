using System.Collections;
using System.Collections.Generic;
using AI;
using Cutscenes.Stages;
using Gameplay;
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

		setupDefaultCampaign();
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

	private void setupDefaultCampaign() {

		//LEVEL 1
		Character[] characters1 = new[] {
				new Character("Alice", true, new playerAgent()),
				new Character("The evil lord zxqv", false, new simpleAgent())
				};
		Level level1 = new Level("DemoMap2", "TestLevel", characters1, new string[] { "tutorialEnd" });

		//LEVEL 2
		Character[] characters2 = new[] {
				new Character("Alice", true, new playerAgent()),
				new Character("The evil lord zxqv", false, new simpleAgent())
				};
		Level level2 = new Level("DemoMap1", "TestLevel", characters2, new string[] { "andysDemo" });

		//LEVEL 3
		Character[] characters3 = new[] {
				new Character("Alice", true, new playerAgent()),
				new Character("The evil lord zxqv", false, new simpleAgent())
				};
		Level level3 = new Level("DemoMap2", "DemoLevel", characters2, new string[] { "andysDemo" });


		//Just to show off my vision of campaign branching. which now looks like it's not gonna happen, but oh well :p
		Campaign testCampaign1 = new Campaign("test", 0, new[] {
			level1,
			level2
		});

		Campaign testCampaign2 = new Campaign("test2", 0, new[] {
			level3,
			level1
		});

		Persistance.campaign = testCampaign1;


	}
}
