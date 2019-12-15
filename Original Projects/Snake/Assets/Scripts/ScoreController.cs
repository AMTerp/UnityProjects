using System;
using System.Collections;
using System.Collections.Generic;
using Snake.Grid;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public event Action<int> scoreEvent;

    public GameObject pelletPrefab;

    private GridController gridController;
    private PlayerController playerController;
    private PelletController pelletController;
    private int currentScore = 0;

    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        playerController = FindObjectOfType<PlayerController>();
        pelletController = new PelletController(this);
        pelletController.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onPlayerPickup()
    {
        currentScore++;
        Debug.Log("New score: " + currentScore);
        if (scoreEvent != null) {
            scoreEvent(currentScore);
        }
    }

    private class PelletController
    {
        private ScoreController scoreController;
        private GameObject currentPellet;
        private GridCell currentPelletCell;

        internal PelletController(ScoreController scoreController) {
            this.scoreController = scoreController;
            this.scoreController.playerController.playerMovementEvent += handlePlayerMovement;
        }

        internal void Start()
        {
            spawnNewPellet();
        }

        private void handlePlayerMovement(GridCell oldCell, GridCell newCell)
        {
            if (newCell.Equals(currentPelletCell)) {
                scoreController.onPlayerPickup();
                Destroy(currentPellet);
                spawnNewPellet();
            }
        }

        // TODO: Prevent spawning a pellet on the player.
        private void spawnNewPellet()
        {
            currentPellet = Instantiate(scoreController.pelletPrefab);
            currentPelletCell = scoreController.gridController.getRandomCell();
            scoreController.gridController.placeInCell(currentPellet, currentPelletCell);
        }
    }
}
