using UnityEngine;
using System.Collections;

public class SkeletonControl : EnemyControl {

    // Use this for initialization
    public override void OnStart() {
        walkingVelocity = 2;
        currentHealth = 2;
        totalHealth = 2;

        // Launch Idle State.
        animationStateMachine = new StateMachine();
        animationStateMachine.ChangeState(new SkeletonSpriteAnimation(GetComponent<SpriteRenderer>(), Run, 6));
        controlStateMachine = new StateMachine();
        controlStateMachine.ChangeState(new StateSkeletonMovement(this));
    }
}
