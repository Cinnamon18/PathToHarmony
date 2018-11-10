using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace AI {
	public class BlairTutorialAgent : Agent {

		public BlairTutorialAgent() : base() { }

		private Queue<Move> moves = new Queue<Move>(new[] {
			new Move(2, 3, 2, 3),
			new Move(1, 4, 1, 5),
			new Move(1, 3, 2, 4),

			new Move(2, 4, 2, 4),
			new Move(1, 5, 3, 4),
			new Move(2, 3, 3, 3),
			new Move(3, 3, 3, 4),

			new Move(3, 3, 3, 4),
			new Move(1, 5, 3, 4),
			new Move(2, 4, 2, 4),
		});

		public override async Task<Move> getMove() {
			if (moves.Count > 0) {
				await Task.Delay(1000);
				return moves.Dequeue();
			} else {
				EliminationAgent backupAgent = new EliminationAgent();
				backupAgent.battlefield = this.battlefield;
				backupAgent.character = this.character;
				return await backupAgent.getMove(); 
			}
		}
	}
}