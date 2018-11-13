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
				S().AddActor(CutsceneSide.FarLeft, "NarratorActor", "Narrator"),
				S().SetMessage("Over a field flanked by a forest on one side and rows and steps of spectating seats on the other, two parties are in the heat of battle.")
					.SetSpeaker("Narrator"),
				S().SetMessage("The commanders of each party shout orders to soldiers who are all holding sparring weapons.")
					.SetSpeaker("Narrator"),
				S().SetMessage("You won't have any input this battle, but next time you'll play as Blair and victory will be your responsibility!")
					.SetSpeaker("Narrator"),
				S().SetMessage("You can move the camera with wasd, or by moving your cursor to the edge of the screen.")
					.SetSpeaker("Narrator"),
				S().SetMessage("There are six different game objectives.")
					.SetSpeaker("Narrator"),
				S().SetMessage("Intercept a VIP")
					.SetSpeaker("Narrator"),
				S().SetMessage("Escort a VIP")
					.SetSpeaker("Narrator"),
				S().SetMessage("Defend a zone")
					.SetSpeaker("Narrator"),
				S().SetMessage("Capture a zone. (You have to stay on it for a full turn)")
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
				// S().SetMessage("Blair has successfully won the mock battle with Juniper.")
				// 	,
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
                S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Headmaster"),
                S().SetMessage("As it is tradition, his majesty the King himself will grant the title of Royal Officer to the victor of the graduation ceremony. Cadet Blair, please step forward.")
                    .SetSpeaker("Headmaster"),
                S().SetMessage("Instructors, knights and the marshal on the spectator seats all look baffled at the old headmaster’s words."),
                S().SetMessage("They look back and forth between the headmaster, who is only beginning to realize his mistake, and the empty throne at the center."),
                S().AddActor(CutsceneSide.Right, "NarratorActor", "Cadets"),
                S().SetMessage("Blair draws their sword and points it, flat side of the blade facing up, to the empty throne."),
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
                S().SetMessage("Blair sits alone on the spectator seat, facing the empty field " +
                    "where the mock battle took place. Juniper walks up the stepped seats and sits next to Blair."),
                S().SetMessage("If anyone saw us, they’d think I’m the victor and you're the loser.")
                    .SetSpeaker("Juniper").SetExpression("Smile"),
                S().SetMessage("Blair chuckles and glances at the empty throne to their right."),
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
				S().SetMessage("Western front. Inside Lord Sweyn’s tent, days later."),
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
				S().SetMessage("We’ll be back soon. Hopefully the garrison is okay and we are overreacting.")
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
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().SetMessage("Xingatan soldiers are busy moving corpses into a pile. Brutal warriors of Tsubin have turned into ragdolls;"),
                S().SetMessage("their arms and legs flailing about helplessly in the hands of Xingatan soldiers. Blair and Juniper watches the scene from a distance."),
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
				S().SetMessage("...Yes. Burn the bodies and form up! We’re heading back!")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				// Scene Transition
				S().AddLeaver("Blair"),
				S().AddLeaver("Juniper"),
				S().SetBackground("TempBackground"),
				// Scene 3
				S().SetMessage("Lord Sweyn’s camp. The whole camp appears much busier."),
				S().AddActor(CutsceneSide.FarLeft, "SweynActor", "Sweyn"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.Right, "BlairActor", "Blair"),
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
				S().SetMessage("War isn’t quite what you believed to be, is it?")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("No. I imagined a sense of pride and accomplishment. In reality, I was just relieved it was over.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Killing our youthful beliefs. That’s how we grow up. I have to head out early. Congratulations on your first victory. Try to get some rest. Try as if your life depends on it.")
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
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Sir Tibolt"),
				S().SetMessage("Royal Officer Blair! Royal Officer Blair!")
				   .SetSpeaker("Sir Tibolt"),
				S().SetMessage("Yes! I’m here! Why hasn’t Lord Sweyn ordered a retreat yet?")
				   .SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("Lord Sweyn is dead.")
					.SetSpeaker("Sir Tibolt"),
				S().SetMessage("A brief silence sets in between them amidst the cacophony of the raging battle."),
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
				S().SetMessage("Blair looks away to the front line tightly locked in a close combat."),
				S().SetMessage("‘What would his majesty do?’")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("There are endless waves of Tsubin warriors freshly landing on the river bank beyond it."),
				S().SetMessage("It is not a suicide mission. Because I will lead it.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("No, you won’t. We’ve just lost one commander. We can’t lose another.")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Let cavalry groups know of my intention. Tell them to disengage, sweep around and meet me 500 yards behind the " +
					"front line. Juniper, lead the main force in retreat.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Are you even listening?")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("No commander in their right mind would lead a suicide mission. So, if I lead it, soldiers will believe it isn’t a suicide mission.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("You can’t win an unwinnable battle just because you believe in it! That’s a fairy tale!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("A kingdom is built on beliefs. What is a battle to a kingdom? And it’s not unwinnable, trust me. Go. You have your orders. Go!")
					.SetSpeaker("Blair").SetExpression("Smile")
			};
		}
	}

	public class PreBattle3 : Cutscene
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
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("The sound of marching steps, horseshoes and cart wheels repeats in calming rhythm."),
				S().SetMessage("Yet, Blair can still hear the clashing of metal and screams of anger and pain."),
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
					.SetSpeaker("Bruno").SetExpression("Frown"),
				S().SetMessage("What is that supposed to mean?")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("Bruno, caught off guard by his own words, avoids Blair’s inquisitive glare and looks forward. A rider holding the royal banner approaches."),
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

	public class PostBattle4 : Cutscene
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
				// Post-battle 4
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.FarRight, "ShreyaActor", "Shreya"),
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
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("They are personal items owned by the royal family. That is all you need to know.")
					.SetSpeaker("Shreya"),
				S().SetMessage("So, you want us to chase down his majesty’s favorite couch?")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("Juniper!")
					.SetSpeaker("Bruno").SetExpression("Frown"),
				S().SetMessage("They took the carts to their camp, and I know which direction it is.")
					.SetSpeaker("Shreya"),
				S().SetMessage("If we attack their camp, raiding parties on the field will scatter and dissolve.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Since our missions align, sure. Show us the way, Captain.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Thank you, Commander, but I must ask you one more thing. You must not open any chests that we retrieve.")
					.SetSpeaker("Shreya"),
				S().SetMessage("If you do, it will be considered a crime against the royal family.")
					.SetSpeaker("Shreya"),
				S().AddLeaver("Blair"),
				S().SetMessage("Blair nods without giving Shreya’s demand much thought. Juniper takes Bruno aside."),
				S().SetMessage("You’ve worked with the King before the plague, right?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Five years ago, when Corbita raided our borders.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Is it usual for our King to send his personal bodyguards away on a transport mission?")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("No. Not even for him.")
					.SetSpeaker("Bruno").SetExpression("Neutral")
			};
		}
	}

	public class PreBattle5 : Cutscene
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
				// Pre-battle 5
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().SetMessage("We must have hit them good for them to abandon their camp like that.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Or they are satisfied by their plunder. I see the stolen carts. I wonder what’s in those chests.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Don’t even think about opening them. Focus on the battle.")
					.SetSpeaker("Blair").SetExpression("Neutral")
			};
		}
	}

	public class PostBattle5 : Cutscene
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
				// Scene 5
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Right, "BrunoActor", "Bruno"),
				S().AddActor(CutsceneSide.FarRight, "SoldierActor", "Soldier"),
				S().SetMessage("A wounded soldier lies on the ground. Life is visibly escaping from his terrified eyes."),
				S().SetMessage("A healer is trying his healing magic on the soldier’s wounds again and again to no avail."),
				S().SetMessage("Next to them lies the corpse of the enemy commander and a strange weapon that shines bright by itself."),
				S().SetMessage("What happened here?")
					.SetSpeaker("Bruno").SetExpression("Frown"),
				S().SetMessage("The enemy commander lunged at us. We got her, but she still managed to stab one of us before she fell.")
					.SetSpeaker("Soldier"),
				S().SetMessage("If it was a simple stab wound, why can’t our healer heal it?")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().AddLeaver("Soldier"),
				S().SetMessage("Juniper suddenly yells at Captain Shreya to stop. Shreya is standing by the corpse of the " +
					"enemy commander. The strange weapon is gone from the ground."),
				S().AddActor(CutsceneSide.FarRight, "ShreyaActor", "Shreya"),
				S().AddActor(CutsceneSide.Left, "JuniperActor", "Juniper"),
				S().SetMessage("What do you have in your hands, Captain?")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("This is Royal Guard business. Stay out of it.")
					.SetSpeaker("Shreya"),
				S().SetMessage("Do you see any Royal Guard here other than yourself?")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Are you threatening me?")
					.SetSpeaker("Shreya"),
				S().SetMessage("Oh, I can do better.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Juniper walks up to the elegant chest with a royal seal on it still loaded on the back of a cart. She opens it without hesitation."),
				S().SetMessage("Juniper! What did you...?")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("Come take a look at what the Royal Guard was sent to transport.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("It’s filled with enough ‘Pieces of Heaven’ to cure half the plagued population of Xingata. Care to explain, Captain?")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("I have nothing to say.")
					.SetSpeaker("Shreya"),
				S().SetMessage("So you knew? The royal family was sitting on this much cure while its subjects suffered and died in misery!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("I know a ‘Piece of Heaven’ when I see one. That dagger in your hand is made out of it, isn’t it? I didn’t even know you could weaponize the crystals.")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Stop. We’re done here. Burn the bodies and pack up.")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("And do what? Hand over the crystals to the King?")
					.SetSpeaker("Juniper").SetExpression("angry"),
				S().SetMessage("Yes. The King must have had a reason. He must have. We can’t lose faith in the Crown at a time of war!")
					.SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("Are you out of your mind? He betrayed Xingata!")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("He would never betray Xingata! He is everything we hold dear and look up to! We deliver the royal possessions to the King. That’s an order.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("Juniper is furious, but the army is already following its commander’s order."),
				S().AddLeaver("Shreya"),
				S().SetMessage("Sometime later, Juniper sneaks up on a chest through the confusion of hustling soldiers, picks up a ‘Piece of Heaven’ and hides it in her knapsack."),
				S().SetMessage("As soon as she turns around, her eyes meet Blair’s gaze from a distance. Their eyes lock for a long moment, then Blair walks away.")
					
			};
		}
	}

	public class PreBattle6 : Cutscene
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
				// Scene 6
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "RouenActor", "King Rouen"),
				S().SetMessage("Blair, immediately after arriving at Xingatan main army camp near Harmony Crater, is called to the King’s tent."),
				S().SetMessage("Inside his tent, King Rouen, in his sweat-stained tunic, sits at the end of a long table."),
				S().SetMessage("He is joined by the marshall and vassal lords. He throws a cup of wine across his tent in fury."),
				S().SetMessage("Get the hell out! All of you. What good is your counsel if all you’re going to tell me is it can’t be done?")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Blair steps aside while the war council leaves.Marshall, looking tired, recognizes Blair and puts a hand on Blair’s shoulder, then walks out."),
				S().SetMessage("Your majesty. It is an honor to finally...")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Don’t just stand there, sit.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Blair freezes for a brief moment while the King yells outside to bring in more drinks."),
				S().SetMessage("Blair eventually takes a seat at the King’s table. Rouen starts digging into the steak on his plate."),
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
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("You heard the question. Don’t make me repeat it.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("… I grew up reading and listening to the tales of our kings and queens. Defenders of Xingatan values and examples of Xingatan virtues.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Heroes who put their kingdom before themselves. I grew up looking up to them, sir. I’ve always wanted to be like them. I’ve wanted to be like....")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Blair’s last word is barely audible over King Rouen’s sudden yelp.Rouen drops his fork and grabs his jaw."),
				S().SetMessage("Fuck!")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Sir?")
					.SetSpeaker("Blair").SetExpression("Surprised"),
				S().SetMessage("Damn toothache.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("When his pain subsides, Rouen eyes Blair for a moment like a detective trying to catch a lie."),
				S().SetMessage("There isn’t a trace of flattery in Blair’s face. Rouen chuckles."),
				S().SetMessage("You know why I’ve called you?")
					.SetSpeaker("King Rouen"),
				S().SetMessage("I’m sure Captain Shreya gave a full report, sir.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("She did. You opened what you were not supposed to.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("I will take full responsibility of…")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I didn’t call you to punish you. Any of you. You proved your loyalty by delivering the crystals back to me.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Tomorrow, all four powers of the world will clash at Harmony Crater. The following battles will determine the fate of this world.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("I want you and your army to lead the charge. Pave the way to a world of everlasting peace under my rule.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Blair’s eyes redden."),
				S().SetMessage("It will be my honor.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Good. Before I let you go, my quartermaster says the number of crystals don’t add up from the chests you retrieved.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("You don’t happen to know anything about that, do you?")
					.SetSpeaker("King Rouen"),
				S().SetMessage("… It must have been Corbitans, sir.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("It must’ve been. Get out and prepare for the assault.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Your majesty.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				// Scene Transition
				S().AddLeaver("King Rouen"),
				S().AddLeaver("Blair"),
				S().SetBackground("TempBackground"),
				// Pre-battle 6
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().SetMessage("The first wave to hit the foot of the wall isn’t meant to survive the siege. You know the King is sending us to our deaths, right?")
					.SetSpeaker("Juniper").SetExpression("Frown"),
				S().SetMessage("… Bruno. What happens once you kill your belief? Are you grown up then?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("No. You must replace it with new ones. Then, you are grown up.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Blair thinks for a moment, then turns to the army."),
				S().SetMessage("You haven’t known me for long, but have I ever failed you? I haven’t lost a single battle.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Beyond that Velgarian fortifications lie the cure. Your ailing families back home are waiting for you, YOU to bring them back.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I won’t fail you now. I won’t fail Xingata. Follow me to the top of the Crater, and I will give you the cure.")
					.SetSpeaker("Blair").SetExpression("Neutral")
			};
		}
	}

	public class PostBattle6 : Cutscene
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
				// Post-battle 6
				S().SetMessage("Blair, Juniper and Bruno stand atop the rim of the crater, bloodied and exhausted. Battle is still fierce along the rim of the Crater."),
				S().SetMessage("Across the vast inner crater lie thousands of shattered pieces of charred meteorites of various sizes among Velgarian tents."),
				S().SetMessage("‘Pieces of Heaven’ that are embedded into the meteorites glitter with light.")
					
			};
		}
	}

	public class PreBattle7 : Cutscene
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
				// Pre-battle 7
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.Left, "JuniperActor", "Juniper"),
				S().AddActor(CutsceneSide.Right, "RouenActor", "King Rouen"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().SetMessage("I can’t believe we are still alive.")
					.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Don’t celebrate yet. This battle is far from over.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("You are alive! Well done, officers. Thanks to you, we now hold the Crater with minimal casualties.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Anything for you, your majesty.")
					.SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("Corbita and Velgari are retreating. Corbitan cavalries were of no use attacking up the slope, and the Velgarians")
					.SetSpeaker("King Rouen"),
				S().SetMessage("that sent Corbitans home saw their lines breaking on the other sides. Ralley your soldiers and join the rank. We now face the full might of Tsubin.")
					.SetSpeaker("King Rouen")
			};
		}
	}

	public class PostBattle7 : Cutscene
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
				// Scene 7
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().SetMessage("Inside Blair’s tent, Blair sits alone on the bed in pitch black darkness. In nocturnal silence, Blair’s mind alone is at war."),
				S().SetMessage("Blair doesn’t hear the approaching footsteps. A shadow enters Blair’s tent."),
				S().SetMessage("Blair tries to say something in protest, but a radiant dagger made out of pure crystal is plunged through Blair’s abdomen."),
				S().SetMessage("The assassin pulls out the dagger for a second strike, but Juniper and Bruno barge in. Bruno lunges to grapple the assassin,"),
				S().SetMessage("but the assassin escapes after a brief struggle and runs out the tent. Bruno pursues. Juniper steps outside and screams for a healer."),
				S().AddActor(CutsceneSide.Left, "SoldierActor", "Healer"),
				S().AddActor(CutsceneSide.FarRight, "JuniperActor", "Juniper"),
				S().SetMessage("I...I can’t.")
				  .SetSpeaker("Healer"),
				S().SetMessage("What do you mean you can’t?!")
				  .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("My magic isn’t working on the commander’s wound. I can’t heal it!")
				  .SetSpeaker("Healer"),
				S().SetMessage("Blair’ wound is bleeding with no sign of slowing down.Their face is trembling in terror. Juniper pulls out a ‘Piece of Heaven’ from under her tunic."),
				S().SetMessage("Do you know how to use it?")
				.SetSpeaker("Juniper").SetExpression("Neutral"),
				S().SetMessage("Where did you...?")
				  .SetSpeaker("Healer"),
				S().SetMessage("Do you or not?!")
				  .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("I, I do, but I don’t know if it will work on…")
				  .SetSpeaker("Healer"),
				S().SetMessage("Just do it!")
				  .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("The light of the crystal slowly dims as the healer murmurs her incantations."),
				S().SetMessage("Bleeding slows to a stop, then the wound starts to seal up.Blair’s quivering iris finds its focus again."),
				S().SetMessage("I thought I was losing you.")
				  .SetSpeaker("Juniper").SetExpression("Smile"),
				S().SetMessage("Thank you. And I’m sorry. That crystal was meant for your sister.")
				  .SetSpeaker("Blair").SetExpression("Smile"),
				S().SetMessage("She isn’t dying in front my eyes.")
				  .SetSpeaker("Juniper").SetExpression("Neutral"),
				S().AddLeaver("Healer"),
				S().AddActor(CutsceneSide.Right, "BrunoActor", "Bruno"),
				S().SetMessage("Bruno comes back into the tent, angry and frustrated."),
				S().SetMessage("Thank the gods you are alive, Blair. I lost him. He must have help within our army, just like the ones that attacked Juniper and me.")
				   .SetSpeaker("Bruno").SetExpression("Angry"),
				S().SetMessage("Or he’s part of our army.")
				   .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Bruno, with his eyes closed, silently nods."),
				S().SetMessage("What are you saying?")
				   .SetSpeaker("Blair").SetExpression("Frown"),
				S().SetMessage("The assassins had to be sent by the King. Who else has crystalline daggers?")
				   .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("We also knowhe’s been hoarding the crystals while his subjects died covered in their own puss.")
				   .SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("One of our soldiers told me that royal wizards are performing a ritual at the center of the Crater as we speak.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("The King must be turning the crystals into weapons once again. This time, all of them at once.")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("We must challenge the King. That is the only way to save Xingata.")
				   .SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("Blair. He is not the hero of your childhood. Be the leader Xingata needs you to be.")
				   .SetSpeaker("Juniper").SetExpression("Neutral")

			};
		}
	}

	public class PreBattle8 : Cutscene
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
				// Scene 8
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "RouenActor", "King Rouen"),
				S().SetMessage("At dawn, thousands of Xingatan soldiers stand at Harmony Crater."),
				S().SetMessage("All of ‘Pieces of Heaven’ have disappeared from scattered meteorites."),
				S().SetMessage("In the center of them all, King Rouen stands with a heavenly sword in his hand."),
				S().SetMessage("Made of pure crystals, the sword emanates a blinding light that pulses like a heartbeat."),
				S().SetMessage("Last night, a hero of Royal Retinue, Commander Blair has been killed by assassins sent from our enemies.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Cowards! They show in their despicable act that they are not rulers chosen by the gods. But I am.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Yesterday’s victory is the gods’ proclamation that it is not my will, but my fate to rule.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("All the world's ‘Pieces of Heaven’ have been condensed into this sword. It is the only salvation from the plague.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("And now that the gods have gifted it to me, I am humanity’s only salvation. Through me and only me, this world will heal and be united.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Rouen stops speaking when he notices Blair, Juniper and Bruno marching down the crater toward him."),
				S().SetMessage("Behind them, the army that followed them from the western rivers marches with them fully armed."),
				S().SetMessage("The army stops some distance away, but Blair, Juniper and Bruno come face to face with Rouen."),
				S().SetMessage("I don’t know what you are trying to accomplish here, Blair. I’ve won.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("You doom our people.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("The power to cure the plague is right here in my hand.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("Meaning you will decide who lives and who dies.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("The past kings and queens you admire so much are eyeing me with envy from the heavens! " +
					"This was their dream. A utopia! A world of peace and order under one absolute ruler!")
					.SetSpeaker("King Rouen"),
				S().SetMessage("I can’t let you have that power.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("I command the entire Xingatan army. What makes you think you can stop me?")
					.SetSpeaker("King Rouen"),
				S().SetMessage("A kingdom is built on beliefs. When that beliefs are betrayed, the kingdom falls.")
					.SetSpeaker("Blair").SetExpression("Angry"),
				S().SetMessage("Blair’s army marches to surround the King. Royal Retinue marches in response. Royal Guards urge the King to fall back."),
				S().SetMessage("However, no other army on the Crater makes a move to stop Blair’s apparent act of revolt."),
				S().SetMessage("I sent a letter to every lord last night. They have lost their loved ones while you sat on your cache of ‘Pieces of Heaven’.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("They won’t fight for me, but they won’t fight for you either.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("This. This is why I should’ve killed you. This time, I’ll kill you with my own hands.")
					.SetSpeaker("King Rouen")
			};
		}
	}

	public class PostBattle8 : Cutscene
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
				// Scene 9
				S().AddActor(CutsceneSide.FarLeft, "BlairActor", "Blair"),
				S().AddActor(CutsceneSide.FarRight, "RouenActor", "King Rouen"),
				S().SetMessage("King Rouen is defeated. He lies on the ground facing Blair who stands before the King with the crystalline sword in hand."),
				S().SetMessage("I took you for a naive kid chasing fairy tales.")
					.SetSpeaker("King Rouen"),
				S().SetMessage("You’ve won the war. You held all the ‘Pieces of Heaven’ your kingdom needed. Why choose this path when you could’ve saved your kingdom?")
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
				S().SetMessage("Rouen grabs the sword’s blade and buries it in his chest."),
				S().SetMessage("The Monarch is dead. Long live…")
					.SetSpeaker("Rouen"),
				S().SetMessage("Rouen, instead of finishing the phrase, grins at Blair. Blair’s tears drop on Rouen as his last breath escapes him."),
				S().AddLeaver("King Rouen"),
				S().SetMessage("Blair takes out the sword and picks up the crown from Rouen’s still head."),
				S().SetMessage("the crown on one hand and the crystalline sword on the other."),
				S().AddActor(CutsceneSide.Right, "JuniperActor", "Juniper"),
				S().AddActor(CutsceneSide.FarRight, "BrunoActor", "Bruno"),
				S().SetMessage("And he dies without naming his successor. Even at his last moment, he thinks nothing of his kingdom.")
					.SetSpeaker("Juniper").SetExpression("Angry"),
				S().SetMessage("Blair. If we don’t take our next step very carefully...")
					.SetSpeaker("Bruno").SetExpression("Neutral"),
				S().SetMessage("To everyone’s shock, Blair puts on the crown."),
				S().SetMessage("I won’t promise you a world without a war. I don’t claim to be humanity’s only salvation.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I claim the crown simply by my own merit. I proved my military leadership.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I proved my courage to stand up to the unjust, even if he is the most powerful man in Xingata. No, the world.")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("I will prove my righteousness by using this sword only to root out the plague from every town of Xingata. So, I ask you. Will you believe in me to lead you?")
					.SetSpeaker("Blair").SetExpression("Neutral"),
				S().SetMessage("Bruno and Juniper are first to kneel.Blair’s army follows suit. Without any expression of agreement, thousands of soldiers standing in Harmony Crater kneel in waves."),
				S().SetMessage("Xingatan lords look to their soldiers, their people, and one-by - one kneel before Blair."),
				S().SetMessage("Blair looks to the dead eyes of Rouen, and declares:"),
				S().SetMessage("Long live the Monarch.")
					.SetSpeaker("Blair").SetExpression("Neutral")
			};
		}
	}

}