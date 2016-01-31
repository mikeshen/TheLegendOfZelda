using UnityEngine;
using System.Collections;

public class BoomerControl : EnemyControl {
    public Sprite[] RunDown;
    public Sprite[] RunUp;
    public Sprite[] RunRight;
    public Sprite[] RunLeft;
    public GameObject boomerang;
    public bool boomerangThrown;

    // Use this for initialization
    public override void OnStart() {
        walkingVelocity = 2.5f;
        currentHealth = 3;
        totalHealth = 3;
        boomerangThrown = false;

        // choose starting animation position
        Sprite startSprite = Run[1];
        if (currentDirection == Direction.NORTH)
            startSprite = Run[3];
        else if (currentDirection == Direction.SOUTH)
            startSprite = Run[1];
        else if (currentDirection == Direction.EAST)
            startSprite = Run[0];
        else if (currentDirection == Direction.WEST)
            startSprite = Run[2];

        // Launch Idle State.
        animationStateMachine = new StateMachine();
        animationStateMachine.ChangeState(new BoomerIdleWithSprite(this, GetComponent<SpriteRenderer>(), startSprite, 6));
        controlStateMachine = new StateMachine();
        controlStateMachine.ChangeState(new StateBoomerMovement(this, boomerang));
    }
}
