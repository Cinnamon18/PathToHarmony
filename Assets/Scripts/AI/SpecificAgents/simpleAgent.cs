using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI
{
    public class simpleAgent : Agent {

        public simpleAgent(Battlefield battlefield, Level level, Action<UnityEngine.Object> Destroy) : base(battlefield, level, Destroy) { }

        public override async Task<Move> getMove() {
            Unit unit = selectUnit();
            Coord unitCoord = battlefield.getUnitCoords(unit);

            Coord targetCoord = findNearestEnemy(unitCoord);

            int minDist = Int32.MaxValue;
            Coord bestCoord = unitCoord;
			foreach (Coord coord in unit.getValidMoves(unitCoord.x, unitCoord.y, battlefield)) {
                int dist = Math.Abs(targetCoord.x - coord.x) + Math.Abs(targetCoord.y - coord.y);
                IBattlefieldItem item = battlefield.battlefieldItemAt(coord.x, coord.y, 0);
				if (!(item is Unit && battlefield.charactersUnits[character].Contains(item as Unit)) && dist < minDist) {
                    minDist = dist;
                    bestCoord = coord;
                }
            }

            return new Move(unitCoord.x,unitCoord.y,bestCoord.x,bestCoord.y);
        }

		private Unit selectUnit() {

            List<Unit> agentsUnits = battlefield.charactersUnits[character];
            Unit unit = null;

			foreach (Unit u in agentsUnits) {
				if (!u.hasMovedThisTurn) {
                    unit = u;
                    break;
                }
            }
            return unit;
        }

		private Coord findNearestEnemy(Coord start) {

            int[,] moveDirs = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
            HashSet<Coord> visited = new HashSet<Coord>();
            Queue<Coord> moveQueue = new Queue<Coord>();
            moveQueue.Enqueue(start);
            visited.Add(start);

			while (moveQueue.Count() > 0) {
                Coord curCoord = moveQueue.Dequeue();
                IBattlefieldItem item = battlefield.battlefieldItemAt(curCoord.x, curCoord.y, 0);
                if (item is Unit) {
					Unit unit = item as Unit;
					if (!battlefield.charactersUnits[character].Contains(unit)) {
						return curCoord;
                    }
                }

                for (int x = 0; x < moveDirs.GetLength(0); x++) {
                    Coord nextCoord = new Coord(curCoord.x + moveDirs[x, 0], curCoord.y + moveDirs[x, 1]);
					if (nextCoord.x >= 0 && nextCoord.y >= 0 && nextCoord.x < battlefield.map.GetLength(0) && nextCoord.y < battlefield.map.GetLength(1)) {
                        if (!visited.Contains(nextCoord)) {
                            visited.Add(nextCoord);
                            moveQueue.Enqueue(nextCoord);
                        }
                    }
                }
            }
            return start;
        }
    }
}
