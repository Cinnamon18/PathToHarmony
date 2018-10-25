using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using System;
using Gameplay;

namespace Gameplay {
	public class Tile : MonoBehaviour, IBattlefieldItem {
		private Material originalMaterial;

		[SerializeField]
		private TileType initialType;
		public TileType tileType { get; set; }
		public static GameObject[][] tileFlavor;

		public Tile() : this(TileType.None) { }

		public Tile(TileType tileType) {
			this.tileType = tileType;
		}

		void Awake() {
			if (Tile.tileFlavor == null) {
				GameObject[][] _tileFlavor = new GameObject[][] {
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {Resources.Load<GameObject>("TileFlavor/" + "SmallRock"), Resources.Load<GameObject>("TileFlavor/" + "SmallTree")},
					new GameObject[] {Resources.Load<GameObject>("TileFlavor/" + "SmallTree")},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {},
					new GameObject[] {}
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