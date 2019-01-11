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

	// S().PauseBattleTheme(),
	// S().SetAudio("CutsceneBgm"),

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
				,
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
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Tutorial"),
				S().SetMessage("Over a field flanked by a forest on one side and rows and steps of spectating seats on the other, two parties are in the heat of battle."),
				S().SetMessage("The commanders of each party shout orders to soldiers holding sparring weapons."),
				S().SetMessage("Welcome to Path to Harmony! In this game, you'll play as Xingata's heroes and lead your forces to victory.")
					.SetSpeaker("Tutorial"),
				S().SetMessage("There are six different game objectives.")
					.SetSpeaker("Tutorial"),
				S().SetMessage("Intercept a VIP")
					.SetSpeaker("Tutorial"),
				S().SetMessage("Escort a VIP")
					.SetSpeaker("Tutorial"),
				S().SetMessage("Defend a zone")
					.SetSpeaker("Tutorial"),
				S().SetMessage("Capture a zone. (You have to stay on it for a full turn)")
					.SetSpeaker("Tutorial"),
				S().SetMessage("Survive until time runs out")
					.SetSpeaker("Tutorial"),
				S().SetMessage("And finally, eliminate all enemies")
					.SetSpeaker("Tutorial"),
				S().SetMessage("Too complicated? Just remember this: A crown on your unit means protect it. A crown on an enemy means eliminate it.")
					.SetSpeaker("Tutorial"),
				S().SetMessage("All objectives can also be won by eliminating all opponents.")
					.SetSpeaker("Tutorial"),
				S().SetMessage("Now lets see how combat happens in Path to Harmony. Follow Blair's instructions and the on screen arrows.")
					.SetSpeaker("Tutorial"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().SetMessage("Let's see... I should probably boost my knight's already strong defense by putting them in the trees.")
					.SetSpeaker("Blair"),
				S().SetMessage("I'll move my archers away from the road so Juniper's calvalry can't get to them.")
					.SetSpeaker("Blair"),
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
				S().SetMessage("Hah! Blair forgot that my rogues' don't suffer a movement penalty through trees. I'll hit their exposed archers!")
					.SetSpeaker("Juniper"),
				S().SetMessage("My calvalry moves faster on roads, so I'll attack Blair's knights directly.")
					.SetSpeaker("Juniper"),
			};
		}
	}

	public class Tutorial2 : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.halfTurnsElapsed == 2) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
			return new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().SetMessage("I forgot about her rogues' movement features! Oh well, I should still be able to finish them before they get me.")
					.SetSpeaker("Blair"),
			};
		}
	}


	public class TutorialJuniperUnitLoss : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.halfTurnsElapsed == 3) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
			return new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "JuniperActor", "Juniper"),
				S().SetMessage("Blair concentrated their fire and eliminated another one of my units! This isn't looking good.")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().AddActor(CutsceneSide.FarRight, "BlairActor", "Blair"),
				S().SetMessage("You gonna be okay without that calvalry, Juniper?.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Can it, Blair.")
					.SetSpeaker("Juniper").SetExpression("Angry"),
			};
		}
	}

	public class TutorialBlairUnitLoss : Cutscene {
		public override bool executionCondition(ExecutionInfo info) {
			if (hasExecuted) {
				return false;
			}
			if (info.halfTurnsElapsed == 4) {
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage() {
			hasExecuted = true;
			return new StageBuilder[] {
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().SetMessage("My archers lost health but they're still up. That’s fine. Small sacrifices are necessary in a battle.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
			};
		}
	}

	public class Tutorial3 : Cutscene {
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
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Tutorial"),
				S().SetMessage("Now it's your turn! Eliminate Juniper's remaining rogue.")
					.SetSpeaker("Tutorial"),
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
				// PostTutorial
				// S().SetMessage("Blair has successfully won the mock battle with Juniper.")
				// 	,
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Headmaster"),
				S().AddActor(CutsceneSide.Right, "NarratorActor", "Narrator"),
				S().SetMessage("As it is tradition, his majesty the King himself will grant the title of Royal Officer to the victor of the graduation ceremony. Cadet Blair, step forward.")
					.SetSpeaker("Headmaster"),
				S().SetMessage("Instructors, knights and the marshal on the spectator seats all look baffled at the old headmaster’s words.")
					.SetSpeaker("Narrator"),
				S().SetMessage("They look back and forth between the headmaster, who is only beginning to realize his mistake, and the empty throne at the center.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Cadets"),
				S().SetMessage("Blair, unwavering, draws their sword and points it, flat side of the blade facing up, to the empty throne.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Glory to his majesty, the King!")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Glory to his majesty, the King!")
					.SetSpeaker("Cadets"),
				// Scene Transition
				S().AddLeaver("Cadets"),
				S().AddLeaver("Blair"),
				S().AddLeaver("Headmaster"),
				S().SetBackground("NewBackground"),
				// Scene 1
				// S().SetAudio("MainTheme1"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				// S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().SetMessage("Blair sits alone on the spectator seat, facing the empty field " +
					"where the mock battle took place. Juniper walks up the stepped seats and sits next to Blair.")
					.SetSpeaker("Narrator"),
				S().SetMessage("If anyone saw us, they’d think I’m the victor and you're the loser.")
					.SetSpeaker("Juniper").SetExpression("Smile"),
				S().SetMessage("Blair chuckles and glances at the empty throne to their right.")
					.SetSpeaker("Narrator"),
				S().SetMessage("I’ve dreamt of meeting his majesty since I was a child.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("You know he’s leading an army on the field.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("I know, but I still can’t shake off this disappointment.")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("You never explained to me why you admire the King so much.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("What’s there to explain?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("You’ve never met him. You’ve never worked under him. You don’t know what he’s really like.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("I know exactly what he is like. He’s the King by divine right; he’s a being beyond us. " +
					"He’s the embodiment of dignity and righteousness.")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("You just described what a king’s supposed to be. You don’t know if he’s the king you " +
					"believe him to be.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().SetMessage("He IS the King, and that’s all we, the officers of his retinue, have to know. " +
					"Congratulations, both of you.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Where have you been? You at least watched our battle, right?")
					.SetSpeaker("Juniper").SetExpression("Smile"),
				S().SetMessage("I did watch you miss the last chance to beat Blair.")
					.SetSpeaker("Bruno").SetExpression("Smile"),
				S().SetMessage("Oh yeah? Let’s see you beat Blair in a mock battle.")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("To answer your first question, I have your orders from the marshall. You and Juniper are to " +
					"travel with me to the western front.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("We will advise Lord Sweyn in matters of tactics and aid him, in any way " +
					"we can, to delay the Tsubin army from reaching Harmony Crater. We leave tomorrow morning.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Deployed the day after graduation, huh. But… what about Piper?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("My sister will not be deployed. Chief Physician told me this morning. It’s the plague.")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("I am so sorry, Juniper.")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("Don’t be. More the reason to win the war, right?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("...right. The war.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I’ll go and arrange your departure. See you tomorrow.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().StopAudio()
		};
		}
	}

	public class PreBattle1 : Cutscene {
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
				// Scene 2
				S().PauseBattleTheme(),
				S().SetAudio("MainTheme1"),
				S().AddActor(CutsceneSide.FarRight, "NarratorActor", "Narrator"),
				S().SetMessage("Western front. Inside Lord Sweyn’s tent, days later.")
					.SetSpeaker("Narrator"),
				S().AddLeaver("Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "SweynActor", "Sweyn"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().AddActor(CutsceneSide.Right, "BlairActor", "Blair"),
				S().SetMessage("Tsubin ships are transporting their army up the rivers of Ida and Iouna. " +
					"We know they will land over here, just past the river fork.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("While they are landing, they will be disorganized and few in numbers. We’ll hit them then, and retreat.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("In the meantime, I’ve ordered all the western garrisons guarding Ida river to " +
					"report in. I haven’t heard back from one of them.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("When did you order the garrisons, my lord?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Four days ago. Only the messenger sent to Border Post Kova didn’t come back yet.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("If the garrison was attacked and there are enemies behind us, we should take care of it before the battle.")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("You want me to send a knight to lead a small force and check the place out?")
					.SetSpeaker("Sweyn"),
				S().SetMessage("Actually, with our senior officer’s approval, Juniper and I’ll lead a detachment to check out " +
					"Border Post Kova.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("It’ll be a great opportunity for us to lead real soldiers for the first time.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("They have my approval and my recommendation. They are the brightest graduates of Royal Academy.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Fine. I’ll have a detachment ready in 20 minutes. Check out the garrison but engage the enemy only when necessary.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("I can’t afford to lose any soldiers when I’m already outnumbered 5 to 1.")
					.SetSpeaker("Sweyn"),
				S().AddLeaver("Sweyn"),
				S().SetMessage("I believe in your judgement, Blair, but this isn’t a mock battle. Be careful. Are you fine " +
					"with Blair taking the lead?")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("I’m fine with that.")
					.SetSpeaker("Juniper").SetExpression("Smile"),
				S().SetMessage("We’ll be back soon. Hopefully the garrison is okay and we're overreacting.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().StopAudio(),
				// Scene Transition
				S().AddLeaver("Bruno"),
				S().AddLeaver("Blair"),
				S().AddLeaver("Juniper"),
				S().SetBackground("NewBackground"),
				// Pre-battle 1
				S().SetAudio("TheBlade"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().SetMessage("There it is, Border Post Kova. At least our flag still flies.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Do you see that thing moving on the river? Is that a raft?")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("With Tsubin soldiers on it! They are crossing uncontested! The garrison must have already fallen.")
					.SetSpeaker("Juniper").SetExpression("Surprised"),
				S().SetMessage("Then we must take it back. (turns to the soldiers) Soldiers of Xingata! Kova has fallen. More enemies " +
					"are crossing the river as we speak.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("If we don’t retake that garrison, enemies will hit us in the back while we " +
					"are facing the main Tsubin force. With me!")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().StopAudio()
			};
		}
	}

	public class PostBattle1 : Cutscene {
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
				// Post-battle 1
				S().PauseBattleTheme(),
				S().AddActor(CutsceneSide.FarRight, "NarratorActor", "Narrator"),
				S().SetMessage("Xingatan soldiers are busy moving corpses into a pile. Brutal warriors of Tsubin have turned into ragdolls;")
					.SetSpeaker("Narrator"),
				S().SetMessage("their arms and legs flailing about helplessly in the hands of Xingatan soldiers. Blair and Juniper watch the scene from a distance.")
					.SetSpeaker("Narrator"),
				S().AddLeaver("Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().SetAudio("TheBlade"),
				S().SetMessage("I’ve ordered neighbouring garrisons to reinforce here.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("This isn’t how I imagined a battle. I don’t know what I imagined.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Hey, hold yourself together.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("How can you be so calm after all this?")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("I’m not. I can’t stop my hands from shaking. But we are fighting for a right cause. We are saving the people of Xingata.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				// Small change in dialogue here from the script. 
				S().SetMessage("We are, right? We have to be. We must be... I need a change in scenery. Let’s get out of here.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("...yes. Burn the bodies and form up! We’re heading back!")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().StopAudio(),
				// Scene Transition
				S().AddLeaver("Blair"),
				S().AddLeaver("Juniper"),
				S().SetBackground("NewBackground"),
				// Scene 3
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.FarRight, "NarratorActor", "Narrator"),
				S().SetMessage("Lord Sweyn’s camp. The whole camp appears much busier.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "SweynActor", "Sweyn"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.Right, "BlairActor", "Blair"),
				S().SetMessage("So, enemies were already crossing behind us. Great work retaking Kova. Tsubin ships are approaching - they'll land within the next three hours.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("They will soon figure out we are few in numbers. We must do damage and retreat.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Oh, we are aware. Every detail of the operation has been ironed out.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("You two will stay with me at the center to hold the line while archers and mages bombard the landing.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("Bruno has an experience leading cavalry, so he will lead a group to keep the enemies from flanking us from our east. I have a knight leading another cavalry group to our west.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("Blair and Juniper look at the tactical map drawn out on the table in silence, still shaken by their last battle.")
					.SetSpeaker("Narrator"),
				S().SetMessage("We form up in 30 minutes. I’ll send for you when we do. Get some rest.")
					.SetSpeaker("Sweyn"),
				S().AddLeaver("Sweyn"),
				S().AddLeaver("Narrator"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().SetMessage("War isn’t quite what you believed to be, is it?")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("No. I imagined a sense of pride and accomplishment. In reality, I was just relieved it was over.")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("Killing our youthful beliefs. That’s how we grow up.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("I have to head out early. Congratulations on your first victory. Try to get some rest. Try as if your life depends on it.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().StopAudio()
			};
		}
	}

	public class PostBattle2 : Cutscene {
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
				// Post-battle 2
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Sir Tibolt"),
				S().SetMessage("Royal Officer Blair! Royal Officer Blair!")
				   .SetSpeaker("Sir Tibolt"),
				S().SetMessage("Yes! I’m here! Why hasn’t Lord Sweyn ordered a retreat yet?")
				   .SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("Lord Sweyn is dead.")
					.SetSpeaker("Sir Tibolt"),
				S().PauseBattleTheme(),
				S().SetMessage("A brief silence sets in between them amidst the cacophony of the raging battle.")
					.SetSpeaker("Narrator"),
				S().SetAudio("MainTheme1"),
				S().SetMessage("Who’s his heir?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Lady Bridget is ruling his land in her father’s stead.")
					.SetSpeaker("Sir Tibolt"),
				S().SetMessage("And the senior officer is away from the main body.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("You’re in command of the battle. What are your orders?")
					.SetSpeaker("Sir Tibolt"),
				S().SetMessage("Who else knows?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Only a handful who were around our lord. I sent for Officer Juniper, so she will know.")
					.SetSpeaker("Sir Tibolt"),
				S().SetMessage("Keep it that way. We’re retreating. Send for the cavalry groups to fall back.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Pick 500 of your best soldiers to hold the enemies back while the rest of the force retreats.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("That’s a suicide mission. No one will take part in it.")
					.SetSpeaker("Sir Tibolt"),
				S().SetMessage("If they don’t, we all die here.")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("No one will lead it.")
					.SetSpeaker("Sir Tibolt"),
				S().SetMessage("Blair looks away to the front line tightly locked in a close combat.")
					.SetSpeaker("Narrator"),
				S().StopAudio(),
				S().SetMessage("‘What would his majesty do?’")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("There are endless waves of Tsubin warriors freshly landing on the river bank beyond it.")
					.SetSpeaker("Narrator"),
				S().SetAudio("TheBlade"),
				S().SetMessage("It is not a suicide mission. Because I will lead it.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("No, you won’t. We’ve just lost one commander. We can’t lose another.")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Let cavalry groups know of my intention. Tell them to disengage, sweep around and meet me 500 yards behind the front line.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Juniper, lead the main force in retreat.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Are you even listening?")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("No commander in their right mind would lead a suicide mission. So, if I lead it, soldiers will believe it isn’t a suicide mission.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("You can’t win an unwinnable battle just because you believe in it! That’s a fairy tale!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("A kingdom is built on beliefs. What is a battle to a kingdom? And it’s not unwinnable, trust me. Go. You have your orders. Go!")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().StopAudio()
			};
		}
	}

	public class PreBattle3 : Cutscene {
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
				// Pre-battle 3
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().SetMessage("Your lord is dead, and I am your new commander. You might think why would a commander lead a suicide mission.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I’m leading, because it’s not a suicide mission. I don’t intend to die here and neither should you.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("All I’m asking is to hold the line until our cavalry reaches us. Hold the line!")
					.SetSpeaker("Blair").SetExpression("Neutral")
			};
		}
	}

	public class PostBattle3 : Cutscene {
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
				// Post-battle 3
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().SetMessage("Riders of Xingata! Protect your lord!")
					.SetSpeaker("Bruno").SetExpression("Angry"),
				S().SetMessage("Cavalry’s here! Hold just a little longer!")
					.SetSpeaker("Blair").SetExpression("Angry"),
				// Scene Transition
				S().AddLeaver("Blair"),
				S().AddLeaver("Bruno"),
				S().SetBackground("NewBackground"),
				// Scene 4
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().SetMessage("After a successful retreat, Blair leads the army to march east to chase the Tsubin army.")
					.SetSpeaker("Narrator"),
				S().SetMessage("The sound of marching steps, horseshoes and cart wheels repeats in calming rhythm.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Yet Blair can still hear the clashing of metal and screams of anger and pain.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("Are you sure you want me in command, not you?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I’ve been in command of a large group before. It just isn’t me.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("What about the protocol you love and cherish?")
					.SetSpeaker("Juniper").SetExpression("Smile"),
				S().SetMessage("Exceptions can be made during the time of war. Besides, Blair has gained the respect of this army.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("(To Blair) What went on inside your head to come up with an idea like that?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("I thought about what his majesty would do in my shoes.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().StopAudio(),
				S().SetAudio("TheBladeTakeTwo"),
				S().SetMessage("You think his majesty would’ve done what you did? Are you sure you know who you’re serving, Blair?")
					.SetSpeaker("Bruno").SetExpression("Frown"),
				S().SetMessage("What is that supposed to mean?")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("Bruno, caught off guard by his own words, avoids Blair’s inquisitive glare and looks forward. A rider holding the royal banner approaches.")
					.SetSpeaker("Narrator"),
				S().AddLeaver("Narrator"),
				S().AddActor(CutsceneSide.Left, "SoldierActor", "Messenger"),
				S().SetMessage("Commander Blair. I have an urgent order from his majesty the King.")
					.SetSpeaker("Messenger"),
				S().SetMessage("From his majesty himself? Let me see it. Drive off Corbitan raiders from the Central Plains? Isn’t the main army closer to the plains?")
					.SetSpeaker("Blair").SetExpression("Surprised"),
				S().SetMessage("The King sent small detachments from the main army to deal with the raiders, but there are more raiders than they could handle.")
					.SetSpeaker("Messenger"),
				S().SetMessage("What about delaying Tsubin army?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("His order says securing the Central Plains is our highest priority.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("He’s right. If we don’t secure the plains, 25,000 Xingatan soldiers will starve to death before they succumb to the plague or the war.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Tell his majesty that we will deal with the raiders with utmost swiftness. (Turns to the army) Spread the word. We’re changing course.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().StopAudio()
			};
		}
	}

	public class PreBattle4 : Cutscene {
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
				// Pre-battle 4
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().SetMessage("Damned raiders are killing unarmed farmers.")
					.SetSpeaker("Bruno").SetExpression("Angry"),
				S().SetMessage("Soldiers of Xingata! Drive off the raiders and save your fellow Xingatans!")
					.SetSpeaker("Blair").SetExpression("Angry")
			};
		}
	}

	public class PostBattle4 : Cutscene {
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
				// Post-battle 4
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Shreya"),
				S().AddActor(CutsceneSide.Right, "SoldierActor", "Soldier"),
				S().SetMessage("Commander. One of the prisoners we rescued wishes to speak to you.")
					.SetSpeaker("Soldier"),
				S().AddLeaver("Soldier"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("I’m Captain Shreya of the Royal Guard.")
					.SetSpeaker("Shreya"),
				S().SetMessage("Royal guard? Why aren’t you with the King?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("We were escorting a convoy of royal possessions to the King but we were ambushed. " +
					"We need you to track down and retrieve the possessions we lost.")
					.SetSpeaker("Shreya"),
				S().SetMessage("What are these royal possessions?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("They are personal items owned by the royal family. That is all you need to know.")
					.SetSpeaker("Shreya"),
				S().SetMessage("So, you want us to chase down his majesty’s favorite couch?")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Juniper!")
					.SetSpeaker("Bruno").SetExpression("Frown"),
				S().SetMessage("They took the carts to their camp, and I know which direction it is.")
					.SetSpeaker("Shreya"),
				S().SetMessage("If we attack their camp, raiding parties on the field will scatter and dissolve.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Since our missions align, sure. Show us the way, Captain.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().StopAudio(),
				S().SetAudio("TheBladeTakeTwo"),
				S().SetMessage("Thank you, Commander, but I must ask you one more thing. You must not open any chests that we retrieve.")
					.SetSpeaker("Shreya"),
				S().SetMessage("If you do, it will be considered a crime against the royal family.")
					.SetSpeaker("Shreya"),
				S().AddLeaver("Shreya"),
				S().AddActor(CutsceneSide.FarRight, "NarratorActor", "Narrator"),
				S().SetMessage("Blair nods without giving Shreya’s demand much thought. Juniper takes Bruno aside.")
					.SetSpeaker("Narrator"),
				S().AddLeaver("Blair"),
				S().SetMessage("You’ve worked with the King before the plague, right?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Five years ago, when Corbita raided our borders.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Is it usual for our King to send his personal bodyguards away on a transport mission?")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("No. Not even for him.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().StopAudio()
			};
		}
	}

	public class PreBattle5 : Cutscene {
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
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Shreya"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("Look up ahead! There's a Corbitan supply train!")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("A supply train? This deep in Xingatan territory? This is no raid! It's an invasion!")
					.SetSpeaker("Bruno").SetExpression("Surprised"),
				S().SetMessage("Our forces are far too small to repell the entire Corbitan army...")
					.SetSpeaker("Blair"),
				S().SetMessage("They must've broken camp before we could arive. And that means his majesty's posessions are with them!")
					.SetSpeaker("Shreya"),
				S().SetMessage("Either way, we cannot let them travel our countryside unopposed. Prepare for battle!")
					.SetSpeaker("Blair"),
			};
		}
	}

	public class PostBattle5 : Cutscene {
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
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Shreya"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("We smashed their escort, but we were too slow. They're already retreating. We have to go after them!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Hold up, Juniper. A supply train isn't a vanguard - it was going to support a larger force somewhere.")
					.SetSpeaker("Bruno").SetExpression("Frown"),
				S().SetMessage("They were traveling west on the Yai road. The only major city from here is...")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Arnan.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				//Juniper doesn't actually care abt the posessions, she just knows it'll help convince blair. Blair is well aware of this.
				S().SetMessage("You want to march to Arnan on the off chance it's under siege? We can't just let them get away!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("They could have crucial intel on Corbitan plans. Not to mention his majesty's posessions!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("No, we cannot let Arnan fall. If we can warn the garrison there they'll have a much better chance of holding the city.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("We can do both. I'll head east with our calvlary. If we ride hard we should be able to catch them before " +
					"they cross back into Corbita. You two take our armor and head west to Arnan.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Hmm... That could work. I'll go with you - you need my help more than Bruno does.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("I will come as well. It was my reponsibility to guard his majesty's posessions and I could not face him if I failed.")
					.SetSpeaker("Shreya").SetExpression("Neutral"),
				S().SetMessage("Splitting our forces when we're almost surely outnumbered? Potentially fighting the enemy on their own ground?")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Can you think of any other way? Arnan needs your experience. We can be spared to chase leads.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Alright alright, you and your silver tongue have convinced me.")
					.SetSpeaker("Bruno").SetExpression("Smile"),
				S().SetMessage("Go! Ride east with our light units! Do not let the Corbitans escape!")
					.SetSpeaker("Bruno").SetExpression("Angry"),
				S().SetMessage("We won't. I expect to see your smiling face when we meet back at the main force.")
					.SetSpeaker("Juniper").SetExpression("Smile"),
			};
		}
	}

	public class PreBattle6 : Cutscene {
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
				S().AddActor(CutsceneSide.FarLeft, "BrunoActor", "Bruno"),
				S().SetMessage("It feels strange leading without Blair and Juniper. I hope they're safe.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Soldier"),
				S().SetMessage("Sir! Corbitan calvalry have been spotted on the roads! They're moving to encircle us!")
					.SetSpeaker("Soldier").SetExpression("Neutral"),
				S().SetMessage("Our fears proved correct. Be brave Xingatans! The walls of Arnan have weathered many attacks, they will not fail us now!")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
			};
		}
	}

	public class PostBattle6 : Cutscene {
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

			};
		}
	}

	public class PreBattle7 : Cutscene {
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
				// Pre-battle 5
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("Look up ahead! In the pass, calvalry wearing Corbitan colors! They recieved reinforcements!")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("I can make out the stolen carts. I wonder what’s in those chests.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Don’t even think about opening them. Focus on the battle.")
					.SetSpeaker("Blair").SetExpression("Neutral")
			};
		}
	}

	public class PostBattle7 : Cutscene {
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
				// Scene 5
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().SetMessage("A wounded soldier lies on the ground. Life is visibly escaping from his terrified eyes.")
					.SetSpeaker("Narrator"),
				S().SetMessage("A healer is trying his healing magic on the soldier’s wounds again and again to no avail.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Next to them lies the corpse of the enemy commander and a strange weapon that shines bright by itself.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Soldier"),
				S().SetMessage("What happened here?")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("The enemy commander lunged at us. We got her, but she still managed to stab one of us before she fell.")
					.SetSpeaker("Soldier"),
				S().SetMessage("If it was a simple stab wound, why can’t our healer heal it?")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().AddLeaver("Soldier"),
				S().StopAudio(),
				S().SetAudio("TheBladeTakeTwo"),
				S().SetMessage("Juniper suddenly yells at Captain Shreya to stop. Shreya is standing by the corpse of the " +
					"enemy commander. The strange weapon is gone from the ground.")
					.SetSpeaker("Narrator"),
				S().AddLeaver("Narrator"),
				S().AddActor(CutsceneSide.Right, "NarratorActor", "Narrator"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Shreya"),
				S().AddActor(CutsceneSide.Left, "JuniperActor", "Juniper"),
				S().SetMessage("What do you have in your hands, Captain?")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("This is Royal Guard business. Stay out of it.")
					.SetSpeaker("Shreya"),
				S().SetMessage("Do you see any Royal Guard here besides yourself?")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Are you threatening me?")
					.SetSpeaker("Shreya"),
				S().SetMessage("Oh, I can do better.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetAudio("ChestOpening"),
				S().SetMessage("Juniper walks up to the elegant chest with a royal seal on it still loaded on the back of a cart. She opens it without hesitation.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Juniper! What did you...?")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("Come take a look at what the Royal Guard was sent to transport.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetAudio("TheBladeTakeTwo"),
				S().SetMessage("It’s filled with enough Pieces of Heaven to cure half the plagued population of Xingata. Care to explain, Captain?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("I have nothing to say.")
					.SetSpeaker("Shreya"),
				S().SetMessage("So you knew? The royal family was sitting on this much cure while its subjects suffered and died in misery!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("I know a Piece of Heaven when I see one. That dagger in your hand is made out of it, isn’t it? I didn’t even know you could weaponize the crystals.")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Stop. We’re done here. Burn the bodies and pack up.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("And do what? Hand over the crystals to the King?")
					.SetSpeaker("Juniper").SetExpression("angry"),
				S().SetMessage("Yes. The King must have had a reason. He must have. We can’t lose faith in the Crown at a time of war!")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("Are you out of your mind? He betrayed Xingata!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("He would never betray Xingata! He is everything we hold dear and look up to! We deliver the royal possessions to the King.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().AddLeaver("Shreya"),
				S().AddLeaver("Blair"),
				S().SetMessage("Juniper is furious, but the army is already following its commander’s order.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Sometime later, Juniper sneaks up on a chest through the confusion of hustling soldiers, picks up a Piece of Heaven, and hides it in her knapsack.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.FarRight, "BlairActor", "Blair"),
				S().SetMessage("As soon as she turns around, her eyes meet Blair’s gaze from a distance. Their eyes lock for a long moment, then Blair walks away.")
					.SetSpeaker("Narrator"),
				S().StopAudio()
			};
		}
	}

	public class PreBattle8 : Cutscene {
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
				// Scene 6
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().SetMessage("Corbitans defeated and crystals in hand, Blair's forces return to the main body of the Xingatan army. The trip is tense, but passes without event.")
					.SetSpeaker("Narrator"),
				S().SetMessage("The two are overjoyed to reunite with Bruno, who tells them of his victory at Arnan.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Blair, immediately after arriving at the camp near Harmony Crater, is called to the King’s tent.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Inside his tent, King Rouen, in his sweat-stained tunic, sits at the end of a long table.")
					.SetSpeaker("Narrator"),
				S().SetMessage("He is joined by the marshall and vassal lords. He throws a cup of wine across his tent in fury.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.FarRight, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarLeft, "RouenActor", "King Rouen"),
				S().SetMessage("Get the hell out! All of you! What good is your counsel if all you’re going to tell me is it can’t be done?")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Blair steps aside while the war council leaves.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Marshall, looking tired, recognizes Blair and puts a hand on Blair’s shoulder, then walks out.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Your majesty. It is an honor to finally...")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetAudio("TheBladeTakeTwo"),
				S().SetMessage("Don’t just stand there, sit.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Blair freezes for a brief moment while the King yells outside to bring in more drinks.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Blair eventually takes a seat at the King’s table. Rouen starts digging into the steak on his plate.")
					.SetSpeaker("Narrator"),
				S().SetMessage("I’ve been hearing your name for the past weeks. Quite an achievement for a fresh graduate.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Thank you, your majesty.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Bruno tells me that you admire me.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Yes, sir. Very much so.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Why?")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Sir?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("You heard the question. Don’t make me repeat it.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("... I grew up reading and listening to the tales of our kings and queens. Defenders of Xingatan values and examples of Xingatan virtues.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Heroes who put their kingdom before themselves. I grew up looking up to them, sir. I’ve always wanted to be like them.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I’ve wanted to be like-")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Blair’s last word is barely audible over King Rouen’s sudden yelp. Rouen drops his fork and grabs his jaw.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Fuck!")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Sir?")
					.SetSpeaker("Blair").SetExpression("Surprised"),
				S().SetMessage("Damn toothache.")
					.SetSpeaker("King Rouen"),
				S().SetAudio("TheBladeTakeTwo"),
				S().SetMessage("When his pain subsides, Rouen eyes Blair for a moment like a detective trying to catch a lie.")
					.SetSpeaker("Narrator"),
				S().SetMessage("There isn’t a trace of flattery in Blair’s face. Rouen chuckles.")
					.SetSpeaker("Narrator"),
				S().SetMessage("You know why I’ve called you?")
					.SetSpeaker("King Rouen"),
				S().SetMessage("I’m sure Captain Shreya gave a full report, sir.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("She did. You opened what you were not supposed to.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("I will take full responsibility for-")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetAudio("TheBlade"),
				S().SetMessage("I didn’t call you to punish you. Any of you. You proved your loyalty by delivering the crystals back to me.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("King Rouen suddenly looks tired. For the first time, Blair notices the deep circles under his eyes.")
					.SetSpeaker("Narrator"),
				S().SetMessage("The truth is, Blair, the war isn't going well. The Velgarian armor has dug in at the crater's rim, and they have us out manned, out manuvered, and out supplied.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Not one of our strategists has come up with a viable plan to dislodge them.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Your majesty, if I may. Just a week ago we repelled a Coribtan attack on Arnan. They were likely trying to take control of our food supply.")
					.SetSpeaker("Blair"),
				S().SetMessage("What if we tried a similar tactic? Cut their supply lines, split the Velgaran army, and force them to fight on open ground.")
					.SetSpeaker("Blair"),
				S().SetMessage("Rouen is shaking his head before Blair's finishes their sentence.")
					.SetSpeaker("Narrator"),
				S().SetMessage("I have neither the soldiers nor the months a siege on Brunwara would require.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("You don't need to wait for a months. Give me two weeks and I'll have Brunwara, and the Velgaran supply lines with it.")
					.SetSpeaker("Blair"),
				S().SetMessage("If you're confident I will consider it. You've proven yourself a competent tactician.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Thank you, your majesty.")
					.SetSpeaker("Blair"),
				S().SetMessage("Blair bows and stands to leave, but Rouen stops them.")
					.SetSpeaker("Narrator"),
				S().StopAudio(),
				S().SetMessage("One more thing. Before I let you go, my quartermaster says the number of crystals don’t add up from the chests you retrieved.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("You don’t happen to know anything about that, do you?")
					.SetSpeaker("King Rouen"),
				S().SetMessage("... It must have been Corbitans, sir.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("It must’ve been. Go prepare for the assault.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Your majesty.")
					.SetSpeaker("Blair").SetExpression("Neutral"),

				//Scene change
				S().AddLeaver("Blair"),
				S().AddLeaver("King Rouen"),
				S().SetBackground("NewBackground"),
				// Scene 1
				S().AddActor(CutsceneSide.FarLeft, "JuniperActor", "Juniper"),
				S().SetMessage("Rouen agrees to Blair's plan, and the three friends find themselves leading their largest force yet.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Juniper elected to scout ahead with a few units, looking for any sign of Velgarian defenses along the roads.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Her forces have setup camp for the night, but Juniper is unable to sleep. She hikes up a nearby hill and admires the fields soaked in moonlight.")
					.SetSpeaker("Narrator"),
				S().SetMessage("It feels wrong to rest this deep in enemy teritory... I miss being able to sleep at night.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("As Juniper rests, she sees movement in the distance.")
					.SetSpeaker("Narrator"),
				S().SetMessage("There's no mistaking it, those are soldiers moving along the road. Their scouts mustve spotted us!")
					.SetSpeaker("Juniper").SetExpression("Surprised"),
				S().SetMessage("Juniper races dowm the hill, shouting as she goes.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Everybody, up! Velgarian ambush!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
			};
		}
	}

	public class PostBattle8 : Cutscene {
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


			};
		}
	}

	public class PreBattle9 : Cutscene {
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
				S().AddActor(CutsceneSide.Left, "JuniperActor", "Juniper"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().SetMessage("That must be half the Velgarian army up ahead!")
					.SetSpeaker("Blair").SetExpression("Surprised"),
				S().SetMessage("The troops that ambushed us must've warned the main force.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("It certainly seems we've distracted them. Let's hope we have the abilities to match our threats.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
			};
		}
	}

	public class PostBattle9 : Cutscene {
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


			};
		}
	}
	public class PreBattle10 : Cutscene {
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
				S().AddActor(CutsceneSide.Left, "JuniperActor", "Juniper"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().SetMessage("At last we arrive, Brunwara, the largest city in Velgari.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
			};
		}
	}

	public class PostBattle10 : Cutscene {
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

			};
		}
	}
	public class PreBattle11 : Cutscene {
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
				// Pre-battle 6
				S().PauseBattleTheme(),
				S().SetAudio("TheBlade"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().SetMessage("Blair, Juniper, and Bruno return safely to the Xingatan camp, where they recieve a hero's welcome.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.FarRight, "RouenActor", "King Rouen"),
				S().SetMessage("Blair, there you are! It seemed impossible, but your plan was successful. You have earned my respect.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Tomorrow, all four powers of the world will clash at Harmony Crater. The following battles will determine the fate of this world.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Right now, the Velgari Alliance holds the Crater. I want you and your army to lead the charge up the Crater tomorrow.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Pave the way to a world of everlasting peace under my rule.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Blair’s eyes redden.")
					.SetSpeaker("Narrator"),
				S().SetMessage("It will be my honor.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Good. I will leave you to prepare.")
					.SetSpeaker("King Rouen"),
				S().AddLeaver("King Rouen"),
				S().AddLeaver("Narrator"),
				S().SetBackground("NewBackground"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.Left, "JuniperActor", "Juniper"),
				S().SetMessage("The first wave to hit the foot of the wall isn’t meant to survive the siege. You know the King is sending us to our deaths, right?")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("Bruno. What happens once you kill your beliefs? Are you grown up then?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("No. You must replace it with new ones. Then, you are grown up.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Blair thinks for a moment, then turns to the army."),
				S().SetMessage("You haven’t known me for long, but have I ever failed you? I haven’t lost a single battle.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Beyond that Velgarian fortifications lie the cure. Your ailing families back home are waiting for you, YOU to bring them back.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I won’t fail you now. I won’t fail Xingata. Follow me to the top of the Crater, and I will give you the cure.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().StopAudio()
			};
		}
	}

	public class PostBattle11 : Cutscene {
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
				// Post-battle 6
				S().AddActor(CutsceneSide.FarLeft, "NarratorActor", "Narrator"),
				S().SetMessage("Blair, Juniper and Bruno stand atop the rim of the crater, bloodied and exhausted. Battle is still fierce along the rim of the Crater.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Across the vast inner crater thousands of shattered pieces of charred meteorites lie among Velgarian tents.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Pieces of Heaven that are embedded in the meteorites glitter with light.")
					.SetSpeaker("Narrator")
			};
		}
	}

	public class PreBattle12 : Cutscene {
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
				// Pre-battle 7
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "JuniperActor", "Juniper"),
				S().AddActor(CutsceneSide.Right, "BrunoActor", "Bruno"),
				S().SetMessage("I can’t believe we are still alive.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Don’t celebrate yet. This battle is far from over.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().AddActor(CutsceneSide.FarRight, "RouenActor", "King Rouen"),
				S().SetMessage("You are alive! Well done, officers. Thanks to you, we now hold the Crater with minimal casualties.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Anything for you, your majesty.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Corbita and Velgari are retreating. Corbitan cavalries were of no use attacking up the slope, and the Velgarians")
					.SetSpeaker("King Rouen"),
				S().SetMessage("that sent Corbitans home saw their lines breaking on the other sides. Rally your soldiers and join the rank. We now face the full might of Tsubin.")
					.SetSpeaker("King Rouen")
			};
		}
	}

	public class PostBattle12 : Cutscene {
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
				// Scene 7
				S().PauseBattleTheme(),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Right, "NarratorActor", "Narrator"),
				S().SetMessage("Battle for Harmony Crater is over.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Inside their tent, Blair sits alone on the bed in pitch black darkness. In nocturnal silence, their mind is at war.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Blair doesn’t hear the approaching footsteps. A shadow enters the tent.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Blair tries to say something in protest, but a radiant dagger made out of pure crystal is plunged through their abdomen.")
					.SetSpeaker("Narrator"),
				S().SetAudio("TheBladeTakeTwo"),
				S().SetMessage("The assassin pulls out the dagger for a second strike, but Juniper and Bruno barge in. Bruno lunges to grapple the assassin,")
					.SetSpeaker("Narrator"),
				S().SetMessage("but the assassin escapes after a brief struggle and runs out the tent. Bruno pursues. Juniper steps outside and screams for a healer.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.Left, "SoldierActor", "Healer"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().SetMessage("I...I can’t.")
				  .SetSpeaker("Healer"),
				S().SetMessage("What do you mean you can’t?!")
				  .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("My magic isn’t working on the commander’s wound. I can’t heal it!")
				  .SetSpeaker("Healer"),
				S().SetMessage("Blair’ wound is bleeding with no sign of slowing down. Blair's face is trembling in terror. Juniper pulls out a Piece of Heaven from under her tunic.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Do you know how to use it?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Where did you...?")
				  .SetSpeaker("Healer"),
				S().SetMessage("Do you or not?!")
				  .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("I, I do, but I don’t know if it will work on...")
				  .SetSpeaker("Healer"),
				S().SetMessage("Just do it!")
				  .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("The light of the crystal slowly dims as the healer murmurs his incantations.")
					.SetSpeaker("Narrator"),
				S().StopAudio(),
				S().SetMessage("The bleeding slows to a stop, then the wound starts to seal up. Blair’s quivering irises finds their focus again.")
					.SetSpeaker("Narrator"),
				S().SetMessage("I thought I was losing you.")
				  .SetSpeaker("Juniper").SetExpression("Smile"),
				S().SetMessage("Thank you. And I’m sorry. That crystal was meant for your sister.")
				  .SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("She isn’t dying in front my eyes.")
				  .SetSpeaker("Juniper").SetExpression("Neutral"),
				S().AddLeaver("Healer"),
				S().AddLeaver("Narrator"),
				S().SetAudio("CutsceneBgm"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().SetMessage("Bruno comes back into the tent, angry and frustrated.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.Right, "BrunoActor", "Bruno"),
				S().SetMessage("Thank the gods you are alive, Blair. I lost him. He must have help within our army, just like the ones that attacked Juniper and me.")
				   .SetSpeaker("Bruno").SetExpression("Angry"),
				S().SetMessage("Or he’s part of our army.")
				   .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Bruno, with his eyes closed, silently nods.")
					.SetSpeaker("Narrator"),
				S().SetMessage("What are you saying?")
				   .SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("The assassins had to be sent by the King. Who else has crystalline daggers?")
				   .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("We also know he’s been hoarding the crystals while his subjects died covered in their own puss.")
				   .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("One of our soldiers told me that royal wizards are performing a ritual at the center of the Crater as we speak.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("The King must be turning the crystals into weapons once again. This time, all of them at once.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("He has no interest in saving his subjects. We must challenge the King. That is the only way to save Xingata.")
				   .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Blair. He is not the hero of your childhood. Be the leader Xingata needs you to be.")
				   .SetSpeaker("Juniper").SetExpression("Neutral")

			};
		}
	}

	public class PreBattle13 : Cutscene {
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
				// Scene 8
				S().PauseBattleTheme(),
				S().AddActor(CutsceneSide.Right, "NarratorActor", "Narrator"),
				S().SetAudio("MainTheme1"),
				S().SetMessage("At dawn, thousands of Xingatan soldiers stand at Harmony Crater.")
					.SetSpeaker("Narrator"),
				S().SetMessage("All of the Pieces of Heaven have disappeared from scattered meteorites.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "RouenActor", "King Rouen"),
				S().SetMessage("In the center of them all, King Rouen stands with a heavenly sword in his hand.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Made of pure crystals, the sword emanates a blinding light that pulses like a heartbeat.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Last night, a hero of Royal Retinue, Commander Blair has been killed by assassins sent from our enemies.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Cowards! They show in their despicable act that they are not rulers chosen by the gods. But I am.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Yesterday’s victory is the gods’ proclamation that it is not my will, but my fate to rule.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("All the world's Pieces of Heaven have been condensed into this sword. It is now the only salvation from the plague.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("And now that the gods have gifted it to me, I am humanity’s only salvation.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Through me and only me, this world will heal and be united.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Rouen stops speaking when he notices Blair, Juniper and Bruno marching down the crater toward him.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Behind them, the army that followed them from the western rivers marches with them fully armed.")
					.SetSpeaker("Narrator"),
				S().SetMessage("The army stops some distance away, but Blair, Juniper and Bruno come face to face with Rouen.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.FarRight, "BlairActor", "Blair"),
				S().SetAudio("TheBlade"),
				S().SetMessage("I don’t know what you are trying to accomplish here, Blair. We’ve won.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("You've won. Only to doom our people.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("The power to cure the plague is right here in my hand.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Meaning you will decide who lives and who dies.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("The past kings and queens you admire so much are eyeing me with envy from the heavens!")
					.SetSpeaker("King Rouen"),
				S().SetMessage("This was their dream. A utopia! A world of peace and order under one absolute ruler!")
					.SetSpeaker("King Rouen"),
				S().SetMessage("I can’t let you have that power.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("I command the entire Xingatan army. What makes you think you can stop me?")
					.SetSpeaker("King Rouen"),
				S().SetMessage("A kingdom is built on beliefs. When that beliefs are betrayed, the kingdom falls.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("Blair’s army marches to surround the King. Royal Retinue marches in response. Royal Guards urge the King to fall back.")
					.SetSpeaker("Narrator"),
				S().SetMessage("However, no other army on the Crater makes a move to stop Blair’s apparent act of revolt.")
					.SetSpeaker("Narrator"),
				S().SetMessage("I sent a letter to every lord last night. They have lost their loved ones while you sat on your cache of Pieces of Heaven.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("They won’t fight for you.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("This. This is why I should’ve killed you. This time, I’ll kill you with my own hands.")
					.SetSpeaker("King Rouen"),
				S().StopAudio()
			};
		}
	}

	public class PostBattle13 : Cutscene {
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
				// Scene 9
				S().PauseBattleTheme(),
				S().SetAudio("MainTheme1"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "RouenActor", "King Rouen"),
				S().SetMessage("King Rouen is defeated. He lies on the ground facing Blair who stands before him with the crystalline sword in hand.")
					.SetSpeaker("Narrator"),
				S().SetMessage("I took you for a naive kid chasing fairy tales.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("You’ve won the war. You held all the Pieces of Heaven your kingdom needed. Why choose this path when you could’ve saved your kingdom?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Because I could’ve saved the world. Rid it of war forever. Be the last king to have known the horrors of war.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("I wasn’t the only one chasing fairy tales, then. All my life, I wanted to be like you.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I believed you were special! Like the kings and queens from the tales! But you were just a man, mortal and mistaken like the rest of us.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Surrender now, and....")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Hand over the crown in peace? I don’t think so.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Rouen grabs the sword’s blade and buries it in his chest.")
					.SetSpeaker("Narrator"),
				S().SetMessage("The Monarch is dead. Long live…")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Rouen, instead of finishing the phrase, grins at Blair. Blair’s tears drop on Rouen as his last breath escapes him.")
					.SetSpeaker("Narrator"),
				S().AddLeaver("King Rouen"),
				S().StopAudio(),
				S().SetMessage("Blair takes out the sword and picks up the crown from Rouen’s still head.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Blair looks down: the crown on one hand and the crystalline sword on the other.")
					.SetSpeaker("Narrator"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().SetMessage("And he dies without naming his successor. Even at his last moment, he thinks nothing of his kingdom.")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Blair. If we don’t take our next step very carefully...")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetAudio("TheBlade"),
				S().SetMessage("To everyone’s shock, Blair puts on the crown.")
					.SetSpeaker("Narrator"),
				S().SetMessage("I won’t promise you a world without a war. I don’t claim to be humanity’s only salvation.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I claim the crown simply by my own merit. I proved my leadership.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I proved my courage to stand up to the unjust, even if he is the most powerful man in Xingata. No, the world.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I will prove my righteousness by using this sword only to root out the plague from every town in the land.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("So, I ask you. Will you believe in me to lead you?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Bruno and Juniper are the first to kneel. Blair’s army follows suit.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Without any expression of agreement, thousands of soldiers standing in Harmony Crater kneel in waves.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Xingatan lords look to their soldiers, their people, and one-by-one kneel before Blair.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Blair looks to the dead eyes of Rouen, and declares:")
					.SetSpeaker("Narrator"),
				S().SetMessage("Long live the Monarch.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
			};
		}
	}

}