using UnityEngine;
using System.Collections;

public class DamageBlock : MonoBehaviour {
		public int damagePoint = 0;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		//接触時の処理
		void OnTriggerEnter2D(Collider2D col){
				//ダメージ処理
				if (col.gameObject.tag == "Player") {
						PlayerCtrl player = col.GetComponent<PlayerCtrl>();
						player.Damage (damagePoint);
				}

		}
}
