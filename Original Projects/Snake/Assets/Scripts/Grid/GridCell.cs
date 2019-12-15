using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake.Grid {
    public class GridCell : MonoBehaviour
    {
        private static bool initialized = false;
        private static GridController gridController;

        public readonly Vector2Int gridPos;
        public readonly CellValidity cellValidity;
        
        internal readonly Vector2 floatPos;

        void Start()
        {
            if (!initialized) {
                initialize();
            }
        }

        public GridCell(int x, int y) {
            gridPos = new Vector2Int(x, y);
            gridController.gridToWorldPos(gridPos);
            cellValidity = gridController.getCellValidityForPos(gridPos);
        }

        private void initialize()
        {
            gridController = FindObjectOfType<GridController>();
            initialized = true;
        }
    }
}