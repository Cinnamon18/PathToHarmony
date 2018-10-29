
using System.Collections.Generic;
using Constants;
using Units;
using UnityEngine;

namespace Gameplay {
	public class Battlefield {
		/*
		Just a little architecture explanation  (which you're welcome to challange):
		
		Preformance outside of AI look-ahead predictions is kinda irrelevant because calls will be almost instant.
		As such, I tried to optimize for the sort of searching people will be doing. I opted for a dense over
		sparse array for the arbitrary index access times (1 vs n), because you do a lot of those in pathfinding/AI,
		and that could be a real bottleneck (I shot	myself in the foot on a different project by doing it sparsely).
		
		I have the dictionary because unit has a pretty self-contained implementation right now (not even any coordinates -
		you have to pass those in if you're getting valid moves) and I feel like a external/upstream/w.e. player
		refrence would spoil that. Also, it allows for much quicker enumeration of a player's units than searching the
		board one by one. The downside is unit addition/deletion will be slower, but they should happen with similar
		frequency to unit listing in the AI routine and I'll take n^2 -> n over n -> 1.

		In terms of code complexity, maintaining the seperate player unit ownership lists shouldn't hurt too much
		because there's no direct information duplication (just refrences)

		Oh also, units is 2d because units shouldn't stack on one another, and maps shouldn't have cliffs.
		thus, the unit will always be in the highest slot of the stack.
		 */
		public Stack<Tile>[,] map { get; set; }
		public Unit[,] units { get; set; }
		public Dictionary<Character, List<Unit>> charactersUnits { get; set; }

		//Pretty much just for if you intend to immediately overwrite its refrences
		public Battlefield() {
			charactersUnits = new Dictionary<Character, List<Unit>>();
		}

		public Battlefield(int x, int y) {
			map = new Stack<Tile>[x, y];
			for (int i = 0; i < x; i++) {
				for (int ii = 0; ii < y; ii++) {
					map[i, ii] = new Stack<Tile>();
				}
			}

			units = new Unit[x, y];
			charactersUnits = new Dictionary<Character, List<Unit>>();
		}

		public IBattlefieldItem battlefieldItemAt(int x, int y, int z) {
			if (x < 0 || y < 0 || z < 0) {
				return null;
			}

			if (x < units.GetLength(0) && y < units.GetLength(1) && units[x, y] != null) {
				return units[x, y];
			} else if (x < map.GetLength(0) && y < map.GetLength(1) && map[x, y].Peek() != null) {
				//TODO: Make this function as expected. use the z index instead of returning the top one. Find out how to do that efficently (o(1) would be great).
				return map[x, y].Peek();
			}

			return null;
		}

		public IBattlefieldItem battlefieldItemAt(int x, int y) {
			if (x < 0 || y < 0) {
				return null;
			}

			if (x < units.GetLength(0) && y < units.GetLength(1) && units[x, y] != null) {
				return units[x, y];
			} else if (x < map.GetLength(0) && y < map.GetLength(1) && map[x, y].Peek() != null) {
				if (map[x,y].Count != 0)
				{
					return map[x, y].Peek();
				}
				
			}

			return null;
		}

		public Coord getUnitCoords(Unit unit) {
			for (int x = 0; x < units.GetLength(0); x++) {
				for (int y = 0; y < units.GetLength(1); y++) {
					if (units[x, y] == unit) {
						return new Coord(x, y);
					}
				}
			}
			return null;
		}

		public void addUnit(Unit unit, Character character, int x, int y) {
			units[x, y] = unit;

			if (!charactersUnits.ContainsKey(character)) {
				charactersUnits[character] = new List<Unit>();
			}

			List<Unit> unitList = charactersUnits[character];
			unitList.Add(unit);
		}

	}
}