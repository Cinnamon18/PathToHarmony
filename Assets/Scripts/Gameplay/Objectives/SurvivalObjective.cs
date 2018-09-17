namespace Gameplay {
	public class SurvivalObjective : GameObjective {

		public SurvivalObjective(Battlefield battlefield, Level level, Character playerCharacter, int maxHalfTurns) :
			base(battlefield, level, playerCharacter, maxHalfTurns) { }

		public override bool isLoseCondition(int halfTurnsElapsed) {
			if (battlefield.charactersUnits[playerCharacter].Count == 0) {
				return true;
			}
			return false;
		}

		public override bool isWinCondition(int halfTurnsElapsed) {
			if (halfTurnsElapsed >= maxHalfTurns) {
				return true;
			}
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