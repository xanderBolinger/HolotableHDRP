using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace WaveFunctionCollapse {

    public class PatternManager
    {
        Dictionary<int, PatternData> patternDataIndexDictionary;
        Dictionary<int, PatternNeighbours> patternPossibleNeighboursDictionary;
        int patternSize = -1;
        IFindNeighbourStrategy strategy;

        public PatternManager(int patternSize) {
            this.patternSize = patternSize;
        }

        public void ProcessGrid<T>(ValueManager<T> valueManager, bool equalWeights, string strategyName = null) {
            NeighbourStrategyFactory strategyFactory = new NeighbourStrategyFactory();
            strategy = strategyFactory.CreateInstance(strategyName == null ? patternSize + "" : strategyName);
            CreatePatterns(valueManager, strategy, equalWeights);

        }

        internal int[][] ConvertPatternToValues<T>(int[][] outputValues)
        {
            int patternOutputWidth = outputValues[0].Length;
            int patternOutputHeight = outputValues.Length;

            int valueGridWidth = patternOutputWidth + patternSize - 1;
            int valueGridHeight = patternOutputHeight + patternSize - 1;
            int[][] valueGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(valueGridHeight, valueGridWidth);

            for (int row = 0; row < patternOutputHeight; row++) {
                for (int col = 0; col < patternOutputWidth; col++) {
                    Pattern pattern = GetPatternDataFromIndex(outputValues[row][col]).Pattern;
                    GetPatternValues(patternOutputWidth, patternOutputHeight, valueGrid, row, col, pattern);

                }
            }

            return valueGrid;
        }

        private void GetPatternValues(int patternOutputWidth, int patternOutputHeight, int[][] valuesGrid, int row, int col, Pattern pattern)
        {
            if (row == patternOutputHeight - 1 && col == patternOutputWidth - 1)
            {
                for (int row_1 = 0; row_1 < patternSize; row_1++)
                {
                    for (int col_1 = 0; col_1 < patternSize; col_1++)
                    {
                        valuesGrid[row + row_1][col + col_1] = pattern.GetGridValue(col_1, row_1);
                    }
                }
            }
            else if (row == patternOutputHeight - 1)
            {
                for (int row_1 = 0; row_1 < patternSize; row_1++)
                {
                    valuesGrid[row + row_1][col] = pattern.GetGridValue(0, row_1);
                }

            }
            else if (col == patternOutputWidth - 1)
            {
                for (int col_1 = 0; col_1 < patternSize; col_1++)
                {
                    valuesGrid[row][col + col_1] = pattern.GetGridValue(col_1, 0);
                }
            }
            else
            {
                valuesGrid[row][col] = pattern.GetGridValue(0, 0);

            }

        }

        private void CreatePatterns<T>(ValueManager<T> valueManager, IFindNeighbourStrategy strategy, bool equalWeights)
        {
            var patternFinderResult = PatternFinder.GetPatternDataFromGrid(valueManager, patternSize, equalWeights);

            Debug.Log("--- Create Patterns ---");

            StringBuilder builder = null;
            List<string> list = new List<string>();
            for (int r = 0; r < patternFinderResult.GetGridLengthY(); r++)
            {
                builder = new StringBuilder();

                for (int c = 0; c < patternFinderResult.GetGridLengthX(); c++)
                {
                    builder.Append(patternFinderResult.GetIndexAt(c, r) + " ");
                }

                list.Add(builder.ToString());

            }

            list.Reverse();
            foreach (var str in list)
            {
                Debug.Log(str);
            }

            Debug.Log("--- Create Patterns Finished ---");

            patternDataIndexDictionary = patternFinderResult.patternIndexDictionary;
            GetPatternNeighbours(patternFinderResult, strategy);
        }

        private void GetPatternNeighbours(PatternDataResults patternFinderResult, IFindNeighbourStrategy strategy)
        {
            patternPossibleNeighboursDictionary = PatternFinder.FindPossibleNeighboursForAllPatterns(strategy, patternFinderResult);
        }

        public PatternData GetPatternDataFromIndex(int index) {
            return patternDataIndexDictionary[index];
        }

        public HashSet<int> GetPossibleNeighoursForPatternInDirection(int patternIndex, Direction dir) {
            return patternPossibleNeighboursDictionary[patternIndex].GetNeighboursInDirection(dir);
        }

        public float GetPatternFrequency(int index) {
            return GetPatternDataFromIndex(index).FrequencyRelative;
        }

        public float GetPatternFrequencyLog2(int index) {
            return GetPatternDataFromIndex(index).FrequencyRelativeLog2;
        }

        public int GetNumberOfPatterns() {
            return patternDataIndexDictionary.Count;
        } 

    }

}


