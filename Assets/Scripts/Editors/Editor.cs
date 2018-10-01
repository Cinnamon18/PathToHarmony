using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using Gameplay;

namespace Editors {
	public abstract class Editor<T> : MonoBehaviour {
        protected T[,,] objs;
		protected bool overwriteData;
		protected string mapFilePath = Serialization.mapFilePath;
		protected string levelFilePath = Serialization.levelFilePath;

		//info for preview
		public GameObject[] previewObj;
		public GameObject currentPreviewObj;
		//public GameObject[] unitPrefabs;
		protected int currentIndex = 0;
		//where to display preview objects
		public Transform previewHolder;

		//x, y, height (from the bottom)
		public Vector3Int initialDim;

		public abstract void serialize();
		public abstract void deserialize();
        public abstract void create(Vector3Int coord, T obj);
        public abstract void remove(Vector3Int coord, T obj, RaycastHit hit);

		private enum EditorType
		{
			Unit,
			Tile,
		}
		private EditorType type;
		protected void setEditorType()
		{
			if (objs.GetType() == typeof(Unit[,,]))
			{
				type = EditorType.Tile;
			}
			else if (objs.GetType() == typeof(Tile[,,]))
			{
				type = EditorType.Unit;
			}
		}
		// Update is called once per frame
		void Update()
        {
            updateControl();
        }

        public void updateControl()
        {
            //Creation and deletion of GameObjects
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Vector3Int objCoords = Util.WorldToGrid(hit.transform.position);
				T obj = objs[objCoords.x, objCoords.y, objCoords.z];

				if (Input.GetButtonDown("Select"))
                {
					create(objCoords, obj);
                }
                else if (Input.GetButtonDown("AltSelect"))
                {
					remove(objCoords, obj, hit);
                }
            }

            updatePreview(Input.GetAxis("MouseScrollWheel"));
        }


		protected void updatePreview(float scroll)
		{
			GameObject oldPreviewTile;
			if (previewHolder.GetChildCount() != 0)
			{
				oldPreviewTile = previewHolder.GetChild(0).gameObject;
				if (scroll != 0)
				{
					if (scroll < 0)
					{
						currentIndex--;
					}
					else if (scroll > 0)
					{
						currentIndex++;
					}
					//Why can't we all just agree on what % means? This makes it "warp back around". My gut says there's a more elegant way to do this, but....
					currentIndex = currentIndex < 0 ? currentIndex + previewObj.Length : currentIndex % previewObj.Length;
					currentPreviewObj = Instantiate(previewObj[currentIndex], oldPreviewTile.transform.position, oldPreviewTile.transform.rotation, previewHolder);
					Destroy(oldPreviewTile);
				}
			} else
			{
				Debug.Log("Must have single preview object in preview holder object.");
			}
		}

		public void incrementPreview()
		{
			updatePreview(1);
		}

		public void decrementPreview()
		{
			updatePreview(-1);
		}

		public void updateOverwriteMode(bool state)
		{
			this.overwriteData = state;
		}
	}
}
