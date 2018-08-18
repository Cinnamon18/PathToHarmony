namespace Gameplay {
	public class Character {
		public string name { get; set; }

		public Character() {

		}

		public Character(string name) {
			this.name = name;
		}

		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType())
				return false;

			Character p = (Character)obj;
			return (p.name == this.name);
		}

		//Definitely could make this more resistant. but uh. 
		public override int GetHashCode() {
			return this.name.GetHashCode();
		}
	}
}