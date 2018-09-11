namespace Buffs {
	public class DamageBuff : Buff{

		float bonus = 1.0f;

		public DamageBuff(float bonus) {
			this.bonus = bonus;
		}

		public float getDamageBonus() {
			return bonus;
		}

	}
}