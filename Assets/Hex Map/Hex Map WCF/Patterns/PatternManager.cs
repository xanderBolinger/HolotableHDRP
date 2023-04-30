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

        private void CreatePatterns<T>(ValueManager<T> valueManager, IFindNeighbourStrategy strategy, bool equalWeights)
        {
            var patternFinderResult = PatternFinder.GetPatternDataFromGrid(valueManager, patternSize, equalWeights);

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


