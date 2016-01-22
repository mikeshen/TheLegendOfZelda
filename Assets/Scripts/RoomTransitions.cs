using UnityEngine;
using System.Collections;

public class RoomTransitions : MonoBehaviour {

	static public RoomTransitions instance;

	public Vector3 newCameraPos;
	public bool cameraIsMoving = false;

	public void ShiftCamera(Direction dir) {

		newCameraPos = gameObject.transform.position;

		switch (dir) {
		case Direction.NORTH:
			newCameraPos.y += 11f;
			break;
		case Direction.SOUTH:
			newCameraPos.y -= 11;
			break;
		case Direction.EAST:
			newCameraPos.x += ShowMapOnCamera.S.screenSize.x;
			break;
		case Direction.WEST:
			newCameraPos.x -= ShowMapOnCamera.S.screenSize.x;
			break;
		}
	}

	public void CheckLinkLocation() {
		Vector3 linkLocation = PlayerControl.instance.transform.position;
		float linkToCameraX = linkLocation.x - transform.position.x;
		float linkToCameraY = linkLocation.y - transform.position.y;

		if (linkToCameraX <= -8) {
			ShiftCamera(Direction.WEST);
		}
		else if (linkToCameraX >= 8) {
			ShiftCamera(Direction.EAST);
		}
		else if (linkToCameraY <= -7) {
			ShiftCamera(Direction.SOUTH);
		}
		else if (linkToCameraY >= 4) {
			ShiftCamera(Direction.NORTH);
		}
	}

	// Use this for initialization
	void Start () {
		instance = this;
		newCameraPos = transform.position;
	}

	// Fixed update used to make camera move consistently regardless of frame rate
	void FixedUpdate () {
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
