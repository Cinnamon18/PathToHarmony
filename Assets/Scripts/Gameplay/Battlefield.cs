
namespace Gameplay {
	public class Battlefield {

		public IBattlefieldItem[,,] map { get; set; }

		public Battlefield(int x, int y, int z) {
			map = new IBattlefieldItem[x, y, z];
		}

	}
}