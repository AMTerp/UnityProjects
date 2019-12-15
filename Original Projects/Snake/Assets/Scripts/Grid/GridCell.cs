using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake.Grid {
    public class GridCell
    {
        public readonly Vector2Int gridPos;
        public readonly CellValidity cellValidity;

        internal readonly Vector2 floatPos;

        public static GridCell of(int x, int y) {
            return GridCellFactory.getCell(x, y);
        }
        private GridCell(int x, int y, GridController gridController) {
            gridPos = new Vector2Int(x, y);
            floatPos = gridController.gridToWorldPos(gridPos);
            cellValidity = gridController.getCellValidityForPos(gridPos);
        }

        public class GridCellFactory : ScriptableObject
        {
            private static GridController gridController = FindObjectOfType<GridController>();
            
            internal static GridCell getCell(int x, int y) {
                return new GridCell(x, y, gridController);
            }
        }
    }
}