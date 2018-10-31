using System.Collections;
using System.Collections.Generic;
using AI;
using Cutscenes.Stages;
using Gameplay;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	private Canvas optionsCanvas;
	[SerializeField]
	private Canvas creditsCanvas;
	[SerializeField]
	private AudioMixer masterMixer;

	// Use this for initialization
	void Start() {
		optionsCanvas.enabled = false;
		creditsCanvas.enabled = false;

		setupDefaultCampaign();
	}

	public void setMasterVolume(float vol) {
		masterMixer.SetFloat("MasterVolume", vol);
	}

	public void setMusicVolume(float vol) {
		masterMixer.SetFloat("MusicVolume", vol);
	}

	public void setSfxVolume(float vol) {
		masterMixer.SetFloat("SfxVolume", vol);
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
				new Character("Alice", true, new PlayerAgent()),
				new Character("The evil lord zxqv", false, new SimpleAgent())
				};
		Level level1 = new Level("DemoMap2", "EasyVictory", characters1, new string[] { Stages.expressionShowOff, Stages.genericDefeat });

		//LEVEL 2
		Character[] characters2 = new[] {
				new Character("Blair", true, new PlayerAgent()),
				new Character("King Xingata", false, new SimpleAgent())
				};
		Level level2 = new Level("DemoMap", "test", characters2, new string[] { Stages.andysDemo, Stages.genericDefeat});

		//LEVEL 3
		Character[] characters3 = new[] {
				new Character("Blair", true, new PlayerAgent()),
				new Character("evil!Juniper", false, new SimpleAgent())
				};
		Level level3 = new Level("DemoMap", "DemoLevel", characters2, new string[] { Stages.andysDemo, Stages.genericDefeat });


		//Just to show off my vision of campaign branching. which now looks like it's not gonna happen, but oh well :p
		Campaign testCampaign1 = new Campaign("test1", 0, new[] {
			level1,
			level2
		});

		Campaign testCampaign2 = new Campaign("test2", 0, new[] {
			level1,
			level3
		});

		Persistance.campaign = testCampaign1;
		Persistance.loadProgress();


	}
}
