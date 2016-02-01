using UnityEngine;
using System.Collections;

public class BlobControl : EnemyControl {

    // Use this for initialization
    public override void OnStart() {
        walkingVelocity = 2.5f;
        currentHealth = 1;
        totalHealth = 1;

        // Launch Idle State.
        animationStateMachine = new StateMachine();
        animationStateMachine.ChangeState(new BlobSpriteAnimation(GetComponent<SpriteRenderer>(), Run, 6));
        controlStateMachine = new StateMachine();
        controlStateMachine.ChangeState(new StateBlobMovement(this));
    }
}
