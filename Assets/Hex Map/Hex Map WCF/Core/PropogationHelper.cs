using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaveFunctionCollapse {
    public class PropogationHelper
    {
        OutputGrid outputGrid;
        CoreHelper coreHelper;
        bool cellWithNoSolutionPresent;
        
        SortedSet<LowEntropyCell> lowEntropySet = new SortedSet<LowEntropyCell>();
        
        Queue<VectorPair> pairsToPropogate = new Queue<VectorPair>();

        public SortedSet<LowEntropyCell> LowEntropySet { get => lowEntropySet; }
        public Queue<VectorPair> PairsToPropogate { get => pairsToPropogate; }

        public PropogationHelper(OutputGrid og, CoreHelper ch) {
            this.outputGrid = og;
            this.coreHelper = ch;
        }

        public bool CheckIfPairShouldBeProcessed(VectorPair propogatePair) {
            return outputGrid.CheckIfValidPosition(propogatePair.cellToPropogatePosition) &&
                !propogatePair.AreWeCheckingPreviousCellAgain();
        }

        public void AnalyzePropogationResults(VectorPair propogatePair, int startCount, int newPossiblePatternCount) {
            
            if (newPossiblePatternCount > 1 && startCount > newPossiblePatternCount) {
                AddNewPairsToPropogateQueue(propogatePair.cellToPropogatePosition, propogatePair.baseCellPosition);
                AddToLowEntropySet(propogatePair.cellToPropogatePosition);
            }

            if (newPossiblePatternCount == 0) {
                cellWithNoSolutionPresent = true;
            }

            if (newPossiblePatternCount == 1) {
                cellWithNoSolutionPresent = coreHelper.CheckCellSolutionForCollision(propogatePair.cellToPropogatePosition, outputGrid);
            }

        }

        internal void EnqueueUncollapseNeighbours(VectorPair propogatePair)
        {
            var uncollapsedNeighbours = coreHelper.CheckIfNeighboursAreCollapsed(propogatePair, outputGrid);

            foreach (var item in uncollapsedNeighbours) {
                pairsToPropogate.Enqueue(item);
            }

        }

        private void AddToLowEntropySet(Vector2Int cellToPropogatePosition)
        {
            var elementOfLowEntropySet = lowEntropySet.Where(x => x.position == cellToPropogatePosition)
                .FirstOrDefault();
            if (elementOfLowEntropySet == null && outputGrid.CheckIfCellIsCollapsed(cellToPropogatePosition) == false)
            {
                float entropy = coreHelper.CalculateEntropy(cellToPropogatePosition, outputGrid);
                lowEntropySet.Add(new LowEntropyCell(cellToPropogatePosition, entropy));
            }
            else {
                lowEntropySet.Remove(elementOfLowEntropySet);
                elementOfLowEntropySet.entropy = coreHelper.CalculateEntropy(cellToPropogatePosition, outputGrid);
                lowEntropySet.Add(elementOfLowEntropySet);
            }
        }

        public void AddNewPairsToPropogateQueue(Vector2Int cellToPropogatePosition, Vector2Int previousCell)
        {
            var list = coreHelper.CreateSixDirectionNeighbours(cellToPropogatePosition, previousCell);
            foreach (var item in list) {
                pairsToPropogate.Enqueue(item);
            }

        }

        public bool CheckForConflicts() {
            return cellWithNoSolutionPresent;
        }

        public void SetConflictFlag() {
            cellWithNoSolutionPresent = true; 
        }

    }
}


