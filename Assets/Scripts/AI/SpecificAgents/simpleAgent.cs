using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;
using Units;
using UnityEngine;

namespace AI {
	public class simpleAgent : Agent {

		public simpleAgent(Battlefield battlefield, Level level, Action<UnityEngine.Object> Destroy) : base(battlefield, level, Destroy) { }

		public override async Task<Move> getMove() {
			Unit unit = selectUnit();
			Coord unitCoord = battlefield.getUnitCoords(unit);	

			List<Coord> targetCoords = findNearestEnemies(unitCoord);

			List<Coord> targets = unit.getTargets(unitCoord.x, unitCoord.y, battlefield, character);

			int minDist = Int32.MaxValue;
			Coord bestCoord = null;
			foreach (Coord targetCoord in targetCoords){
				if (targets.Any(t => t.x == targetCoord.x && t.y == targetCoord.y)) {
					return new Move(unitCoord.x, unitCoord.y, targetCoord.x, targetCoord.y);
				}
				
				foreach (Coord coord in unit.getValidMoves(unitCoord.x, unitCoord.y, battlefield)) {
					int dist = this.manhattanDistance(coord, targetCoord);
					if (dist < minDist) {
						minDist = dist;
						bestCoord = coord;
					}
				}
			}
			
			//Just so the player can keep track of what's happening
			await Task.Delay(300);

			return new Move(unitCoord.x, unitCoord.y, bestCoord.x, bestCoord.y);
		}
	}
}
