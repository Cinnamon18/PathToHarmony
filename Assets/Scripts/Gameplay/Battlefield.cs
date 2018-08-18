
using System.Collections.Generic;
using Units;

namespace Gameplay {
	public class Battlefield {
		/*
		Just a little explination of my architecture (which you're welcome to challange):
		
		Preformance outside of AI look-ahead predictions is kinda irrelevant because it'll be almost instant.
		As such, I tried to optimize for the sort of searching people will be doing. I opted for a dense over
		sparse array for the arbitrary index access times (1 vs n), because you do a lot of those in pathfinding/AI,
		and that could be a real bottleneck (I shot	myself in the foot on a different project by doing it sparsely).
		
		I have the dictionary because unit has a pretty self-contained implementation right now (not even any coordinates -
		you have to pass those in if you're getting valid moves) and I feel like a external/upstream/w.e. player
		refrence would spoil that. Also, it allows for much quicker enumeration of a player's units than searching the
		board one by one. The downside is unit addition/deletion will be slower, but they should happen with similar
		frequency in the AI routine and I'll take n^2 -> n over n -> 1.

		In terms of code complexity, maintaining the seperate player unit ownership lists shouldn't hurt too much
		because there's no direct information duplication (just refrences)

		Oh also, units is 2d because units shouldn't stack on one another, and maps shouldn't have cliffs.
		thus, the unit will always be in the highest slot of the stack.
		 */
		public Stack<Tile>[,] map { get; set; }
		public Unit[,] units { get; set; }
		public Dictionary<Character, List<Unit>> playersUnits { get; set; }

		//Pretty much just for if you intend to immediately overwrite its refrences
		public Battlefield() { }

		public Battlefield(int x, int y) {
			map = new Stack<Tile>[x, y];
			for (int i = 0; i < x; i++) {
				for (int ii = 0; ii < y; ii++) {
					map[i,ii] = new Stack<Tile>();
				}
			}
			
			units = new Unit[x, y];
			playersUnits = new Dictionary<Character, List<Unit>>();
		}

		public void addUnit(Unit unit, Character character, int x, int y) {
			units[x, y] = unit;
			List<Unit> unitList = playersUnits[character];
			if (unitList == null) {
				unitList = new List<Unit>(new[] { unit });
			} else {
				unitList.Add(unit);
			}
		}

	}
}