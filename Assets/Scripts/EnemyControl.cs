using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

    public float walkingVelocity = 4f;
    public float currentHealth = 2;
    public int totalHealth = 2;
    public Direction currentDirection;

    public Sprite[] SkeletonRun;

    StateMachine animationStateMachine;
    StateMachine controlStateMachine;

    // Use this for initialization
    void Start() {
        // Launch Idle State.
        animationStateMachine = new StateMachine();
        animationStateMachine.ChangeState(new SkeletonSpriteAnimation( this,
                                                                  GetComponent<SpriteRenderer>(),
                                                                  SkeletonRun, 6));

        controlStateMachine = new StateMachine();
        controlStateMachine.ChangeState(new StateSkeletonMovement(this));
    }

    void Update() {
        animationStateMachine.Update();
        controlStateMachine.Update();
    }

}