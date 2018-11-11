using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.Audio;

public static class Persistence {
	public static Campaign campaign { get; set; }

    public static float MasterVolume {
        get {
            return masterVolume;
        }

        set {
            masterVolume = value;
        }
    }

    public static float MusicVolume {
        get {
            return musicVolume;
        }

        set {
            musicVolume = value;
        }
    }

    public static float SfxVolume {
        get {
            return sfxVolume;
        }

        set {
            sfxVolume = value;
        }
    }

    public static bool IsMuted {
        get {
            return isMuted;
        }

        set {
            isMuted = value;
        }
    }

    private const string CAMPAIGN_LEVEL_INDEX = "campaignLevelIndex";
	public const string MASTER_VOLUME = "MasterVolume";
	public const string MUSIC_VOLUME = "MusicVolume";
	public const string SFX_VOLUME = "SfxVolume";
	public const string IS_MUTED = "IsMuted";

	private static float masterVolume;
	private static float musicVolume;
	private static float sfxVolume;
	private static bool isMuted;

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
		updateMixer(mixer);

		PlayerPrefs.SetFloat(MASTER_VOLUME, masterVolume);
		PlayerPrefs.SetFloat(MUSIC_VOLUME, musicVolume);
		PlayerPrefs.SetFloat(SFX_VOLUME, sfxVolume);
		PlayerPrefs.SetInt(IS_MUTED, isMuted?1:0);
		PlayerPrefs.Save();
	}

	public static void loadAudioSettings(AudioMixer mixer) {
		if (PlayerPrefs.HasKey(MASTER_VOLUME)) {
			masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME);
		}
		if (PlayerPrefs.HasKey(MUSIC_VOLUME)) {
			musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME);
		}
		if (PlayerPrefs.HasKey(SFX_VOLUME)) {
			sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME);
		}
		if (PlayerPrefs.HasKey(IS_MUTED)) {
			isMuted = PlayerPrefs.GetInt(IS_MUTED) == 1;
		}

		updateMixer(mixer);
	}

	public static void updateMixer(AudioMixer mixer) {
		mixer.SetFloat(MASTER_VOLUME, isMuted?-100:masterVolume);
		mixer.SetFloat(MUSIC_VOLUME, isMuted?-100:musicVolume);
		mixer.SetFloat(SFX_VOLUME, isMuted?-100:sfxVolume);
	}
}
