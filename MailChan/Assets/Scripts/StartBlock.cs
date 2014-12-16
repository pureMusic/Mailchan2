using UnityEngine;
using System.Collections;

public class StartBlock : MonoBehaviour {

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnTriggerEnter2D(Collider2D col){
				if (col.gameObject.tag == "Player") {
						//プレイヤーの再開位置を設定、着地地点
						PlayerCtrl pCtrl = col.gameObject.GetComponent<PlayerCtrl> ();
						pCtrl.setRestartPos (transform.position);
						pCtrl.setWarpFlag (false);

						//カメラの再開位置を設定
						GameObject camera = GameObject.Find ("Main Camera");
						Camera cCtrl = camera.GetComponent<Camera> ();
						cCtrl.setRestartPos (cCtrl.scrollStartPos, cCtrl.screenNumX);
				}
		}
}
