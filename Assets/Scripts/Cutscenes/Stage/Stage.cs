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

		public void Start() {
			StartCoroutine(Invoke(
				S().AddActor(Instantiate(actorPrefab), "B*ll"),
				S().SetMessage("Tell me about <w>J*n</w>! Why does she wear the <r>mask</r>?!")
				   .SetSpeaker("B*ll"),
				S().SetMessage("A lotta loyalty <r>for</r> a <r><w>hired gun</w></r>!")
					.SetSpeaker("B*ll")
				));
		}

		public IEnumerator Invoke(params StageBuilder[] stageBuilders) {
			foreach (StageBuilder stageBuilder in stageBuilders) {
				yield return Invoke(stageBuilder);
			}
		}

		public IEnumerator Invoke(StageBuilder stageBuilder) {
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

			if (stageBuilder.newcomer != null) {
				if (FindActor(stageBuilder.newcomer.name) != null) {
					throw new UnityException(
						"There already exists an actor in the scene with name: " 
						+ stageBuilder.newcomer.name);
				}
				yield return AddActor(stageBuilder.newcomer);
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

			Vector2 startPos = new Vector2(
				Mathf.Sign(endPos.x) * dimensions.rect.width / 2, 
				actor.transform.position.y);

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