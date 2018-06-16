using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {

    [SerializeField]
    private static float gridWidth = 10.0f;
    public static float GridWidth {
        get {
            return gridWidth;
        }
        set {
            gridWidth = value;
        }
    }

    [SerializeField]
    private static float gridHeight = 4.0f;
    public static float GridHeight {
        get {
            return gridHeight;
        }
        set {
            gridHeight = value;
        }
    }


    public static Vector3 WorldToGrid(Vector3 world) {
        Vector3 grid = new Vector3(world.x / GridWidth, world.z / GridWidth, world.y / GridHeight);
        if (((int)(grid.x) != grid.x) || ((int)(grid.y) != grid.y) || ((int)(grid.z) != grid.z)) {
            Debug.Log("The coordinate being converted doesn't fall perfectly on the grid: " + world);
        }
        return grid;
    }

    public static Vector3 GridToWorld(Vector3 grid) {
        return new Vector3(grid.x * GridWidth, grid.z * GridHeight, grid.y * GridWidth);
    }


    public static void highlightObject(GameObject objectToHighlight) {
        Material[] materials = objectToHighlight.GetComponent<Renderer>().materials;
        foreach (Material material in materials) {
            //Set the main Color of the Material to green
            material.shader = Shader.Find("_Color");
            material.SetColor("_Color", Color.green);

            //Find the Specular shader and change its Color to red
            material.shader = Shader.Find("Specular");
            material.SetColor("_SpecColor", Color.red);
        }
    }

    public static void unhighlightObject(GameObject objectToHighlight) {
        Material[] materials = objectToHighlight.GetComponent<Renderer>().materials;
        foreach (Material material in materials) {
            //Set the main Color of the Material to green
            material.shader = Shader.Find("_Color");
            material.SetColor("_Color", Color.blue);

            //Find the Specular shader and change its Color to red
            material.shader = Shader.Find("Specular");
            material.SetColor("_SpecColor", Color.red);
        }
    }
}
