using Constants;
using Gameplay;

namespace Cutscenes.Stages {
	public class ExecutionInfo {
		public Battlefield battlefield;
		public GameObjective objective;
		public int halfTurnsElapsed;
		public BattleLoopStage battleStage;
		public bool afterVictoryImage;

		public ExecutionInfo(Battlefield battlefield, GameObjective objective, int halfTurnsElapsed, BattleLoopStage battleStage, bool afterVictoryImage) {
			this.battlefield = battlefield;
			this.objective = objective;
			this.halfTurnsElapsed = halfTurnsElapsed;
			this.battleStage = battleStage;
			this.afterVictoryImage = afterVictoryImage;
		}
	}
}