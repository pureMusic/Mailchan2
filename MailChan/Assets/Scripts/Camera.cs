using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

		public GameObject player;			//プレイヤー
		public float screenNumX = 1;		//現在のスクロール可能画面数
		private float screenSizeX = 352f;	//画面サイズ
		private float screenSizeY = 198f;
		public Vector2 scrollStartPos;		//スクロール開始位置
		public bool ctrlFlag = true;		//スクロール制御フラグ
		public float scrollTime = 120f;		//スクロール時間
		private Vector2 restartPos;			//スタート地点
		private float restartNumX = 0;

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

		//横スクロール
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

		//縦スクロール
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

		//スタート地点設定
		public void setRestartPos(Vector2 vec, float x){
				restartPos = vec;
				restartNumX = x;
		}
}
