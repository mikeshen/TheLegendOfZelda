using UnityEngine;
using System.Collections;

public class DestroyTiles : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Link") {
			GameObject[] g = GameObject.FindGameObjectsWithTag("Tile");
			for (int i = 0; i < g.Length; i++) {
				if (g[i].GetComponent<Rigidbody>() != null) {
					g[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
					g[i].GetComponent<Rigidbody>().useGravity = true;
					g[i].GetComponent<Rigidbody>().isKinematic = false;
				}
				else {
					g[i].AddComponent<Rigidbody>();
				}
			}
				
		}
	}
}
