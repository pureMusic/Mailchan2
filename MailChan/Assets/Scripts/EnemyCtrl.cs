using UnityEngine;
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


		// Use this for initialization
		void Start () {
				nowSprite = gameObject.GetComponent<SpriteRenderer> ();
				nowSprite.sprite = defSprite;
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		//行動制御---------------------------------------------------------------------
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

		//ダメージ制御-------------------------------------------------------------
		public void Damage(int damage){
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

		//共通処理----------------------------------------------------------------------

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
