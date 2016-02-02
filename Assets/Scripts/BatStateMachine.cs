using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BatSpriteAnimation : State {
    BatControl ec;
    SpriteRenderer renderer;
    Sprite[] animation;
    int animation_length;
    float animation_start_time;
    int fps;

    public BatSpriteAnimation(BatControl ec, SpriteRenderer renderer, Sprite[] animation, int fps) {
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

        if (ec.currentState == BatState.STOPPED)
            renderer.sprite = animation[0];
        else {
            // Modulus is necessary so we don't overshoot the length of the animation.
            int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation_length);
            renderer.sprite = animation[current_frame_index];
        }
    }
}

public class StateBatStopped : State {
    BatControl ec;
    public float timePassed = Random.Range(2f, 5f);

    public StateBatStopped(BatControl ec) {
        this.ec = ec;
        ec.currentState = BatState.STOPPED;
        ec.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

    public override void OnUpdate(float time_delta_fraction) {
        timePassed -= Time.deltaTime;

        if (timePassed < 0)
            state_machine.ChangeState(new StateBatSpeedUp(this.ec));
    }
}


public class StateBatSpeedUp : State {
    BatControl ec;
    float distance;
    float startVelocity = 0.5f;
    Vector3 startPosition;
    Vector3 targetPosition;
    Vector3 direction;

    public StateBatSpeedUp(BatControl ec) {
        this.ec = ec;
        ec.currentState = BatState.MOVING;
        startPosition = ec.transform.position;
        int roomStartX = PlayerControl.instance.roomOffsetX * 16 + 2;
        int roomStartY = PlayerControl.instance.roomOffsetY * 11 + 2;
        int x = Random.Range(roomStartX, roomStartX + 11);
        int y = Random.Range(roomStartY, roomStartY + 6);
        targetPosition = new Vector3(x, y, 0);
        direction = (targetPosition - startPosition).normalized;
        distance = Vector3.Distance(targetPosition, startPosition);
    }

    public override void OnUpdate(float time_delta_fraction) {
        Vector3 currentPosition = ec.transform.position;
        float percentage = Vector3.Distance(startPosition, currentPosition) / distance;
        float currentVelocity = ec.walkingVelocity * percentage;
        currentVelocity = currentVelocity > 0.5f ? currentVelocity : 0.5f;
        ec.GetComponent<Rigidbody>().velocity = direction * currentVelocity;
        if (Vector3.Distance(targetPosition, currentPosition) < 0.1f) {
            ec.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ec.transform.position = targetPosition;
            int moves = Random.Range(1, 4);
            state_machine.ChangeState(new StateBatMove(ec, moves));
        }
    }
}

public class StateBatMove : State {
    BatControl ec;
    Vector3 targetPosition;
    Vector3 direction;
    int moves;

    public StateBatMove(BatControl ec, int moves) {
        this.ec = ec;
        this.moves = moves;
        int roomStartX = PlayerControl.instance.roomOffsetX * 16 + 2;
        int roomStartY = PlayerControl.instance.roomOffsetY * 11 + 2;
        int x = Random.Range(roomStartX, roomStartX + 11);
        int y = Random.Range(roomStartY, roomStartY + 6);
        targetPosition = new Vector3(x, y, 0);

        direction = (targetPosition - ec.transform.position).normalized;
        ec.GetComponent<Rigidbody>().velocity = direction * ec.walkingVelocity;
    }

    public override void OnUpdate(float time_delta_fraction) {
        Vector3 currentPosition = ec.transform.position;
        if (Vector3.Distance(targetPosition, currentPosition) < 0.1f) {
            if (moves <= 0)
                state_machine.ChangeState(new StateBatSlowDown(ec));
            else
                state_machine.ChangeState(new StateBatMove(ec, --moves));
        }
    }
}

public class StateBatSlowDown : State {
    BatControl ec;
    float distance;
    Vector3 startPosition;
    Vector3 targetPosition;
    Vector3 direction;

    public StateBatSlowDown(BatControl ec) {
        this.ec = ec;
        startPosition = ec.transform.position;
        int roomStartX = PlayerControl.instance.roomOffsetX * 16 + 2;
        int roomStartY = PlayerControl.instance.roomOffsetY * 11 + 2;
        int x = Random.Range(roomStartX, roomStartX + 11);
        int y = Random.Range(roomStartY, roomStartY + 6);
        targetPosition = new Vector3(x, y, 0);
        direction = (targetPosition - startPosition).normalized;
        distance = Vector3.Distance(targetPosition, startPosition);
    }

    public override void OnUpdate(float time_delta_fraction) {
        Vector3 currentPosition = ec.transform.position;
        float percentage = Vector3.Distance(targetPosition, currentPosition) / distance;
        float currentVelocity = ec.walkingVelocity * percentage;
        currentVelocity = currentVelocity > 0.5f ? currentVelocity : 0.5f;
        ec.GetComponent<Rigidbody>().velocity = direction * currentVelocity;
        if (Vector3.Distance(targetPosition, currentPosition) < 0.1f) {
            ec.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ec.transform.position = targetPosition;
            state_machine.ChangeState(new StateBatStopped(ec));
        }
    }
}
