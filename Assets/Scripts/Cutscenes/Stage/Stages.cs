using System.Collections.Generic;
using UnityEngine;

namespace Cutscenes.Stages {
	public class Stages {

		private static Dictionary<string, StageBuilder[]> stages = new Dictionary<string, StageBuilder[]>();

		public static StageBuilder[] getStage(string stageID) {
			try {
				return stages[stageID];
			} catch (KeyNotFoundException e) {
				Debug.LogError("Invalid cutscene key ID. Check that the scene is present in Stages.cs");
				return null;
			}
		}

		public static void setupCutscenes() {
			stages.Add("andysDemo", new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "J*n"),
				S().AddActor(CutsceneSide.FarRight, "BlairActor", "L*za"),
				S().SetMessage("I wanna show you something.")
					.SetSpeaker("L*za"),
				S().SetMessage("It's a little...<s><r>unconventional</r></s>.")
					.SetSpeaker("L*za"),
				S().SetMessage("Is it...<s>illegal</s>?")
					.SetSpeaker("J*n"),
				S().AddLeaver("L*za"),
				S().AddActor(CutsceneSide.Left, "BlairActor", "H*race"),
				S().AddActor(CutsceneSide.Right, "BlairActor", "C*risse"),
				S().SetMessage("Would you believe I'm actually from <w><r>Earth</r></w>?")
					.SetSpeaker("J*n"),
				S().AddLeaver("H*race"),
				S().AddLeaver("J*n"),
				S().AddLeaver("C*risse")
			});

			stages.Add("tutorialEnd", new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().SetMessage("If anyone saw us, they’d think I’m the victor and you are the loser.")
					.SetSpeaker("Juniper"),
				S().SetMessage("I’ve dreamt of this day since I was a child. Of the day I finally meet his majesty.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("You know he is leading an army on the field.")
					.SetSpeaker("Juniper"),
				S().SetMessage("I know, but I still can’t shake off my disappointment.")
					.SetSpeaker("Blair"),
				S().SetMessage("You never explained to me why you admire the King so much.")
					.SetSpeaker("Juniper"),
				S().SetMessage("What is there to explain?")
					.SetSpeaker("Blair"),
				S().SetMessage("You’ve never met him. You’ve never worked under him. You don’t know what he is really like.")
					.SetSpeaker("Juniper"),
				S().SetMessage("I know exactly what he is like. He is the King by divine right. He is the embodiment of dignity. Of always putting his kingdom before himself.")
					.SetSpeaker("Blair"),
				S().SetMessage("You just described what a king is supposed to be. You don’t know if he is the king you believe him to be.")
					.SetSpeaker("Juniper"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().SetMessage("He IS the King, and that is all we, the officers of his retinue, have to know.")
					.SetSpeaker("Bruno"),
				S().SetMessage("Congratulations, both of you.")
					.SetSpeaker("Bruno"),
				S().SetMessage("Where have you been? You at least watched our battle, right?")
					.SetSpeaker("Juniper")
			});
		}



		// shorthand for easier setup
		private static StageBuilder S() {
			return new StageBuilder();
		}

	}
}
