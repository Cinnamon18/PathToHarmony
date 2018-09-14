using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Editors {
	public abstract class Editor : MonoBehaviour {

		public abstract void serialize();
		public abstract void deserialize();
	}
}
