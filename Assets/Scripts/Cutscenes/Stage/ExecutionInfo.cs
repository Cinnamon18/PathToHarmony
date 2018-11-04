using Constants;
using Gameplay;

namespace Cutscenes.Stages {
	public class ExecutionInfo {
		public Battlefield battlefield;
		public GameObjective objective;
		public int halfTurnsElapsed;
		public BattleLoopStage battleStage;

		public ExecutionInfo(Battlefield battlefield, GameObjective objective, int halfTurnsElapsed, BattleLoopStage battleStage) {
			this.battlefield = battlefield;
			this.objective = objective;
			this.halfTurnsElapsed = halfTurnsElapsed;
			this.battleStage = battleStage;
		}
	}
}