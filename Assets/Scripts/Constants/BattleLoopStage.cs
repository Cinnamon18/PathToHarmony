using System;

namespace Constants {
	public enum BattleLoopStage {
		Initial = 0,
		Pick,
		BattleLoopStart,
		TurnChange,
		TurnChangeEnd,
		UnitSelection,
		ActionSelection,
		EndTurn
	}

	public static class BattleLoopExtensions {

		private static int numMembers = Enum.GetNames(typeof(BattleLoopStage)).Length;

		public static BattleLoopStage NextPhase(this BattleLoopStage stage) {
			//We don't go back into pick mid-battle
			if (stage == BattleLoopStage.EndTurn) {
				return BattleLoopStage.BattleLoopStart;
			}
			
			return (BattleLoopStage)(((int)(stage) + 1));
		}
	}
}