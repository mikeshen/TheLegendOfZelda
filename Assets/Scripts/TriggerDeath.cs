using UnityEngine;
using System.Collections;

public class TriggerDeath : MonoBehaviour {
	bool set;
	// Use this for initialization
	void Start () {
		set = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag != "Link" || set)
			return;
		set = true;
		for (int i = 7; i <= 13; i++)
			for (int j = 12; j <= 18; j++)
				Instantiate(GameState.instance.prefabs[(int)Prefab.BATS], new Vector3(i, j), Quaternion.identity);
	}
}
