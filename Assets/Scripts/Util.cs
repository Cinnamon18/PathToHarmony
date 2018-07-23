using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
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

	public static Vector3Int WorldToGrid(Vector3 world) {
		Vector3Int grid = new Vector3Int((int) (world.x / GridWidth), (int) (world.z / GridWidth), (int) (world.y / GridHeight));
		if (((int)(grid.x) != grid.x) || ((int)(grid.y) != grid.y) || ((int)(grid.z) != grid.z)) {
			Debug.LogWarning("The coordinate being converted doesn't fall perfectly on the grid: " + world);
		}
		return grid;
	}

	public static Vector3 GridToWorld(Vector3Int grid) {
		return new Vector3(grid.x * GridWidth, grid.z * GridHeight, grid.y * GridWidth);
	}

	//TODO: Find a way to programatically create a highlight effect. Shader??
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

	//Is there a way to generalize this? I'm sure! But it sounds like it'd involve recursion and i don't feel like dealing with that rn
	public static T[] Flatten2DArray<T>(T[,] unflatArray) {
		T[] flattenedArray = new T[unflatArray.GetLength(0) * unflatArray.GetLength(1)];
		for (int y = 0; y < unflatArray.GetLength(0); y++) {
			for (int z = 0; z < unflatArray.GetLength(1); z++) {
				flattenedArray[z + unflatArray.GetLength(1) * y] = unflatArray[y, z];
			}
		}
		return flattenedArray;
	}

	public static T[] Flatten3DArray<T>(T[,,] unflatArray) {
		T[] flattenedArray = new T[unflatArray.GetLength(0) * unflatArray.GetLength(1) * unflatArray.GetLength(2)];
		for (int x = 0; x < unflatArray.GetLength(0); x++) {
			for (int y = 0; y < unflatArray.GetLength(1); y++) {
				for (int z = 0; z < unflatArray.GetLength(2); z++) {
					// flattenedArray[z + unflatArray.GetLength(1) * y + (x * unflatArray.GetLength(2) * unflatArray.GetLength(1))] = unflatArray[x, y, z];
					flattenedArray[x + unflatArray.GetLength(1) * (y + unflatArray.GetLength(0) * z)] = unflatArray[x, y, z];
				}
			}
		}
		return flattenedArray;
	}




	//turns out this is a surprisngly tricky concept, so we're just gonna stick with the standard unity one for now.

	public static void Log(params System.Object[] logItems) {
		foreach (System.Object logItem in logItems) {
			//If its an array (or collection or w/e), go ahead and print out all the items
			IEnumerable enumerable = logItem as System.Collections.IEnumerable;
			if (enumerable != null && logItem.GetType() != typeof(String)) {
				StringBuilder serializedObject = new StringBuilder();
				foreach (var value in enumerable) {
					serializedObject.Append(value + " ");
				}
				Debug.Log(logItem + serializedObject.ToString());
			} else {
				Debug.Log(logItem);
			}
		}
	}

	//I'm not totally sure how this behaves with collections. I guess address that as it comes up
	//I'm spoiled by browser js log, so i'm gonna remake it :p
	// public static string deepToString(System.Object obj) {
	// 	if (obj == null) {
	// 		return "null";
	// 	}

	// 	Type objType = obj.GetType();
	// 	if (objType.IsPrimitive || objType == typeof(Decimal) || objType == typeof(String)) {
	// 		return objType + ": " + obj.ToString() + "\n";
	// 	}

	// 	StringBuilder serializedObject = new StringBuilder();
	// 	if (objType.IsArray) {
	// 		IEnumerable enumerable = obj as IEnumerable;
	// 		foreach (var value in enumerable) {
	// 			serializedObject.Append(deepToString(value));
	// 		}
	// 		return serializedObject.ToString();
	// 	} else {
	// 		serializedObject.Append(obj.ToString());
	// 		foreach (var field in objType.GetFields()) {
	// 			serializedObject.Append(field.Name + ": " + deepToString(field.GetValue(obj)));
	// 		}
	// 		return serializedObject.ToString();
	// 	}
	// }
}
