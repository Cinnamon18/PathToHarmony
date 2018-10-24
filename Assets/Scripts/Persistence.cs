using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public static class Persistance {
	public static Character playerCharacter { get; set; }
	public static Campaign campaign { get; set; }

	private const string PLAYER_NAME_PREF = "playerName";
	private const string LEVEL_INDEX_PREF = "levelIndex";


	public static void savePlayerCharacterInfo() {
		PlayerPrefs.SetString(PLAYER_NAME_PREF, playerCharacter.name);
		save();
	}

	public static void saveCampaignProgress() {
		PlayerPrefs.SetInt(LEVEL_INDEX_PREF, campaign.levelIndex);
		save();
	}

	
	public static int getLevelIndex() {
	//check if the key does not exist, i.e. if this is the first time we are running this
	if(!PlayerPrefs.HasKey(LEVEL_INDEX_PREF)) {
		PlayerPrefs.SetInt(LEVEL_INDEX_PREF, 0);
		return 0;
	}
		return PlayerPrefs.GetInt(LEVEL_INDEX_PREF);
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
