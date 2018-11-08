using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using System;
using Gameplay;

namespace Gameplay {
	public class Tile : MonoBehaviour, IBattlefieldItem {
		private Material originalMaterial;

		public TileType initialType;
		public TileType tileType { get; set; }
		public static GameObject[][] tileFlavor;
		public TileEffects tileEffects;
		public Tile() : this(TileType.None) { }

		public Tile(TileType tileType) {
			this.tileType = tileType;
		}

		void Awake() {

			GameObject smallRock = Resources.Load<GameObject>("TileFlavor/" + "SmallRock");
			GameObject smallTree = Resources.Load<GameObject>("TileFlavor/" + "SmallTree");
			GameObject bush = Resources.Load<GameObject>("TileFlavor/" + "Bush");
			GameObject pieceOfHeaven = Resources.Load<GameObject>("TileFlavor/" + "PieceOfHeaven");


			if (Tile.tileFlavor == null) {
				GameObject[][] _tileFlavor = new GameObject[][] {
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {smallRock, smallTree, bush},
					new GameObject[] {smallTree, bush},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {bush},
					new GameObject[] {bush},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {smallRock, pieceOfHeaven}
				};

				Tile.tileFlavor = _tileFlavor;
			}
		}

		public GameObject[] getFlavorPrefabs() {
			return Tile.tileFlavor[(int)(tileType)];
		}

		public void vibrateUnhappily() {
			//TODO (just some programatic animation to make this reusable between different tiles, probably)
		}

		public string serialize() {
			return "" + ((int)(this.tileType));
		}

		public override string ToString() {
			return tileType.ToString();
		}

	}
}