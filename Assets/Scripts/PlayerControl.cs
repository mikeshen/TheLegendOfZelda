using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerControl : MonoBehaviour {

    public float walkingVelocity = 4f;
    public int rupeeCount = 0;

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
	
	// Update is called once per frame
	void Update () {
        animationStateMachine.Update();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0f)
            verticalInput = 0f;
        GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, verticalInput, 0) * walkingVelocity;
	}

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Rupee") {
            Destroy(coll.gameObject);
            rupeeCount++;
        }
        else if (coll.gameObject.tag == "Heart") {
        }
    }
}
