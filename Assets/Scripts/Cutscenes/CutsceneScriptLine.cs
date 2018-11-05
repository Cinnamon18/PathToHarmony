namespace Cutscenes {
	public class CutsceneScriptLine {
		public CutsceneAction action;
		public CutsceneBackground background;
		public CutsceneSide side;
		public CutsceneCharacter character;
		public Expression expression;
		public string dialogue;

		public CutsceneScriptLine(
			CutsceneAction action,
			CutsceneBackground background = CutsceneBackground.None,
			CutsceneSide side = CutsceneSide.Left,
			CutsceneCharacter character = null,
			Expression expression = Expression.Neutral,
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