using UnityEngine;
using System.Collections;

public class Boomerang : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll) {
        PlayerControl pc = PlayerControl.instance;
        if (coll.gameObject.tag == "Rupee") {
            Destroy(coll.gameObject);
            pc.rupeeCount++;
        }
        else if (coll.gameObject.tag == "Heart") {
            Destroy(coll.gameObject);
            if (pc.currentHealth + 1 <= pc.totalHealth)
                pc.currentHealth++;
            else
                pc.currentHealth = pc.totalHealth;
        }
        else if (coll.gameObject.tag == "Key") {
            Destroy(coll.gameObject);
            pc.keyCount++;
        }
    }
}
