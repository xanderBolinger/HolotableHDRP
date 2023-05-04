using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaveFunctionCollapse {
    public class CoreHelper
    {
        
        float totalFrequency = 0;
        float totalFrequencyLog = 0;
        PatternManager patternManager;

        public CoreHelper(PatternManager patternManager)
        {
            this.patternManager = patternManager;

            for (int i = 0; i < patternManager.GetNumberOfPatterns(); i++) {
                totalFrequency += patternManager.GetPatternFrequency(i);
            }

            totalFrequencyLog = Mathf.Log(totalFrequency, 2);

        }

        public int SelectSolutionPatternFromFrequency(List<int> possibleValues) {
            List<float> valueFrequenciesFractions = GetListOfWeightsFromIndices(possibleValues);
            float randomValue = UnityEngine.Random.Range(0, valueFrequenciesFractions.Sum());
            float sum = 0;
            int index = 0;
            foreach (var item in valueFrequenciesFractions) {
                sum += item;
                if (randomValue <= sum) {
                    return index;
                }
                index++; 
            }

            return index-1; 

        }

        private List<float> GetListOfWeightsFromIndices(List<int> possibleValues)
        {
            /*var valueFrequencies = possibleValues.Aggregate(new List<float>(), (acc, val) =>
            {
                acc.Add(patternManager.GetPatternFrequency(val));
                return acc;
            }, 
                acc => acc
            ).ToList();

            return valueFrequencies;*/
            return possibleValues.Select(patternManager.GetPatternFrequency).ToList();
        }

        public List<VectorPair> CreateSixDirectionNeighbours(Vector2Int cellCoordinates, Vector2Int previousCell) {
            List<VectorPair> list = new List<VectorPair>();

            foreach (Direction dir in typeof(Direction).GetEnumValues()) {

                list.Add(new VectorPair(cellCoordinates, dir, previousCell));

            }

            return list;

        }

        public List<VectorPair> CreateSixDirectionNeighbours(Vector2Int cellCoordinates) {
            return CreateSixDirectionNeighbours(cellCoordinates, cellCoordinates);
        }

        public float CalculateEntropy(Vector2Int position, OutputGrid outputGrid) {
            float sum = 0;
            
            foreach (var possibleIndex in outputGrid.GetPossibleValueForPosition(position)) {
                sum += patternManager.GetPatternFrequencyLog2(possibleIndex);
            }

            return totalFrequencyLog - (sum / totalFrequency);
        }

        public List<VectorPair> CheckIfNeighboursAreCollapsed(VectorPair pairToCheck, OutputGrid outputGrid) {
            return CreateSixDirectionNeighbours(pairToCheck.cellToPropogatePosition, pairToCheck.baseCellPosition)
                .Where(x => outputGrid.CheckIfValidPosition(x.cellToPropogatePosition) 
                && outputGrid.CheckIfCellIsCollapsed(x.cellToPropogatePosition)==false)
                .ToList();
        }

        public bool CheckCellSolutionForCollision(Vector2Int cellCoordinates, OutputGrid outputGrid) {
            foreach (var neighbour in CreateSixDirectionNeighbours(cellCoordinates)) {
                if (outputGrid.CheckIfValidPosition(neighbour.cellToPropogatePosition) == false) {
                    continue;
                }

                HashSet<int> possibleIndices = new HashSet<int>();

                foreach (var patternIndexAtNeighbour in outputGrid.GetPossibleValueForPosition(neighbour.cellToPropogatePosition)) {
                    
                    var possibleNeighboursForBase = patternManager
                        .GetPossibleNeighoursForPatternInDirection(patternIndexAtNeighbour, 
                        neighbour.directionFromBase.GetOppositeDirectionTo());

                    possibleIndices.UnionWith(possibleNeighboursForBase);


                }

                if (possibleIndices.Contains(outputGrid.GetPossibleValueForPosition(cellCoordinates).First()) == false) {
                    return true;
                }

            }

            return false;
        }

    }
}


