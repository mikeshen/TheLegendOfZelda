using UnityEngine;
using System.Collections;

public class BlockPushing : MonoBehaviour {

	Vector3 startPosition;
	Vector3 currentPosition;
	bool destroyed;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		destroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!destroyed) {
			currentPosition = transform.position;
			Vector3 distance = currentPosition - startPosition;
			if (Mathf.Abs(distance.x) >= 1 || Mathf.Abs(distance.y) >= 1) {
				destroyed = true;
				Destroy(this.GetComponent<Rigidbody>());
				GameObject SecretDoor = GameObject.FindGameObjectWithTag("SecretDoor");
				Destroy(SecretDoor.GetComponent<BoxCollider>());
				SpriteRenderer[] srChildren = SecretDoor.GetComponentsInChildren<SpriteRenderer>();
				foreach (SpriteRenderer sr in srChildren)
					sr.sortingOrder = 1;
			}
		}
	}
}
