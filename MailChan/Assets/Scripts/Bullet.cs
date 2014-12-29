using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour {
		public enum BulletType {
				STRAIGHT,
				TARGET_E_TO_P,
				ANGLE,
		};

		public int damagePoint = 1;			//ダメージ量
		public float bulletSpeed = 128f;	//速度
		public float angle;					//角度
		public BulletType ctrlType;			//弾の種類
		Vector2 targetPos;					//誘導タイプのターゲット
	

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
	
		}

		public void BulletCtrl(bool faceRight){
				switch(ctrlType){
				//等速直進
				case BulletType.STRAIGHT:
						rigidbody2D.velocity = new Vector2 ((faceRight ? 1 : -1) * bulletSpeed, 0);
						break;
				
				//自機狙い
				case BulletType.TARGET_E_TO_P:
						GameObject target = GameObject.Find ("Mailchan");
						targetPos = new Vector2 (target.transform.position.x, target.transform.position.y);
						float radian = Mathf.Atan2 (targetPos.y - transform.position.y, targetPos.x - transform.position.x);
						rigidbody2D.velocity = new Vector2 (bulletSpeed * Mathf.Cos (radian), bulletSpeed * Mathf.Sin (radian));
						break;

				//角度付き
				case BulletType.ANGLE:
						rigidbody2D.velocity = new Vector2 ((faceRight ? 1 : -1) * bulletSpeed * Mathf.Cos(angle)
								, bulletSpeed * Mathf.Sin(angle));
						break;
				}
		}

		public void BulletCtrl(bool faceRight, float angle){
				switch(ctrlType){

				//角度付き
				case BulletType.ANGLE:
						rigidbody2D.velocity = new Vector2 ((faceRight ? 1 : -1) * bulletSpeed * Mathf.Cos(angle)
								, bulletSpeed * Mathf.Sin(angle));
						break;
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
						if (enemy.guardFlag) {
								Vector3 vec = this.transform.rigidbody2D.velocity;
								vec.x = (-1) * vec.x;	//向きの反転
								vec.y = Mathf.Abs (vec.x);//ななめになるように設定
								this.transform.rigidbody2D.velocity = vec;
								//当たり判定を削除
								this.transform.collider2D.enabled = false;

						} else {
								Destroy (this.gameObject);
						}
				}
		}
}
