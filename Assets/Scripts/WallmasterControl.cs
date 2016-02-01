using UnityEngine;
using System.Collections;

public class WallmasterControl : EnemyControl {

    // Use this for initialization
    public override void OnStart() {
        walkingVelocity = 1.5f;
        currentHealth = 2;
        totalHealth = 2;
        
        // Launch Idle State.
        animationStateMachine = new StateMachine();
        animationStateMachine.ChangeState(new WallmasterSpriteAnimation(this, GetComponent<SpriteRenderer>(), Run, 6));
        controlStateMachine = new StateMachine();
        controlStateMachine.ChangeState(new StateWallmasterMovement(this));
    }

    void OnTriggerStay(Collider coll) {
        if (coll.gameObject.tag == "Link") {
            // Send Link and camera back to the beginning
            PlayerControl.instance.transform.position = transform.position;
            //PlayerControl.instance.transform.position = new Vector3(39.5f, 3.5f, 0);
        }
    }

}