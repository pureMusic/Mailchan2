using UnityEngine;
using System.Collections;

public class SceneCtrl : MonoBehaviour {
		public enum SceneName{
				PROLOGUE,	//プロローグ画面
				TITLE,		//タイトル画面
				STAGE,		//ステージ画面
				GAMEOVER,	//ゲームオーバー画面
				CHANGE		//シーン切り替え
		}; 

		public SceneName scene;
		public GameObject pause;
		private GameObject pObj;
		private bool ctrlFlagTmp;
		private bool changeFlag;
		private AudioSource bgm;

		// Use this for initialization
		void Start () {
				switch (scene) {
				case SceneName.STAGE:
						changeFlag = false;
						bgm = this.gameObject.GetComponent<AudioSource> ();
						break;
				case SceneName.GAMEOVER:
						changeFlag = false;
						StartCoroutine("FadeIn",GameObject.Find("Black").GetComponent<SpriteRenderer>());
						break;
				default:
						changeFlag = true;
						break;

				}
		}
		
		// Update is called once per frame
		void Update () {
				switch (scene) {
				case SceneName.PROLOGUE:
						if (Input.GetKeyDown ("return") || !GameObject.Find ("PrologueMessage").animation.isPlaying) {
								Application.LoadLevel ("Title");
						}
						break;

				case SceneName.TITLE:
						if (Input.GetKeyDown ("return")) {
								Application.LoadLevel ("SampleStage");
						}
						break;

				case SceneName.STAGE:
						PlayerCtrl playerCtrl = GameObject.Find ("Mailchan").GetComponent<PlayerCtrl> ();
						if (playerCtrl.getLife () == 0) {
								bgm.Stop();
								StartCoroutine ("StageRestart");
								playerCtrl.setLife (-1);
						}
						//ポーズ
						if (playerCtrl.pauseFlag && Input.GetKeyDown (KeyCode.P)) {
								if (Time.timeScale != 0) {
										Time.timeScale = 0;
										ctrlFlagTmp = playerCtrl.ctrlFlag;
										playerCtrl.ctrlFlag = false;
										pObj = Instantiate (pause, this.transform.position, this.transform.rotation) as GameObject;
								} else if(Time.timeScale == 0){
										Time.timeScale = 1;
										playerCtrl.ctrlFlag = ctrlFlagTmp;
										Destroy (pObj);
								}
						}
						if (changeFlag) {
								Application.LoadLevel ("GameOver");
						}
						break;

				case SceneName.GAMEOVER:
						if (changeFlag && Input.GetKeyDown (KeyCode.Return)) {
								Application.LoadLevel ("Title");
						}
						break;

				case SceneName.CHANGE:
						/*if (scene == SceneName.TITLE) {
								Application.LoadLevel ("Title");
						} else if (scene == SceneName.STAGE) {
								Application.LoadLevel ("Stage");
						}*/
				default:
						break;

				}
		}

		IEnumerator StageRestart(){
				yield return new WaitForSeconds (5);

				//自機の再設定
				PlayerCtrl playerCtrl = GameObject.Find ("Mailchan").GetComponent<PlayerCtrl> ();
				playerCtrl.playerNum -= 1;

				if (playerCtrl.playerNum >= 0) {
						playerCtrl.ReStart ();

						//カメラの再設定
						GameObject.Find ("Main Camera").GetComponent<Camera> ().Restart ();

						//オブジェクトの削除
						//敵・弾オブジェクトを削除
						GameObject[] deleteObj = GameObject.FindGameObjectsWithTag ("Enemy");
						foreach (GameObject obj in deleteObj) {
								Destroy (obj);
						}
						deleteObj = GameObject.FindGameObjectsWithTag ("EnemyBullet");
						foreach (GameObject obj in deleteObj) {
								Destroy (obj);
						}
						//BGM再生
						bgm.Play ();
				} else {
						changeFlag = true;
				}
		}

		IEnumerator FadeIn(SpriteRenderer sprite){
				float time = 2f;	//エフェクト時間
				Color c = sprite.color;
				while (sprite.color.a > 0) {
						print (c.a);
						c.a -= 1f / (60 * time);
						if (c.a < 0) {
								c.a = 0;
						}
						sprite.color = c;
						yield return null;
				}
				changeFlag = true;
		}

}
