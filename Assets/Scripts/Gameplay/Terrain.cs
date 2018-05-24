using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Terrain : MonoBehaviour{

    private Material originalMaterial;

    public TerrainType terrain { get; set; }

    public Terrain() {
        terrain = TerrainType.Normal;
    }

    public Terrain(TerrainType terrainType) {
        terrain = terrainType;
    }

}