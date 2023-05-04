using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WaveFunctionCollapse {
    public class VectorPair
    {

        public Vector2Int baseCellPosition { get; set; }
        public Vector2Int cellToPropogatePosition { get; set; }
        public Vector2Int previousCellPosition { get; set; }
        public Direction directionFromBase { get; set; }

        public VectorPair(Vector2Int baseCellPosition, Direction directionFromBase,
            Vector2Int previousCellPosition)
        {
            this.baseCellPosition = baseCellPosition;
            this.directionFromBase = directionFromBase;
            this.previousCellPosition = previousCellPosition;
            this.cellToPropogatePosition = DirectionHelper.GetHexInDirection(directionFromBase, baseCellPosition);
        }

        public bool AreWeCheckingPreviousCellAgain() {
            return previousCellPosition == cellToPropogatePosition;
        }

    }
}


