using System;
using Gameplay;

namespace AI {
	//A move for a unit, only contains destination information but has a weight
	public class AIMove : Coord, IComparable<AIMove> {

		public int weight;

		public AIMove(int x, int y, int movePointsLeft) : base(x, y) {
			this.weight = movePointsLeft;
		}

		public int CompareTo(AIMove otherMove) {
			return this.weight - otherMove.weight;
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType())
				return false;

			AIMove p = (AIMove)obj;
			return (x == p.x) && (y == p.y) && weight == p.weight;
		}

		public override int GetHashCode() {
			return x * 743 + y + 541 * weight;
		}
	}
}