using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WaveFunctionCollapse {
    public class NeighboursStrategySize2 : IFindNeighbourStrategy
    {
    

        public Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternFinderResult) {
            //Debug.Log("Find Neighbours");
            Dictionary<int, PatternNeighbours> result = new Dictionary<int, PatternNeighbours>();

            foreach (var patternDataToCheck in patternFinderResult.patternIndexDictionary) {
                foreach (var possibleNeighbourForPattern in patternFinderResult.patternIndexDictionary) {
                    FindNeighboursInDirection(result, patternDataToCheck, possibleNeighbourForPattern);
                }
            }

            return result;

        }

        private void FindNeighboursInDirection(Dictionary<int, PatternNeighbours> result, 
            KeyValuePair<int, PatternData> patternDataToCheck, 
            KeyValuePair<int, PatternData> possibleNeighbourForPattern)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
                if (patternDataToCheck.Value.CompareGrid(dir, possibleNeighbourForPattern.Value)) {
                    if (!result.ContainsKey(patternDataToCheck.Key)) {
                        result.Add(patternDataToCheck.Key, new PatternNeighbours());
                    }
                    result[patternDataToCheck.Key].AddPatternToDictionary(dir, possibleNeighbourForPattern.Key);
                }
            }
        }
    }
}


