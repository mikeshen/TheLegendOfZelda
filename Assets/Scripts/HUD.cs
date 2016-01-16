using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public Text rupeeText;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int rupeeCount = PlayerControl.instance.rupeeCount;
        rupeeText.text = "Rupees: " + rupeeCount.ToString();
	
	}
}
