using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool paused { get; private set; }
    public GameObject pauseUi;
    public SwitchScene toMainMenuSwitcher;

    void Start()
    {
        TogglePause(false);
    }

    void Update()
    {
        CheckPlayerInput();
    }

    // When the "Quit to main menu" button is clicked.
    public void OnMainMenuClick()
    {
        Time.timeScale = 1f;
        paused = false;
        toMainMenuSwitcher.ChangeScene();
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
