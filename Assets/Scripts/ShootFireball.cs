using UnityEngine;
using System.Collections;

public class ShootFireball : MonoBehaviour {

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void FixedUpdate() {
        float roomCenterX = PlayerControl.instance.roomOffsetX * 16 + 7.5f;
        float roomCenterY = PlayerControl.instance.roomOffsetY * 11 + 5;
        Vector3 roomCenter = new Vector3(roomCenterX, roomCenterY, 0);
        Vector3 fireballLoc = transform.position;

        float xDifference = Mathf.Abs(roomCenter.x - fireballLoc.x);
        float yDifference = Mathf.Abs(roomCenter.y - fireballLoc.y);

        if (xDifference >= 6.25f || yDifference >= 3.5f || GetComponent<Rigidbody>().velocity == Vector3.zero) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Link" && !PlayerControl.instance.isInvincible)
            PlayerControl.instance.takeDamage(0.5f, coll);
    }
}
