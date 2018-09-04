namespace Cutscenes {
	public class CutsceneScriptLine {
		public CutsceneAction action;
		public CutsceneBackground background;
		public CutsceneSide side;
		public CutsceneCharacter character;
		public CharacterExpression expression;
		public string dialogue;

		public CutsceneScriptLine(
			CutsceneAction action,
			CutsceneBackground background = CutsceneBackground.None,
			CutsceneSide side = CutsceneSide.Left,
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