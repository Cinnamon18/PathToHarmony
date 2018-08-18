
using System;
using Units;

namespace Constants {
	public static class ConstantTables {

		//First dimension is MoveType, second is TileType
		public static int[,] MovementCost = new int[,] {{0,0,0},
														{0,0,0},
														{0,0,0} };

		//First dimension is DamageType, second is ArmorType
		//This ones a little tricky, it's the % reduction to the base damage, so a reduction of
		// 20 means 20%, and final damage would be baseDamage * 0.8
		public static int[,] DamageReduction = new int[,] {{0,0,0},
														   {0,0,0},
														   {0,0,0} };

		//First dimension is TileType. Holds the % defense a Tile piece grants
		public static int[] TileDefense = new int[] { 0, 0, 0, 0 };

		//First dimension is the unit's type. Holds how many tiles the unit can move.
		public static int[] UnitMoveDistance = new int[] { 0, 0, 0, 0 };

	}

	public static class ConstantExtensions {
		public static int Cost(this MoveType moveType, TileType tileType) {
			return ConstantTables.MovementCost[(int)(moveType), (int)(tileType)];
		}

		public static int Cost(this TileType tileType, MoveType moveType) {
			return moveType.Cost(tileType);
		}

		public static int DefenseBonus(this TileType tileType) {
			return ConstantTables.TileDefense[(int)(tileType)];
		}

		public static int DamageReduction(this DamageType damageType, ArmorType armorType) {
			return ConstantTables.DamageReduction[(int)(damageType), (int)(armorType)];
		}

		public static int DamageReduction(this ArmorType armorType, DamageType damageType) {
			return damageType.DamageReduction(armorType);
		}
	}
}