using System.Collections.Generic;
using Units;

namespace Gameplay {
	public class EscortObjective : GameObjective {

		public List<Unit> vips = new List<Unit>();
		private int lastHalfTurnsElapsed = 0;

		public EscortObjective(Battlefield battlefield, Level level, Character playerCharacter, int maxHalfTurns) :
			base(battlefield, level, playerCharacter, maxHalfTurns) { }

		public EscortObjective(Battlefield battlefield, Level level, Character playerCharacter, int maxHalfTurns, List<Unit> vips) :
			base(battlefield, level, playerCharacter, maxHalfTurns) {
				this.vips = vips;
			 }

		public override bool isLoseCondition(int halfTurnsElapsed) {
			if (battlefield.charactersUnits[playerCharacter].Count == 0) {
				return true;
			}
			if (halfTurnsElapsed >= maxHalfTurns) {
				return true;
			}
			return false;
		}

		public override bool isWinCondition(int halfTurnsElapsed) {
			foreach (Unit unit in this.vips) {
				foreach (Character c in battlefield.charactersUnits.Keys) {
					if (battlefield.charactersUnits[c].Contains(unit)) {
						return false;
					}
				}
			}
			return true;
		}
	}
}
