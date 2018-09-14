using Constants;

namespace Buffs {
	public abstract class Buff {

		public readonly BuffType buffType;

		public Buff(BuffType buffType) {
			this.buffType = buffType;
		}

	}
}