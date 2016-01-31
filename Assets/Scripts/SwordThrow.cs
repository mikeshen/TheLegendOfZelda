using UnityEngine;
using System.Collections;

public class SwordThrow : MonoBehaviour {

    // Use this for initialization
    void Start() {}

    // Update is called once per frame
    void Update() {
        float roomCenterX = PlayerControl.instance.roomOffsetX * 16 + 7.5f;
        float roomCenterY = PlayerControl.instance.roomOffsetY * 11 + 5f;
        Vector3 roomCenter = new Vector3(roomCenterX, roomCenterY, 0);
        Vector3 magicSwordPosition = transform.position;

        float xDifference = Mathf.Abs(roomCenter.x - magicSwordPosition.x);
        float yDifference = Mathf.Abs(roomCenter.y - magicSwordPosition.y);

        if (xDifference >= 6.25f || yDifference >= 3.25f || GetComponent<Rigidbody>().velocity == Vector3.zero) {
            PlayerControl.instance.swordThrown = false;
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag != "Link") {
            PlayerControl.instance.swordThrown = false;
            Destroy(gameObject);
        }
    }
}
