using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes.Stages {
	public class Actor : MonoBehaviour {
		[SerializeField]
		private Image image;

		[SerializeField]
		public Side side;

		[SerializeField]
		private LayoutElement layoutElement;

		public new string name;

		public int Id {
			get {
				return GetInstanceID();
			}
		}

		public bool IsIgnoreLayout {
			set {
				layoutElement.ignoreLayout = value;
			}
		}
	}
}