using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

    public float walkingVelocity;
    public float currentHealth;
    public int totalHealth;
    public float damageCooldown;
    public float boomerangCooldown;
    public Direction currentDirection;
    public int index;


    public Sprite[] Run;

    public StateMachine animationStateMachine;
    public StateMachine controlStateMachine;

    // Use this for initialization
    void Start() {
        currentDirection = (Direction)Random.Range(0, 3);
        boomerangCooldown = 0;
        damageCooldown = 0;
        OnStart();
    }

    void FixedUpdate() {
        if (damageCooldown > 0)
            enemyDamageAnimation();
        if (boomerangCooldown > 0) {
            boomerangCooldown -= Time.deltaTime;
            return;
        }
        else
            boomerangCooldown = 0;

        animationStateMachine.Update();
        controlStateMachine.Update();
    }

    public virtual void OnStart() {}

    public void enemyDamageAnimation() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr.color == Color.blue)
            sr.color = Color.red;
        else if (sr.color == Color.red)
            sr.color = Color.white;
        else
            sr.color = Color.blue;
        damageCooldown -= Time.deltaTime;
        if (damageCooldown <= 0f) {
            sr.color = Color.white;
            damageCooldown = 0f;
        }
    }
}
