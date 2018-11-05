using System;
using Gameplay;

namespace AI {
	//A move for a unit, only contains destination information but has a weight
	public class AIMove : Coord, IComparable<AIMove> {

		public float weight;

		public AIMove(int x, int y, float weight) : base(x, y) {
			this.weight = weight;
		}

		public int CompareTo(AIMove otherMove) {
			if (this.weight > otherMove.weight) {
				return 1;
			} else if (this.weight == otherMove.weight) {
				return 0;
			} else {
				return -1;
			}
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType())
				return false;

			AIMove p = (AIMove)obj;
			return (x == p.x) && (y == p.y) && weight == p.weight;
		}

		public override int GetHashCode() {
			return (int) (x * 743 + y + 541 * weight);
		}
	}
}