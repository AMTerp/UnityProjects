using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake.Grid {
    public class GridController : MonoBehaviour
    {
        private const String LEFT_BLACK_BAR_TAG = "LeftBlackBar";
        private const String RIGHT_BLACK_BAR_TAG = "RightBlackBar";
        private const float CELL_SIZE = 1f;

        public int yNumCells;

        public GameObject cellSpritePrefab;

        private Camera playerCamera;
        private Vector2 screenArea;
        private int xNumCells;
        private float xStart;
        private float yStart;

        void Awake()
        {
            playerCamera = FindObjectOfType<Camera>();
            playerCamera.orthographicSize = yNumCells / 2f;
            screenArea = calculateScreenArea(playerCamera);
            xNumCells = Mathf.FloorToInt(screenArea.x);
            xStart = -xNumCells / 2f + CELL_SIZE / 2f;
            yStart = -yNumCells / 2f + CELL_SIZE / 2f;
            drawGrid();
            placeBlackBars();
        }

        public void placeInCell(GameObject gameObject, GridCell gridCell)
        {
            gameObject.transform.position = gridCell.floatPos;
        }

        // Returns the center cell for the grid. If there is truly no center (due to even number of x and/or y cells), returns the cell closer to the origin (bottom left).
        public GridCell getCenter()
        {
            return GridCell.of((xNumCells - 1) / 2, (yNumCells - 1) / 2);
        }
        

        internal Vector2 gridToWorldPos(Vector2Int gridPos) {
            return gridToWorldPos(gridPos.x, gridPos.y);
        }
        

        internal Vector2 gridToWorldPos(int x, int y) {
            return new Vector2(xStart + x, yStart + y);
        }

        internal CellValidity getCellValidityForPos(Vector2Int gridPos)
        {
            return gridPos.x < 0 || gridPos.x > xNumCells || gridPos.y < 0 || gridPos.y > yNumCells ? CellValidity.IN_GRID : CellValidity.OUTSIDE_GRID;
        }
        
        private Vector2 calculateScreenArea(Camera camera)
        {
            float yLength = 2 * camera.orthographicSize;
            float xLength = yLength * camera.aspect;
            return new Vector2(xLength, yLength);
        }

        private void drawGrid()
        {
            for (int x = 0; x < xNumCells; x++)
            {
                for (int y = 0; y < yNumCells; y++) {
                    Vector2 pos = gridToWorldPos(x, y);
                    GameObject cell = Instantiate(cellSpritePrefab, pos, Quaternion.identity);
                }
            }
        }

        private void placeBlackBars()
        {
            float emptyXLength = screenArea.x - xNumCells;
            float blackBarDistanceFromEdge = emptyXLength / 2f;
            float leftBlackBarXPos = -screenArea.x / 2f + blackBarDistanceFromEdge;
            float rightBlackBarXPos = -leftBlackBarXPos;
            Transform leftBlackBar = GameObject.FindWithTag(LEFT_BLACK_BAR_TAG).transform;
            Transform rightBlackBar = GameObject.FindWithTag(RIGHT_BLACK_BAR_TAG).transform;
            leftBlackBar.position = new Vector2(leftBlackBarXPos, 0);
            rightBlackBar.position = new Vector2(rightBlackBarXPos, 0);
        }
    }
}