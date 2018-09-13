using Constants;

namespace Buffs {
	public class DamageBuff : Buff {

		private float bonus = 1.0f;

		public DamageBuff(float bonus) : base(BuffType.Damage) {
			this.bonus = bonus;
		}

		public float getDamageBonus() {
			return bonus;
		}

	}
}