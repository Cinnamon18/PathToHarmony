using System.Collections;
using System.Collections.Generic;
using AI;
using Cutscenes.Stages;
using Gameplay;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	private Canvas optionsCanvas;
	[SerializeField]
	private Canvas creditsCanvas;
	[SerializeField]
	private AudioMixer masterMixer;
	[SerializeField]
	private Button muteButton;
	[SerializeField]
	private FadeOutTransition fade;


	// Use this for initialization
	void Start() {
		optionsCanvas.enabled = false;
		creditsCanvas.enabled = false;

		//setup audio sliders
		Persistence.loadAudioSettings(masterMixer);
		Slider[] optionsSliders = optionsCanvas.GetComponentsInChildren<Slider>();
		optionsSliders[0].value = Persistence.MasterVolume;//PlayerPrefs.GetFloat(Persistence.MASTER_VOLUME);
		optionsSliders[1].value = Persistence.MusicVolume;//PlayerPrefs.GetFloat(Persistence.MUSIC_VOLUME);
		optionsSliders[2].value = Persistence.SfxVolume;//PlayerPrefs.GetFloat(Persistence.SFX_VOLUME);
		muteButton.GetComponentInChildren<Text>().text = (Persistence.IsMuted ? "Audio Muted" : "Audio Not Muted");

		setupDefaultCampaign();
	}

	public void setMasterVolume(float vol) {
		Persistence.MasterVolume = vol;
		Persistence.saveAudioSettings(masterMixer);
	}

	public void setMusicVolume(float vol) {
		Persistence.MusicVolume = vol;
		Persistence.saveAudioSettings(masterMixer);
	}

	public void setSfxVolume(float vol) {
		Persistence.SfxVolume = vol;
		Persistence.saveAudioSettings(masterMixer);
	}

	public void toggleMuteAudio() {
		Persistence.IsMuted = !Persistence.IsMuted;
		muteButton.GetComponentInChildren<Text>().text = (Persistence.IsMuted ? "Audio Muted" : "Audio Not Muted");
		Persistence.saveAudioSettings(masterMixer);
	}

	public void playGame() {
		Persistence.saveProgress();
		fade.fadeToScene("DemoBattle");
	}

	public void resumeGame() {
		Persistence.loadProgress();
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

		// //LEVEL 1
		// Character[] characters1 = new[] {
		// 		new Character("Alice", true, new PlayerAgent()),
		// 		new Character("The evil lord zxqv", false, new eliminationAgent())
		// 		};
		// Level level1 = new Level("DemoMap2", "EasyVictory", characters1, new Cutscene[] { new AndysDemo(), new ExpressionShowOff(), new GenericDefeat() });

		// //LEVEL 2
		// Character[] characters2 = new[] {
		// 		new Character("Blair", true, new PlayerAgent()),
		// 		new Character("King Xingata", false, new SimpleAgent())
		// 		};
		// Level level2 = new Level("DemoMap", "test", characters2, new Cutscene[] { new AndysDemo(), new GenericDefeat() });

		// //LEVEL 3
		// Character[] characters3 = new[] {
		// 		new Character("Blair", true, new PlayerAgent()),
		// 		new Character("evil!Juniper", false, new SimpleAgent())
		// 		};
		// Level level3 = new Level("DemoMap", "DemoLevel", characters3, new Cutscene[] { new AndysDemo(), new GenericDefeat() });


		// //Just to show off my vision of campaign branching. which now looks like it's not gonna happen, but oh well :p
		// Campaign testCampaign1 = new Campaign("test1", 0, new[] {
		// 	level1,
		// 	level2
		// });

		// Campaign testCampaign2 = new Campaign("test2", 0, new[] {
		// 	level1,
		// 	level3
		// });

		// // Persistance.campaign = testCampaign1;



		// Level capture = new Level("DemoMap", "CaptureTest", characters3, new Cutscene[] { new TutorialEnd(), new GenericDefeat(), new ExpressionShowOff() });
		// Level defend = new Level("DemoMap", "DefendTest", characters3, new Cutscene[] { new GenericDefeat() });
		// Level escort = new Level("DemoMap", "EscortTest", characters3, new Cutscene[] { new GenericDefeat() });
		// Level intercept = new Level("DemoMap", "InterceptTest", characters3, new Cutscene[] { new GenericDefeat() });

		// Campaign gameModeShowOff = new Campaign("gamemode show off", 0, new[] {
		// 	capture, defend, escort, intercept
		// });

		// // Persistence.campaign = gameModeShowOff;





		//Actual final in game campaign

		//Level 0: Tutorial
		Character[] TutorialCharacters = new[] {
			new Character("Blair", true, new BlairTutorialAgent()),
			new Character("Juniper", false, new JuniperTutorialAgent())
		};
		Level level0 = new Level("TutorialMap", "TutorialLevel", TutorialCharacters, new Cutscene[] { new TutorialStart(), new TutorialTileDefense(), new TutorialJuniperUnitLoss(), new TutorialBlairUnitLoss(), new TutorialEnd() });

		//Level 1: border post kova
		Character[] BorderPostCharacter = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Tsubin Infantry", false, new EliminationAgent())
		};
		Level level1 = new Level("BorderPost", "1", BorderPostCharacter, new Cutscene[] { new PreBattle1(), new PostBattle1()});

		//Level 2: Midas river
		Character[] MidasRiverCharacter = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Tsubin Infantry", false, new EliminationAgent())
		};
		Level level2 = new Level("MidasRiver", "2", MidasRiverCharacter, new Cutscene[] { new PostBattle2() });


		//Level 3: Retreat
		Character[] RetreatCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Tsubin Infantry", false, new EliminationAgent())
		};
		Level level3 = new Level("Retreat", "3", RetreatCharacters, new Cutscene[] { new PostBattle3()});

		//Level 4: Central Plains
		Character[] CentralPlainsCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Corbitan Raiders", false, new EliminationAgent())
		};
		Level level4 = new Level("CentralPlains", "4", CentralPlainsCharacters, new Cutscene[] { });

		//Level 5: Chasing
		Character[] ChasingCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Corbitan Raiders", false, new EliminationAgent())
		};
		Level level5 = new Level("Chasing", "5", ChasingCharacters, new Cutscene[] { });

		//Level 6: Crater Attack
		Character[] CraterAttackCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Velgarian Army", false, new EliminationAgent())
		};
		Level level6 = new Level("CraterAttack", "6", CraterAttackCharacters, new Cutscene[] { });

		//Level 7: Crater Battle
		Character[] CraterBattleCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Tsubin Army", false, new EliminationAgent())
		};
		Level level7 = new Level("CraterBattle", "7", CraterBattleCharacters, new Cutscene[] { });

		//Level 8: Battle for Xingata
		Character[] XingataCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("King Rouen", false, new EliminationAgent())
		};
		Level level8 = new Level("CraterCenter", "8", XingataCharacters, new Cutscene[] { });



		Campaign RealActualFinCampaign = new Campaign("Path to Harmony", 0, new[] {
			level0,
			level1,
			level2,
			level3,
			level4,
			level5,
			level6,
			level7,
			level8
		});

		Persistence.campaign = RealActualFinCampaign;
	}
}
