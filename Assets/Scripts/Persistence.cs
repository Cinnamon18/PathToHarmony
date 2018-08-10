using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public static class Persistance {
	public static Character playerCharacter { get; set; }
	public static Campaign campaign { get; set; }

	private const string playerNamePref = "playerName";

	public static void savePlayerCharacterInfo() {
		PlayerPrefs.SetString(playerNamePref, playerCharacter.name);
	}

	public static Character getPlayerCharacter() {
		try {
			Character playerCharacter = new Character();
			playerCharacter.name = PlayerPrefs.GetString(playerNamePref);
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
