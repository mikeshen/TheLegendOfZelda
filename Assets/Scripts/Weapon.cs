using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag == "Enemy") {
            EnemyControl ec = coll.gameObject.GetComponent<EnemyControl>();
            ec.currentHealth--;
            if (ec.currentHealth <= 0)
                Destroy(coll.gameObject);
        }
    }
}
