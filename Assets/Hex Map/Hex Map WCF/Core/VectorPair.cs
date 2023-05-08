using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WaveFunctionCollapse {
    public class VectorPair
    {

        /* public Vector2Int baseCellPosition { get; set; }
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
         }*/
        public Vector2Int baseCellPosition { get; set; }
        public Vector2Int cellToPropagatePosition { get; set; }
        private Vector2Int previousCellPosition { get; set; }
        public Direction DiectionFromBase { get; set; }

        public VectorPair(Vector2Int baseCellPosition, Vector2Int cellToPropagatePosition, Direction directionFromBase, Vector2Int previousCellPosition)
        {
            this.baseCellPosition = baseCellPosition;
            this.cellToPropagatePosition = cellToPropagatePosition;
            this.DiectionFromBase = directionFromBase;
            this.previousCellPosition = previousCellPosition;
        }

        public bool AreWeCheckingPreviousCellAgain()
        {
            return previousCellPosition == cellToPropagatePosition;
        }

    }
}


