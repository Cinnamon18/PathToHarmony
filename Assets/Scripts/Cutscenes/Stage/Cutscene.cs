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
				// PostTutorial
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
                    .SetSpeaker("Blair").SetExpression("Neutral"),
                S().SetMessage("Glory to his majesty, the King!")
                    .SetSpeaker("Cadets"),
				// Scene Transition
				S().AddLeaver("Cadets"),
				S().AddLeaver("Blair"),
				S().AddLeaver("Narrator"),
				S().AddLeaver("Headmaster"),
                S().SetBackground("TempBackground"),
				// Scene 1
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
                S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
                S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
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
                S().AddActor(CutsceneSide.Right, "BrunoActor", "Bruno"),
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
                S().SetMessage("… Right. The war.")
                    .SetSpeaker("Blair").SetExpression("Neutral"),
                S().SetMessage("I’ll go and arrange your departure. See you tomorrow.")
                    .SetSpeaker("Bruno").SetExpression("Neutral")
        };
		}
	}

	public class PreBattle1 : Cutscene
	{
		public override bool executionCondition(ExecutionInfo info)
		{
			if (hasExecuted)
			{
				return false;
			}
			if (info.halfTurnsElapsed == 0)
			{
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage()
		{
			hasExecuted = true;
			return new StageBuilder[] {
				// Scene 2
				S().AddActor(CutsceneSide.FarLeft, "NarratorActor", "Narrator"),
				S().SetMessage("Western front. Inside Lord Sweyn’s tent, days later.")
					.SetSpeaker("Narrator"),
				S().AddLeaver("Narrator"),
				S().AddActor(CutsceneSide.FarLeft, "SweynActor", "Sweyn"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().AddActor(CutsceneSide.Right, "BlairActor", "Blair"),
				S().SetMessage("Tsubin ships are transporting their army up the rivers of Ida and Iouna. We know they will land " +
					"over here, just past the river fork. While they are landing, they will be disorganized and few in numbers. ")
					.SetSpeaker("Sweyn"),
				S().SetMessage("We’ll hit them then, and retreat. In the meantime, I’ve ordered all the western garrisons guarding Ida river to " +
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
					"Border Post Kova. It’ll be a great opportunity for us to lead real soldiers for the first time.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("They have my approval and my recommendation. They are the brightest graduates of Royal Academy.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Fine. I’ll have a detachment ready in 20 minutes. Check out the garrison but engage the enemy only " +
					"when necessary. I can’t afford to lose any soldiers when I’m already outnumbered 5 to 1.")
					.SetSpeaker("Sweyn"),
				S().AddLeaver("Sweyn"),
				S().SetMessage("I believe in your judgement, Blair, but this isn’t a mock battle. Be careful. Are you fine with Blair taking the lead?")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("I’m fine with that.")
					.SetSpeaker("Juniper").SetExpression("Smile"),
				S().SetMessage("We’ll be back soon. Hopefully the garrison is okay and we are overreacting.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				// Scene Transition
				S().AddLeaver("Bruno"),
				S().AddLeaver("Blair"),
				S().AddLeaver("Juniper"),
				S().SetBackground("TempBackground"),
				// Pre-battle 1
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().SetMessage("There it is, Border Post Kova. At least our flag still flies.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Do you see that thing moving on the river? Is that a raft?")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("With Tsubin soldiers on it. They are crossing uncontested. The garrison must have already fallen.")
					.SetSpeaker("Juniper").SetExpression("Surprised"),
				S().SetMessage("Then we must take it back. (turns to the soldiers) Soldiers of Xingata! Kova has fallen. More enemies " +
					"are crossing the river as we speak.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("If we don’t retake that garrison now, enemies will hit us in the back while we " +
					"are facing the main Tsubin force. With me!")
					.SetSpeaker("Blair").SetExpression("Neutral")
			};
		}
	}

	public class PostBattle1 : Cutscene
	{
		public override bool executionCondition(ExecutionInfo info)
		{
			if (hasExecuted)
			{
				return false;
			}
			if (info.afterVictoryImage)
			{
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage()
		{
			hasExecuted = true;
			return new StageBuilder[] {
				// Post-battle 1
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().SetMessage("Xingatan soldiers are busy moving corpses into a pile. Brutal warriors of Tsubin have turned into ragdolls; " +
					"their arms and legs flailing about helplessly in the hands of Xingatan soldiers. Blair and Juniper watche the scene from a distance."),
				S().SetMessage("I’ve ordered neighbouring garrisons to reinforce here.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("This isn’t how I imagined a battle. I don’t know what I imagined.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Hold yourself together.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("How can you be so calm after all this?")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("I’m not. I can’t stop my hands from shaking. But we are fighting for a right cause. We are saving the people of Xingata.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Yeah. We must be. I need a change in scenery. Let’s get out of here.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("...Yes. Burn the bodies and form up! We’re heading back!")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				// Scene Transition
				S().AddLeaver("Blair"),
				S().AddLeaver("Juniper"),
				S().AddLeaver("Narrator"),
				S().SetBackground("TempBackground"),
				// Scene 3
				S().AddActor(CutsceneSide.Right, "NarratorActor", "Narrator"),
				S().SetMessage("Lord Sweyn’s camp. The whole camp appears much busier."),
				S().AddActor(CutsceneSide.FarLeft, "SweynActor", "Sweyn"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.FarRight, "BlairActor", "Blair"),
				S().SetMessage("So, enemies were already crossing behind us. Great work retaking Kova, because Tsubin ships are approaching. They will land within the next three hours.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("They will soon figure out we are few in numbers. We must do the most damage and retreat.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Oh, we are aware. Every detail of the operation has been ironed out.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("You two will stay with me at the center to hold the line while archers and mages bombard the landing.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("Bruno has a experience leading cavalry, so he will lead a group to keep the enemies from flanking us from our east. I have a knight leading another cavalry group to our west.")
					.SetSpeaker("Sweyn"),
				S().SetMessage("Blair and Juniper look at the tactical map drawn out on the table in silence, still shaken by their last battle."),
				S().SetMessage("We form up in 30 minutes. I’ll send for you when we do. Get some rest.")
					.SetSpeaker("Sweyn"),
				S().AddLeaver("Sweyn"),
				S().AddLeaver("Narrator"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("War isn’t quite what you believed to be, is it?")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("No. I imagined a sense of pride and accomplishment. In reality, I was just relieved it was over.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Killing our youthful beliefs. That’s how we grow up. I have to head out early. Congratulations on your first victory. Try to get some rest. Try as if your life depends on it.")
					.SetSpeaker("Bruno").SetExpression("Neutral")
			};
		}
	}

	public class PostBattle2 : Cutscene
	{
		public override bool executionCondition(ExecutionInfo info)
		{
			if (hasExecuted)
			{
				return false;
			}
			if (info.afterVictoryImage)
			{
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage()
		{
			hasExecuted = true;
			return new StageBuilder[] {
				// Post-battle 2
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Sir Tibolt"),
				S().SetMessage("Royal Officer Blair! Royal Officer Blair!"),
				S().SetMessage("Yes! I’m here! Why hasn’t Lord Sweyn ordered a retreat yet?")
				   .SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("Lord Sweyn is dead.")
					.SetSpeaker("Sir Tibolt"),
				S().SetMessage("A brief silence sets in between them amidst the cacophony of the raging battle.")
					.SetSpeaker("Narrator"),
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
				S().SetMessage("‘What would his majesty do?’")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("There are endless waves of Tsubin warriors freshly landing on the river bank beyond it.")
					.SetSpeaker("Narrator"),
				S().SetMessage("It is not a suicide mission. Because I will lead it.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("No, you won’t. We’ve just lost one commander. We can’t lose another.")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("Let cavalry groups know of my intention. Tell them to disengage, sweep around and meet me 500 yards behind the " +
					"front line. Juniper, lead the main force in retreat.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Are you even listening?")
					.SetSpeaker("Juniper").SetExpression("angry"),
				S().SetMessage("No commander in their right mind would lead a suicide mission. So, if I lead it, soldiers will believe it isn’t a suicide mission.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("You can’t win an unwinnable battle just because you believe in it! That’s a fairy tale!")
					.SetSpeaker("Juniper").SetExpression("angry"),
				S().SetMessage("A kingdom is built on beliefs. What is a battle to a kingdom? And it’s not unwinnable, trust me. Go. You have your orders. Go!")
					.SetSpeaker("Blair").SetExpression("Smile"),
				// Scene Transition
				S().AddLeaver("Blair"),
				S().AddLeaver("Juniper"),
				S().AddLeaver("Narrator"),
				S().AddLeaver("Sir Tibolt"),
				S().SetBackground("TempBackground"),
				// Pre-battle 3
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().SetMessage("Your lord is dead, and I am your new commander. You might think why would a commander lead a suicide mission. " +
					"I’m leading, because it’s not a suicide mission.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I don’t intend to die here and neither should you. All I’m asking is to " +
					"hold the line until our cavalry reaches us. Hold the line!")
					.SetSpeaker("Blair").SetExpression("Neutral")
			};
		}
	}

	public class PostBattle3 : Cutscene
	{
		public override bool executionCondition(ExecutionInfo info)
		{
			if (hasExecuted)
			{
				return false;
			}
			if (info.afterVictoryImage)
			{
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage()
		{
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
				S().SetBackground("TempBackground"),
				// Scene 4
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "NarratorActor", "Narrator"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("The sound of marching steps, horseshoes and cart wheels repeats in calming rhythm. " +
					"Yet, Blair can still hear the clashing of metal and screams of anger and pain.")
					.SetSpeaker("Narrator"),
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
				S().SetMessage("You think his majesty would’ve done what you did? Are you sure you know who you’re serving, Blair?")
					.SetSpeaker("Bruno").SetExpression("frown"),
				S().SetMessage("What is that supposed to mean?")
					.SetSpeaker("Blair").SetExpression("frown"),
				S().SetMessage("Bruno, caught off guard by his own words, avoids Blair’s inquisitive glare and looks forward. A rider holding the royal banner approaches.")
					.SetSpeaker("Narrator"),
				S().AddLeaver("Narrator"),
				S().AddActor(CutsceneSide.Left, "SoldierActor", "Messenger"),
				S().SetMessage("Commander Blair. I have an urgent order from his majesty the King.")
					.SetSpeaker("Messenger"),
				S().SetMessage("From his majesty himself? Let me see it. Drive off Corbitan raiders from the Central Plains? Isn’t the main army closer to the plains?")
					.SetSpeaker("Blair").SetExpression("Surprised"),
				S().SetMessage("The King sent small detachments from the main army to deal with the raiders, but there are more raiders than detachments could handle.")
					.SetSpeaker("Messenger"),
				S().SetMessage("What about delaying Tsubin army?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("His order says securing the Central Plains is our highest priority.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("He’s right. If we don’t secure the plains, 25,000 Xingatan soldiers will starve to death before they succumb to the plague or the war.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Tell his majesty that we will deal with the raiders with utmost swiftness. (Turns to the army) Spread the word. We’re changing course.")
					.SetSpeaker("Blair").SetExpression("Neutral")
			};
		}
	}

	public class PreBattle4 : Cutscene
	{
		public override bool executionCondition(ExecutionInfo info)
		{
			if (hasExecuted)
			{
				return false;
			}
			if (info.halfTurnsElapsed == 0)
			{
				hasExecuted = true;
				return true;
			}
			return false;
		}

		public override StageBuilder[] getStage()
		{
			hasExecuted = true;
			return new StageBuilder[] {
			};
		}
	}

}