﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlobSpriteAnimation : State {
    SpriteRenderer renderer;
    Sprite[] animation;
    int animation_length;
    float animation_start_time;
    int fps;

    public BlobSpriteAnimation(SpriteRenderer renderer, Sprite[] animation, int fps) {
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

public class StateBlobMovement : State {
    EnemyControl ec;
    public Vector3 targetLoc;
    public Vector3 velocity;
    public float timePassed = 0;
    public float pauseTime = Random.Range(0.5f, 1);

    public StateBlobMovement(EnemyControl ec) {
        this.ec = ec;
        targetLoc = ec.transform.position;
    }

    public override void OnUpdate(float time_delta_fraction) {

        timePassed += Time.deltaTime;
        Vector3 currentPosition = ec.transform.position;
        velocity = (targetLoc - currentPosition).normalized;

        if ((Vector3.Distance(targetLoc, currentPosition)) <= 0.1f && (timePassed > pauseTime)) {
            timePassed = 0;
            pauseTime = Random.Range(0.5f, 1);
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
