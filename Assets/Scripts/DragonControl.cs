using UnityEngine;
using System.Collections;

public class DragonControl : EnemyControl {
    public static DragonControl instance;
    public GameObject fireballPrefab; 

	// Use this for initialization
	public override void OnStart() {
		walkingVelocity = 2;
		currentHealth = 6;
		totalHealth = 6;

		// Launch Idle State.
		animationStateMachine = new StateMachine();
		animationStateMachine.ChangeState(new DragonSpriteAnimation(GetComponent<SpriteRenderer>(), Run, 6));
		controlStateMachine = new StateMachine();
		controlStateMachine.ChangeState(new StateDragonMovement(this));
	}
}