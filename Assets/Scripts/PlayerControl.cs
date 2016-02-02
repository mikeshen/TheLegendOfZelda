using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerControl : MonoBehaviour {

    public float walkingVelocity = 4;
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
    public bool warped = false;

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
	void Start() {
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

	void FixedUpdate() {
        checkBowRoomTransition();
        if (isInvincible)
            LinkDamageAnimation();
        if (RoomTransitions.instance.moveCamera)
            return;

        animationStateMachine.Update();
        controlStateMachine.Update();
        if (controlStateMachine.IsFinished())
            controlStateMachine.ChangeState(new StateLinkNormalMovement(this));
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Rupee") {
            Destroy(coll.gameObject);
            rupeeCount++;
        }
        else if (coll.gameObject.tag == "Rupee5") {
            Destroy(coll.gameObject);
            rupeeCount += 5;
        }
        else if (coll.gameObject.tag == "HeartUp") {
            Destroy(coll.gameObject);
            totalHealth++;
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
        else if (!isInvincible) {
            if (coll.gameObject.tag == "Enemy" && coll.gameObject.GetComponent<EnemyControl>().boomerangCooldown <= 0)
                takeDamage(0.5f, coll);
            else if (coll.gameObject.tag == "SpikeTrap")
                takeDamage(1, coll);

        }
    }

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "LockedDoor" && keyCount > 0)
			deleteDoors(coll.gameObject);
	}

	// CUSTOM FUNCTIONS

    public void takeDamage(float amount, Collider coll) {
        isInvincible = true;
        coolDown = 1;
        currentHealth -= amount;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        if (currentHealth <= 0) {
            sr.color = Color.black;
            Application.LoadLevel("Dungeon");
        }
		Vector3 knockbackDir = (instance.transform.position - coll.transform.position).normalized;
		if (Mathf.Abs(knockbackDir.x) > Mathf.Abs(knockbackDir.y)) {
			if (knockbackDir.x < 0)
				knockbackDir = Vector3.left;
			else
				knockbackDir = Vector3.right;
		}
		else {
			if (knockbackDir.y < 0)
				knockbackDir = Vector3.down;	
			else
				knockbackDir = Vector3.up;
		}

		instance.GetComponent<Rigidbody>().AddForce(knockbackDir * 9999);
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
            GameState.destroyOnScreen();
            inBowRoom = true;
            transform.position = new Vector3(83, 9f, 0);
            RoomTransitions.instance.transform.position = new Vector3(87.5f, 6.4f, -10);
            PlayerControl.instance.roomOffsetX = 5;
            PlayerControl.instance.roomOffsetY = 0;
            // spawn below
        }

        if ((transform.position.y >= 10f) && (transform.position.y <= 11) && transform.position.x == 83) {
            GameState.destroyOnScreen();
            transform.position = new Vector3(22, 58, 0);
            RoomTransitions.instance.transform.position = new Vector3(23.5f, 61.4f, -10);
            PlayerControl.instance.roomOffsetX = 1;
            PlayerControl.instance.roomOffsetY = 5;
            // spawn below
            inBowRoom = false;
        }

    }
}
