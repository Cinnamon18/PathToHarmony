﻿using System.Collections;
using System.Collections.Generic;
using AI;
using Cutscenes.Stages;
using Gameplay;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	private Canvas optionsCanvas;
	[SerializeField]
	private Canvas creditsCanvas;
	[SerializeField]
	private AudioMixer masterMixer;

	public FadeOutTransition transition;

	// Use this for initialization
	void Start() {
		optionsCanvas.enabled = false;
		creditsCanvas.enabled = false;

		//setup audio sliders
		Persistance.loadAudioSettings(masterMixer);
		Slider[] optionsSliders = optionsCanvas.GetComponentsInChildren<Slider>();
		optionsSliders[0].value = PlayerPrefs.GetFloat(Persistance.MASTER_VOLUME);
		optionsSliders[1].value = PlayerPrefs.GetFloat(Persistance.MUSIC_VOLUME);
		optionsSliders[2].value = PlayerPrefs.GetFloat(Persistance.SFX_VOLUEME);

		setupDefaultCampaign();
	}

	public void setMasterVolume(float vol) {
		masterMixer.SetFloat(Persistance.MASTER_VOLUME, vol);
		Persistance.saveAudioSettings(masterMixer);
	}

	public void setMusicVolume(float vol) {
		masterMixer.SetFloat(Persistance.MUSIC_VOLUME, vol);
		Persistance.saveAudioSettings(masterMixer);
	}

	public void setSfxVolume(float vol) {
		masterMixer.SetFloat(Persistance.SFX_VOLUEME, vol);
		Persistance.saveAudioSettings(masterMixer);
	}

	public void playGame() {
		Persistance.saveProgress();
		//SceneManager.LoadScene("DemoBattle");
		transition.fadeToScene("DemoBattle");
	}

	public void resumeGame() {
		playGame();
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
				new Character("The evil lord zxqv", false, new eliminationAgent())
				};
		Level level1 = new Level("DemoMap2", "EasyVictory", characters1, new Cutscene[] { new ExpressionShowOff(), new GenericDefeat() });

		//LEVEL 2
		Character[] characters2 = new[] {
				new Character("Blair", true, new PlayerAgent()),
				new Character("King Xingata", false, new SimpleAgent())
				};
		Level level2 = new Level("DemoMap", "test", characters2, new Cutscene[] { new AndysDemo(), new GenericDefeat() });

		//LEVEL 3
		Character[] characters3 = new[] {
				new Character("Blair", true, new PlayerAgent()),
				new Character("evil!Juniper", false, new SimpleAgent())
				};
		Level level3 = new Level("DemoMap", "DemoLevel", characters3, new Cutscene[] { new AndysDemo(), new GenericDefeat() });


		//Just to show off my vision of campaign branching. which now looks like it's not gonna happen, but oh well :p
		Campaign testCampaign1 = new Campaign("test1", 0, new[] {
			level1,
			level2
		});

		Campaign testCampaign2 = new Campaign("test2", 0, new[] {
			level1,
			level3
		});

		// Persistance.campaign = testCampaign1;



		Level capture = new Level("DemoMap", "CaptureTest", characters3, new Cutscene[] { new ExpressionShowOff() });
		Level defend = new Level("DemoMap", "DefendTest", characters3, new Cutscene[] { new GenericDefeat() });
		Level escort = new Level("DemoMap", "EscortTest", characters3, new Cutscene[] { new GenericDefeat() });
		Level intercept = new Level("DemoMap", "InterceptTest", characters3, new Cutscene[] { new GenericDefeat() });

		Campaign gameModeShowOff = new Campaign("gamemode show off", 0, new[] {
			capture, defend, escort, intercept
		});

		Persistance.campaign = gameModeShowOff;


	}
}
