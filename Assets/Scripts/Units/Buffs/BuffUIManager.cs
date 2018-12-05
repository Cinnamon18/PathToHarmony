using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Buffs {
	public class BuffUIManager : MonoBehaviour {

		private GameObject indicator;
		private GameObject buffModel;

		private float rotateSpeed = 100f;

		void Start() {
			buffModel = Resources.Load<GameObject>("Buffs/DebuffCircle");
		}

		void Update() {
			if (indicator != null) {
				float xRot = Mathf.PerlinNoise(0, Time.time * 0.75f) * 25 - 12.5f;
				// float zRot = Mathf.PerlinNoise(1, Time.time * 0.5f) * 20;
				indicator.transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
				indicator.transform.eulerAngles = new Vector3(xRot, indicator.transform.eulerAngles.y, 0);
			}
		}

		public void addBuff(Buff buff) {
			// Vector3 offset = new Vector3(3.5f, 3.3f, 4f);
			Vector3 offset = new Vector3(0, 3.0f, 0);
			if (this.gameObject.GetComponentInParent<LightHorse>() != null ||
				this.gameObject.GetComponentInParent<HeavyHorse>() != null) {
				offset += new Vector3(0, 0.8f, 0);
			}

			indicator = Instantiate(buffModel,
				transform.parent.position + offset,
				buffModel.transform.rotation,
				transform.parent);
		}

		public void removeBuff(Buff buff) {
			Destroy(indicator);
		}

	}
}