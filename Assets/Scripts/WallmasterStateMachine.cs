using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallmasterSpriteAnimation : State {
    EnemyControl ec;
    SpriteRenderer renderer;
    Sprite[] animation;
    int animation_length;
    float animation_start_time;
    int fps;

    public WallmasterSpriteAnimation(EnemyControl ec, SpriteRenderer renderer, Sprite[] animation, int fps) {
        this.ec = ec;
        this.renderer = renderer;
        this.animation = animation;
        this.animation_length = animation.Length;
        this.fps = fps;

        if (this.animation_length <= 0)
            Debug.LogError("Empty animation submitted to state machine!");
    }

    public override void OnStart() {
        animation_start_time = Time.time;
    }

    public override void OnUpdate(float time_delta_fraction) {
        if (this.animation_length <= 0) {
            Debug.LogError("Empty animation submitted to state machine!");
            return;
        }

        // Modulus is necessary so we don't overshoot the length of the animation.
        int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation_length);
        renderer.sprite = animation[current_frame_index];
    }
}

public class StateWallmasterMovement : State {
    EnemyControl ec;
    public Vector3 targetLoc;
    public Vector3 velocity;

    public StateWallmasterMovement(EnemyControl ec) {
        this.ec = ec;
		// For X triggers
		if (ec.transform.position.y == 41 || ec.transform.position.y == 35) {
			targetLoc = ec.transform.position + (3 * Vector3.left);
			ec.GetComponent<Rigidbody>().velocity = Vector3.left * ec.walkingVelocity;
		}
		// For Y triggers
		else {
			targetLoc = ec.transform.position + (3 * Vector3.down);
			ec.GetComponent<Rigidbody>().velocity = Vector3.down * ec.walkingVelocity;
		}
    }

    public override void OnUpdate(float time_delta_fraction) {
        Vector3 currentPosition = ec.transform.position;

		// For X triggers
		if (ec.transform.position.y == 41 && PlayerControl.instance.transform.position.y != 38 || ec.transform.position.y == 35) {
			ec.GetComponent<Rigidbody>().velocity = Vector3.left * ec.walkingVelocity;
		}

		// For Y triggers
		else {
			ec.GetComponent<Rigidbody>().velocity = Vector3.down * ec.walkingVelocity;
		}

        if (Vector3.Distance(targetLoc, currentPosition) <= 0.1f) {
			GameObject.Destroy(ec.transform.root.gameObject);
			if (ec.transform.position == PlayerControl.instance.transform.position)
				Utilities.warpLinkToStart();
        }
    }
}
