using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour {

	public BattleControl control;
	public Button self;
	public Image image;
	// Update is called once per frame
	void Update () {
		if (control.playerCharacter == control.currentCharacter) {
			self.interactable = true;
			image.color = new Color(image.color.r, image.color.g, image.color.b,Mathf.Min(1, image.color.a + 1 * Time.deltaTime));
		} else {
			self.interactable = false;
			image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Max(0.25f, image.color.a - 1 * Time.deltaTime));
		}
	}
}
