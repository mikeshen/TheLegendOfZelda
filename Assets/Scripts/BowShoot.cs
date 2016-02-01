using UnityEngine;
using System.Collections;

public class BowShoot : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        float roomCenterX = PlayerControl.instance.roomOffsetX * 16 + 7.5f;
        float roomCenterY = PlayerControl.instance.roomOffsetY * 11 + 5;
        Vector3 roomCenter = new Vector3(roomCenterX, roomCenterY, 0);
        Vector3 bowPosition = transform.position;

        float xDifference = Mathf.Abs(roomCenter.x - bowPosition.x);
        float yDifference = Mathf.Abs(roomCenter.y - bowPosition.y);

        if (xDifference >= 6.25f || yDifference >= 3.5f || GetComponent<Rigidbody>().velocity == Vector3.zero) {
            PlayerControl.instance.arrowShot = false;
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider coll) {
        PlayerControl pc = PlayerControl.instance;
        if (coll.gameObject.tag == "Rupee") {
            Destroy(coll.gameObject);
            pc.rupeeCount++;
        }
        else if (coll.gameObject.tag == "Heart") {
            Destroy(coll.gameObject);
            if (pc.currentHealth + 1 <= pc.totalHealth)
                pc.currentHealth++;
            else
                pc.currentHealth = pc.totalHealth;
        }
        else if (coll.gameObject.tag == "Key") {
            Destroy(coll.gameObject);
            pc.keyCount++;
        }
        else if (coll.gameObject.tag == "Enemy") {
            PlayerControl.instance.arrowShot = false;
            Destroy(gameObject);
        }
    }
}
