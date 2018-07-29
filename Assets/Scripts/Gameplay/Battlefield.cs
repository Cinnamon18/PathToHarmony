
namespace Gameplay {
	public class Battlefield {

		public IBattlefieldItem[,,] map { get; set; }

		public Battlefield(): this(1, 1, 1) { }

		public Battlefield(int x, int y, int z) {
			map = new IBattlefieldItem[x, y, z];
		}

	}
}