
namespace Gameplay {
	//General move from one point to another
	public struct Move {
		public int fromX;
		public int fromY;
		public int toX;
		public int toY;

		public Move(int fromX, int fromY, int toX, int toY) {
			this.fromX = fromX;
			this.fromY = fromY;
			this.toX = toX;
			this.toY = toY;
		}
	}
}