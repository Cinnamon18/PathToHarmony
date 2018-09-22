using System.Collections.Generic;
using Units;

namespace Gameplay {
	public class DefendObjective : GameObjective {

		public List<Coord> capturePoints = new List<Coord>();
		public int timeToHold = 0;
		private int timeHeld = 0;
		private int lastHalfTurnsElapsed = 0;
		private bool holding = false;

		public DefendObjective(Battlefield battlefield, Level level, Character playerCharacter, int maxHalfTurns) :
			base(battlefield, level, playerCharacter, maxHalfTurns) { }

		public DefendObjective(Battlefield battlefield, Level level, Character playerCharacter, int maxHalfTurns, List<Coord> capturePoints, int timeToHold) :
			base(battlefield, level, playerCharacter, maxHalfTurns) {
				this.capturePoints = capturePoints;
				this.timeToHold = timeToHold;
			}

		public override bool isLoseCondition(int halfTurnsElapsed) {
			if (battlefield.charactersUnits[playerCharacter].Count == 0) {
				return true;
			}
			foreach (Coord coord in this.capturePoints) {
				Unit unit = battlefield.units[coord.x, coord.y];
				if (unit == null || unit.getCharacter(battlefield) == playerCharacter) {
					timeHeld = 0;
					return false;
				}
			}

			//I know this looks clunky but it's the best I could think of so the timer doesn't tick when you first step on the goal, but does every consecutive turn
			if (!this.holding) {
				this.holding = true;
				lastHalfTurnsElapsed = halfTurnsElapsed;
			} else if (lastHalfTurnsElapsed < halfTurnsElapsed) {
				timeHeld++;
				lastHalfTurnsElapsed = halfTurnsElapsed;
			}
			return timeHeld >= timeToHold;
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
