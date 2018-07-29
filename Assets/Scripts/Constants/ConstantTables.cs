
namespace Constants {
	public static class ConstantTables {

		//First dimension is MoveType, second is TerrainType
		public static int[,] MovementCost = new int[,] {{0,0,0},
														{0,0,0},
														{0,0,0} };

		//First dimension is DamageType, second is ArmorType
		//This ones a little tricky, it's the % reduction to the base damage, so a reduction of
		// 20 means 20%, and final damage would be baseDamage * 0.8
		public static int[,] DamageReduction = new int[,] {{0,0,0},
													   {0,0,0},
													   {0,0,0} };

		//First dimension is TerrainType. Holds the % defense a terrain piece grants
		public static int[] TerrainDefense = new int[] { 0, 0, 0, 0 };


	}

	public static class Extensions {
		public static int Cost(this MoveType moveType, TerrainType terrainType) {
			return ConstantTables.MovementCost[(int)(moveType), (int)(terrainType)];
		}

		public static int Cost(this TerrainType terrainType, MoveType moveType) {
			return moveType.Cost(terrainType);
		}

		public static int DefenseBonus(this TerrainType terrainType) {
			return ConstantTables.TerrainDefense[(int)(terrainType)];
		}

		public static int DamageReduction(this DamageType damageType, ArmorType armorType) {
			return ConstantTables.DamageReduction[(int)(damageType), (int)(armorType)];
		}

				public static int DamageReduction(this ArmorType armorType, DamageType damageType) {
			return damageType.DamageReduction(armorType);
		}
	}
}