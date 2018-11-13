using Cutscenes.Textboxes;
using StoppableCoroutines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Entry point for these cutscenes. See Start() method.
/// i dunno (yet) how to make this workable mid-battle because...
/// 
/// (1) CharTweener's code is buggy and likes to shift any movement based
/// character effects up by 350 y units. To counteract this I shift my
/// textbox and camera up as well.
/// 
/// (2) The cutscene system used in the Demo scene uses Screen Space Overlay as the Canvas
/// This one uses World Space because of issue (1). Screen Space Overlay doesn't let me shift
/// up my textboxes, causing some characters in the text to be offset (literally unplayable).
/// 
/// like seriously if you decide to do the wave effect and don't do the offsetting stuff
/// it'll shift the wavy text offscreen so you won't be able to see it >:(
/// </summary>
namespace Cutscenes.Stages {
	public class Stage : MonoBehaviour {
		[SerializeField]
		private RectTransform dimensions;

		[SerializeField]
		private Textbox textbox;

		[SerializeField]
		private Image background;

		[SerializeField]
		private Transform left;

		[SerializeField]
		private Transform farLeft;

		[SerializeField]
		private Transform right;

		[SerializeField]
		private Transform farRight;

		[SerializeField]
		private Transform textboxBackground;

		[SerializeField]
		public GameObject skipButton;

		private List<Actor> actors = new List<Actor>();

		public bool isRunning = false;
		private bool skipCutFlag = false;
		private StoppableCoroutine currentDialogLine;

		/// <summary>
		/// Built in rich text tags won't work now, will need to implement custom
		/// tags in its place.
		/// 
		/// Don't forget to close those tags! The text gets very glitchy if you don't close them.
		/// <w>This is how you close a tag</w>
		/// </summary>
		public void Start() {
			if (SceneManager.GetActiveScene().name == "Story Test") {
				startCutscene(new AndysDemo());
			}
		}

		void Update() {
			if (Input.GetButtonDown("Select")) {
				stopCurrentCutsceneLine();
			}
		}

		public void startCutscene(Cutscene cutscene) {
			showVisualElements();
			StartCoroutine(Invoke(cutscene.getStage()));
		}

		public IEnumerator Invoke(params StageBuilder[] stageBuilders) {
			isRunning = true;
			yield return RaiseUpTextbox();

			foreach (StageBuilder stageBuilder in stageBuilders) {
				if (skipCutFlag) {
					break;
				}

				yield return Invoke(stageBuilder);
			}
			skipCutFlag = false;
			isRunning = false;
			hideVisualElements();
		}

		private IEnumerator RaiseUpTextbox() {
			float targetY = textboxBackground.position.y;

			yield return Util.Lerp(0.75f, t => {
				Util.SetChildrenAlpha(textboxBackground, Mathf.Sqrt(t));
				textboxBackground.position = Util.SmoothStep(
					new Vector2(0, targetY * 4),
					new Vector2(0, targetY),
					Mathf.Sqrt(t)
					);
				//manual correction factor (:
				textboxBackground.position += new Vector3(0, 0, -20);
			});
		}

		private IEnumerator Invoke(StageBuilder stageBuilder) {

			if (stageBuilder.newcomer != null) {
				if (FindActor(stageBuilder.newcomer.name) != null) {
					throw new UnityException(
						"There already exists an actor in the scene with name: "
						+ stageBuilder.newcomer.name);
				}

				yield return AddActor(stageBuilder.newcomer, stageBuilder.newcomer.side);
			}

			if (stageBuilder.expression != null) {
				Actor foundActor = FindActor(stageBuilder.speaker);

				if (foundActor == null) {
					throw new UnityException(
						"There exists no actor in the scene with name: "
						+ stageBuilder.speaker
						);
				}

				foundActor.image.sprite = stageBuilder.expression;
			}

			if (stageBuilder.sfx != null) {
				Audio.playSfx(stageBuilder.sfx);
			}

			if (stageBuilder.background != null) {
				Sprite foundSprite = (Resources.Load<Sprite>("Sprites/" + stageBuilder.background));

				if (foundSprite == null) {
					throw new UnityException(
						"There exists no background " + stageBuilder.background + " in the Resources/Sprites folder, or it has not been imported as a Sprite."
					);
				}

				yield return changeBackground(foundSprite);
			}

			if (stageBuilder.message != null) {
				CutsceneSide side = CutsceneSide.None;

				if (!string.IsNullOrEmpty(stageBuilder.speaker)) {
					Actor foundActor = FindActor(stageBuilder.speaker);

					if (foundActor == null) {
						throw new UnityException(
							"There exists no actor in the scene with name: "
							+ stageBuilder.speaker
							);
					}

					side = foundActor.side;

					foreach (Actor actor in actors) {
						if (actor.side != side) {
							actor.IsDark = true;
						}
					}
				}

				textbox.AddText(side, stageBuilder.speaker, stageBuilder.message);

				//I approximate it to take ~0.03 seconds per letter, but we do more so players can actually read
				float playTimeGuess = (float)(stageBuilder.message.Length * 0.06 + 1.5);
				currentDialogLine = this.StartStoppableCoroutine(waitForSeconds(playTimeGuess));
				yield return currentDialogLine.WaitFor();

				foreach (Actor actor in actors) {
					actor.IsDark = false;
				}
			}

			if (!string.IsNullOrEmpty(stageBuilder.leaverName)) {
				Actor foundActor = FindActor(stageBuilder.leaverName);

				if (foundActor == null) {
					throw new UnityException(
						"There exists no actor in the scene with name: "
						+ stageBuilder.leaverName
						);
				}
				yield return RemoveActor(foundActor);
			}

			yield break;
		}

		private Actor FindActor(string name) {
			return actors.Find(a => a.name.Equals(name));
		}

		private IEnumerator AddActor(Actor actor, CutsceneSide side) {
			Transform holderToUse = GetSideParent(side);

			if (holderToUse.GetComponentInChildren<Actor>() != null) {
				throw new UnityException("There is aleady an actor in this spot:" + holderToUse.name);
			}

			Vector2 endPos = new Vector2(holderToUse.transform.position.x, 0);

			Vector2 startPos = new Vector2(
				((side == CutsceneSide.Left || side == CutsceneSide.FarLeft) ? -1 : 1) * (dimensions.rect.width / 2 + 300),
				actor.transform.position.y);

			// Debug.Log(startPos);

			if (side == CutsceneSide.Left || side == CutsceneSide.FarLeft) {
				actor.transform.localScale = new Vector3(-1, 1, 1);
			}


			actor.transform.SetParent(background.transform);
			actor.transform.position = startPos;

			yield return Util.Lerp(1, t => {
				actor.transform.localPosition = Util.SmoothStep(startPos, endPos, t);
			});

			actor.transform.SetParent(holderToUse);
			actors.Add(actor);

			yield break;
		}

		private IEnumerator RemoveActor(Actor actor) {

			CutsceneSide side = actor.side;


			Vector2 endPos = new Vector2(
				((side == CutsceneSide.Left || side == CutsceneSide.FarLeft) ? -1 : 1) * (dimensions.rect.width / 2 + 300),
				actor.transform.position.y
				);

			Vector2 startPos = new Vector2(actor.transform.position.x, left.position.y);

			actor.transform.SetParent(background.transform);

			yield return Util.Lerp(1, t => {
				actor.transform.position = Vector2.Lerp(startPos, endPos, t * t);
			});

			Destroy(actor.gameObject);
			actors.Remove(actor);
		}

		private IEnumerator changeBackground(Sprite sprite) {
			Color initial = background.color;
			Color final = background.color;
			final.a = 0;

			GameObject blackBackground = Instantiate(background.gameObject,
				background.transform.position + new Vector3(0, 0, 1),
				background.transform.rotation,
				background.transform.parent.transform);
			blackBackground.GetComponent<Image>().color = new Color(0, 0, 0, 1);
			blackBackground.transform.SetSiblingIndex(0);

			yield return Util.Lerp(1f, t => {
				background.color = Color.Lerp(initial, final, t * t);
			});

			background.sprite = sprite;

			yield return Util.Lerp(1f, t => {
				background.color = Color.Lerp(final, initial, t * t);
			});

			Destroy(blackBackground);
		}

		private Transform GetSideParent(CutsceneSide side) {
			Transform parent = null;
			switch (side) {
				case CutsceneSide.FarLeft:
					parent = farLeft;
					break;
				case CutsceneSide.Left:
					parent = left;
					break;
				case CutsceneSide.Right:
					parent = right;
					break;
				case CutsceneSide.FarRight:
					parent = farRight;
					break;
			}
			return parent;
		}

		public void hideVisualElements() {
			this.gameObject.SetActive(false);
			skipButton.SetActive(false);
		}

		public void showVisualElements() {
			this.gameObject.SetActive(true);
			skipButton.SetActive(true);
		}


		public void skipCutscene() {
			// foreach (Actor actor in actors) {
			// 	Destroy(actor.gameObject);
			// }
			// actors.Clear();
			// textbox.AddText(Cut, stageBuilder.speaker, stageBuilder.message);

			stopCurrentCutsceneLine();
			skipCutFlag = true;
		}

		private void stopCurrentCutsceneLine() {
			if (currentDialogLine != null) {
				currentDialogLine.Stop();
			}
			Audio.stopAudio(false);
		}

		private IEnumerator waitForSeconds(float seconds) {
			yield return new WaitForSeconds(seconds);
		}

	}
}