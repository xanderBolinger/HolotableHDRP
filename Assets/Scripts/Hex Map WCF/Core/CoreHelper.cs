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

            /*for (int i = 0; i < patternManager.GetNumberOfPatterns(); i++) {
                totalFrequency += patternManager.GetPatternFrequency(i);
            }

            totalFrequencyLog = Mathf.Log(totalFrequency, 2);*/

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

        public List<VectorPair> Create4DirectionNeighbours(Vector2Int cellCoordinates, Vector2Int previousCell)
        {
            List<VectorPair> list = new List<VectorPair>()
            {
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(1, 0), Direction.Right,previousCell),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(-1, 0), Direction.Left, previousCell),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, 1), Direction.Up, previousCell),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, -1), Direction.Down, previousCell)
            };
            return list;
        }

        public List<VectorPair> Create4DirectionNeighbours(Vector2Int cellCoordinates)
        {
            return Create4DirectionNeighbours(cellCoordinates, cellCoordinates);
        }

        /*public List<VectorPair> CreateSixDirectionNeighbours(Vector2Int cellCoordinates, Vector2Int previousCell) {
            List<VectorPair> list = new List<VectorPair>();

            foreach (Direction dir in typeof(Direction).GetEnumValues()) {

                list.Add(new VectorPair(cellCoordinates, dir, previousCell));

            }

            return list;

        }

        public List<VectorPair> CreateSixDirectionNeighbours(Vector2Int cellCoordinates) {
            return CreateSixDirectionNeighbours(cellCoordinates, cellCoordinates);
        }*/

        public float CalculateEntropy(Vector2Int position, OutputGrid outputGrid) {
            float sum = 0;
            
            foreach (var possibleIndex in outputGrid.GetPossibleValuesForPosition(position)) {
                totalFrequency += patternManager.GetPatternFrequency(possibleIndex);
                sum += patternManager.GetPatternFrequencyLog2(possibleIndex);
            }

            totalFrequencyLog = Mathf.Log(totalFrequency, 2);

            return totalFrequencyLog - (sum / totalFrequency);
        }

        public List<VectorPair> CheckIfNeighboursAreCollapsed(VectorPair pairToCheck, OutputGrid outputGrid)
        {

            return Create4DirectionNeighbours(pairToCheck.cellToPropagatePosition, pairToCheck.baseCellPosition)
                .Where(x => outputGrid.CheckIfValidPosition(x.cellToPropagatePosition) && outputGrid.CheckIfCellIsCollapsed(x.cellToPropagatePosition) == false)
                .ToList();

        }

        public bool CheckCellSolutionForCollisions(Vector2Int cellCoordinates, OutputGrid outputGrid)
        {
            foreach (var neighbour in Create4DirectionNeighbours(cellCoordinates))
            {
                if (outputGrid.CheckIfValidPosition(neighbour.cellToPropagatePosition) == false)
                {
                    continue;
                }
                HashSet<int> possibleIndices = new HashSet<int>();
                foreach (var patternIndexAtNeighbour in outputGrid.GetPossibleValuesForPosition(neighbour.cellToPropagatePosition))
                {
                    var possibleNeighborusForBase = patternManager.GetPossibleNeighoursForPatternInDirection(patternIndexAtNeighbour, neighbour.DiectionFromBase.GetOppositeDirectionTo());
                    possibleIndices.UnionWith(possibleNeighborusForBase);
                }
                if (possibleIndices.Contains(outputGrid.GetPossibleValuesForPosition(cellCoordinates).First()) == false)
                {

                    return true;
                }
            }

            return false;
        }

        /*public List<VectorPair> CheckIfNeighboursAreCollapsed(VectorPair pairToCheck, OutputGrid outputGrid) {
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
        }*/

    }
}


