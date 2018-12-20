using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace AI {
	public class BlairTutorialAgent : Agent {

		private GameObject arrow1 = Resources.Load<GameObject>("TutorialArrow1");
		private GameObject arrow2 = Resources.Load<GameObject>("TutorialArrow2");
		private PlayerAgent playerAgent = new PlayerAgent();

		public BlairTutorialAgent() : base() {

		}

		private Queue<Move> moves = new Queue<Move>(new[] {
			new Move(2, 3, 2, 4),
			new Move(1, 4, 0, 4),
			new Move(1, 3, 1, 5),

			new Move(0, 4, 3, 4),
			new Move(1, 5, 3, 4),
			new Move(2, 4, 3, 4),

			new Move(0, 4, 2, 5),
			new Move(1, 5, 2, 5),
			new Move(2, 4, 2, 5),
		});

		public override async Task<Move> getMove() {
			playerAgent.battlefield = this.battlefield;
			playerAgent.character = this.character;

			Move expectedMove = null;
			if (moves.Count > 0) {
				expectedMove = moves.Dequeue();
			}
			if (expectedMove == null) {
				return await playerAgent.getMove();
			}

			Vector3 offset = new Vector3(0, 14, 0);
			offset += new Vector3(0, Util.GridHeight * battlefield.map[expectedMove.from.x, expectedMove.from.y].Count,0);
			GameObject arrowInstanceFrom = Object.Instantiate(arrow1, Util.GridToWorld(expectedMove.from) + offset, arrow1.transform.rotation);
			GameObject arrowInstanceTo = Object.Instantiate(arrow2, Util.GridToWorld(expectedMove.to) + offset, arrow2.transform.rotation);

			Move playerMove = null;
			do {
				playerMove = await playerAgent.getMove();
				if (!playerMove.Equals(expectedMove)) {
					Audio.playSfx("Error");
				}
			} while (!playerMove.Equals(expectedMove));

			Object.Destroy(arrowInstanceFrom);
			Object.Destroy(arrowInstanceTo);

			return playerMove;
		}
	}
}