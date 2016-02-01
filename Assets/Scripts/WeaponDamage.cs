using UnityEngine;
using System.Collections;

public class WeaponDamage : MonoBehaviour {

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Enemy")
            GameState.instance.enemyTakeDamage(coll.gameObject);
    }
}
