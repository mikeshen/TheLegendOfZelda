using UnityEngine;
using System.Collections;

public class WallmasterTriggerY : MonoBehaviour {
    public GameObject WallmasterPrefab;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Link") {
         // Send Link and camera back to the beginning
            PlayerControl pc = PlayerControl.instance;
            pc.warped = true;
            pc.transform.position = new Vector3(39.5f, 3.5f, 0);
            ShowMapOnCamera.S.transform.position = new Vector3(39.5f, 6.4f, -10);
            GameState.destroyOnScreen();
            pc.roomOffsetX = 2;
            pc.roomOffsetY = 0;
            // spawn enemies here
        }
    }
}
