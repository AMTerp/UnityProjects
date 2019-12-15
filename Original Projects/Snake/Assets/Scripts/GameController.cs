using System;
using System.Collections;
using System.Collections.Generic;
using Snake.Grid;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event Action resetGameEvent;

    private GridController gridController;
    private PlayerController playerController;
    private ScoreController scoreController;

    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        playerController = FindObjectOfType<PlayerController>();
        scoreController = FindObjectOfType<ScoreController>();
        playerController.playerMovementEvent += onPlayerMove;
        playerController.ateOwnTailEvent += onEatOwnTail;
    }

    private void onEatOwnTail()
    {
        triggerGameReset();
    }

    private void onPlayerMove(GridCell newCell)
    {
        if (newCell.cellValidity == CellValidity.OUTSIDE_GRID) {
            triggerGameReset();
        }
    }

    private void triggerGameReset() {
        if (resetGameEvent != null) {
            resetGameEvent();
        }
    }
}
