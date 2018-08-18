namespace Gameplay {
	//A move for a unit, only contains destination information
	public class UnitMove {
		public int x;
		public int y;

		public UnitMove(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType())
				return false;

			UnitMove p = (UnitMove)obj;
			return (x == p.x) && (y == p.y);
		}

		//I think this should be pretty collision resistant? I feel like a prime would help? idk???
		public override int GetHashCode() {
			return x * 743 + y;
		}
	}
}