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
		Application.targetFrameRate = 60;
        if (instance != null)
            Debug.LogError("Multiple Link objects detected");
        instance = this;

        // Launch Idle State.
        animationStateMachine = new StateMachine();
        animationStateMachine.ChangeState(new StateIdleWithSprite(this, 
                                                                  GetComponent<SpriteRenderer>(), 
                                                                  linkRunDown[0]));

        controlStateMachine = new StateMachine();
        controlStateMachine.ChangeState(new StateLinkNormalMovement(this));
	}

	void Update () {
        animationStateMachine.Update();
        controlStateMachine.Update();
        if (controlStateMachine.IsFinished())
            controlStateMachine.ChangeState(new StateLinkNormalMovement(this));
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
		} else if (coll.gameObject.tag == "UnderBlock") {
			pushBlock(coll.gameObject);
		}
    }

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "LockedDoor" && keyCount > 0)
			deleteDoors(coll.gameObject);
	}

	// CUSTOM FUNCTIONS

	void deleteDoors(GameObject door) {
		Destroy(door.GetComponent<BoxCollider>());
		SpriteRenderer[] srChildren = door.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer sr in srChildren)
			sr.sortingOrder = 1;
		keyCount--;
	}

	void pushBlock(GameObject block) {
		block.GetComponent<BoxCollider>().enabled = false;
		GameObject OverBlock = GameObject.Find("023x038");
		OverBlock.GetComponent<SpriteRenderer>().sortingOrder = 1;
		OverBlock.AddComponent<Rigidbody>();
		Rigidbody rb = OverBlock.GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ;
		rb.useGravity = false;
		rb.freezeRotation = true;
		rb.mass = 0;
		rb.drag = 0;
		OverBlock.AddComponent<BlockPushing>();
	}
}
