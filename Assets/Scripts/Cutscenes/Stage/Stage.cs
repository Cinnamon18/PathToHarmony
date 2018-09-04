using Cutscenes.Textboxes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		private Actor actorPrefab;

		[SerializeField]
		private Transform textboxBackground;
		
		private List<Actor> actors = new List<Actor>();

		/// <summary>
		/// Rich text tags won't work now. Sad!
		/// </summary>
		public void Start() {
			StartCoroutine(Invoke(
				S().AddActor(CutsceneSide.FarLeft, Instantiate(actorPrefab), "J*n"),
				S().AddActor(CutsceneSide.FarRight, Instantiate(actorPrefab), "L*za"),
				S().SetMessage("I wanna show you something.")
					.SetSpeaker("L*za"),
				S().SetMessage("It's a little...<s><r>unconventional</r></s>.")
					.SetSpeaker("L*za"),
				S().SetMessage("Is it...<s>illegal</s>?")
					.SetSpeaker("J*n"),
				S().AddLeaver("L*za"),
				S().AddActor(CutsceneSide.Left, Instantiate(actorPrefab), "H*race"),
				S().AddActor(CutsceneSide.Right, Instantiate(actorPrefab), "C*risse"),
				S().SetMessage("Would you believe I'm actually from <w><r>Earth</r></w>?")
					.SetSpeaker("J*n"),
				S().AddLeaver("H*race"),
				S().AddLeaver("J*n"),
				S().AddLeaver("C*risse")
				));
		}

		public IEnumerator Invoke(params StageBuilder[] stageBuilders) {

			float targetY = textboxBackground.position.y;

			yield return Util.Lerp(0.75f, t => {
				Util.SetChildrenAlpha(textboxBackground, Mathf.Sqrt(t));
				textboxBackground.position = Util.SmoothStep(
					new Vector2(0, targetY * 4),
					new Vector2(0, targetY),
					Mathf.Sqrt(t)
					);
			});

			foreach (StageBuilder stageBuilder in stageBuilders) {
				yield return Invoke(stageBuilder);
			}
		}

		public IEnumerator Invoke(StageBuilder stageBuilder) {

			if (stageBuilder.newcomer != null) {
				if (FindActor(stageBuilder.newcomer.name) != null) {
					throw new UnityException(
						"There already exists an actor in the scene with name: "
						+ stageBuilder.newcomer.name);
				}

				yield return AddActor(stageBuilder.newcomer, stageBuilder.newcomer.side);
			}

			if (stageBuilder.message != null) {
				CutsceneSide side = CutsceneSide.None;

				if (!string.IsNullOrEmpty(stageBuilder.speaker)) {
					side = FindActor(stageBuilder.speaker).side;	

					foreach (Actor actor in actors) {
						if (actor.side != side) {
							actor.IsDark = true;
						}
					}
				}

				textbox.AddText(side, stageBuilder.speaker, stageBuilder.message);
				yield return new WaitForSeconds(5);
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
			
			Debug.Log(startPos);

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

		// shorthand for easier setup
		private StageBuilder S() {
			return new StageBuilder();
		}

	}
}