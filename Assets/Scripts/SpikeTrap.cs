using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour {
    public bool refractory = false;
    public Vector3 defaultPosition = Vector3.zero;


    // Use this for initialization
    void Start() {
        defaultPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
    }
}