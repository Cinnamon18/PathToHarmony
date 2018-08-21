using System;
using Gameplay;

namespace AI {
	//A move for a unit, only contains destination information
	public class AIUnitMove : UnitMove, IComparable<AIUnitMove> {

		public int movePoints;

		public AIUnitMove(int x, int y, int movePointsLeft) : base(x, y) {
			this.movePoints = movePointsLeft;
		}

		public int CompareTo(AIUnitMove otherMove) {
			return this.movePoints - otherMove.movePoints;
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType())
				return false;

			AIUnitMove p = (AIUnitMove)obj;
			return (x == p.x) && (y == p.y) && movePoints == p.movePoints;
		}

		public override int GetHashCode() {
			return x * 743 + y + 541 * movePoints;
		}
	}
}