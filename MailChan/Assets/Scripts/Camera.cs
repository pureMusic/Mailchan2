using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

		public GameObject player;			//プレイヤー
		public float screenNumX = 1;		//現在のスクロール可能画面数
		public static float screenSizeX = 352f;	//画面サイズ
		public static float screenSizeY = 198f;
		public Vector3 scrollStartPos;		//スクロール開始位置
		public bool ctrlFlag = true;		//スクロール制御フラグ
		public float scrollTime = 120f;		//スクロール時間
		private Vector3 restartPos;			//スタート地点
		private float restartNumX = 0;

		// Use this for initialization
		void Start () {
				scrollStartPos = new Vector3 (0, 0, -10);
		}

		public void Restart(){
				screenNumX = restartNumX;
				scrollStartPos = restartPos;
				transform.position = scrollStartPos;
				ctrlFlag = true;
		}

		// Update is called once per frame
		void Update () {

				if (ctrlFlag) {
						//カメラ移動（追従）
						Vector3 v = transform.position;
						v.x = player.transform.position.x;

						//見切れ防止
						if (v.x < scrollStartPos.x) {
								v.x = scrollStartPos.x;
						} else if (v.x > (screenNumX - 1) * screenSizeX + scrollStartPos.x) {
								v.x = (screenNumX - 1) * screenSizeX + scrollStartPos.x;
						}

						transform.position = v;
				}
		}

		//横スクロール
		IEnumerator ScrollX(int vec){
				int i = 0;
				Vector3 v = transform.position;
				for(i = 0; i < (int)scrollTime; i++){
						v.x += vec * screenSizeX / scrollTime;
						transform.position = v;
						yield return new WaitForSeconds(1/scrollTime);

				}
				if (screenNumX > 0) {
						scrollStartPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
				} else {
						screenNumX = (-1) * screenNumX;
						scrollStartPos = new Vector3 (transform.position.x - (screenNumX - 1) * screenSizeX, transform.position.y, transform.position.z);
				}
				ctrlFlag = true;

		}

		//縦スクロール
		IEnumerator ScrollY(int vec){
				int i = 0;
				Vector3 v = transform.position;
				for(i = 0; i < (int)scrollTime; i++){
						v.y += vec * screenSizeY / scrollTime;
						transform.position = v;
						yield return new WaitForSeconds(1/scrollTime);

				}
				if (screenNumX > 0) {
						scrollStartPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
				} else {
						screenNumX = (-1) * screenNumX;
						scrollStartPos = new Vector3 (transform.position.x - (screenNumX - 1) * screenSizeX, transform.position.y, transform.position.z);
				}
				ctrlFlag = true;

		}

		//スタート地点設定
		public void setRestartPos(Vector3 vec, float x){
				restartPos = vec;
				restartNumX = x;
		}
}
