using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public Text rupeeText;
	public Text heartText;
	public Text keyText;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int rupeeCount = PlayerControl.instance.rupeeCount;
        rupeeText.text = "Rupees: " + rupeeCount.ToString();

		float currentHealth = PlayerControl.instance.currentHealth;
		int totalHealth = PlayerControl.instance.totalHealth;

		heartText.text = "Health: " + currentHealth.ToString() + " / " + totalHealth.ToString(); 

		int numKeys = PlayerControl.instance.numKeys;

		keyText.text = "Keys: " + numKeys.ToString();
	}
}
