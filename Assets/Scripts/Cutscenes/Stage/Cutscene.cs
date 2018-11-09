using Constants;

namespace Cutscenes.Stages {
	public abstract class Cutscene {
		public abstract bool executionCondition(ExecutionInfo info);
		public abstract StageBuilder[] getStage();
		protected bool hasExecuted = false;

		// shorthand for easier setup
		protected static StageBuilder S() {
			return new StageBuilder();
		}
	}

	public class AndysDemo : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.objective.isWinCondition(info.halfTurnsElapsed) && info.battleStage == BattleLoopStage.EndTurn) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
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
		}
	}


	public class Scene1 : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.halfTurnsElapsed == 0) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
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
		}
	}

	public class GenericDefeat : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.objective.isLoseCondition(info.halfTurnsElapsed) && info.battleStage == BattleLoopStage.EndTurn) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
			return new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().SetMessage("We are defeated!")
					.SetSpeaker("Blair"),
				S().AddLeaver("Blair")
			};

		}
	}

	public class ExpressionShowOff : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.halfTurnsElapsed == 1) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
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
		}
	}

	public class TutorialStart : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.halfTurnsElapsed == 0) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
			return new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "NarratorActor", "Narrator"),
				S().SetMessage("Over a field flanked by a forest on one side and rows and steps of spectating seats on the other, two parties are in the heat of battle.")
					.SetSpeaker("Narrator"),
				S().SetMessage("The commanders of each party shout orders to soldiers who are all holding sparring weapons.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Meta: You won't have any input this battle, but next time you'll play as Blair and victory will be your responsibility!")
					.SetSpeaker("Narrator"),
				S().SetMessage("There are six different game objectives.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Capture a VIP")
					.SetSpeaker("Narrator"),
				S().SetMessage("Escort a VIP")
					.SetSpeaker("Narrator"),
				S().SetMessage("Defend a zone")
					.SetSpeaker("Narrator"),
				S().SetMessage("Capture a zone")
					.SetSpeaker("Narrator"),
				S().SetMessage("Survive until time runs out")
					.SetSpeaker("Narrator"),
				S().SetMessage("And finally, eliminate all enemies")
					.SetSpeaker("Narrator"),
				S().SetMessage("Too complicated? Just remember this: A crown on your unit means protect it. A crown on an enemy means eliminate it.")
					.SetSpeaker("Narrator"),
				S().SetMessage("All objectives can also be won by eliminating all opponents.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Now lets see how combat happens in Path to Harmony!")
					.SetSpeaker("Narrator"),
			};
		}
	}

	public class TutorialTileDefense : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.halfTurnsElapsed == 1) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
			return new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "JuniperActor", "Juniper"),
				S().SetMessage("I'll place my rogues in the trees so Blair's units can't hit them as easily.")
					.SetSpeaker("Juniper"),
				S().SetMessage("My calvalry moves faster on roads, so I'll attack Blair's archers directly before they can chip away at me.")
					.SetSpeaker("Juniper"),
			};
		}
	}


	public class TutorialJuniperUnitLoss : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.halfTurnsElapsed == 5) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
			return new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "JuniperActor", "Juniper"),
				S().SetMessage("Blair eliminated one of my units! This isn't looking good.")
					.SetSpeaker("Juniper").SetExpression("Frown"),
			};
		}
	}

	public class TutorialBlairUnitLoss : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.halfTurnsElapsed == 6) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
			return new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().SetMessage("I lost a unit. That’s fine. Small sacrifices are necessary in a battle. His majesty would’ve done the same.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
			};
		}
	}

	public class TutorialEnd : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.afterVictoryImage) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
			return new StageBuilder[] {
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				// S().SetMessage("Blair has successfully won the mock battle with Juniper.")
				// 	.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Headmaster"),
				S().SetMessage("As it is tradition, his majesty the King himself will grant the title of Royal Officer to the victor of the graduation ceremony. Cadet Blair, please step forward.")
					.SetSpeaker("Headmaster"),
				S().SetMessage("Instructors, knights and the marshal on the spectator seats all look baffled at the old headmaster’s words.")
					.SetSpeaker("Narrator"),
				S().SetMessage("They look back and forth between the headmaster, who is only beginning to realize his mistake, and the empty throne at the center.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.Right, "NarratorActor", "Cadets"),
				S().SetMessage("Blair draws their sword and points it, flat side of the blade facing up, to the empty throne.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Glory to his majesty, the King!")
					.SetSpeaker("Blair"),
				S().SetMessage("Glory to his majesty, the King!")
					.SetSpeaker("Cadets")
			};
		}
	}







}