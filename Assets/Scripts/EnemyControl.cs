using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

    public float walkingVelocity;
    public float currentHealth;
    public int totalHealth;
    public Direction currentDirection;

    public Sprite[] Run;

    public StateMachine animationStateMachine;
    public StateMachine controlStateMachine;

    // Use this for initialization
    void Start() {
        currentDirection = (Direction)Random.Range(0, 3);
        OnStart();
    }

    void Update() {
        animationStateMachine.Update();
        controlStateMachine.Update();
    }

    public virtual void OnStart() {}
}