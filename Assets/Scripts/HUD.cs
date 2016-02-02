using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    public Text rupeeText;
    public Text heartText;
    public Text keyText;
    public Text oldManText;
    public bool isPaused = false;
	public bool pausedNoHUD = false;
    public string secretMessage = "Eastmost Penninsula is the secret.";
    public float typingLength = 4.2f;
    private float percentTyped = 0;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        int rupeeCount = PlayerControl.instance.rupeeCount;
        rupeeText.text = "Rupees: " + rupeeCount.ToString();

        float currentHealth = PlayerControl.instance.currentHealth;
        int totalHealth = PlayerControl.instance.totalHealth;

        heartText.text = "Health: " + currentHealth.ToString() + " / " + totalHealth.ToString();

        int keyCount = PlayerControl.instance.keyCount;

        keyText.text = "Keys: " + keyCount.ToString();

        Vector3 mainCamera = RoomTransitions.instance.transform.position;

        if (mainCamera.y >= 39 && mainCamera.y <= 40 && mainCamera.x >= 7 && mainCamera.x <= 8) {
            int currNumLetters = (int)(secretMessage.Length * percentTyped);
            oldManText.text = secretMessage.Substring(0, currNumLetters);
            percentTyped += Time.deltaTime / typingLength;
            percentTyped = Mathf.Min(1.0f, percentTyped);
        }
        else {
            oldManText.text = "";
            percentTyped = 0;
        }

        //if (PlayerControl.instance.transform.position.x <)

        // Allows pausing to occur
		if (Input.GetKeyDown(KeyCode.RightShift)) {
			pausedNoHUD = !pausedNoHUD;

			if (pausedNoHUD)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}

        if (Input.GetKeyDown(KeyCode.Return)) {
            isPaused = !isPaused;

            if (isPaused)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        if (!isPaused) {
            rupeeText.color = Color.white;
            heartText.color = Color.white;
        }

        // Implements selection on pause menu
        if (isPaused) {
            keyText.text = "Paused!";
            if (PlayerControl.instance.hasBow) {
                if (!PlayerControl.instance.bowOrBoomerang)
                    heartText.color = Color.yellow;
                heartText.text = "Bow";
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    PlayerControl.instance.bowOrBoomerang = false;
                    heartText.color = Color.yellow;
                    rupeeText.color = Color.white;
                }
            }
            else
                heartText.text = "";
            if (PlayerControl.instance.hasBoomerang) {
                if (PlayerControl.instance.bowOrBoomerang)
                    rupeeText.color = Color.yellow;
                rupeeText.text = "Boomerang";
                if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    PlayerControl.instance.bowOrBoomerang = true;
                    rupeeText.color = Color.yellow;
                    heartText.color = Color.white;
                }
            }
            else
                rupeeText.text = "";
        }

    }
}
