using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaveFunctionCollapse {

    public class ValueManager<T>
    {
        int[][] grid;
        Dictionary<int, IValue<T>> valueIndexDictionary = new Dictionary<int, IValue<T>>();
        int index = 0;

        public ValueManager(IValue<T>[][] gridOfValues) {
            CreateGridOfIndices(gridOfValues);
        }


        private void CreateGridOfIndices(IValue<T>[][] gridOfValues) {
            grid = MyCollectionExtension.CreateJaggedArray<int[][]>(gridOfValues.Length, gridOfValues[0].Length);

            for (int row = 0; row < gridOfValues.Length; row++) {
                for (int col = 0; col < gridOfValues[0].Length; col++) {
                    SetIndexToGridPosition(gridOfValues, row, col);
                }
            }

        }

        internal Vector2 GetGridSize()
        {
            if (grid == null)
                return Vector2.zero;

            return new Vector2(grid[0].Length, grid.Length);
        }

        private void SetIndexToGridPosition(IValue<T>[][] gridOfValues, int row, int col)
        {
            if (valueIndexDictionary.ContainsValue(gridOfValues[row][col]))
            {
                var key = valueIndexDictionary.FirstOrDefault(x => x.Value.Equals(gridOfValues[row][col]));
                grid[row][col] = key.Key;
            }
            else {
                grid[row][col] = index;
                index++;
                valueIndexDictionary.Add(grid[row][col], gridOfValues[row][col]);
            }

        }

        public int GetGridValue(int x, int y) {
            if (y >= grid.Length || x >= grid[0].Length || x < 0 || y < 0) {
                throw new System.IndexOutOfRangeException("Grid does not contain x and y value: " + x + ", " + y);
            }

            return grid[y][x];

        }

        public IValue<T> GetValueFromIndex(int index) {
            if (valueIndexDictionary.ContainsKey(index))
            {
                return valueIndexDictionary[index];
            }
            else {
                throw new System.Exception("No index: "+index+", in valueIndexDictionary");
            }
        }

        public int GetGridValuesIncludingOffset(int x, int y) {
            int yMax = grid.Length;
            int xMax = grid[0].Length;

            //Debug.Log("yMax: "+yMax);
            //Debug.Log("xMax: " + xMax);

            if (x < 0 && y < 0)
            {
                return GetGridValue(xMax + x, yMax + y);
            }
            if (x < 0 && y >= yMax)
            {
                return GetGridValue(xMax + x, y - yMax);
            }
            if (x >= xMax && y < 0)
            {
                return GetGridValue(x - xMax, yMax + y);
            }
            if (x >= xMax && y >= yMax)
            {
                return GetGridValue(x - xMax, y - yMax);
            }
            if (x < 0)
            {
                return GetGridValue(xMax + x, y);
            }
            if (x >= xMax)
            {
                return GetGridValue(x - xMax, y);
            }
            if (y >= yMax)
            {
                return GetGridValue(x, y - yMax);
            }
            if (y < 0)
            {
                return GetGridValue(x, yMax + y);
            }
            return GetGridValue(x, y);


        }

        public int[][] GetPatternValuesFromGridAt(int x, int y, int patternSize)
        {
            int[][] arrayToReturn = MyCollectionExtension.CreateJaggedArray<int[][]>(patternSize, patternSize);

            for (int row = 0; row < patternSize; row++)
            {
                for (int col = 0; col < patternSize; col++)
                {

                    arrayToReturn[row][col] = GetGridValuesIncludingOffset(x + col, y + row);

                }
            }

            return arrayToReturn;
        }

    }

    
}


