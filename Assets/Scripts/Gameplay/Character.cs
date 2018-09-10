using System.Threading.Tasks;
using AI;

namespace Gameplay {
	public class Character {
		public string name { get; set; }
		public bool isPlayerCharacter;
		public Agent agent;

		public Character() {

		}

		public Character(string name, bool isPlayerCharacter, Agent agent) {
			this.name = name;
			this.isPlayerCharacter = isPlayerCharacter;
			
			agent.character = this;
			this.agent = agent;
		}

		public async Task<Move> getMove() {
			return await agent.getMove();
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType())
				return false;

			Character p = (Character)obj;
			return (p.name == this.name && p.isPlayerCharacter == this.isPlayerCharacter);
		}

		//Definitely could make this more resistant. but uh. 
		public override int GetHashCode() {
			return this.name.GetHashCode() + this.isPlayerCharacter.GetHashCode();
		}
	}
}