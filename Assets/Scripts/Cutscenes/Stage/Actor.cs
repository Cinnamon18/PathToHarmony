using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes.Stages {
	public class Actor : MonoBehaviour {
		[SerializeField]
		public Image image;

		[SerializeField]
		public CutsceneSide side;

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

		public bool IsDark {
			set {
				image.color = (value ? Color.grey : Color.white);
			}
		}
	}
}