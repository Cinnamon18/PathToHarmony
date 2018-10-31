using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Cutscenes.Stages {
	public class Stages {

		public const string andysDemo = "andysDemo";
		public const string tutorialEnd = "tutorialEnd";
		public const string genericDefeat = "genericDefeat";
		public const string expressionShowOff = "expressionShowOff";
		private static HashSet<string> hasExecuted = new HashSet<string>();

		//hhh sorry we're throwing good OO design out the window but. The execution on these coroutines is already a little weird
		//#31DaysTillDemo
		//It's a polling approach to give more control to the caller...
		public static bool testExecutionCondition(string stageID, Battlefield battlefield, GameObjective objective, int halfTurnsElapsed) {

			if (hasExecuted.Contains(stageID)) {
				return false;
			}

			switch (stageID) {
				case andysDemo:
					if (objective.isWinCondition(halfTurnsElapsed)) {
						hasExecuted.Add(stageID);
						return true;
					}
					break;
				case tutorialEnd:
					if (halfTurnsElapsed == 0) {
						hasExecuted.Add(stageID);
						return true;
					}
					break;
				case genericDefeat:
					if (objective.isLoseCondition(halfTurnsElapsed)) {
						hasExecuted.Add(stageID);
						return true;
					}
					break;
				case expressionShowOff:
					if (halfTurnsElapsed == 1) {
						hasExecuted.Add(stageID);
						return true;
					}
					break;
				default:
					Debug.LogError("Invalid cutscene key ID \"" + stageID + "\". Check that the scene is present in Stages.cs");
					return false;
			}

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
						S().AddLeaver("C*risse"),
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
							.SetSpeaker("Blair").SetAudio("KnightAttack"),
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
							.SetSpeaker("Juniper"),
						S().AddLeaver("Blair"),
						S().AddLeaver("Juniper"),
					};
					break;
				case genericDefeat:
					return new StageBuilder[] {
						S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
						S().SetMessage("We are defeated!")
							.SetSpeaker("Blair"),
						S().AddLeaver("Blair")
					};
					break;
				case expressionShowOff:
					return new StageBuilder[] {
						S().AddActor(CutsceneSide.FarLeft, "JuniperActor", "Juniper"),
						S().AddActor(CutsceneSide.FarRight, "NarratorActor", "Narrator"),
						S().SetMessage("Hey, hey, hey yall!")
							.SetSpeaker("Juniper").SetExpression("Neutral"),
						S().SetMessage("Look how many expressions i can make!")
							.SetSpeaker("Juniper").SetExpression("Smile"),
						S().SetMessage("Juniper takes a deep breath, activating her clone-jutsu.")
							.SetSpeaker("Narrator"),
						S().AddLeaver("Narrator"),
						S().AddActor(CutsceneSide.Left, "JuniperActor", "Juniper 2"),
						S().SetMessage("Grrr!")
							.SetSpeaker("Juniper 2").SetExpression("Angry"),
						S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper 3"),
						S().SetMessage("Wow, that's startling")
							.SetSpeaker("Juniper 3").SetExpression("Surprised"),
						S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper 4"),
						S().SetMessage("Awww, no more room for clones :(")
							.SetSpeaker("Juniper 4").SetExpression("Frown"),
						S().AddLeaver("Juniper"),
						S().AddLeaver("Juniper 2"),
						S().AddLeaver("Juniper 3"),
						S().AddLeaver("Juniper 4"),

					};
					break;
				default:
					Debug.LogError("Invalid cutscene key ID \"" + stageID + "\". Check that the scene is present in Stages.cs");
					return new StageBuilder[0];
			}
		}

		// shorthand for easier setup
		private static StageBuilder S() {
			return new StageBuilder();
		}

	}
}
