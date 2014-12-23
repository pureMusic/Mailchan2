using UnityEngine;
using System.Collections;

public class ScrollCtrl : MonoBehaviour {
		public bool scrollVecTHFV = true;
		public float screenNumX = 1;		//スクロール後のX方向スクロール画面数

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnTriggerEnter2D(Collider2D col){
				if (col.gameObject.tag == "Player") {

						GameObject camera = GameObject.Find ("Main Camera");
						Camera cameraCtrl = camera.GetComponent<Camera> ();
						PlayerCtrl pCtrl = col.gameObject.GetComponent<PlayerCtrl> ();

						//敵・弾オブジェクトを削除
						GameObject[] deleteObj = GameObject.FindGameObjectsWithTag ("Enemy");
						foreach(GameObject obj in deleteObj){
								Destroy (obj);
						}
						deleteObj = GameObject.FindGameObjectsWithTag ("EnemyBullet");
						foreach(GameObject obj in deleteObj){
								Destroy (obj);
						}

						//カメラの制御
						cameraCtrl.screenNumX = screenNumX;
						cameraCtrl.ctrlFlag = false;

						//プレイヤーの制御
						pCtrl.ctrlFlag = false;

						if (scrollVecTHFV) {
								cameraCtrl.StartCoroutine ("ScrollX");
								pCtrl.StartCoroutine ("ScrollX");
						} else {
								cameraCtrl.StartCoroutine ("ScrollY");
								pCtrl.StartCoroutine ("ScrollY");
						}
				}
		}
}
