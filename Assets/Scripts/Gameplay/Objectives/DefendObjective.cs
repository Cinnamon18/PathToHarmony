using System.Collections.Generic;
using Units;

namespace Gameplay {
	public class DefendObjective : GameObjective {

		public List<Coord> capturePoints = new List<Coord>();
		public int timeToHold = 0;
		private int[] timeHeld;
		private int[] lastHalfTurnsElapsed;
		private bool[] holding;

		public DefendObjective(Battlefield battlefield, Level level, Character playerCharacter, int maxHalfTurns) :
			base(battlefield, level, playerCharacter, maxHalfTurns) { }

		public DefendObjective(Battlefield battlefield, Level level, Character playerCharacter, int maxHalfTurns, List<Coord> capturePoints, int timeToHold) :
			base(battlefield, level, playerCharacter, maxHalfTurns) {
			this.capturePoints = capturePoints;
			this.timeToHold = timeToHold;
			this.timeHeld = new int[capturePoints.Count];
			this.lastHalfTurnsElapsed = new int[capturePoints.Count];
			this.holding = new bool[capturePoints.Count];

			// foreach (Coord coord in capturePoints) {
			// 	battlefield.map[coord.x, coord.y].Peek().gameObject.AddComponent<cakeslice.Outline>().color = 3;
			// }
		}

		public override string getName() {
			return "Defend";
		}

		public override bool isLoseCondition(int halfTurnsElapsed) {
			if (battlefield.charactersUnits[playerCharacter].Count == 0) {
				return true;
			}
			for (int i = 0; i < capturePoints.Count; i++) {
				Coord coord = capturePoints[i];
				Unit unit = battlefield.units[coord.x, coord.y];
				if (unit != null && unit.getCharacter(battlefield) != playerCharacter) {
					if (!holding[i]) {
						holding[i] = true;
						lastHalfTurnsElapsed[i] = halfTurnsElapsed;
					} else if (lastHalfTurnsElapsed[i] < halfTurnsElapsed) {
						timeHeld[i]++;
						lastHalfTurnsElapsed[i] = halfTurnsElapsed;
					}
					return timeHeld[i] >= timeToHold;
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
