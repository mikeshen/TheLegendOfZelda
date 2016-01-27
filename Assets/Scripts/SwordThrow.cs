using UnityEngine;
using System.Collections;

public class SwordThrow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 cameraPosition = ShowMapOnCamera.S.transform.position;
        Vector3 magicSwordPosition = transform.position;

        float xDifference = Mathf.Abs(cameraPosition.x - magicSwordPosition.x);
        float yDifference = Mathf.Abs(cameraPosition.y - magicSwordPosition.y);

        if (xDifference >= 20 || yDifference >= 20 || GetComponent<Rigidbody>().velocity == Vector3.zero) {
            PlayerControl.instance.swordThrown = false;
            Destroy(gameObject);
        }
            
	}

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.tag != "Link") {
            PlayerControl.instance.swordThrown = false;
            Destroy(gameObject);
        }
    }
}
