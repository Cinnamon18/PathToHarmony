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
		public Textbox textbox;

		[SerializeField]
		public Image background;

		[SerializeField]
		public Transform actorHolder;

		[SerializeField]
		private Transform actorPrefab;

		/// <summary>
		/// Rich text tags won't work now. Sad!
		/// </summary>
		public void Start() {
			StartCoroutine(Invoke(
				S().AddActor(Instantiate(actorPrefab), "B*ll"),
				S().SetMessage("Tell me about <w>J*n</w>! <s>Why does she wear the</s> <r>mask</r>?!")
				   .SetSpeaker("B*ll"),
				S().SetMessage("<s>Just like that... I've failed you.</s>"),
				S().SetMessage("A lotta <s>loyalty</s> <r>for</r> a <w><r>hired gun</r></w>!")
					.SetSpeaker("B*ll"),
				S().AddActor(Instantiate(actorPrefab), "J*n")
					.SetSpeaker("J*n")
					.SetMessage("<w><r>Or perhaps she's wondering why someone would shoot a man before throwing him off a plane.</w></r>"),
				S().SetMessage("<w><r>At least you can talk! Who are you</r></w>?!")
					.SetSpeaker("B*ll"),
				S().SetMessage("No one cared who I was until I put on the <r><w>The Mask</r></w>.")
					.SetSpeaker("J*n"),
				S().SetMessage("<w>Who</w> are <r>you?</r>"),
				S().SetMessage("It doesn't matter who <w>WE</w> are. What matters is our <w><r>plan.</r></w>")
					.SetSpeaker("J*n")
				));
		}

		public IEnumerator Invoke(params StageBuilder[] stageBuilders) {
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
				yield return AddActor(stageBuilder.newcomer);
			}

			if (stageBuilder.message != null) {
				NameType nameType = NameType.NONE;

				if (!string.IsNullOrEmpty(stageBuilder.speaker)) {

					if (FindActor(stageBuilder.speaker).localPosition.x < 0) {
						nameType = NameType.LEFT;
					} else {
						nameType = NameType.RIGHT;
					}
					
				}

				textbox.AddText(nameType, stageBuilder.speaker, stageBuilder.message);
				yield return new WaitForSeconds(5);
			}

			if (!string.IsNullOrEmpty(stageBuilder.leaverName)) {
				Transform foundActor = FindActor(stageBuilder.leaverName);

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

		private Transform FindActor(string name) {
			return actorHolder.Find(name);
		}

		private IEnumerator AddActor(Transform actor) {
			GameObject dummy = new GameObject();
			dummy.transform.SetParent(actorHolder.transform);

			Vector2 endPos = dummy.transform.position;

			Destroy(dummy);

			Debug.Log(endPos);

			Vector2 startPos = new Vector2(
				Mathf.Sign(endPos.x) * dimensions.rect.width / 2, 
				actor.transform.position.y);

			Debug.Log(startPos);

			actor.transform.SetParent(background.transform);
			actor.transform.position = startPos;

			yield return Util.Lerp(1, t => {
				actor.transform.localPosition = Vector2.Lerp(startPos, endPos, Mathf.Sqrt(t));
			});

			actor.transform.SetParent(actorHolder);

			yield break;
		}

		private IEnumerator RemoveActor(Transform actor) {

			Vector2 endPos = new Vector2(
				Mathf.Sign(actor.localPosition.x) * (dimensions.rect.width / 2),
				actor.position.y
				);

			Vector2 startPos = actor.transform.position;

			yield return Util.Lerp(1, t => {
				actor.transform.localPosition = Vector2.Lerp(startPos, endPos, t * t);
			});

			Destroy(actor.gameObject);
		}

		// shorthand for easier setup
		private StageBuilder S() {
			return new StageBuilder();
		}

	}
}