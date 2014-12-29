using UnityEngine;
using System.Collections;

public class ScrollCtrl : MonoBehaviour {
		public enum scrollVec {LEFT, RIGHT, UP, DOWN};
		public scrollVec vec = scrollVec.RIGHT;
		public float screenNumX = 1;		//スクロール後のX方向スクロール画面数

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnTriggerEnter2D(Collider2D col){
				if (col.gameObject.tag == "Player") {
						PlayerCtrl pCtrl = col.gameObject.GetComponent<PlayerCtrl> ();
						if (!pCtrl.scrollFlag) { 
								GameObject camera = GameObject.Find ("Main Camera");
								Camera cameraCtrl = camera.GetComponent<Camera> ();


								//敵・弾オブジェクトを削除
								GameObject[] deleteObj = GameObject.FindGameObjectsWithTag ("Enemy");
								foreach (GameObject obj in deleteObj) {
										Destroy (obj);
								}
								deleteObj = GameObject.FindGameObjectsWithTag ("EnemyBullet");
								foreach (GameObject obj in deleteObj) {
										Destroy (obj);
								}

								//カメラの制御
								cameraCtrl.screenNumX = screenNumX;
								cameraCtrl.ctrlFlag = false;

								//プレイヤーの制御
								pCtrl.ctrlFlag = false;

								switch (vec) {
								case scrollVec.LEFT:
										cameraCtrl.StartCoroutine ("ScrollX", -1);
										pCtrl.StartCoroutine ("ScrollX", -1);
										break;

								case scrollVec.RIGHT:
										cameraCtrl.StartCoroutine ("ScrollX", 1);
										pCtrl.StartCoroutine ("ScrollX", 1);
										break;

								case scrollVec.UP:
										cameraCtrl.StartCoroutine ("ScrollY", 1);
										pCtrl.StartCoroutine ("ScrollY", 1);
										break;

								case scrollVec.DOWN:
										cameraCtrl.StartCoroutine ("ScrollY", -1);
										pCtrl.StartCoroutine ("ScrollY", -1);
										break;
								}
						}
				}
		}
}
