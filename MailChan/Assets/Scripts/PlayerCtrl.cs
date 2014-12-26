using UnityEngine;
using System.Collections;
using System;

public class PlayerCtrl : MonoBehaviour {
		//固定パラメータ
		public static float speed = 64f;			//横移動速度
		public static float jumpForce = 256f;		//ジャンプ力
		public static int bulletMaxNum = 3; 		//画面内の弾の最大数
		private int maxLifePoint = 28;		//最大ライフポイント
		private float maxChargePoint = 28f;	//最大チャージポイント
		public static float chargeTime = 2f;		//チャージにかかる秒数
		public static float scrollTime = 120f;		//スクロール時間
		private int maxPlayerNum = 9;		//最大残機数
		private float gravity = 64;				//重力

		//現在パラメータ
		private Vector3 restartPos;			//スタート地点
		private float ladderPosX = 0;		//掴まる梯子の座標
		private bool facingRight = true;	//向いてる方向
		private int lifePoint = 28;			//ライフポイント
		private float chargePoint = 0;		//チャージ量
		private int shotCounter = 0;		//ショットモーション制御カウンター
		public int playerNum = 3;			//残機数(初期値）

		//フラグ
		private bool jumpFlag = false;		//ジャンプフラグ
		bool walkFlag = false;				//横移動フラグ
		bool shotFlag = false;				//ショットフラグ
		private bool hitFlag = false;		//ダメージ判定フラグ
		public bool ctrlFlag = true;		//コントロールフラグ
		private bool ladderFlag = false;	//梯子移動フラグ
		private bool ladderEnable = false;	//梯子接触フラグ
		private bool warpFlag = true;		//ワープフラグ

		//プレハブ
		public GameObject nBullet;			//通常ショットのプレハブ
		public GameObject cBullet;			//チャージショットのプレハブ
		public GameObject hitRenderer;		//ダメージスプライト
		public GameObject explode;			//ライフ0時のエフェクトプレハブ

		void Start(){
				//ライフの設定
				lifePoint = maxLifePoint;

				//重力値の設定
				this.transform.rigidbody2D.gravityScale = gravity;

				//ダメージエフェクトの設定
				hitRenderer = Instantiate (hitRenderer, this.transform.position, this.transform.rotation) as GameObject;
				hitRenderer.transform.parent = this.transform;
				hitRenderer.transform.renderer.enabled = false;

				//スタート地点の設定
				restartPos = new Vector3 (0, 0, 0);
		}

		public void ReStart(){

				this.transform.rigidbody2D.gravityScale = gravity;
				//スプライト非表示
				this.transform.renderer.enabled = true;
				this.gameObject.layer = LayerMask.NameToLayer ("Player");
				//ダメージエフェクトの設定
				hitRenderer.transform.renderer.enabled = false;

				//スタート地点の設定
				transform.position = restartPos;

				//現在パラメータ
				facingRight = true;	//向いてる方向
				transform.localScale = new Vector3 ((facingRight ? 1 : -1), 1, 1);
				lifePoint = maxLifePoint;			//ライフポイント
				chargePoint = 0;		//チャージ量
				shotCounter = 0;		//ショットモーション制御カウンター

				//フラグ
				jumpFlag = false;				//ジャンプフラグ
				walkFlag = false;				//横移動フラグ
				shotFlag = false;				//ショットフラグ
				hitFlag = false;		//ダメージ判定フラグ
				ctrlFlag = true;		//コントロールフラグ
				ladderFlag = false;	//梯子移動フラグ
				ladderEnable = false;	//梯子接触フラグ
				warpFlag = true;		//ワープフラグ

				GameObject lifeBar = GameObject.Find ("LifeBar1");
				Vector2 vec = lifeBar.transform.localScale;
				vec.y = lifePoint;
				lifeBar.transform.localScale = vec;
		}

		void Update(){
				if (Input.GetKeyDown (KeyCode.Escape))
						Application.LoadLevel (0);
				if (warpFlag) {
						transform.position = new Vector3 (transform.position.x
														, transform.position.y - 8
														, transform.position.z);
				}else if (lifePoint > 0 && ctrlFlag) {
						//入力情報を取得
						float x = 0;
						float y = 0;
						if (Input.GetKey(KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
								y = 1;
						} else if (Input.GetKey(KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
								y = -1;
						}
						if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
								x = 1;
						} else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
								x = -1;
						}

						//梯子移動
						if (ladderFlag) {
								//移動-------------------------------------------------
								transform.position = new Vector3(transform.position.x
										, transform.position.y + y * 1
										, transform.position.z);

								//梯子を離す
								if (Input.GetKeyDown (KeyCode.Space)) {
										ladderEnable = false;
										ladderFlag = false;
										rigidbody2D.isKinematic = false;
								}
						} 

						//通常時
						else {
								Vector2 v = rigidbody2D.velocity;

								//ジャンプ制御------------------------------------

								//落下
								if (v.y < -0.1f) {	//誤差修正のため0ではなく-0.1とする
										jumpFlag = true;
								}
								//着地
								if (Mathf.Abs( v.y*1000) < 1 && jumpFlag) {
										jumpFlag = false;
								}

								//ジャンプボタン押下
								if (Input.GetKeyDown ("space") && !jumpFlag) {
										jumpFlag = true;
										v.y = jumpForce;
								}
								//ジャンプボタンを離す
								if (Input.GetKeyUp ("space") && jumpFlag && v.y > 0) {
										v.y = -1f;
								}


			
								//横移動-------------------------------------------------
								if (x == 0)
										walkFlag = false;
								else
										walkFlag = true;
						
								v.x = x * speed;
								rigidbody2D.velocity = v;

								//梯子移動-----------------------------------------------
								if (ladderEnable && y != 0) {
										ladderFlag = true;
										transform.rigidbody2D.velocity = new Vector2 (0, 0);
										transform.rigidbody2D.isKinematic = true;
										transform.position = new Vector3 (ladderPosX, transform.position.y, transform.position.z);
								}
						}

						//右を向いていて、左の入力があったとき、もしくは左を向いていて、右の入力があったとき
						if ((x > 0 && !facingRight) || (x < 0 && facingRight)) {
								//右を向いているかどうかを、入力方向をみて決める
								facingRight = (x > 0);
								//localScale.xを、右を向いているかどうかで更新する
								transform.localScale = new Vector3 ((facingRight ? 1 : -1), 1, 1);
						}

						//ショット制御---------------------------------------------------------------
						if (Input.GetKeyDown ("return")) {
								if (GameObject.Find("BulletList").transform.childCount < bulletMaxNum) {
										ShotCtrl ();
								}
						}
						ChargeCheck ();
						if (Input.GetKeyUp ("return") && chargePoint > maxChargePoint / 3) {
								ShotCtrl ();
						}
						if (!Input.GetKey ("return")) {
								chargePoint = 0;
						}
						//ショットモーション制御
						if (shotFlag) {
								if (shotCounter++ > 20) {
										shotFlag = false;
										shotCounter = 0;
								}
						}
				}

				//アニメーション用フラグを設定
				float walkShotFlag = (shotFlag ? 1.0f : 0f);
				GetComponent<Animator> ().SetBool ("walkFlag", walkFlag);
				GetComponent<Animator> ().SetBool ("shotFlag", shotFlag);
				GetComponent<Animator> ().SetFloat ("walkshot", walkShotFlag);
				GetComponent<Animator> ().SetFloat ("jumpSpeed", transform.rigidbody2D.velocity.y);
				GetComponent<Animator> ().SetBool ("jumpFlag", jumpFlag);
				GetComponent<Animator> ().SetBool ("ladderFlag", ladderFlag);
				GetComponent<Animator> ().SetFloat ("ladderPos", (int)Mathf.Abs(transform.position.y + Camera.screenSizeY / 2) / 8 % 2);
				GetComponent<Animator> ().SetBool ("warpFlag", warpFlag);

				//デバッグ---------------------------------------------------
				//MyDebug ();

		}

		//デバッグ用
		void MyDebug(){
				//print ("FPS:" + 1 / Time.deltaTime);
				Debug.Log ("jumpFlag:" + jumpFlag);
				Debug.Log ("v:" + rigidbody2D.velocity);
		}

		//向き判定
		public int getFacingRight(){
				return (facingRight ? 1 : -1);
		}

		//ダメージ制御-------------------------------------------------------------
		public void Damage(int damage){
				if (!hitFlag) {
						hitFlag = true;
						lifePoint -= damage;
						if (lifePoint <= 0) {
								lifePoint = 0;
								//キャラクター座標を固定
								this.transform.rigidbody2D.velocity = new Vector2 (0, 0);
								this.transform.rigidbody2D.gravityScale = 0;
								//スプライト非表示
								this.transform.renderer.enabled = false;
								this.gameObject.layer = LayerMask.NameToLayer ("PlayerThrough");

								//ライフ０時のエフェクト
								GameObject explodeEffect = Instantiate (explode, this.transform.position, this.transform.rotation) as GameObject;
								Destroy (explodeEffect, 5);

						} else {
								//無敵時間発生
								StartCoroutine ("InvincibleTime");
						}
						GameObject lifeBar = GameObject.Find ("LifeBar1");
						Vector2 vec = lifeBar.transform.localScale;
						vec.y = lifePoint;
						lifeBar.transform.localScale = vec;
				}
		}

		//無敵時間発生
		IEnumerator InvincibleTime(){
				int count = 0;
				hitRenderer.transform.position = this.transform.position; 
				this.gameObject.layer = LayerMask.NameToLayer ("PlayerThrough");
				//ダメージエフェクト表示
				for (count = 0; count < 10; count++) {
						hitRenderer.renderer.enabled = (count % 2 == 0 ? true : false);
						this.transform.renderer.enabled = (count % 2 == 0 ? false : true);
						yield return new WaitForSeconds (1 / 60f);
				}
				//ダメージエフェクトを終了し、無敵継続
				for (count = 0; count < 50; count++) {
						this.transform.renderer.enabled = (count % 2 == 0 ? false : true);
						yield return new WaitForSeconds (1 / 60f);
				}

				//ダメージを受けるようにする
				hitFlag = false;
				this.gameObject.layer = LayerMask.NameToLayer ("Player");
		}

		//ショット制御--------------------------------------------------------------

		//ショットコントロール
		void ShotCtrl(){
				Vector3 v3 = transform.position;

				//ショットの生成
				v3.x += 20 * getFacingRight ();
				GameObject bulletCtrl;

				if (chargePoint != maxChargePoint) {
						bulletCtrl = Instantiate (nBullet, v3, transform.rotation) as GameObject;
				}else {
						bulletCtrl = Instantiate (cBullet, v3, transform.rotation) as GameObject;
				}
				Bullet b = bulletCtrl.GetComponent<Bullet> ();
				b.BulletCtrl (0, facingRight);

				//ショットを管理用リストの子に設定
				b.transform.parent = GameObject.Find("BulletList").transform;

				//ショットカウンターをリセット
				shotCounter = 0;

				shotFlag = true;
		}

		//チャージ時間管理
		bool ChargeCheck(){
				chargePoint += maxChargePoint / (chargeTime * 50);
				if (chargePoint > maxChargePoint) {
						chargePoint = maxChargePoint;
				}

				GameObject chargeBar = GameObject.Find ("ChargeBar1");
				Vector2 vec = chargeBar.transform.localScale;
				vec.y = chargePoint;
				chargeBar.transform.localScale = vec;

				return (chargePoint == maxChargePoint ? true : false);
		}


		//接触制御--------------------------------------------------------------------

		//接触時の処理
		void OnTriggerEnter2D(Collider2D col){
				//移動床に乗る
				if (transform.parent == null && col.gameObject.tag == "MoveBlock") {
						transform.parent = col.gameObject.transform;
				}

				//梯子フラグを設定
				if (col.gameObject.tag == "Ladder") {
						ladderEnable = true;
						ladderPosX = col.gameObject.transform.position.x;
				}

		}

		//離れた時の処理
		void OnTriggerExit2D(Collider2D col){
				//移動床から離れる
				if(transform.parent != null && col.gameObject.tag == "MoveBlock"){
						transform.parent = null;
				}

				//梯子から離れる
				if (col.gameObject.tag == "Ladder") {
						ladderEnable = false;
						ladderFlag = false;
						rigidbody2D.isKinematic = false;
				}
		}

		//接触中の処理
		void OnTriggerStay2D(Collider2D col){

		}

		//スクロール制御--------------------------------------------------------------------------
		IEnumerator ScrollX(int vec){
				int i = 0;
				rigidbody2D.velocity = new Vector2 (0, 0);
				rigidbody2D.isKinematic = true;
				Vector3 v = transform.position;
				for(i = 0; i < (int)scrollTime; i++){
						v.x += vec * 32 / scrollTime;
						transform.position = v;
						yield return new WaitForSeconds(1/scrollTime);

				}
				ctrlFlag = true;
				rigidbody2D.isKinematic = false;

		}

		IEnumerator ScrollY(int vec){
				int i = 0;
				bool isKinematicTmp = rigidbody2D.isKinematic;
				rigidbody2D.velocity = new Vector2 (0, 0);
				rigidbody2D.isKinematic = true;
				Vector3 v = transform.position;
				for(i = 0; i < (int)scrollTime; i++){
						v.y += vec * 64 / scrollTime;
						transform.position = v;
						yield return new WaitForSeconds(1/scrollTime);

				}
				ctrlFlag = true;
				rigidbody2D.isKinematic = isKinematicTmp;

		}
	

		//登場ワープ制御------------------------------------------------------------
		//スタート地点を更新
		public void setRestartPos(Vector3 vec){
				Vector3 tmp = vec;
				tmp.y += Camera.screenSizeY;
				restartPos = tmp;
		}

		//ワープフラグセッタ
		public void setWarpFlag(bool flag){
				warpFlag = flag;
		}
		//ワープフラグゲッタ
		public bool getWarpFlag(){
				return warpFlag;
		}


		/// ////////////////////////////////////////////////////////////////////////////
		//ゲッタ
		public int getLife(){
				return lifePoint;
		}

		public bool getLadderFlag(){
				return ladderFlag;
		}

		public bool getJumpFlag(){
				return jumpFlag;
		}
				
		//セッタ
		public void setLife(int num){
				lifePoint = num;
		}

		public void setLadderFlag(bool flag){
				ladderFlag = flag;
		}

		public void setJumpFlag(bool flag){
				jumpFlag = flag;
		}
}
