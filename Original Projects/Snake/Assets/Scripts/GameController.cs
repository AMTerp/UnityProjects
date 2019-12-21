using System;
using System.Collections;
using System.Collections.Generic;
using Snake.Grid;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static event Action resetGameEvent;

    private static GridController gridController;
    private static PlayerController playerController;
    private static ScoreController scoreController;

    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        playerController = FindObjectOfType<PlayerController>();
        scoreController = FindObjectOfType<ScoreController>();
        playerController.playerMovementEvent += onPlayerMove;
        playerController.ateOwnTailEvent += onEatOwnTail;
        SceneManager.sceneUnloaded += OnSceneChange;
    }

    public static void ToggleCursor(bool enableCursor)
    {
        Cursor.lockState = enableCursor ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = enableCursor;
    }

    private static void onEatOwnTail()
    {
        triggerGameReset();
    }

    private static void onPlayerMove(GridCell newCell)
    {
        if (newCell.cellValidity == CellValidity.OUTSIDE_GRID) {
            triggerGameReset();
        }
    }

    private static void triggerGameReset() {
        if (resetGameEvent != null) {
            resetGameEvent();
        }
    }

    private void OnSceneChange(Scene current)
    {
        // Reset subscriptions.
        resetGameEvent = null;
    }
}
