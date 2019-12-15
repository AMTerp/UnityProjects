using System;
using System.Collections;
using System.Collections.Generic;
using Snake.Grid;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<GridCell> playerMovementEvent;
    public float cellsPerSecondMovement;
    public GameObject snakeCellPrefab;

    private GridController gridController;
    private ScoreController scoreController;
    private PlayerMovementController playerMovementController;

    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        scoreController = FindObjectOfType<ScoreController>();
        scoreController.scoreEvent += onScoreEvent;
        GridCell startingCell = gridController.getCenter();
        gridController.placeInCell(gameObject, startingCell);
        playerMovementController = new PlayerMovementController(this, startingCell, cellsPerSecondMovement);
    }

    void Update()
    {
        playerMovementController.run();
    }

    private void onScoreEvent(int aNewScore)
    {
        playerMovementController.incrementTail();
    }

    private class PlayerMovementController
    {
        private Direction currentDirection = Direction.RIGHT;
        private PlayerController playerController;
        private float cellsPerSecond;
        private float movementWaitTime;
        private float timeUntilNextMove;
        private LinkedList<SnakeCell> snakeCells;

        internal PlayerMovementController(PlayerController playerController, GridCell startingCell, float cellsPerSecond) {
            this.playerController = playerController;
            snakeCells = new LinkedList<SnakeCell>();
            snakeCells.AddFirst(new SnakeCell(this, startingCell));
            this.cellsPerSecond = cellsPerSecond;
            this.movementWaitTime = 1 / cellsPerSecond;
            this.timeUntilNextMove = movementWaitTime;
        }

        internal void run()
        {
            updateInputDirection();
            checkMovement();
        }

        internal void incrementTail()
        {
            snakeCells.AddLast(new SnakeCell(this, snakeCells.Last.Value.pos));
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
            GridCell prevHeadPos = snakeCells.First.Value.pos;
            GridCell nextHeadPos = prevHeadPos;
            switch (currentDirection) {
                case Direction.UP: {
                    nextHeadPos = GridCell.of(prevHeadPos.gridPos.x, prevHeadPos.gridPos.y + 1);
                    break;
                }
                case Direction.DOWN: {
                    nextHeadPos = GridCell.of(prevHeadPos.gridPos.x, prevHeadPos.gridPos.y - 1);
                    break;
                }
                case Direction.LEFT: {
                    nextHeadPos = GridCell.of(prevHeadPos.gridPos.x - 1, prevHeadPos.gridPos.y);
                    break;
                }
                case Direction.RIGHT: {
                    nextHeadPos = GridCell.of(prevHeadPos.gridPos.x + 1, prevHeadPos.gridPos.y);
                    break;
                }
                default: {
                    throw new InvalidOperationException("Enum not handled: " + currentDirection);
                };
            }

            SnakeCell last = snakeCells.Last.Value;
            snakeCells.RemoveLast();
            snakeCells.AddFirst(last);
            last.pos = nextHeadPos;
            playerController.gridController.placeInCell(last.gameObject, nextHeadPos);

            if (playerController.playerMovementEvent != null) {
                playerController.playerMovementEvent(nextHeadPos);
            }
        }

        private class SnakeCell {
            internal GridCell pos;
            internal readonly GameObject gameObject;

            internal SnakeCell(PlayerMovementController playerMovementController, GridCell pos) {
                this.pos = pos;
                gameObject = Instantiate(playerMovementController.playerController.snakeCellPrefab);
                gameObject.transform.parent = playerMovementController.playerController.transform;
                playerMovementController.playerController.gridController.placeInCell(gameObject, pos);
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
