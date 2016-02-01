using UnityEngine;
using System.Collections;

public class BlockPush : MonoBehaviour {

	Vector3 startPosition;
	Vector3 currentPosition;
	bool destroyed;

	// Use this for initialization
	void Start() {
		startPosition = transform.position;
		destroyed = false;
	}

	// Update is called once per frame
	void Update() {
		if (RoomTransitions.instance.moveCamera) {
			GameObject.Find("UnderBlock1").GetComponent<BoxCollider>().enabled = true;
			GameObject.Find("UnderBlock2").GetComponent<BoxCollider>().enabled = true;
		}

		if (!destroyed) {
			currentPosition = transform.position;
			Vector3 distance = currentPosition - startPosition;
			if (Mathf.Abs(distance.x) >= 1 || Mathf.Abs(distance.y) >= 1) {
				destroyed = true;
				Destroy(this.GetComponent<Rigidbody>());
				// To only destroy the secret door when Link is actually near it
				GameObject SecretDoor = GameObject.FindGameObjectWithTag("SecretDoor");
				if ((PlayerControl.instance.transform.position - SecretDoor.transform.position).magnitude < 20) {
					Destroy(SecretDoor.GetComponent<BoxCollider>());
					SpriteRenderer[] srChildren = SecretDoor.GetComponentsInChildren<SpriteRenderer>();
					foreach (SpriteRenderer sr in srChildren)
						sr.sortingOrder = 1;
				}
			}
		}
	}
}
