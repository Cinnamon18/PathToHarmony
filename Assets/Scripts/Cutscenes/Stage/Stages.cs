using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Cutscenes.Stages {
	public class Stages {

		private const string andysDemo = "andysDemo";
		private const string tutorialEnd = "tutorialEnd";
		private const string genericDefeat = "genericDefeat";
		private static List<string> hasExecuted = new List<string>();

		//hhh sorry we're throwing type safety out the window but. The execution on these coroutines is already a little weird
		//#31DaysTillDemo
		//It's a polling approach to give more control to the caller...
		public static bool testExecutionCondition(string stageID, Battlefield battlefield, GameObjective objective, int halfTurnsElapsed) {
			if (stageID == andysDemo && !hasExecuted.Contains(andysDemo)) {
				//Execute on victory condition
				hasExecuted.Add(andysDemo);
				return objective.isWinCondition(halfTurnsElapsed);
			} else if (stageID == tutorialEnd && !hasExecuted.Contains(andysDemo)) {
				//Execute on turn one
				hasExecuted.Add(andysDemo);
				return halfTurnsElapsed == 0;
			} else if (stageID == genericDefeat && !hasExecuted.Contains(andysDemo)) {
				//Execute on defeat condition
				hasExecuted.Add(andysDemo);
				return objective.isLoseCondition(halfTurnsElapsed);
			}
			// throw new UnityException("Invalid stageID \"" + stageID + "\" tested for");
			return false;
		}

		public static StageBuilder[] getStage(string stageID) {
			//You can switch const strings? Hell Yeah!
			switch (stageID) {
				case andysDemo:
					return new StageBuilder[] {
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
					};
					break;
				case tutorialEnd:
					return new StageBuilder[] {
						S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
						S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
						S().SetMessage("If anyone saw us, they’d think I’m the victor and you were the loser.")
							.SetSpeaker("Juniper"),
						S().SetMessage("I’ve dreamt of this day since I was a child. Of the day I finally get to meet his majesty.")
							.SetSpeaker("Blair").SetExpression("Smile"),
						S().SetMessage("You know he's leading an army on the field, right?")
							.SetSpeaker("Juniper"),
						S().SetMessage("I know, but I still can’t shake off my disappointment.")
							.SetSpeaker("Blair").SetAudio("DemoClip"),
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
					};
					break;
				case genericDefeat:
					return new StageBuilder[] {
						S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
						S().SetMessage("We are defeated!")
							.SetSpeaker("Blair")
					};
					break;
				default:
					throw new UnityException("Invalid cutscene key ID \"" + stageID + "\". Check that the scene is present in Stages.cs");
					return null;
			}
		}

		// shorthand for easier setup
		private static StageBuilder S() {
			return new StageBuilder();
		}

	}
}
