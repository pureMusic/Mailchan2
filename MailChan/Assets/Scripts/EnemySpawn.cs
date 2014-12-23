using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {
		public GameObject EnemyPrefab;		//生成する敵のプレハブ


		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
				
		//フレームイン処理
		void OnBecameVisible(){
				GameObject obj;

				if (this.transform.childCount == 0) {
						obj = Instantiate (EnemyPrefab, this.transform.position, this.transform.rotation) as GameObject;
						obj.transform.parent = this.transform;
				}
		}
}
