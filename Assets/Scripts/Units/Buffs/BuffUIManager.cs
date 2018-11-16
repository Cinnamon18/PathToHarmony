using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Buffs {
	public class BuffUIManager : MonoBehaviour {

		private GameObject indicator;
		private GameObject buffModel;

		private float rotateSpeed = 20f;

		void Start() {
			buffModel = Resources.Load<GameObject>("Buffs/DebuffCircle");
		}

		void Update() {
			if (indicator != null)
				indicator.transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
		}

		public void addBuff(Buff buff) {
			Vector3 offset = new Vector3(0, -0.3f, 0);
			if (this.gameObject.GetComponentInParent<LightHorse>() != null) {
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