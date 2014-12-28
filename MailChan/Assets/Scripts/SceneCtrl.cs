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

		// Use this for initialization
		void Start () {
				//GameObject.Find ("PrologueMessage").animation.Play ();
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
								StartCoroutine ("StageRestart");
								playerCtrl.setLife (-1);
						}
								

						break;

				case SceneName.GAMEOVER:
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
				} else {
						//scene = SceneName.TITLE;
				}
		}
}
