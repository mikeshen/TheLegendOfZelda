using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomerIdleWithSprite : State {
    BoomerControl ec;
    SpriteRenderer renderer;
    Sprite sprite;
    float animation_start_time;
    int fps;

    public BoomerIdleWithSprite(BoomerControl ec, SpriteRenderer renderer, Sprite sprite, int fps) {
        this.ec = ec;
        this.renderer = renderer;
        this.sprite = sprite;
        this.fps = fps;
    }

    public override void OnStart() {
        renderer.sprite = sprite;
        animation_start_time = Time.time;
    }

	public override void OnUpdate(float time_delta_fraction) {
        Sprite[] run = ec.RunDown;
        if (ec.currentDirection == Direction.NORTH)
            run = ec.RunUp;
        else if (ec.currentDirection == Direction.SOUTH)
            run = ec.RunDown;
        else if (ec.currentDirection == Direction.EAST)
            run = ec.RunRight;
        else if (ec.currentDirection == Direction.WEST)
            run = ec.RunLeft;

        // Modulus is necessary so we don't overshoot the length of the animation.
        int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % 2);
        renderer.sprite = run[current_frame_index];
    }
}

public class StateBoomerMovement : State {
    BoomerControl ec;
    GameObject boomerang;
    public Vector3 targetLoc;
    public Vector3 velocity;

    public StateBoomerMovement(BoomerControl ec, GameObject boomerang) {
        this.ec = ec;
        this.boomerang = boomerang;
        targetLoc = ec.transform.position;
        velocity = Vector3.zero;
    }

    public override void OnUpdate(float time_delta_fraction) {
        if (!ec.boomerangThrown) {
            if (Random.Range(0f, 1f) > 0.994f) {
                ec.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ec.boomerangThrown = true;

                // Spawn the weapon object
                GameObject weaponInstance = MonoBehaviour.Instantiate(boomerang, ec.transform.position, Quaternion.identity) as GameObject;
                weaponInstance.GetComponent<EnemyBoomerang>().ec = this.ec;
                return;
            }

            if (Vector3.Distance(targetLoc, ec.transform.position) <= .1f) {
                float fixedX = Mathf.Round(ec.transform.position.x);
                float fixedY = Mathf.Round(ec.transform.position.y);
                Vector3 fixedPos = new Vector3(fixedX, fixedY, 0);
                ec.transform.position = fixedPos;

                velocity = Utilities.nextDirection(ec, ec.transform.position);
                targetLoc = ec.transform.position + velocity;
                ec.currentDirection = Utilities.findDirection(velocity);
            }
            ec.GetComponent<Rigidbody>().velocity = velocity * ec.walkingVelocity;
        }
    }
}
