using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using System.Threading;

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
		Vector3Int grid = new Vector3Int((int)(world.x / GridWidth), (int)(world.z / GridWidth), (int)(world.y / GridHeight));
		if (((int)(grid.x) != grid.x) || ((int)(grid.y) != grid.y) || ((int)(grid.z) != grid.z)) {
			Debug.LogWarning("The coordinate being converted doesn't fall perfectly on the grid: " + world);
		}
		return grid;
	}

	public static Vector3Int WorldToGrid(float x, float y, float z) {
		return WorldToGrid(new Vector3(x, y, z));
	}

	public static Vector3 GridToWorld(Vector3Int grid) {
		return new Vector3(grid.x * GridWidth, grid.z * GridHeight, grid.y * GridWidth);
	}

	public static Vector3 GridToWorld(int x, int y, int z) {
		return GridToWorld(new Vector3Int(x, y, z));
	}

	public static Vector3 GridToWorld(float x, float y, float z) {
		return new Vector3(x * GridWidth, z * GridHeight, y * GridWidth);
	}

	//What can I say, I've come to love typescript. We might have to overload this. But hey, if you're new to async
	//here's an easy way to start! And you wont get spoiled by the .net 5 async/await sugar :p
	//also oof don't use ui methods with this....
	public static void setTimeout(Action action, int timeout) {
		Thread t = new Thread(() => {
			Thread.Sleep(timeout);
			action.Invoke();
		});
		t.Start();
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
}
