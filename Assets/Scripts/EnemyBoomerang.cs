using UnityEngine;
using System.Collections;

public class EnemyBoomerang : MonoBehaviour {
    public BoomerControl ec;
    Vector3 returnPosition;
    Vector3 directionOffset;
    bool returning = false;
    int frameCount = 0;

    // Use this for initialization
    void Start() {
        returnPosition = transform.position;
        if (ec.currentDirection == Direction.NORTH)
            directionOffset = new Vector3(0, 1, 0);
        else if (ec.currentDirection == Direction.EAST)
            directionOffset = new Vector3(1, 0, 0);
        else if (ec.currentDirection == Direction.SOUTH)
            directionOffset = new Vector3(0, -1, 0);
        else if (ec.currentDirection == Direction.WEST)
            directionOffset = new Vector3(-1, 0, 0);

        directionOffset = directionOffset * 5.5f + returnPosition;
    }

    // Update is called once per frame
    void Update() {
        if (!returning) {
            frameCount++;
            transform.Rotate(Vector3.forward, 1000 * Time.deltaTime);
            transform.position = Vector3.Slerp(transform.position, directionOffset, 0.1f);

            float roomCenterX = PlayerControl.instance.roomOffsetX * 16 + 7.5f;
            float roomCenterY = PlayerControl.instance.roomOffsetY * 11 + 5;
            Vector3 roomCenter = new Vector3(roomCenterX, roomCenterY, 0);
            Vector3 boomerangPosition = transform.position;

            float xDifference = Mathf.Abs(roomCenter.x - boomerangPosition.x);
            float yDifference = Mathf.Abs(roomCenter.y - boomerangPosition.y);
            float distance = Vector3.Distance(transform.position, directionOffset);

            if (distance < 1) {
                setBoomerangReturn();
            }
            else if (xDifference >= 6f || yDifference >= 4f) {
                if (frameCount == 1) {
                    ec.boomerangThrown = false;
                    Destroy(gameObject);
                }
                else 
                    setBoomerangReturn();
            }

        }
        else if (Vector3.Distance(returnPosition, transform.position) < 0.1) {
            ec.boomerangThrown = false;
            Destroy(gameObject);
        }
        else
            transform.Rotate(Vector3.forward, 1000 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Link" && ! PlayerControl.instance.isInvincible) {
            PlayerControl.instance.takeDamage(1);
            setBoomerangReturn();
        }
    }

    void setBoomerangReturn() {
        returning = true;
        transform.Rotate(Vector3.forward, 1000 * Time.deltaTime);
        Vector3 direction = (returnPosition - transform.position).normalized;
        GetComponent<Rigidbody>().velocity = direction * 6.5f;
    }
}
