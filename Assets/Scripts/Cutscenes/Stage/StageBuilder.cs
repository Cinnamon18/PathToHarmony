using Cutscenes.Textboxes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes.Stages {
	public class StageBuilder {

		public string speaker;
		public string message;
		public Transform newcomer;
		public string leaverName;

		public StageBuilder SetSpeaker(string speaker) {
			this.speaker = speaker;
			return this;
		}

		public StageBuilder SetMessage(string message) {
			this.message = message;
			return this;
		}

		public StageBuilder AddActor(Transform newcomer, string name) {
			newcomer.gameObject.name = name;
			this.newcomer = newcomer;
			return this;
		}

		public StageBuilder AddLeaver(string leaverName) {
			this.leaverName = leaverName;
			return this;
		}

	}
}