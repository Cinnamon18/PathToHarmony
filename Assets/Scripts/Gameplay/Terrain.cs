using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class Terrain {

    TerrainType terrain;
    
    public Terrain() {
        terrain = TerrainType.Normal;
    }

    public Terrain(TerrainType terrainType) {
        terrain = terrainType;
    }

}