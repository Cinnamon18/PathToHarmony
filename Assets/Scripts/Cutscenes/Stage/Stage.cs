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
		private Transform leftActorHolder;

		[SerializeField]
		private Transform rightActorHolder;

		[SerializeField]
		private Transform actorPrefab;

		// uses instanceid
		private IDictionary<int, Side> actorSide = new Dictionary<int, Side>();

		/// <summary>
		/// Rich text tags won't work now. Sad!
		/// </summary>
		public void Start() {
			StartCoroutine(Invoke(
				S().AddActor(Side.Left, Instantiate(actorPrefab), "B*ll"),
				S().SetMessage("Tell me about <w>J*n</w>! <s>Why does she wear the</s> <r>mask</r>?!")
				   .SetSpeaker("B*ll"),
				S().SetMessage("<s>Just like that... I've failed you.</s>"),
				S().SetMessage("A lotta <s>loyalty</s> <r>for</r> a <w><r>hired gun</r></w>!")
					.SetSpeaker("B*ll"),
				S().AddActor(Side.Right, Instantiate(actorPrefab), "J*n")
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
				yield return AddActor(stageBuilder.newcomer, stageBuilder.newcomerSide);
			}

			if (stageBuilder.message != null) {
				Side side = Side.None;

				if (!string.IsNullOrEmpty(stageBuilder.speaker)) {
					side = GetSide(FindActor(stageBuilder.speaker).GetInstanceID());			
				}

				textbox.AddText(side, stageBuilder.speaker, stageBuilder.message);
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
			return leftActorHolder.Find(name) ?? rightActorHolder.Find(name);
		}

		private Side GetSide(int instanceId) {
			return actorSide[instanceId];
		}

		private IEnumerator AddActor(Transform actor, Side side) {
			Transform dummy = Instantiate(actorPrefab);

			Transform holderToUse = (side == Side.Left) ? leftActorHolder.transform : rightActorHolder.transform;

			dummy.transform.SetParent(holderToUse);
			dummy.GetComponent<Image>().enabled = false;
			yield return new WaitForSeconds(0.001f); // lol
			Vector2 endPos = new Vector2(dummy.transform.position.x, 0);

			Destroy(dummy.gameObject);

			Debug.Log(endPos);

			Vector2 startPos = new Vector2(
				((side == Side.Left) ? -1 : 1) * (dimensions.rect.width / 2 + 300), 
				actor.transform.position.y);
			
			Debug.Log(startPos);

			actor.transform.SetParent(background.transform);
			actor.transform.position = startPos;

			yield return Util.Lerp(1, t => {
				actor.transform.localPosition = Util.SmoothStep(startPos, endPos, t);
			});

			actor.transform.SetParent(holderToUse);
			actorSide.Add(actor.GetInstanceID(), side);

			yield break;
		}

		private IEnumerator RemoveActor(Transform actor) {

			Side side = GetSide(actor.GetInstanceID());

			Vector2 endPos = new Vector2(
				((side == Side.Left) ? -1 : 1) * dimensions.rect.width / 2,
				actor.position.y
				);

			Vector2 startPos = actor.transform.position;

			yield return Util.Lerp(1, t => {
				actor.transform.localPosition = Vector2.Lerp(startPos, endPos, t * t);
			});

			Destroy(actor.gameObject);
			actorSide.Remove(actor.GetInstanceID());
		}

		// shorthand for easier setup
		private StageBuilder S() {
			return new StageBuilder();
		}

	}
}