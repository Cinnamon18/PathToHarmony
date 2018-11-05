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

		public override string getName() {
			return "Escort";
		}

		public override bool isLoseCondition(int halfTurnsElapsed) {
			if (battlefield.charactersUnits[playerCharacter].Count == 0) {
				return true;
			}
			foreach (Unit unit in this.vips) {
				if (!battlefield.charactersUnits[playerCharacter].Contains(unit)) {
					return true;
				}
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
