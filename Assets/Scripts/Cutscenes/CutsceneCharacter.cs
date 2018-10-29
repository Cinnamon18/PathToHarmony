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

		public string name;
		public Sprite[] expressions;
		public Sprite currentExpression;

		public CutsceneCharacter(string name, Sprite[] expressions) {
			this.name = name;
			this.expressions = expressions;
			currentExpression = this.expressions[0];
		}

		public void setExpression(Expression expression) {
			currentExpression = expressions[(int)(expression)];
		}

		public static readonly CutsceneCharacter blair = new CutsceneCharacter("Blair", blairExpressions);
		public static readonly CutsceneCharacter juniper = new CutsceneCharacter("Juniper", juniperExpressions);
		public static readonly CutsceneCharacter bruno = new CutsceneCharacter("Bruno", brunoExpressions);

	}
}