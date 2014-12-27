using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

		public ItemSpawn.ItemType type;				//アイテムの種類
		public bool deletable = true;				//true：一定時間で消滅
		private int counter = 8;					//消滅までの時間
		private int flashTime = 2;					//点滅時間

		// Use this for initialization
		void Start () {
				//フレームを秒に換算
				counter = counter * 60;
				flashTime = flashTime * 60;
		}
		
		// Update is called once per frame
		void Update () {
				if (deletable && counter > 0) {
						counter--;
						//消滅
						if (counter == 0) {
								Destroy (this.gameObject);
						} 
						//消滅前に点滅
						else if (counter < flashTime) {
								this.transform.renderer.enabled = ((counter/2) % 2 == 0 ? false : true);
						}
				}
		}

		public void Effect(PlayerCtrl p){
				switch (type) {

				case ItemSpawn.ItemType.RECOVERY_BIG:	//回復大
						p.setLife (p.getLife () + 7);
						p.LifeBarUpdate ();
						break;

				case ItemSpawn.ItemType.RECOVERY_SMALL:	//回復小
						p.setLife (p.getLife () + 3);
						p.LifeBarUpdate ();
						break;
				}
				Destroy (this.gameObject);
		}

		//接触時の処理
		void OnTriggerEnter2D(Collider2D col){
				//アイテム取得
				if (col.gameObject.tag == "Player") {
						Effect(col.gameObject.GetComponent<PlayerCtrl>());
				}

		}
}
