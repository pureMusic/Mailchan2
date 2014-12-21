using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour {
		public int damagePoint = 1;			//ダメージ量
		public float bulletSpeed = 128f;	//速度
		public float radian;
		Vector2 targetPos;					//誘導タイプのターゲット
		private int ctrlType;				//弾の種類

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
				if (ctrlType == 0) {
						//回転させる
						//transform.Rotate (0, 0, 90) ;
				}
		}

		public void BulletCtrl(int bulletType, bool faceRight){
				ctrlType = bulletType;
				//等速直進
				if (bulletType == 0) {
						rigidbody2D.velocity = new Vector2 ((faceRight ? 1 : -1) * bulletSpeed, 0);
				}
				//自機狙い
				if (bulletType == 1) {
						GameObject target = GameObject.Find ("Player");
						targetPos = new Vector2(target.transform.position.x, target.transform.position.y);
						radian = Mathf.Atan2 (targetPos.y - transform.position.y, targetPos.x - transform.position.x);
						rigidbody2D.velocity = new Vector2 (bulletSpeed * Mathf.Cos (radian), bulletSpeed * Mathf.Sin (radian));

				}
		}

		//画面外処理
		void OnBecameInvisible(){
				//弾の削除
				Destroy (this.gameObject);
		}

		//衝突処理
		void OnTriggerEnter2D(Collider2D col){
				//敵の弾が自機にヒット
				if (col.gameObject.tag == "Player" && this.gameObject.tag == "EnemyBullet") {
						PlayerCtrl player = col.GetComponent<PlayerCtrl>();
						player.Damage (damagePoint);
				}

				//自機の弾が敵にヒット
				if (col.gameObject.tag == "Enemy" && this.gameObject.tag == "PlayerBullet") {
						EnemyCtrl enemy = col.GetComponent<EnemyCtrl>();
						enemy.Damage (damagePoint);
						Destroy (this.gameObject);
				}
		}
}
