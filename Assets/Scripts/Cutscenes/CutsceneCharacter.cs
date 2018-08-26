using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes {
	public class CutsceneCharacter {
		public static Sprite[] blairExpressions = new Sprite[] {
			Resources.Load<Sprite>("Sprites/TempChar1Neutral")
		};
		public static Sprite[] juniperExpressions = new Sprite[] {
			Resources.Load<Sprite>("Sprites/TempChar2Neutral")
		};
		public static Sprite[] brunoExpressions = new Sprite[] {
			null
		};

		public Sprite[] expressions;
		public Sprite currentExpression;

		public CutsceneCharacter(Sprite[] expressions) {
			this.expressions = expressions;
			currentExpression = this.expressions[0];
		}

		public void setExpression(CutsceneCharacterExpressions expression) {
			currentExpression = expressions[(int)(expression)];
		}

		public static readonly CutsceneCharacter blair = new CutsceneCharacter(blairExpressions);
		public static readonly CutsceneCharacter juniper = new CutsceneCharacter(juniperExpressions);
		public static readonly CutsceneCharacter bruno = new CutsceneCharacter(brunoExpressions);

	}
}