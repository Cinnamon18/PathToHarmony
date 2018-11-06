using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {

	private static Dictionary<string, AudioClip> tracks;
	private static AudioSource sfxSource;
	private static AudioSource musicSource;

	public AudioSource initialSfx;
	public AudioSource initialMusic;

	public static string[] battleBgm = new string[] {"BattleTheme1"};

	// Use this for initialization
	void Start() {
		if (tracks == null) {
			tracks = new Dictionary<string, AudioClip>();
			tracks.Add("ArcherAttack", Resources.Load<AudioClip>("Audio/ArcherAttack"));
			tracks.Add("ClericAttack", Resources.Load<AudioClip>("Audio/ClericAttack"));
			tracks.Add("KnightAttack", Resources.Load<AudioClip>("Audio/KnightAttack"));
			tracks.Add("LightHorseAttack", Resources.Load<AudioClip>("Audio/LightHorseAttack"));
			tracks.Add("MageAttack", Resources.Load<AudioClip>("Audio/MageAttack"));
			tracks.Add("RogueAttack", Resources.Load<AudioClip>("Audio/RogueAttack"));
			tracks.Add("BattleTheme1", Resources.Load<AudioClip>("Audio/BattleTheme1"));

		}

		sfxSource = initialSfx;
		musicSource = initialMusic;
	}

	public static void playSfx(string id) {
		playSound(id, false, false);
	}

	public static void playSound(string id, bool isMusic, bool loop) {
		AudioClip clip = tracks[id];
		if (clip == null) {
			throw new UnityException("The requested audio track has not yet been imported in the Audio.cs file");
		}

		AudioSource source = isMusic ? musicSource : sfxSource;
		source.clip = clip;
		source.loop = loop;
		source.Play();
	}

	public static void stopAudio(bool isMusic) {
		AudioSource source = isMusic ? musicSource : sfxSource;
		source.Stop();
	}

	public static void pauseAudio(bool isMusic) {
		AudioSource source = isMusic ? musicSource : sfxSource;
		source.Pause();
	}

	public static void resumeAudio(bool isMusic) {
		AudioSource source = isMusic ? musicSource : sfxSource;
		source.UnPause();
	}

}
