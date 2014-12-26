using UnityEngine;
using System.Collections;

public class LadderTop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		//接触中の処理
		void OnTriggerStay2D(Collider2D col){
				if (col.gameObject.tag == "Player") {
						PlayerCtrl p = col.gameObject.GetComponent<PlayerCtrl> ();
						if ((Input.GetKey(KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) && !p.getLadderFlag()) {
								p.setLadderFlag (true);
								p.transform.position = new Vector3 (this.transform.position.x
																, p.transform.position.y - 16
																, p.transform.position.z);;
								p.transform.rigidbody2D.velocity = new Vector2 (0, 0);
								p.transform.rigidbody2D.isKinematic = true;

						}
				}
		}
}
