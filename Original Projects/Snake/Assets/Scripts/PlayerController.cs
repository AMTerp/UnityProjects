using System;
using System.Collections;
using System.Collections.Generic;
using Snake.Grid;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<GridCell, GridCell> playerMovementEvent;
    public float cellsPerSecondMovement;

    private GridController gridController;
    private PlayerMovementController playerMovementController;

    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        GridCell startingCell = gridController.getCenter();
        gridController.placeInCell(gameObject, startingCell);
        playerMovementController = new PlayerMovementController(this, startingCell, cellsPerSecondMovement);
    }

    void Update()
    {
        playerMovementController.run();
    }

    private class PlayerMovementController
    {
        private Direction currentDirection = Direction.RIGHT;
        private PlayerController playerController;
        private float cellsPerSecond;
        private float movementWaitTime;
        private float timeUntilNextMove;
        private GridCell currentCell;

        internal PlayerMovementController(PlayerController playerController, GridCell startingCell, float cellsPerSecond) {
            this.playerController = playerController;
            this.currentCell = startingCell;
            this.cellsPerSecond = cellsPerSecond;
            this.movementWaitTime = 1 / cellsPerSecond;
            this.timeUntilNextMove = movementWaitTime;
        }

        internal void run()
        {
            updateInputDirection();
            checkMovement();
        }

        private void updateInputDirection()
        {
            if (playerPressedUp()) {
                currentDirection = Direction.UP;
            } else if (playerPressedDown()) {
                currentDirection = Direction.DOWN;
            } else if (playerPressedLeft()) {
                currentDirection = Direction.LEFT;
            } else if (playerPressedRight()) {
                currentDirection = Direction.RIGHT;
            }
        }

        private bool playerPressedUp()
        {
            return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        }

        private bool playerPressedDown()
        {
            return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
        }

        private bool playerPressedLeft()
        {
            return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        }

        private bool playerPressedRight()
        {
            return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        }

        private void checkMovement()
        {
            timeUntilNextMove -= Time.deltaTime;
            if (timeUntilNextMove < 0) {
                move();
                timeUntilNextMove = movementWaitTime;
            }
        }

        private void move()
        {
            GridCell previousCell = currentCell;
            switch (currentDirection) {
                case Direction.UP: {
                    currentCell = GridCell.of(currentCell.gridPos.x, currentCell.gridPos.y + 1);
                    break;
                }
                case Direction.DOWN: {
                    currentCell = GridCell.of(currentCell.gridPos.x, currentCell.gridPos.y - 1);
                    break;
                }
                case Direction.LEFT: {
                    currentCell = GridCell.of(currentCell.gridPos.x - 1, currentCell.gridPos.y);
                    break;
                }
                case Direction.RIGHT: {
                    currentCell = GridCell.of(currentCell.gridPos.x + 1, currentCell.gridPos.y);
                    break;
                }
                default: {
                    throw new InvalidOperationException("Enum not handled: " + currentDirection);
                };
            }
            playerController.gridController.placeInCell(playerController.gameObject, currentCell);

            if (playerController.playerMovementEvent != null) {
                playerController.playerMovementEvent(previousCell, currentCell);
            }
        }
    }

    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}
