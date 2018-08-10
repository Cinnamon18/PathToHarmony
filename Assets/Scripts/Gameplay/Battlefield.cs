
namespace Gameplay {
	public class Battlefield {

		public IBattlefieldItem[,,] map { get; set; }

		//Pretty much just for if you intend to immediately overwrite it
		public Battlefield() { }

		public Battlefield(int x, int y, int z) {
			map = new IBattlefieldItem[x, y, z];
		}

	}
}