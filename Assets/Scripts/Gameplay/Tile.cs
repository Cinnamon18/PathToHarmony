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

		public Tile() : this(TileType.None) { }

		public Tile(TileType tileType) {
			this.tileType = tileType;
		}

		void Start() {
			animate();
		}

		private void animate() {
			//TODO use blender animations for flowing water and whatnot.
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