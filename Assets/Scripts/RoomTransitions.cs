using UnityEngine;
using System.Collections;

public class RoomTransitions : MonoBehaviour {

	static public RoomTransitions instance;

	public Vector3 newCameraPos;
	public bool cameraIsMoving = false;

	void CheckLinkLocation() {
		Vector3 linkLocation = PlayerControl.instance.transform.position;
		float linkToCameraX = linkLocation.x - transform.position.x;
		float linkToCameraY = linkLocation.y - transform.position.y;

		if (linkToCameraX <= -8) {
            newCameraPos = gameObject.transform.position;
			newCameraPos.x -= ShowMapOnCamera.S.screenSize.x;
            PlayerControl.instance.roomOffsetX--;
		}
		else if (linkToCameraX >= 8) {
            newCameraPos = gameObject.transform.position;
			newCameraPos.x += ShowMapOnCamera.S.screenSize.x;
            PlayerControl.instance.roomOffsetX++;
		}
		else if (linkToCameraY <= -7) {
            newCameraPos = gameObject.transform.position;
			newCameraPos.y -= 11;
            PlayerControl.instance.roomOffsetX++;
		}
		else if (linkToCameraY >= 4) {
            newCameraPos = gameObject.transform.position;
			newCameraPos.y += 11f;
            PlayerControl.instance.roomOffsetY++;
		}
	}

	// Use this for initialization
	void Start () {
		instance = this;
		newCameraPos = transform.position;
	}

	// Fixed update used to make camera move consistently regardless of frame rate
	void FixedUpdate() {
        if (PlayerControl.instance.inBowRoom)
            return;

		CheckLinkLocation();
		transform.position = Vector3.MoveTowards (transform.position, newCameraPos, 0.1f);
		if (transform.position == newCameraPos) {
			cameraIsMoving = false;
		} 
		else {
			cameraIsMoving = true;
			// Stops Link from moving
			PlayerControl.instance.GetComponent<Rigidbody>().velocity = new Vector3();
		}
	}
}
