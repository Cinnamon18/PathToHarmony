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
		Audio.masterMixer = masterMixer;
		Persistence.loadAudioSettings(masterMixer);
		Slider[] optionsSliders = optionsCanvas.GetComponentsInChildren<Slider>();
		optionsSliders[0].value = Persistence.MasterVolume;//PlayerPrefs.GetFloat(Persistence.MASTER_VOLUME);
		optionsSliders[1].value = Persistence.MusicVolume;//PlayerPrefs.GetFloat(Persistence.MUSIC_VOLUME);
		optionsSliders[2].value = Persistence.SfxVolume;//PlayerPrefs.GetFloat(Persistence.SFX_VOLUME);
		muteButton.GetComponentInChildren<Text>().text = (Persistence.IsMuted ? "Audio Muted" : "Audio Not Muted");

		Audio.playSound("MainTheme1", true, true);

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
		fade.fadeToScene("Controls");
	}

	public void resumeGame() {
		Persistence.loadProgress();

		if (Persistence.campaign.levelIndex > 0) {
			fade.fadeToScene("demobattle");
		} else {
			playGame();
		}
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
		Level level0 = new Level("TutorialMap", "TutorialLevel", TutorialCharacters, new Cutscene[] { new TutorialStart(), new Tutorial2(), new Tutorial3(), new TutorialTileDefense(), new TutorialJuniperUnitLoss(), new TutorialBlairUnitLoss(), new TutorialEnd() });

		//Level 1: border post kova
		Character[] BorderPostCharacter = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Tsubin Infantry", false, new DefendAgent())
		};
		Level level1 = new Level("BorderPost", "1", BorderPostCharacter, new Cutscene[] { new PreBattle1(), new PostBattle1() });

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
		Level level3 = new Level("Retreat", "3", RetreatCharacters, new Cutscene[] { new PreBattle3(), new PostBattle3() });

		//Level 4: Central Plains
		Character[] CentralPlainsCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Corbitan Raiders", false, new EliminationAgent())
		};
		Level level4 = new Level("CentralPlains", "4", CentralPlainsCharacters, new Cutscene[] { new PreBattle4(), new PostBattle4() });

		// Level 5: SupplyTrain
		Character[] SupplyTrainCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Corbitan Train", false, new EliminationAgent())
		};
		Level level5 = new Level("SupplyTrain", "5", SupplyTrainCharacters, new Cutscene[] { new PreBattle5(), new PostBattle5() });

		// Level 6: DefendCity
		Character[] DefendCityCharacters = new[] {
			new Character("Bruno", true, new PlayerAgent()),
			new Character("Corbitan Army", false, new EliminationAgent())
		};
		Level level6 = new Level("DefendCity", "6", DefendCityCharacters, new Cutscene[] { new PreBattle6() /*, new PostBattle6()*/ });

		// Level 7: Chasing
		Character[] ChasingCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Corbitan Train", false, new EliminationAgent())
		};
		Level level7 = new Level("Chasing", "7", ChasingCharacters, new Cutscene[] { new PreBattle7(), new PostBattle7() });

		//Level 8: PatrolAmbush
		Character[] PatrolAmbushCharacters = new[] {
			new Character("Juniper", true, new PlayerAgent()),
			new Character("Velgaran Ambush", false, new EliminationAgent())
		};
		Level level8 = new Level("PatrolAmbush", "8", PatrolAmbushCharacters, new Cutscene[] { new PreBattle8(), new PostBattle8() });

		//Level 9: DistractDistraction
		Character[] DistractDistractionCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Velgaran Army", false, new EliminationAgent())
		};
		Level level9 = new Level("DistractDistraction2", "9", DistractDistractionCharacters, new Cutscene[] { new PreBattle9(), new PostBattle9() });

		//Level 10: CapitalSiege
		Character[] CapitalSiegeCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Velgaran Guard", false, new DefendAgent())
		};
		Level level10 = new Level("CapitalSiege", "10", CapitalSiegeCharacters, new Cutscene[] { new PreBattle10(), new PostBattle10() });

		//Level 11: Crater Attack
		Character[] CraterAttackCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Velgarian Army", false, new EliminationAgent())
		};
		Level level11 = new Level("CraterAttack", "11", CraterAttackCharacters, new Cutscene[] { new PreBattle11(), new PostBattle11() });

		//Level 12: Crater Battle
		Character[] CraterBattleCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("Tsubin Army", false, new EliminationAgent())
		};
		Level level12 = new Level("CraterBattleNew", "12", CraterBattleCharacters, new Cutscene[] { new PreBattle12(), new PostBattle12() });

		//Level 13: Battle for Xingata
		Character[] XingataCharacters = new[] {
			new Character("Blair", true, new PlayerAgent()),
			new Character("King Rouen", false, new EliminationAgent())
		};
		Level level13 = new Level("CraterCenter", "13", XingataCharacters, new Cutscene[] { new PreBattle13(), new PostBattle13() });



		Campaign RealActualFinCampaign = new Campaign("Path to Harmony", 0, new[] {
			level0,
			level1,
			level2,
			level3,
			level4,
			level5,
			level6,
			level7,
			level8,
			level9,
			level10,
			level11,
			level12,
			level13
		});

		Persistence.campaign = RealActualFinCampaign;
	}
}
