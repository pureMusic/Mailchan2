using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

		public GameObject player;
		public float screenNumX = 1;
		private float screenSizeX = 1024f;
		private float screenSizeY = 576f;
		public Vector2 scrollStartPos;
		public bool ctrlFlag = true;
		public float scrollTime = 120f;

		// Use this for initialization
		void Start () {
				scrollStartPos = new Vector2 (0, 0);
		}

		// Update is called once per frame
		void Update () {

				if (ctrlFlag) {
						//カメラ移動（追従）
						Vector3 v = transform.position;
						v.x = player.transform.position.x;

						//見切れ防止
						float posX = (screenNumX - 1) * screenSizeX + scrollStartPos.x;

						if (v.x < scrollStartPos.x) {
								v.x = scrollStartPos.x;
						} else if (v.x > (screenNumX - 1) * screenSizeX + scrollStartPos.x) {
								v.x = (screenNumX - 1) * screenSizeX + scrollStartPos.x;
						}

						transform.position = v;
				}
		}

		IEnumerator ScrollX(){
				int i = 0;
				Vector3 v = transform.position;
				for(i = 0; i < (int)scrollTime; i++){
						v.x += screenSizeX / scrollTime;
						transform.position = v;
						yield return new WaitForSeconds(1/scrollTime);

				}
				scrollStartPos = new Vector2 (transform.position.x, transform.position.y);
				ctrlFlag = true;

		}

		IEnumerator ScrollY(){
				int i = 0;
				Vector3 v = transform.position;
				for(i = 0; i < (int)scrollTime; i++){
						v.y += screenSizeY / scrollTime;
						transform.position = v;
						yield return new WaitForSeconds(1/scrollTime);

				}
				scrollStartPos = new Vector2 (transform.position.x, transform.position.y);
				ctrlFlag = true;

		}
}
