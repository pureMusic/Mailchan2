using UnityEngine;
using System.Collections;

public class MoveBlock : MonoBehaviour {
		public float moveLength = 256;
		public float moveSpeed = 4;
		public string moveVec = "H";
		private float moved = 0;
		private int vecFlag = 1;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
				//移動
				Vector2 v = transform.position;
				if (Mathf.Abs (moved) >= moveLength / 2) {
						vecFlag = -1 * vecFlag;
				}
				if (moveVec == "H") {
						v.x += vecFlag * moveSpeed;
				} else if (moveVec == "V") {
						v.y += vecFlag * moveSpeed;
				}
				moved += vecFlag * moveSpeed;
				transform.position = v;
			
		}

}
