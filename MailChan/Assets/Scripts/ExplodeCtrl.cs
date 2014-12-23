using UnityEngine;
using System.Collections;

public class ExplodeCtrl : MonoBehaviour {
		public float speed = 0;		//移動速度
		public float angle = 0;		//角度
		private float radian = 0;	

		// Use this for initialization
		void Start () {
				radian = Mathf.PI * angle / 180;
		}
		
		// Update is called once per frame
		void Update () {
				transform.position = new Vector3 (transform.position.x + speed * Mathf.Cos (radian)
												, transform.position.y + speed * Mathf.Sin (radian)
												, transform.position.z);
		}
}
