using UnityEngine;
using System.Collections;

public class BowDamage : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Enemy") {
            EnemyControl ec = coll.gameObject.GetComponent<EnemyControl>();
            ec.currentHealth--;
            GameState.instance.enemyTakeDamage(coll.gameObject);

        }
    }
}
