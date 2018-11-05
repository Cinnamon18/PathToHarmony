using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.Audio;

public static class Persistance {
	public static Campaign campaign { get; set; }

	private const string CAMPAIGN_LEVEL_INDEX = "campaignLevelIndex";
	public const string MASTER_VOLUME = "MasterVolume";
	public const string MUSIC_VOLUME = "MusicVolume";
	public const string SFX_VOLUEME = "SfxVolume";

	public static void saveProgress() {
		PlayerPrefs.SetInt(CAMPAIGN_LEVEL_INDEX, campaign.levelIndex);
		Util.Log("Persistence: Saved level index of " + campaign.levelIndex);
		PlayerPrefs.Save();
	}

	public static void loadProgress() {
		if (PlayerPrefs.HasKey(CAMPAIGN_LEVEL_INDEX)) {
			campaign.levelIndex = PlayerPrefs.GetInt(CAMPAIGN_LEVEL_INDEX);
			Util.Log("Persistence: Loaded level index of " + campaign.levelIndex);
		}
	}

	public static void saveAudioSettings(AudioMixer mixer) {
		float masterVolume = 0;
		float musicVolume = 0;
		float sfxVolume = 0;

		mixer.GetFloat(MASTER_VOLUME, out masterVolume);
		mixer.GetFloat(MUSIC_VOLUME, out musicVolume);
		mixer.GetFloat(SFX_VOLUEME, out sfxVolume);

		PlayerPrefs.SetFloat(MASTER_VOLUME, masterVolume);
		PlayerPrefs.SetFloat(MUSIC_VOLUME, musicVolume);
		PlayerPrefs.SetFloat(SFX_VOLUEME, sfxVolume);

		PlayerPrefs.Save();
	}

	public static void loadAudioSettings(AudioMixer mixer) {
		if (PlayerPrefs.HasKey(MASTER_VOLUME)) {
			mixer.SetFloat(MASTER_VOLUME, PlayerPrefs.GetFloat(MASTER_VOLUME));
		}
		if (PlayerPrefs.HasKey(MUSIC_VOLUME)) {
			mixer.SetFloat(MUSIC_VOLUME, PlayerPrefs.GetFloat(MUSIC_VOLUME));
		}
		if (PlayerPrefs.HasKey(SFX_VOLUEME)) {
			mixer.SetFloat(SFX_VOLUEME, PlayerPrefs.GetFloat(SFX_VOLUEME));
		}
	}

}
