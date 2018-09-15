namespace Gameplay {
	public class EliminationObjective : GameObjective {

		public EliminationObjective(Battlefield battlefield, Level level, Character playerCharacter) : base(battlefield, level, playerCharacter) { }

		public override bool isLoseCondition() {
			if (battlefield.charactersUnits[playerCharacter].Count == 0) {
				return true;
			}
			return false;
		}

		public override bool isWinCondition() {
			foreach (Character character in battlefield.charactersUnits.Keys) {
				if (character != playerCharacter) {
					if (battlefield.charactersUnits[character].Count != 0) {
						return false;
					}
				}
			}
			return true;
		}
	}
}