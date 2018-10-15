using Constants;
using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Editors
{
	public class TileGenerator : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] prefabs;

		public GameObject getTileByType(TileType type)
		{
			foreach (GameObject obj in prefabs)
			{

				if (obj.GetComponent<Tile>().initialType == type)
				{
					return obj;
				}
		
			}
			return null;
		}

		public GameObject[] getPrefabs()
		{
			return prefabs;
		}


	}
}

