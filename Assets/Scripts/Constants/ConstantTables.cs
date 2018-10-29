using System;
using Units;

namespace Constants {
	public static class ConstantTables {

		//First dimension is MoveType, second is TileType. Unnecessary decimmal points are to make collumns line up
		public static double[,] MovementCost = new double[,]
			{ {1,1,1.00,1.5,0.75,100, 0.75, 0.75, 0.75, 0.75, 0.75, 1, 1, 1, 4, 4, 4, 4, 4, 4}, //Unarmored
			  {1,1,1.00,1.5,0.75,100, 0.75, 0.75, 0.75, 0.75, 0.75, 1, 1, 1, 4, 4, 4, 4, 4, 4}, //Light Infantry
			  {1,1,1.00,2.0,0.75,100, 0.75, 0.75, 0.75, 0.75, 0.75, 1, 1, 1, 4, 4, 4, 4, 4, 4}, //Armored Infantry
			  {1,1,0.75,4.0,0.50,100, 0.50, 0.50, 0.50, 0.50, 0.50, 1, 1, 1, 6, 6, 6, 6, 6, 6} }; //Mounted

		//First dimension is DamageType, second is ArmorType
		//This ones a little tricky, it's the % reduction to the base damage, so a reduction of
		// 20 means 20%, and final damage would be baseDamage * 0.8

		public static int[,] DamageReduction = new int[,]
			{ {0,30,50,70}, //No weapon
			  {0,20,30,40}, //Slash
			  {0,10,40,50}, //Stab
			  {0,10,20,30}, //Blunt
			  {200,200,200,200}}; //Heal

		//First dimension is TileType. Holds the % defense a Tile piece grants
		public static int[] TileDefense = new int[]
			{ 0, 0, 10, 20, 0, 0, 0, 0, 0, 0, 0, 10, 10, 10, 40, 40, 40, 30, 30, 30 };

	}

	//If these look real weird to you coming from java they're called exension methods!
	//google 'em, they're handy (but also a bit irrisponsible).
	public static class ConstantExtensions {
		public static float Cost(this MoveType moveType, TileType tileType) {
			return (float) ConstantTables.MovementCost[(int)(moveType), (int)(tileType)];
		}

		public static float Cost(this TileType tileType, MoveType moveType) {
			return moveType.Cost(tileType);
		}

		public static int DamageReduction(this DamageType damageType, ArmorType armorType) {
			return ConstantTables.DamageReduction[(int)(damageType), (int)(armorType)];
		}

		public static int DamageReduction(this ArmorType armorType, DamageType damageType) {
			return damageType.DamageReduction(armorType);
		}

		public static int DefenseBonus(this TileType tileType) {
			return ConstantTables.TileDefense[(int)(tileType)];
		}


	}
}