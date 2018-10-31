﻿using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthUIManager : MonoBehaviour {

	private const int MAX_MODELS_PER_UNIT = 10;
	private const float COL_WIDTH = 2.6f;
	private const float ROW_HEIGHT = 1.8f;

	[SerializeField]
	public Image healthBar;
	[SerializeField]
	private GameObject unitModelPrefab;
	[SerializeField]
	private Unit unit;

	private List<GameObject> unitModels;
	private Material material;

	void Start() {
		setHealth(unit.maxHealth);
	}

	//It requires currentHealth to be passed in to do the rerender.
	//If a use case arises, we can make one that rerenders and one that doesn't. I like this class being stateless tho
	public void setMaterial(Material material, int currentHealth) {
		this.material = material;
		setHealth(currentHealth);
	}

	public void setHealth(int health) {
		//This may be removed.
		healthBar.fillAmount = 1f * unit.getHealth() / unit.maxHealth;

		int howManyModels = (int)Mathf.Ceil((float)(MAX_MODELS_PER_UNIT * health) / unit.maxHealth);
		clearModels();

		switch (howManyModels) {
			case 1:
				unitModels = new List<GameObject> {
					InstantiateRelative(0, 0, 0)
				};
				break;
			case 2:
				unitModels = new List<GameObject> {
					InstantiateRelative(0, 0, -COL_WIDTH / 2),
					InstantiateRelative(0, 0, COL_WIDTH / 2)
				};
				break;
			case 3:
				unitModels = new List<GameObject> {
					InstantiateRelative(0, 0, COL_WIDTH / 2),
					InstantiateRelative(0, 0, -COL_WIDTH / 2),
					InstantiateRelative(-ROW_HEIGHT, 0, 0)
				};
				break;
			case 4:
				unitModels = new List<GameObject> {
					InstantiateRelative(ROW_HEIGHT / 2, 0, -COL_WIDTH / 2),
					InstantiateRelative(-ROW_HEIGHT/ 2, 0, -COL_WIDTH / 2),
					InstantiateRelative(ROW_HEIGHT / 2, 0, COL_WIDTH / 2),
					InstantiateRelative(-ROW_HEIGHT / 2, 0, COL_WIDTH / 2)
				};
				break;
			case 5:
				unitModels = new List<GameObject> {
					InstantiateRelative(ROW_HEIGHT / 2, 0, -COL_WIDTH),
					InstantiateRelative(ROW_HEIGHT / 2, 0, 0),
					InstantiateRelative(ROW_HEIGHT / 2, 0, COL_WIDTH),
					InstantiateRelative(-ROW_HEIGHT / 2, 0, COL_WIDTH / 2),
					InstantiateRelative(-ROW_HEIGHT / 2, 0, -COL_WIDTH / 2)
				};
				break;
			case 6:
				unitModels = new List<GameObject> {
					InstantiateRelative(ROW_HEIGHT, 0, COL_WIDTH),
					InstantiateRelative(ROW_HEIGHT, 0, 0),
					InstantiateRelative(ROW_HEIGHT, 0, -COL_WIDTH),
					InstantiateRelative(0 / 2, 0, -COL_WIDTH / 2),
					InstantiateRelative(0, 0, COL_WIDTH / 2),
					InstantiateRelative(-ROW_HEIGHT, 0, 0)
				};
				break;
			case 7:
				unitModels = new List<GameObject> {
					InstantiateRelative(0, 0, COL_WIDTH),
					InstantiateRelative(0, 0, 0),
					InstantiateRelative(0, 0, -COL_WIDTH),
					InstantiateRelative(-ROW_HEIGHT, 0, COL_WIDTH / 2),
					InstantiateRelative(-ROW_HEIGHT, 0, -COL_WIDTH / 2),
					InstantiateRelative(ROW_HEIGHT, 0, COL_WIDTH / 2),
					InstantiateRelative(ROW_HEIGHT, 0, -COL_WIDTH / 2)
				};
				break;
			case 8:
				unitModels = new List<GameObject> {
					InstantiateRelative(0, 0, COL_WIDTH),
					InstantiateRelative(0, 0, 0),
					InstantiateRelative(0, 0, -COL_WIDTH),
					InstantiateRelative(ROW_HEIGHT, 0, COL_WIDTH),
					InstantiateRelative(ROW_HEIGHT, 0, 0),
					InstantiateRelative(ROW_HEIGHT, 0, -COL_WIDTH),
					InstantiateRelative(-ROW_HEIGHT, 0, COL_WIDTH / 2),
					InstantiateRelative(-ROW_HEIGHT, 0, -COL_WIDTH / 2),
				};
				break;
			case 9:
				unitModels = new List<GameObject> {
					InstantiateRelative(0, 0, COL_WIDTH),
					InstantiateRelative(0, 0, 0),
					InstantiateRelative(0, 0, -COL_WIDTH),
					InstantiateRelative(ROW_HEIGHT, 0, COL_WIDTH),
					InstantiateRelative(ROW_HEIGHT, 0, 0),
					InstantiateRelative(ROW_HEIGHT, 0, -COL_WIDTH),
					InstantiateRelative(-ROW_HEIGHT, 0, COL_WIDTH),
					InstantiateRelative(-ROW_HEIGHT, 0, 0),
					InstantiateRelative(-ROW_HEIGHT, 0, -COL_WIDTH),
				};
				break;
			case 10:
				unitModels = new List<GameObject> {
					InstantiateRelative(-ROW_HEIGHT / 2, 0, COL_WIDTH),
					InstantiateRelative(-ROW_HEIGHT / 2, 0, 0),
					InstantiateRelative(-ROW_HEIGHT / 2, 0, -COL_WIDTH),
					InstantiateRelative(ROW_HEIGHT / 2, 0, COL_WIDTH),
					InstantiateRelative(ROW_HEIGHT / 2, 0, 0),
					InstantiateRelative(ROW_HEIGHT / 2, 0, -COL_WIDTH),
					InstantiateRelative(-3.0 / 2 * ROW_HEIGHT, 0, COL_WIDTH / 2),
					InstantiateRelative(-3.0 / 2 * ROW_HEIGHT, 0, -COL_WIDTH / 2),
					InstantiateRelative(3.0 / 2 * ROW_HEIGHT, 0, COL_WIDTH / 2),
					InstantiateRelative(3.0 / 2 * ROW_HEIGHT, 0, -COL_WIDTH / 2),
				};
				break;
			default:
				Debug.LogError("unit at > 100%, health ui manager not really sure what to do with that");
				break;
		}
	}

	private GameObject InstantiateRelative(double x, double y, double z) {
		GameObject model = Instantiate(unitModelPrefab,
			unit.transform.position + new Vector3((float)(x), (float)(y), (float)(z)),
			unitModelPrefab.transform.rotation,
			unit.transform);
		model.GetComponentInChildren<SkinnedMeshRenderer>().material = this.material;
		return model;
	}

	private void clearModels() {
		//We null check here instead of gaurenteeing it because other methods can be called before Start()...
		//Because... Unity... 99% synchronous except when it's not.
		if (unitModels == null) {
			return;
		}

		foreach (GameObject model in unitModels) {
			Destroy(model);
		}

		unitModels.Clear();
	}

	public List<Animator> getAnimators() {
		List<Animator> animators = new List<Animator>();
		foreach (GameObject model in unitModels) {
			animators.Add(model.GetComponent<Animator>());
		}
		return animators;
	}

	public List<GameObject> getModels() {
		List<GameObject> models = new List<GameObject>();
		foreach (GameObject model in unitModels) {
			models.Add(model.GetComponentInChildren<SkinnedMeshRenderer>().gameObject);
		}
		return models;
	}


}
