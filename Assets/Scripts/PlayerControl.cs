using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerControl : MonoBehaviour {

    public float walkingVelocity = 4f;
    public int rupeeCount = 0;

	public int totalHealth = 3;
	public float currentHealth = 3;
	public int keyCount = 0;

    public static PlayerControl instance;

	public Sprite[] linkRunDown;
	public Sprite[] linkRunUp;
	public Sprite[] linkRunRight;
	public Sprite[] linkRunLeft;

	StateMachine animationStateMachine;
	StateMachine controlStateMachine;
	
	public EntityState currentState = EntityState.NORMAL;
	public Direction currentDirection = Direction.SOUTH;

	public GameObject selectedWeaponPrefab;

	// Use this for initialization
	void Start () {
        if (instance != null)
            Debug.LogError("Multiple Link objects detected");
        instance = this;

        // Launch Idle State.
        animationStateMachine = new StateMachine();
        animationStateMachine.ChangeState(new StateIdleWithSprite(this, 
                                                                  GetComponent<SpriteRenderer>(), 
                                                                  linkRunDown[0]));
	}

	void FixedUpdate () {
	    animationStateMachine.Update();

	    float horizontalInput = Input.GetAxis("Horizontal");
	    float verticalInput = Input.GetAxis("Vertical");

	    // Mod 1 gets decimal, to be used for Grid-based movement
	    Vector3 location = instance.transform.position;
	    float horizontalOffset = location.x % 1;
	    float verticalOffset = location.y % 1;


	    if (horizontalInput != 0) {
	        verticalInput = 0;
	        if (verticalOffset != 0 && verticalOffset != 0.5f) {
	            horizontalInput = 0;
	            location.y = gridPosition(verticalOffset, location.y, ref verticalInput);
	            transform.position = location;
	        }
	    } else if (verticalInput != 0) {
	        if (horizontalOffset != 0 && horizontalOffset != 0.5f) {
	            verticalInput = 0;
	            location.x = gridPosition(horizontalOffset, location.x, ref horizontalInput);
				transform.position = location;
	        }
	    }

	    if (!RoomTransitions.instance.cameraIsMoving) {
	        GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, verticalInput, 0) * walkingVelocity;
	    }
	}

    void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Rupee") {
			Destroy(coll.gameObject);
			rupeeCount++;
		} else if (coll.gameObject.tag == "Heart") {
			Destroy(coll.gameObject);
			if (currentHealth + 1 <= totalHealth) {
				currentHealth++;
			} else {
				currentHealth = totalHealth;
			}
		} else if (coll.gameObject.tag == "Key") {
			Destroy(coll.gameObject);
			keyCount++;
		}
    }

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "LockedDoor" && keyCount > 0)
			deleteDoors(coll.gameObject);
	}

	// CUSTOM FUNCTIONS

	float gridPosition(float offset, float location, ref float input) {
		float margin = 0.85f;
		if (offset <= 0.25f) {
			input = -1;
			if (offset < margin) {
				input = 0;
				location = Mathf.Floor(location);
			}
		}
		else if (offset >= 0.75f) {
			input = 1;
			if (Mathf.Abs(offset - 0.75f) < margin) {
				input = 0;
				location = Mathf.Ceil(location);
			}
		}
		else if (offset < 0.5f) {
			input = 1;
			if (Mathf.Abs(offset - 0.5f) < margin) {
				input = 0;
				location = Mathf.Floor(location) + 0.5f;
			}
		}
		else {
			input = -1;
			if (Mathf.Abs(offset - 0.5f) < margin) {
				input = 0;
				location = Mathf.Floor(location) + 0.5f;
			}
		}
		return location;
	}

	void deleteDoors(GameObject door) {
		Destroy(door.GetComponent<BoxCollider>());
		SpriteRenderer[] srChildren = door.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer sr in srChildren)
			sr.sortingOrder = 1;
		keyCount--;
	}
}
