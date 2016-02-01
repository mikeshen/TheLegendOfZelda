using UnityEngine;
using System.Collections;

public class WallmasterTriggerX : MonoBehaviour {
	public GameObject WallmasterPrefab;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Link") {
            PlayerControl pc = PlayerControl.instance;

			Vector3 handSpawnPos = pc.transform.position + (3 * Vector3.right);
			handSpawnPos.y = Mathf.Round(handSpawnPos.y);

			MonoBehaviour.Instantiate(WallmasterPrefab, handSpawnPos, Quaternion.identity);
        }
    }
}