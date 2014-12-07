using UnityEngine;
using System.Collections;

public class ScrollMode : MonoBehaviour {
		public bool scrollXFlag = false;
		public bool scrollYFlag = false;
		public float screenNumX = 1;
		public float screenNumY = 1;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnTriggerEnter2D(Collider2D col){
				if (col.gameObject.tag == "Player") {
						GameObject camera = GameObject.Find ("Main Camera");
						Camera cameraCtrl = camera.GetComponent<Camera> ();
						cameraCtrl.scrollXFlag = scrollXFlag;
						cameraCtrl.scrollYFlag = scrollYFlag;
						cameraCtrl.screenNumX = screenNumX;
						cameraCtrl.screenNumY = screenNumY;
						cameraCtrl.scrollStartPos = new Vector2 (transform.position.x, transform.position.y);
				}
		}
}
