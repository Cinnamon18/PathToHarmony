using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Cutscenes {
	public class Cutscene : MonoBehaviour {

		private static Sprite[] backgrounds;
		private CutsceneCharacter[] characters;
		private CutsceneCharacter leftCharacter;
		private CutsceneCharacter rightCharacter;
		public bool inProgress;

		[SerializeField]
		public Image currentBackground;
		[SerializeField]
		public Image leftImage;
		[SerializeField]
		public Image rightImage;

		void Awake() {
			backgrounds = new Sprite[] {
			Resources.Load<Sprite>("Sprites/EmptyBackground"),
			Resources.Load<Sprite>("Sprites/TempBackground")
		};
		}

		//Called by the level writer. This is my attempt to both let the user assign refrences in the inspector
		// (which i'm pretty sure means we can't use a factory) and avoid the "can we beat the first frame"
		// race condition. I hope. I think. Godddd unity :( 

		public void setup(CutsceneCharacter[] characters, CutsceneCharacter left, CutsceneCharacter right) {
			this.characters = characters;
			leftCharacter = left;
			rightCharacter = right;
		}

		// Use this for initialization
		IEnumerator Start() {
			inProgress = true;
			yield return PlayScene();
			this.inProgress = false;
		}

		//TODO: If someone wants to reimplement this with some sort of coutroutine lambda hack and get rid of
		// the ugly ugly PlayScene and the CutsceneAction type I'll love you forever. Maybe smth like:
		// https://answers.unity.com/questions/542115/is-there-any-way-to-use-coroutines-with-anonymous.html
		IEnumerator PlayScene() {
			yield return transitionIn(leftCharacter, CharacterSide.Left);
			yield return transitionIn(rightCharacter, CharacterSide.Right);
			setBackground(CutsceneBackgrounds.None);
			yield return transitionOut(CharacterSide.Left);
			yield return transitionOut(CharacterSide.Right);
		}


		public void setBackground(CutsceneBackgrounds background) {
			currentBackground.sprite = backgrounds[(int)(background)];
		}

		public void focusSide(CharacterSide side) {
			if (side == CharacterSide.Left) {
				restoreColor(leftImage);
				greyOut(rightImage);
			} else if (side == CharacterSide.Right) {
				restoreColor(rightImage);
				greyOut(leftImage);
			}
		}

		public async void setCharacter(CutsceneCharacter character, CharacterSide side) {
			CutsceneCharacter oldCharacter = (side == CharacterSide.Left) ? leftCharacter : rightCharacter;
			if (oldCharacter != null) {
				transitionOut(side);

			}
			await Task.Delay(TimeSpan.FromMilliseconds(500));
			oldCharacter = character;
			transitionIn(character, side);
		}

		public IEnumerator transitionOut(CharacterSide side) {
			Image img;
			CutsceneCharacter oldCharacter;
			string animationName;
			if (side == CharacterSide.Left) {
				img = leftImage;
				oldCharacter = leftCharacter;
				animationName = "CutsceneLeftCharacterOut";
			} else {
				img = rightImage;
				oldCharacter = rightCharacter;
				animationName = "CutsceneRightCharacterOut";
			}

			Animation anim = img.GetComponent<Animation>();
			anim.Play(animationName);
			// while (anim.isPlaying) {
			// 	yield return null;
			// }
			yield return WaitForAnimation(anim);

			img = null;
			oldCharacter = null;
		}

		public IEnumerator transitionIn(CutsceneCharacter character, CharacterSide side) {
			Image img;
			CutsceneCharacter oldCharacter;
			string animationName;
			if (side == CharacterSide.Left) {
				img = leftImage;
				oldCharacter = leftCharacter;
				animationName = "CutsceneLeftCharacterIn";
			} else {
				img = rightImage;
				oldCharacter = rightCharacter;
				animationName = "CutsceneRightCharacterIn";
			}

			img.sprite = character.currentExpression;
			Animation anim = img.GetComponent<Animation>();
			anim.Play(animationName);
			// while (anim.isPlaying) {
			// 	yield return null;
			// }
			yield return WaitForAnimation(anim);
		}

		private IEnumerator WaitForAnimation(Animation animation) {
			do {
				yield return null;
			} while (animation.isPlaying);
		}

		public void restoreColor(Image img) {

		}

		public void greyOut(Image img) {

		}

	}
}