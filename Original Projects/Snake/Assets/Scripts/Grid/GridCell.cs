using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snake.Grid {
    public class GridCell
    {
        public readonly Vector2Int gridPos;
        public readonly CellValidity cellValidity;

        internal readonly Vector2 floatPos;

        public static GridCell of(int x, int y) {
            return GridCellFactory.getCell(x, y);
        }

        public static void initializeFactory() {
            GridCellFactory.initialize();
        }

        private GridCell(Vector2Int gridPos, GridController gridController) {
            this.gridPos = gridPos;
            this.floatPos = gridController.gridToWorldPos(gridPos);
            this.cellValidity = gridController.getCellValidityForPos(gridPos);
        }

        public override bool Equals(object obj) {
            return Equals(obj as GridCell);
        }

        public bool Equals(GridCell other) {
            return other != null && this.gridPos == other.gridPos;
        }

        private class GridCellFactory : ScriptableObject
        {
            private static GridController gridController = null;
            private static Dictionary<Vector2Int, GridCell> gridCellCache = new Dictionary<Vector2Int, GridCell>();

            internal static void initialize()
            {
                gridController = FindObjectOfType<GridController>();
                gridCellCache.Clear();
            }

            internal static GridCell getCell(int x, int y) {
                Vector2Int gridPos = new Vector2Int(x, y);
                GridCell requestedCell;
                if (gridCellCache.TryGetValue(gridPos, out requestedCell)) {
                    return requestedCell;
                } else {
                    requestedCell = new GridCell(gridPos, gridController);
                    gridCellCache.Add(gridPos, requestedCell);
                    return requestedCell;
                }
            }
        }
    }
}