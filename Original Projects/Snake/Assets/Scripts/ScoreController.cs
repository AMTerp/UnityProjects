using System;
using System.Collections;
using System.Collections.Generic;
using Snake.Grid;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public event Action<int> scoreEvent;

    public GameObject pelletPrefab;
    public Text scoreText;

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
        setScoreText(0);
        GameController.resetGameEvent += reset;
    }

    public void reset()
    {
        currentScore = 0;
        setScoreText(currentScore);
        pelletController.reset();
    }

    private void onPlayerPickup()
    {
        currentScore++;
        setScoreText(currentScore);
        if (scoreEvent != null) {
            scoreEvent(currentScore);
        }
    }

    private void setScoreText(int newScore)
    {
        scoreText.text = newScore.ToString();
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

        internal void reset()
        {
            destroyPellet();
            spawnNewPellet();
        }

        private void handlePlayerMovement(GridCell newCell)
        {
            if (newCell.Equals(currentPelletCell)) {
                scoreController.onPlayerPickup();
                Destroy(currentPellet);
                spawnNewPellet();
            }
        }

        private void destroyPellet()
        {
            Destroy(currentPellet);
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
