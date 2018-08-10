namespace Gameplay {
	public class Campaign {
		public string name { get; set; }
		public int levelIndex { get; set; }
		public Level[] levels { get; set; }

		public Campaign(string name = "", int levelIndex = 0, Level[] levels = null) {
			this.name = name;
			this.levelIndex = levelIndex;
			this.levels = levels;
		}

	}

}