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

		public TileType tile { get; set; }

		public Tile() {
			tile = TileType.None;
		}

		public Tile(TileType tileType) {
			tile = tileType;
		}

		public string serialize() {
			return "" + ((int)(this.tile));
		}

		public override string ToString() {
			return tile.ToString();
		}

		public void vibrateUnhappily() {
			//TODO (just some programatic animation to make this reusable between different tiles, probably)
		}
	}
}