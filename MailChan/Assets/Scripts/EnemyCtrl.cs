﻿using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {
		public int damagePoint = 0;		//接触ダメージ量
		public string enemyType = "00";	//敵の種類
		public int lifePoint = 1;		//ライフポイント
		SpriteRenderer nowSprite;		//現在のスプライト
		public Sprite defSprite;		//通常時スプライト
		public Sprite moveSprite;		//移動時スプライト
		public Sprite attackSprite;		//攻撃時スプライト
		public GameObject bullet;		//Bulletのプレハブ
		public bool faceRight = false;	//向いてる方向
		public GameObject destroy;		//消滅エフェクトのプレハブ
		public bool moveFlag;			//移動フラグ
		public float moveSpeed;			//移動速度
		public bool animeFlag;			//個別アニメーション用フラグ
		public bool guardFlag = false;	//ガード状態フラグ


		// Use this for initialization
		void Start () {
				nowSprite = gameObject.GetComponent<SpriteRenderer> ();
				nowSprite.sprite = defSprite;
		}
		
		// Update is called once per frame
		void Update () {
				//移動処理
				Vector2 v = rigidbody2D.velocity;
				if (moveFlag) {
						v.x = moveSpeed * (faceRight ? 1 : -1);
				}

				rigidbody2D.velocity = v;

				//向き変更
				setFaceRight ();

				//アニメーション用フラグを設定
				GetComponent<Animator> ().SetBool ("animeFlag", animeFlag);
		}

		//行動制御---------------------------------------------------------------------
		//tenp
		IEnumerator Pattern00(){
				while (true) {
						yield return new WaitForSeconds (1);
				}
		}

		IEnumerator Pattern01(){
				while (true) {
						yield return new WaitForSeconds (3);
						nowSprite.sprite = attackSprite;

						Vector3 v3 = this.transform.position;
						GameObject bulletCtrl = Instantiate (bullet, v3, this.transform.localRotation) as GameObject;
						Bullet b = bulletCtrl.GetComponent<Bullet> ();
						b.BulletCtrl (1, faceRight);

						faceRight = (faceRight ? false : true);
						GameObject bulletCtrl2 = Instantiate (bullet, v3, this.transform.localRotation) as GameObject;
						Bullet b2 = bulletCtrl2.GetComponent<Bullet> ();
						b2.BulletCtrl (0, faceRight);

						yield return new WaitForSeconds (2);
						nowSprite.sprite = defSprite;

				}
		}

		//メッソール
		IEnumerator Pattern02(){
				int counter = 4;	//ガード時間
				guardFlag = true;

				while (true) {
						yield return new WaitForSeconds (counter);
						//攻撃開始
						animeFlag = true;
						guardFlag = false;
						yield return new WaitForSeconds (1);
						LockPlayer ();
						moveFlag = true;
						Vector3 v3 = this.transform.position;
						GameObject bulletCtrl = Instantiate (bullet, v3, this.transform.localRotation) as GameObject;
						Bullet b = bulletCtrl.GetComponent<Bullet> ();
						b.BulletCtrl (1, faceRight);
						yield return new WaitForSeconds (1);
						moveFlag = false;
						yield return new WaitForSeconds (1);
						guardFlag = true;
						animeFlag = false;
				}
		}

		//ポスト
		IEnumerator Pattern03(){
				int counter = 4;	//ガード時間
				guardFlag = true;

				while (true) {
						yield return new WaitForSeconds (counter);

						//攻撃開始
						animeFlag = true;
						yield return new WaitForSeconds (40 / 60f);
						LockPlayer ();
						guardFlag = false;
						Vector3 v3 = this.transform.position;
						GameObject bulletCtrl = Instantiate (bullet, v3, this.transform.localRotation) as GameObject;
						Bullet b = bulletCtrl.GetComponent<Bullet> ();
						b.BulletCtrl (1, faceRight);
						yield return new WaitForSeconds (45/60f);
						guardFlag = true;
						animeFlag = false;
				}
		}

		//ペリカン
		IEnumerator Pattern04(){

				while (true) {
						yield return new WaitForSeconds (1);
				}
		}



		//共通処理----------------------------------------------------------------------

		//ダメージ制御
		public void Damage(int damage){
				if (!guardFlag) {
						lifePoint -= damage;

						//死亡判定
						if (lifePoint <= 0) {
								//消滅エフェクト
								destroy = Instantiate (destroy, this.transform.position, this.transform.rotation) as GameObject;
								Destroy (destroy, 1 / 6f);	//10F

								//オブジェクト削除
								Destroy (this.gameObject);
						}
				}
		}

		//プレイヤーの方を見る
		void LockPlayer(){
				//GameObject target = GameObject.FindGameObjectWithTag("Player");
				GameObject target = GameObject.Find ("Mailchan");
				if (target.transform.position.x > this.transform.position.x) {
						faceRight = true;
				} else {
						faceRight = false;
				}
		}

		//向き変更
		void setFaceRight(){
				if (faceRight) {
						this.transform.rotation = new Quaternion(0,180,0,0); 
				} else {
						this.transform.rotation = new Quaternion(0,0,0,0); 
				}
		}

		//接触時の処理
		void OnTriggerEnter2D(Collider2D col){
				//ダメージ処理
				if (col.gameObject.tag == "Player") {
						PlayerCtrl player = col.GetComponent<PlayerCtrl>();
						player.Damage (damagePoint);
				}

		}

		//フレームイン処理
		void OnBecameVisible(){
				//行動開始
				StartCoroutine ("Pattern" + enemyType);
		}

		//フレームアウト処理
		void OnBecameInvisible(){
				//オブジェクトの削除
				Destroy (this.gameObject);
		}
}
