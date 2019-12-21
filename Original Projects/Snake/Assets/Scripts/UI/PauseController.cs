using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool paused { get; private set; }
    public GameObject pauseUi;

    void Start()
    {
        TogglePause(false);
    }

    void Update()
    {
        CheckPlayerInput();
    }
    
    private void CheckPlayerInput()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
        {
            return;
        }

        TogglePause();
    }

    private void TogglePause() {
        TogglePause(!paused);
    }

    private void TogglePause(bool enablePause)
    {
        pauseUi.SetActive(enablePause);
        GameController.ToggleCursor(enablePause);
        Time.timeScale = enablePause ? 0f : 1f;
        paused = enablePause;
    }
}
