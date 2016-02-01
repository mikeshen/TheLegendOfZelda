using UnityEngine;
using System.Collections;

public class BoomerangThrow : MonoBehaviour {
    Vector3 directionOffset = Vector3.zero;
    bool returning = false;
    int frameCount = 0;

    // Use this for initialization
    void Start() {
        int secondaryDirection = PlayerControl.instance.secondaryDirection;

        if (PlayerControl.instance.currentDirection == Direction.NORTH)
            directionOffset = Vector3.up;
        else if (PlayerControl.instance.currentDirection == Direction.EAST)
            directionOffset = new Vector3(1, secondaryDirection, 0);
        else if (PlayerControl.instance.currentDirection == Direction.SOUTH)
            directionOffset = Vector3.down;
        else if (PlayerControl.instance.currentDirection == Direction.WEST)
            directionOffset = new Vector3(-1, secondaryDirection, 0);

        directionOffset = directionOffset.normalized * 5.5f + transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!returning) {
            frameCount++;
            transform.Rotate(Vector3.forward, 1000 * Time.fixedDeltaTime);
            transform.position = Vector3.Slerp(transform.position, directionOffset, 0.1f);

            float roomCenterX = PlayerControl.instance.roomOffsetX * 16 + 7.5f;
            float roomCenterY = PlayerControl.instance.roomOffsetY * 11 + 5;
            Vector3 roomCenter = new Vector3(roomCenterX, roomCenterY, 0);
            Vector3 boomerangPosition = transform.position;

            float xDifference = Mathf.Abs(roomCenter.x - boomerangPosition.x);
            float yDifference = Mathf.Abs(roomCenter.y - boomerangPosition.y);
            float distance = Vector3.Distance(transform.position, directionOffset);

            if (distance < 1)
                returning = true;
            else if (xDifference >= 6.25f || yDifference >= 4f) {
                if (frameCount == 1) {
                    PlayerControl.instance.boomerangThrown = false;
                    Destroy(gameObject);
                }
                else
                    returning = true;
            }

        }
        else {
            transform.Rotate(Vector3.forward, 1000 * Time.deltaTime);
            Vector3 direction = (PlayerControl.instance.transform.position - transform.position).normalized;
            GetComponent<Rigidbody>().velocity = direction * 6.5f;
        }

    }

    void OnTriggerEnter(Collider coll) {
        // TODO: stun enemy here 
        if (coll.gameObject.tag == "Enemy") {
            returning = true;
        }
        else if (coll.gameObject.tag == "Link" && returning) {
            PlayerControl.instance.boomerangThrown = false;
            Destroy(gameObject);
        }
    }
}
