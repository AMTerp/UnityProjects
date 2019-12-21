using System;
using System.Collections;
using System.Collections.Generic;
using Snake.Grid;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<GridCell> playerMovementEvent;
    public event Action ateOwnTailEvent;
    public GameObject snakeCellPrefab;

    private float cellsPerSecondMovement;
    private int snakeGrowthTilesPerPellet;
    private GridController gridController;
    private ScoreController scoreController;
    private GameController gameController;
    private PlayerMovementController playerMovementController;

    void Start()
    {
        cellsPerSecondMovement = SettingsProvider.cellsPerSecondMovement;
        snakeGrowthTilesPerPellet = SettingsProvider.snakeGrowthTilesPerPellet;
        gridController = FindObjectOfType<GridController>();
        scoreController = FindObjectOfType<ScoreController>();
        gameController = FindObjectOfType<GameController>();
        playerMovementController = new PlayerMovementController(this, cellsPerSecondMovement);
        gameController.resetGameEvent += reset;
        scoreController.scoreEvent += onScoreEvent;
    }

    void Update()
    {
        playerMovementController.run();
    }

    private void onScoreEvent(int aNewScore)
    {
        playerMovementController.incrementTail(snakeGrowthTilesPerPellet);
    }

    private void reset()
    {
        playerMovementController.reset();
    }

    private class PlayerMovementController
    {
        private Direction prevDirection;
        private Direction nextDirection;
        private PlayerController playerController;
        private float cellsPerSecond;
        private float movementWaitTime;
        private float timeUntilNextMove;
        private LinkedList<SnakeCell> snakeCells;

        internal PlayerMovementController(PlayerController playerController, float cellsPerSecond) {
            this.playerController = playerController;
            this.cellsPerSecond = cellsPerSecond;
            this.movementWaitTime = 1 / cellsPerSecond;
            this.timeUntilNextMove = movementWaitTime;
            prevDirection = Direction.LEFT;
            nextDirection = Direction.RIGHT;
            snakeCells = new LinkedList<SnakeCell>();
            GridCell startingCell = playerController.gridController.getCenter();
            snakeCells.AddFirst(new SnakeCell(this, startingCell));
        }

        internal void reset()
        {
            foreach (SnakeCell snakeCell in snakeCells) {
                Destroy(snakeCell.gameObject);
            }
            nextDirection = Direction.RIGHT;
            snakeCells = new LinkedList<SnakeCell>();
            GridCell startingCell = playerController.gridController.getCenter();
            snakeCells.AddFirst(new SnakeCell(this, startingCell));
        }

        internal void run()
        {
            timeUntilNextMove -= Time.deltaTime;
            updateInputDirection();
            checkIfWillEatOwnTail();
            checkMovement();
        }

        internal void incrementTail(int numSnakeCellsToGrowBy)
        {
            for (int i = 0; i < numSnakeCellsToGrowBy; i++) {
                snakeCells.AddLast(new SnakeCell(this, snakeCells.Last.Value.pos));
            }
        }

        private void updateInputDirection()
        {
            if (playerPressedUp() && prevDirection != Direction.DOWN) {
                nextDirection = Direction.UP;
            } else if (playerPressedDown() && prevDirection != Direction.UP) {
                nextDirection = Direction.DOWN;
            } else if (playerPressedLeft() && prevDirection != Direction.RIGHT) {
                nextDirection = Direction.LEFT;
            } else if (playerPressedRight() && prevDirection != Direction.LEFT) {
                nextDirection = Direction.RIGHT;
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
            if (timeUntilNextMove < 0) {
                move();
                timeUntilNextMove = movementWaitTime;
            }
        }

        private void move()
        {
            GridCell prevHeadPos = snakeCells.First.Value.pos;
            GridCell nextHeadPos = calculateNextGridCell(prevHeadPos, nextDirection);

            prevDirection = nextDirection;
            SnakeCell last = snakeCells.Last.Value;
            snakeCells.RemoveLast();
            snakeCells.AddFirst(last);
            last.pos = nextHeadPos;
            playerController.gridController.placeInCell(last.gameObject, nextHeadPos);

            if (playerController.playerMovementEvent != null) {
                playerController.playerMovementEvent(nextHeadPos);
            }
        }

        private GridCell calculateNextGridCell(GridCell currPos, Direction direction) {
            switch (direction) {
                case Direction.UP: {
                    return GridCell.of(currPos.gridPos.x, currPos.gridPos.y + 1);
                }
                case Direction.DOWN: {
                    return GridCell.of(currPos.gridPos.x, currPos.gridPos.y - 1);
                }
                case Direction.LEFT: {
                    return GridCell.of(currPos.gridPos.x - 1, currPos.gridPos.y);
                }
                case Direction.RIGHT: {
                    return GridCell.of(currPos.gridPos.x + 1, currPos.gridPos.y);
                }
                default: {
                    throw new InvalidOperationException("Enum not handled: " + direction);
                };
            }
        }

        private void checkIfWillEatOwnTail()
        {
            if (timeUntilNextMove > 0) {
                return;
            }

            GridCell nextHeadGridPos = calculateNextGridCell(snakeCells.First.Value.pos, nextDirection);
            LinkedListNode<SnakeCell> currSnakeCell = snakeCells.First;
            // When the head moves, so does the snake cell at the end of the tail. Therefore, we don't want to check
            // if we end up eating it.
            while (currSnakeCell.Next != null && currSnakeCell.Next.Next != null) {
                currSnakeCell = currSnakeCell.Next;
                if (currSnakeCell.Value.pos.Equals(nextHeadGridPos)) {
                    playerController.ateOwnTailEvent();
                    return;
                }
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

        private enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
    }
}
