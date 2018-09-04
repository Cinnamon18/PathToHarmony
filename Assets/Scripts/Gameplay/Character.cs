namespace Gameplay {
	public class Character {
		public string name { get; set; }
		public bool isPlayerCharacter;

		public Character() {

		}

		public Character(string name, bool isPlayerCharacter) {
			this.name = name;
			this.isPlayerCharacter = isPlayerCharacter;
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