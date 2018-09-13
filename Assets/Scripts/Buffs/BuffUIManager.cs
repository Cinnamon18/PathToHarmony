using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Buffs {
	public class BuffUIManager : MonoBehaviour {

		private const float ICON_WIDTH = 1.3f;//I just tuned this value by hand...
		private List<Image> buffIcons = new List<Image>();
		private List<Buff> buffs = new List<Buff>();

		//Hmm this doesn't seem necessary but it's less fragile than saying parent.parent in code...
		[SerializeField]
		Unit unit;
		[SerializeField]
		Image buffIconPrefab;
		[SerializeField]
		Sprite[] buffIconSprites;

		public void addBuff(Buff buff) {
			buffs.Add(buff);

			Image newIcon = Instantiate(buffIconPrefab, this.transform);
			newIcon.sprite = buffIconSprites[(int)(buff.buffType)];
			newIcon.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, ICON_WIDTH * buffIcons.Count);
			buffIcons.Add(newIcon);
		}

		public void removeBuff(Buff buff) {
			int index = buffs.IndexOf(buff);
			buffs.Remove(buff);

			Image removedImage = buffIcons[index];
			buffIcons.Remove(removedImage);

			Destroy(removedImage);
		}

	}
}