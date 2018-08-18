
namespace Constants {
    public struct WeaponType {
		public DamageType damageType;
		public bool isRanged;
		public int baseDamage;

		public WeaponType(DamageType damageType, bool isRanged, int baseDamage) {
			this.damageType = damageType;
			this.isRanged = isRanged;
			this.baseDamage = baseDamage;
		}

		public static readonly WeaponType KnightWeapon = new WeaponType(DamageType.Slash, false, 50);
	}
}