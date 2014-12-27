using UnityEngine;
using System.Collections;

public class ItemSpawn : MonoBehaviour {
		public enum ItemType
		{
				RECOVERY_BIG = 0,
				RECOVERY_SMALL,
		};
		private int maxInt = 256;
		public GameObject[] items;

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void CreateItem(){
				int itemNum = Random.Range (0, maxInt);
				GameObject item;
				if (itemNum < maxInt / 4 / 4) {
						item = Instantiate (items [(int)ItemType.RECOVERY_BIG], this.transform.position, this.transform.rotation) as GameObject;
				} else if (itemNum < maxInt / 4) {
						item = Instantiate (items [(int)ItemType.RECOVERY_SMALL], this.transform.position, this.transform.rotation) as GameObject;
				}
		}
}
