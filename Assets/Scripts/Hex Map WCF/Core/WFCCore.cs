using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse {
    public class WFCCore
    {
        OutputGrid outputGrid;
        PatternManager patternManager;

        private int maxIterations = 0;

        public WFCCore(int outputWidth, int outputHeight, int mi, PatternManager pm) {
            this.outputGrid = new OutputGrid(outputWidth, outputHeight, pm.GetNumberOfPatterns());
            this.maxIterations = mi;
            this.patternManager = pm;
        }

        public int[][] CreateOutputGrid() {
            int iteration = 0;

            while (iteration < this.maxIterations) {
                CoreSolver solver = new CoreSolver(this.outputGrid, this.patternManager);
                int innerIteration = 100000;
                while (!solver.CheckForConflicts() && !solver.CheckIfSolved()) {
                    Vector2Int position = solver.GetLowestEntropyCell();
                    solver.CollapseCell(position);
                    solver.Propogate();
                    innerIteration--;
                    if (innerIteration <= 0) {
                        Debug.LogError("Propogation is taking too long");
                        return new int[0][];
                    }
                }
                if (solver.CheckForConflicts())
                {
                    Debug.LogError("\n Conflict Occured. Iteration: " + iteration);
                    iteration++;
                    outputGrid.ResetAllPossibilities();
                    solver = new CoreSolver(this.outputGrid, this.patternManager);
                }
                else {
                    Debug.Log("Solved On: "+iteration);
                    this.outputGrid.PrintResultsToConsole();
                    break;
                }
            }

            if (iteration >= this.maxIterations) {
                Debug.LogError("Could not solve tilemap, iterations greater than max iterations.");
            }
            return outputGrid.GetSolvedOutputGrid();
        }

    }
}


