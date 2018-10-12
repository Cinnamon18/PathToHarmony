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

		public StageBuilder SetSpeaker(string speaker) {
			this.speaker = speaker;
			return this;
		}

		public StageBuilder SetMessage(string message) {
			this.message = message;
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