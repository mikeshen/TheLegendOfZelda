using UnityEngine;
using System.Collections;

public enum BatState {STOPPED, MOVING};
public class BatControl : EnemyControl {
    public BatState currentState = BatState.STOPPED;


    // Use this for initialization
    public override void OnStart() {
        walkingVelocity = 4;
        currentHealth = 1;
        totalHealth = 1;

        // Launch Idle State.
        animationStateMachine = new StateMachine();
        animationStateMachine.ChangeState(new BatSpriteAnimation(this, GetComponent<SpriteRenderer>(), Run, 6));
        controlStateMachine = new StateMachine();
        controlStateMachine.ChangeState(new StateBatStopped(this));
    }
}
