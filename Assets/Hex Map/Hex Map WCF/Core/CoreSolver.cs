using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




namespace WaveFunctionCollapse {


    public class CoreSolver
    {

        PatternManager patternManager;
        OutputGrid outputGrid;
        CoreHelper coreHelper;
        PropogationHelper propogationHelper;
        
        public CoreSolver(OutputGrid og, PatternManager pm) {
            this.outputGrid = og;
            this.patternManager = pm;
            this.coreHelper = new CoreHelper(this.patternManager);
            this.propogationHelper = new PropogationHelper(this.outputGrid, this.coreHelper);
        }

        public void Propogate() {
            while (propogationHelper.PairsToPropogate.Count > 0) {

                var propogatePair = propogationHelper.PairsToPropogate.Dequeue();

                if (propogationHelper.CheckIfPairShouldBeProcessed(propogatePair)) {
                    ProcessCell(propogatePair);
                }

                if (propogationHelper.CheckForConflicts() || outputGrid.CheckIfGridIsSolved()) {
                    return;
                }

            }

            if (propogationHelper.CheckForConflicts() && propogationHelper.PairsToPropogate.Count == 0
                && propogationHelper.LowEntropySet.Count == 0) {
                propogationHelper.SetConflictFlag();
            }

        }

        private void ProcessCell(VectorPair propogatePair)
        {

            if (outputGrid.CheckIfCellIsCollapsed(propogatePair.cellToPropagatePosition))
            {
                propogationHelper.EnqueueUncollapseNeighbours(propogatePair);
            }
            else {
                PropogateNeighbour(propogatePair);
            }

        }

        private void PropogateNeighbour(VectorPair propogatePair)
        {
            var possibleValuesAtNeighbour = outputGrid.GetPossibleValuesForPosition(propogatePair.cellToPropagatePosition);
            int startCount = possibleValuesAtNeighbour.Count;

            RemoveImpossibleNeighbours(propogatePair, possibleValuesAtNeighbour);

            int newPossiblePatternCount = possibleValuesAtNeighbour.Count;

            propogationHelper.AnalyzePropogationResults(propogatePair, startCount, newPossiblePatternCount);

        }

        private void RemoveImpossibleNeighbours(VectorPair propogatePair, HashSet<int> possibleValuesAtNeighbour)
        {
            HashSet<int> possibleIndices = new HashSet<int>();

            foreach (var patternIndexAtBase in outputGrid.GetPossibleValuesForPosition(propogatePair.baseCellPosition)) {
                var possibleNeighboursForBase = patternManager.GetPossibleNeighoursForPatternInDirection(patternIndexAtBase, propogatePair.DiectionFromBase);
                possibleIndices.UnionWith(possibleNeighboursForBase);
            }

            possibleValuesAtNeighbour.IntersectWith(possibleIndices);
        }

        public Vector2Int GetLowestEntropyCell() {
            if (propogationHelper.LowEntropySet.Count <= 0)
            {
                return outputGrid.GetRandomCell();
            }
            else {
                var lowestEntropElement = propogationHelper.LowEntropySet.First();
                Vector2Int returnVector = lowestEntropElement.position;
                propogationHelper.LowEntropySet.Remove(lowestEntropElement);
                return returnVector;
            }
        }

        public void CollapseCell(Vector2Int cellCoordinates) {
            var possibleValue = outputGrid.GetPossibleValuesForPosition(cellCoordinates).ToList();

            // List is collapsed or collision
            if (possibleValue.Count == 0 || possibleValue.Count == 1)
                return;
            else { 
                int index = coreHelper.SelectSolutionPatternFromFrequency(possibleValue);
                outputGrid.SetPatternOnPosition(cellCoordinates.x, cellCoordinates.y, possibleValue[index]);
            }

            if (!coreHelper.CheckCellSolutionForCollisions(cellCoordinates, outputGrid))
            {
                propogationHelper.AddNewPairsToPropagateQueue(cellCoordinates, cellCoordinates);
            }
            else {
                propogationHelper.SetConflictFlag();
            }

        }

        public bool CheckIfSolved() {
            return outputGrid.CheckIfGridIsSolved();
        }

        public bool CheckForConflicts() {
            return propogationHelper.CheckForConflicts();
        }

    }


}


