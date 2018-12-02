using Constants;

namespace Units {
	public class HeavyHorse : MeleeUnit {
		public HeavyHorse() : base(ArmorType.Heavy, 100, MoveType.Mounted, 3, DamageType.Pierce, 40, Faction.Xingata) {

		}
	}
}