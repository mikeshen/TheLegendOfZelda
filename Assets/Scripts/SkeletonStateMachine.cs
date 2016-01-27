using UnityEngine;
using System.Collections;

public class SkeletonSpriteAnimation : State {
    EnemyControl ec;
    SpriteRenderer renderer;
    Sprite[] animation;
    int animation_length;
    float animation_progression;
    float animation_start_time;
    int fps;

    public SkeletonSpriteAnimation(EnemyControl ec, SpriteRenderer renderer, Sprite[] animation, int fps) {
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

public class StateSkeletonMovement : State {
    EnemyControl ec;

    public StateSkeletonMovement(EnemyControl ec) {
        this.ec = ec;
    }

    public override void OnUpdate(float time_delta_fraction) {

        Vector3 currentPosition = ec.transform.position;
        // Gets fractional offset from being a whole number
        float xOffset = currentPosition.x % 1;
        float yOffset = currentPosition.y % 1;

        float newXVelocity = ec.GetComponent<Rigidbody>().velocity.x;
        float newYVelocity = ec.GetComponent<Rigidbody>().velocity.y;

        float randomFloat = Random.Range(0f, 1f);
        
        if (xOffset == 0 && yOffset == 0) {
            if (randomFloat < 0.25f) {
                newXVelocity = -.01f;
            }
            else if (randomFloat < 0.5f) {
                newXVelocity = .01f;
            } 
            else if (randomFloat < 0.75f) {
                newYVelocity = .01f;
            }
            else {
                newYVelocity = -.01f;
            }
        }

        ec.GetComponent<Rigidbody>().velocity = new Vector3(-ec.walkingVelocity, 0, 0);


    }
}
