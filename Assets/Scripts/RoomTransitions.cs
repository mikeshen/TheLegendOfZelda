using UnityEngine;
using System.Collections;

public class RoomTransitions : MonoBehaviour {

	static public RoomTransitions instance;

	public Vector3 newCameraPos;
    public bool moveCamera = false;

	void CheckLinkLocation() {
		Vector3 linkLocation = PlayerControl.instance.transform.position;
		float linkToCameraX = linkLocation.x - transform.position.x;
		float linkToCameraY = linkLocation.y - transform.position.y;

		if (linkToCameraX <= -8) {
            moveCamera = true;
            newCameraPos = gameObject.transform.position;
			newCameraPos.x -= ShowMapOnCamera.S.screenSize.x;
            GameState.destroyOnScreen();
            PlayerControl.instance.roomOffsetX--;
		}
		else if (linkToCameraX >= 8) {
            moveCamera = true;
            newCameraPos = gameObject.transform.position;
			newCameraPos.x += ShowMapOnCamera.S.screenSize.x;
            GameState.destroyOnScreen();
            PlayerControl.instance.roomOffsetX++;
		}
		else if (linkToCameraY <= -7) {
            moveCamera = true;
            newCameraPos = gameObject.transform.position;
			newCameraPos.y -= 11;
            GameState.destroyOnScreen();
            PlayerControl.instance.roomOffsetY--;
		}
		else if (linkToCameraY >= 4) {
            moveCamera = true;
            newCameraPos = gameObject.transform.position;
			newCameraPos.y += 11f;
            GameState.destroyOnScreen();
            PlayerControl.instance.roomOffsetY++;
		}

        if (moveCamera) 
            GameState.instance.spawnRoom(PlayerControl.instance.roomOffsetX, PlayerControl.instance.roomOffsetY);

	}

	// Use this for initialization
	void Start() {
		instance = this;
		newCameraPos = transform.position;
	}

	// Fixed update used to make camera move consistently regardless of frame rate
	void FixedUpdate() {
        if (PlayerControl.instance.inBowRoom)
            return;

		if (moveCamera) {
			// Stops Link from moving
            transform.position = Vector3.MoveTowards(transform.position, newCameraPos, 0.1f);
			PlayerControl.instance.GetComponent<Rigidbody>().velocity = new Vector3();
            if (transform.position == newCameraPos)
                moveCamera = false;
		}
        else if (!PlayerControl.instance.inBowRoom)
            CheckLinkLocation();
	}
}
