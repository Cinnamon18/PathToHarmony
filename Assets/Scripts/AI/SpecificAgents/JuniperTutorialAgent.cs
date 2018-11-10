using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay;

namespace AI {
	public class JuniperTutorialAgent : Agent {

		public JuniperTutorialAgent() : base() { }

		private Queue<Move> moves = new Queue<Move>(new[] {
			new Move(6, 5, 3, 4),
			new Move(3, 4, 2, 4),
			new Move(5, 5, 2, 5),
			new Move(2, 5, 2, 4),


			new Move(3, 4, 2, 4),
			new Move(2, 5, 2, 4),

			new Move(2, 5, 2, 4),
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