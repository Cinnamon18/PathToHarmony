using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public static class Persistance {
	public static Character playerCharacter { get; set; }
	public static Campaign campaign { get; set; }

	private const string PLAYER_NAME_PREF = "playerName";
	private const string CAMPAIGN_LEVEL_INDEX = "campaignLevelIndex";


	

	public static void saveProgress() {
		PlayerPrefs.SetInt(CAMPAIGN_LEVEL_INDEX, campaign.levelIndex);
		Util.Log("Persistence: Saved level index of " + campaign.levelIndex);
		save();
	}

	public static void loadProgress() {
		if (PlayerPrefs.HasKey(CAMPAIGN_LEVEL_INDEX)) {
			campaign.levelIndex = PlayerPrefs.GetInt(CAMPAIGN_LEVEL_INDEX);
			Util.Log("Persistence: Loaded level index of " + campaign.levelIndex);
		}
	}

	public static void savePlayerCharacterInfo() {
		PlayerPrefs.SetString(PLAYER_NAME_PREF, playerCharacter.name);
		save();
		
	}

	public static Character getPlayerCharacter() {
		try {
			Character playerCharacter = new Character();
			playerCharacter.name = PlayerPrefs.GetString(PLAYER_NAME_PREF);
			return playerCharacter;
		} catch (Exception e) {
			//Some sort of I/O exception presumably
			throw (e);
		}
	}

	public static void save() {
		PlayerPrefs.Save();
	}
}
