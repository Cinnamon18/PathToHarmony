namespace Gameplay {
	//coordinate on the board, often a vector for the movement of a unit
	public class Coord {
		public int x;
		public int y;

		public Coord(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType())
				return false;

			Coord p = (Coord)obj;
			return (x == p.x) && (y == p.y);
		}

		//I think this should be pretty collision resistant? I feel like a prime would help? idk???
		public override int GetHashCode() {
			return x * 743 + y;
		}
	}
}