using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerControl : MonoBehaviour {

    public float walkingVelocity = 4f;
    public int rupeeCount = 0;

	public float currentHealth = 3;
	public int totalHealth = 3;
	public int keyCount = 0;

    public bool swordThrown = false;
    public bool arrowShot = false;
    public bool boomerangThrown = false;
    public bool bowOrBoomerang = false;
    public bool isInvincible = false;

    public bool hasBoomerang = false;
    public bool hasBow = false;
    public bool inBowRoom = false;

    private float coolDown = 0;

    // Game State
    public int roomOffsetX;
    public int roomOffsetY;

    public static PlayerControl instance;

	public Sprite[] linkRunDown;
	public Sprite[] linkRunUp;
	public Sprite[] linkRunRight;
	public Sprite[] linkRunLeft;

	StateMachine animationStateMachine;
	StateMachine controlStateMachine;

	public EntityState currentState = EntityState.NORMAL;
	public Direction currentDirection = Direction.SOUTH;
    public int secondaryDirection = 0;

	public GameObject[] weapons;

	// Use this for initialization
	void Start () {
        roomOffsetX = 2;
        roomOffsetY = 0;
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

	void FixedUpdate () {
        if (isInvincible)
            LinkDamageAnimation();

        if (RoomTransitions.instance.cameraIsMoving)
            return;

        animationStateMachine.Update();
        controlStateMachine.Update();
        if (controlStateMachine.IsFinished())
            controlStateMachine.ChangeState(new StateLinkNormalMovement(this));
        checkBowRoomTransition();
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Rupee") {
            Destroy(coll.gameObject);
            rupeeCount++;
        }
        else if (coll.gameObject.tag == "Heart") {
            Destroy(coll.gameObject);
            if (currentHealth + 1 <= totalHealth)
                currentHealth++;
            else
                currentHealth = totalHealth;
        }
        else if (coll.gameObject.tag == "Key") {
            Destroy(coll.gameObject);
            keyCount++;
        }
        else if (coll.gameObject.tag == "UnderBlock")
            pushBlock(coll.gameObject);
        else if (coll.gameObject.tag == "Bow") {
            Destroy(coll.gameObject);
            hasBow = true;
        }
        else if (coll.gameObject.tag == "Boomerang") {
            if (!hasBow)
                bowOrBoomerang = true;
            Destroy(coll.gameObject);
            hasBoomerang = true;
        }
        else if (coll.gameObject.tag == "Enemy" && !isInvincible)
            takeDamage(1);
    }

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "LockedDoor" && keyCount > 0)
			deleteDoors(coll.gameObject);
	}

	// CUSTOM FUNCTIONS

    public void takeDamage(int amount) {
        isInvincible = true;
        coolDown = 1f;
        currentHealth -= amount;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
    }

	void deleteDoors(GameObject door) {
		Destroy(door.GetComponent<BoxCollider>());
		SpriteRenderer[] srChildren = door.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer sr in srChildren)
			sr.sortingOrder = 1;
		keyCount--;
	}

	void pushBlock(GameObject block) {
        block.GetComponent<BoxCollider>().enabled = false;
        GameObject OverBlock = null;
        if (GameObject.Find("023x038") != null)
            OverBlock = GameObject.Find("023x038");
        else if (GameObject.Find("022x060") != null)
            OverBlock = GameObject.Find("022x060");
        OverBlock.GetComponent<SpriteRenderer>().sortingOrder = 1;
        OverBlock.AddComponent<Rigidbody>();
        Rigidbody rb = OverBlock.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.mass = 12;
        rb.drag = 0;
        OverBlock.AddComponent<BlockPush>();
    }

    void LinkDamageAnimation() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr.color == Color.blue)
            sr.color = Color.red;
        else if (sr.color == Color.red)
            sr.color = Color.white;
        else
            sr.color = Color.blue;
        coolDown -= Time.deltaTime;
        if (coolDown <= 0f) {
            sr.color = Color.white;
            isInvincible = false;
            coolDown = 0f;
        }
    }
    void checkBowRoomTransition() {
        if ((transform.position.x >= 24) && (transform.position.x <= 25) && transform.position.y == 60) {
            inBowRoom = true;
            Vector3 linkPos = transform.position;
            Vector3 newCameraPos = RoomTransitions.instance.transform.position;
            linkPos.x = 19;
            linkPos.y = 79;
            newCameraPos.x = 23.51f;
            newCameraPos.y = 77.3f;
            transform.position = linkPos;
            RoomTransitions.instance.transform.position = newCameraPos;
        }

        if ((transform.position.y >= 80) && (transform.position.y <= 81) && transform.position.x == 19) {
            Vector3 linkPos = transform.position;
            Vector3 newCameraPos = RoomTransitions.instance.transform.position;
            linkPos.x = 22;
            linkPos.y = 58;
            newCameraPos.x = 23.51f;
            newCameraPos.y = 61.41f;
            transform.position = linkPos;
            RoomTransitions.instance.transform.position = newCameraPos;
            inBowRoom = false;
        }
    }
}
