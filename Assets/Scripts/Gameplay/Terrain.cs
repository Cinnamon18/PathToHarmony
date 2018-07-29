using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using System;
using Gameplay;

public class Terrain : MonoBehaviour, IBattlefieldItem {
	private Material originalMaterial;

	[SerializeField]
	private TerrainType initialType;

	public TerrainType terrain { get; set; }

	public Terrain() {
		terrain = initialType;
	}

	public Terrain(TerrainType terrainType) {
		terrain = terrainType;
	}

	public string serialize() {
		return "" + ((int)(this.terrain));
	}

	public override string ToString() {
		return terrain.ToString();
	}

	public void vibrateUnhappily() {
		//TODO (just some programatic animation to make this reusable between different tiles, probably)
	}
}