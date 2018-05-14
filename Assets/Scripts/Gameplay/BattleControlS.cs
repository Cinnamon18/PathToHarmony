using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;

public class BattleControlS : MonoBehaviour {

    public const int mapXSize = 10;
    public const int mapYSize = 10;
    public Unit[,] units = new Unit[mapXSize, mapYSize];
    public Terrain[,] terrain = new Terrain[mapXSize, mapYSize];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
