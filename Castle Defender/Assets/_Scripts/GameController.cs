using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public int gateHealth;

	// Use this for initialization
	void Start () {
        // Lock the cursor to the center of the screen and hide it.
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E))
        {
            Application.Quit();
        }
	}
}
