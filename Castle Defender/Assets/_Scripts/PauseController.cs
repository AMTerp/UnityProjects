using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {

    private bool paused;
    private GameController gameController;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        paused = false;
        unpause();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                pause();
            }
            else
            {
                unpause();
            }
        }
	}

    void pause()
    {
        Time.timeScale = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        gameController.uiDisableMouseClick = true;
        Cursor.lockState = CursorLockMode.None;
        paused = true;
    }

    void unpause()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Time.timeScale = 1;
        gameController.uiDisableMouseClick = false;
        Cursor.lockState = CursorLockMode.Locked;
        paused = false;
    }
}
