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
        playerController.playerMovementEvent += handlePlayerMove;
    }

    private void handlePlayerMove(GridCell newCell)
    {
        if (newCell.cellValidity == CellValidity.OUTSIDE_GRID) {
            if (resetGameEvent != null) {
                resetGameEvent();
            }
        }
    }
}
