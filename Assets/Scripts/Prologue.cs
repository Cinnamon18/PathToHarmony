using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prologue : MonoBehaviour {
	[SerializeField]
	private FadeOutTransition fade;
	public Text prologueText;
	public float scrollSpeed = 25f;
	private float initialScrollSpeed;

	void Start() {
		initialScrollSpeed = scrollSpeed;
		Audio.playSound("MainTheme1", true, true);
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Select")) {
			scrollSpeed = initialScrollSpeed * 5f;
		}

		if (Input.GetButtonUp("Select")) {
			scrollSpeed = initialScrollSpeed;
		}

		if (!Input.GetButtonDown("Select") && (Input.anyKeyDown)) {
			fade.fadeToScene("DemoBattle");
		}

		prologueText.transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);

		if (prologueText.transform.position.y > 2400) {
			fade.fadeToScene("DemoBattle");
		}
	}
}
