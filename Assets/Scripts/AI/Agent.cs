using System;
using System.Threading.Tasks;
using Gameplay;

namespace AI {
	public abstract class Agent {
		protected Battlefield battlefield;
		public Level level;
		public Character character;
		public Action<UnityEngine.Object> Destroy;
		
		public Agent(Battlefield battlefield, Level level, Action<UnityEngine.Object> Destroy) {
			this.battlefield = battlefield;
			this.level = level;
			this.Destroy = Destroy;
		}

		//Returns the agent's move, based on the state of the battlefield
		public abstract Task<Move> getMove();
	}
}