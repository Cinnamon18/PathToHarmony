using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;

namespace Editors {
	public abstract class Editor<T> : MonoBehaviour {
        protected T[,,] objs;
		protected bool overwriteData;
		public abstract void serialize();
		public abstract void deserialize();
        public abstract void create(Vector3Int coord, T obj);
        public abstract void remove(Vector3Int coord, T obj, RaycastHit hit);
		public abstract void updatePreview(float scroll);


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

		public void updateOverwriteMode(bool state)
		{
			this.overwriteData = state;
		}
	}
}
