using UnityEngine;
using System.Collections;

public class CameraTest : MonoBehaviour {
	public float smooth = 5.0f;
	public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(PlayerControl.instance.transform.position.x, 6.4f, -10);
	}
}
