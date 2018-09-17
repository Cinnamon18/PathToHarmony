namespace Gameplay {
	public abstract class GameObjective {

		protected Battlefield battlefield;
		protected Level level;
		protected Character playerCharacter;

		public GameObjective(Battlefield battlefield, Level level, Character playerCharacter) {
			this.battlefield = battlefield;
			this.level = level;
			this.playerCharacter = playerCharacter;
		}

		public abstract bool isWinCondition();
		public abstract bool isLoseCondition();

	}
}