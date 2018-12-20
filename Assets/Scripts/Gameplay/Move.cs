
namespace Gameplay {
	//General move from one point to another
	public class Move {
		public Coord from;
		public Coord to;

		public Move() { }

		public Move(Coord from, Coord to) {
			this.from = from;
			this.to = to;
		}

		public Move(int fromX, int fromY, int toX, int toY) {
			this.from = new Coord(fromX, fromY);
			this.to = new Coord(toX, toY);
		}

		public override bool Equals(object other) {
			if (other == null || GetType() != other.GetType())
				return false;

			Move otherMove = (Move)other;
			return this.from.Equals(otherMove.from) && this.to.Equals(otherMove.to);
		}

		public override int GetHashCode() {
			return from.GetHashCode() * 347 + to.GetHashCode();
		}
	}
}