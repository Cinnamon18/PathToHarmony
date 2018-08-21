using System;
using Gameplay;

namespace AI {
	//A move for a unit, only contains destination information
	public class AIUnitMove : UnitMove, IComparable<AIUnitMove> {

		public int movePointsLeft;

		public AIUnitMove(int x, int y, int movePointsLeft) : base(x, y) {
			this.movePointsLeft = movePointsLeft;
		}

		public int CompareTo(AIUnitMove otherMove) {
			return this.movePointsLeft - otherMove.movePointsLeft;
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType())
				return false;

			AIUnitMove p = (AIUnitMove)obj;
			return (x == p.x) && (y == p.y) && movePointsLeft == p.movePointsLeft;
		}

		public override int GetHashCode() {
			return x * 743 + y + 541 * movePointsLeft;
		}
	}
}