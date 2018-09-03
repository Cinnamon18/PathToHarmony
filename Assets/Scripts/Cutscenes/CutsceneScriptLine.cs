namespace Cutscenes {
	public class CutsceneScriptLine {
		public CutsceneAction action;
		public CutsceneBackground background;
		public Side side;
		public CutsceneCharacter character;
		public CharacterExpression expression;
		public string dialogue;

		public CutsceneScriptLine(
			CutsceneAction action,
			CutsceneBackground background = CutsceneBackground.None,
			Side side = Side.Left,
			CutsceneCharacter character = null,
			CharacterExpression expression = CharacterExpression.Default,
			string dialogue = "") {

			this.action = action;
			this.background = background;
			this.side = side;
			this.character = character;
			this.expression = expression;
			this.dialogue = dialogue;
		}
	}
}