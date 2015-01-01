using UnityEngine;
using System.Collections;

public class ConveryorBlock : MonoBehaviour {
		public int delaySpeed = 6;
		public Sprite[] sprite;
		private SpriteRenderer rendererCtrl;

		// Use this for initialization
		void Start () {
				if (transform.childCount == 0) {
						rendererCtrl = this.gameObject.GetComponent<SpriteRenderer> ();
						delaySpeed = transform.parent.gameObject.GetComponent<ConveryorBlock> ().delaySpeed;
						StartCoroutine ("Anime");
				}
		}
		
		// Update is called once per frame
		void Update () {
				if (transform.parent == null) {
						foreach (Transform child in transform.FindChild("List")) {
								if (child.tag == "Player") {
										Vector3 pos = child.position;
										pos.x += (1f / delaySpeed) * (transform.rotation.y == 0 ? 1 : -1);
										child.position = pos;
								} else {
										Vector3 pos = child.GetChild(0).position;
										pos.x += (1f / delaySpeed) * (transform.rotation.y == 0 ? 1 : -1);
										child.GetChild(0).position = pos;
								}
						}
				}
		}

		//アニメーション
		IEnumerator Anime(){
				while (true) {
						int num = (int)(SceneCtrl.gameCounter / delaySpeed) % 3;
						rendererCtrl.sprite = sprite [num];
						yield return null;
				}
		}

		//接触中の処理
		void OnTriggerEnter2D(Collider2D col){
				print (col.gameObject.tag);
				print (transform.parent);

				if (transform.parent == null && col.gameObject.tag == "Player") {
						col.gameObject.transform.parent = transform.FindChild ("List").transform;
				}else if(transform.parent == null && col.gameObject.tag == "Enemy") {
						col.gameObject.transform.parent.parent = transform.FindChild ("List").transform;
				}
		}
		void OnTriggerExit2D(Collider2D col){
				if (transform.parent == null && col.gameObject.tag == "Player") {
						col.gameObject.transform.parent = null;
				}else if(transform.parent == null && col.gameObject.tag == "Enemy") {
						col.gameObject.transform.parent.parent = null;
				}
		}

}
