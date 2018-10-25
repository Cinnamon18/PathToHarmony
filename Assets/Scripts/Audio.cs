using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {

	private static Dictionary<string, AudioClip> tracks = new Dictionary<string, AudioClip>();
	private static AudioSource sfxSource;
	private static AudioSource musicSource;

	public AudioSource initialSfx;
	public AudioSource initialMusic;

	// Use this for initialization
	void Start() {
		tracks.Add("DemoClip", Resources.Load<AudioClip>("Audio/DemoSoundEffect"));
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
