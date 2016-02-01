using UnityEngine;
using System.Collections;

public class SpikeTrapComponent : MonoBehaviour {
    public Direction direction;
    Vector3 directionOffset;
    SpikeTrap spikeParent;
    int speed = 6;
    bool linkEntered = false;
    bool moving = false;
    bool returning = false;
	// Use this for initialization
	void Start() {
        spikeParent = transform.parent.GetComponent<SpikeTrap>();
        if (direction == Direction.NORTH)
            directionOffset = Vector3.up;
        else if (direction == Direction.EAST)
            directionOffset = Vector3.right;
        else if (direction == Direction.SOUTH)
            directionOffset = Vector3.down;
        else if (direction == Direction.WEST)
            directionOffset = Vector3.left;
    }
	
	// Update is called once per frame
	void Update() {
        if (linkEntered && !spikeParent.refractory) {
            spikeParent.refractory = true;
            moving = true;
            spikeParent.GetComponentInParent<Rigidbody>().velocity = speed * directionOffset;
        }

        if (moving) {
            if (((direction == Direction.NORTH || direction == Direction.SOUTH) && Vector3.Distance(transform.parent.position, spikeParent.defaultPosition) > 2.5f) ||
                ((direction == Direction.EAST || direction == Direction.WEST) && Vector3.Distance(transform.parent.position, spikeParent.defaultPosition) > 5)
                ) {
                returning = true;
                spikeParent.GetComponentInParent<Rigidbody>().velocity = -speed / 3 * directionOffset;
            }
            else if (returning && Vector3.Distance(spikeParent.defaultPosition, transform.parent.position) < 0.1f) {
                spikeParent.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
                transform.parent.position = spikeParent.defaultPosition;
                returning = false;
                moving = false;
                spikeParent.refractory = false;
            }
        }
	}

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Link")
            linkEntered = true;
    }

    void OnTriggerExit(Collider coll) {
        if (coll.gameObject.tag == "Link")
            linkEntered = false;
    }
}
