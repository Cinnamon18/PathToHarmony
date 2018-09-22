namespace Gameplay {
	public abstract class GameObjective {

		protected Battlefield battlefield;
		protected Level level;
		protected Character playerCharacter;
		public int maxHalfTurns;

		public GameObjective(Battlefield battlefield, Level level, Character playerCharacter, int maxHalfTurns) {
			this.battlefield = battlefield;
			this.level = level;
			this.playerCharacter = playerCharacter;
			this.maxHalfTurns = maxHalfTurns;
		}

		public abstract bool isWinCondition(int halfTurnsElapsed);
		public abstract bool isLoseCondition(int halfTurnsElapsed);

	}
}