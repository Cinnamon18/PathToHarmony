using Cutscenes.Textboxes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes.Stages {
	public class StageBuilder {

		public string speaker;
		public string message;
		public Actor newcomer;
		public string leaverName;
		public Sprite expression;
		public string sfx;

		public StageBuilder SetSpeaker(string speaker) {
			this.speaker = speaker;
			return this;
		}

		public StageBuilder SetMessage(string message) {
			this.message = message;
			return this;
		}

		public StageBuilder SetExpression(string expression) {
			this.expression = Resources.Load<Sprite>("CharacterSprites/" + speaker + expression);
			if (this.expression == null) {
				throw new UnityException("Expression \"" + speaker + expression + "\" not found");
			}

			return this;
		}

		public StageBuilder SetAudio(string audioID) {
			this.sfx = audioID;
			return this;
		}

		public StageBuilder AddActor(CutsceneSide side, string actorName, string name) {
			Actor newcomer = Resources.Load<Actor>("Actors/" + actorName);
			newcomer = GameObject.Instantiate(newcomer);

			newcomer.name = name;
			this.newcomer = newcomer;
			this.newcomer.side = side;
			return this;
		}

		public StageBuilder AddLeaver(string leaverName) {
			this.leaverName = leaverName;
			return this;
		}


	}
}