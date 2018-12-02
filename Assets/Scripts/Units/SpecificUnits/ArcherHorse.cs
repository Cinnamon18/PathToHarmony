using Constants;

namespace Units {
	public class ArcherHorse : RangedUnit {
		public ArcherHorse() : base(ArmorType.Light, 100, MoveType.Mounted, 4, DamageType.Pierce, 30, 3, Faction.Xingata) {

		}
	}
}
