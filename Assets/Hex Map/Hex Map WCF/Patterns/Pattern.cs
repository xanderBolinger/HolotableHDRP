using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WaveFunctionCollapse {

    public class Pattern
    {

        private int index;
        private int[][] grid; 

        public string HashIndex { get; set; }
        public int Index { get => index; }

        public Pattern(int[][] grid, string hashCode, int index) {
            this.grid = grid;
            this.HashIndex = hashCode;
            this.index = index; 
        }

        public void SetGridValue(int x, int y, int value) {
            grid[y][x] = value; 
        }

        public int GetGridValue(int x, int y) {
            return grid[x][y];
        }

        public bool CheckValueAtPosition(int x, int y, int value) {
            return value.Equals(GetGridValue(x, y));
        }

        internal bool ComparePatternToAnotherPattern(Direction dir, Pattern pattern) {
            int[][] myGrid = GetGridValuesInDirection(dir);
            int[][] otherGrid = pattern.GetGridValuesInDirection(dir.GetOppositeDirectionTo());

            for (int row = 0; row < myGrid.Length; row++) {
                for (int col = 0; col < myGrid[0].Length; col++) {
                    if (myGrid[row][col] != otherGrid[row][col]) {
                        return false; 
                    }
                }
            }

            return true;
        }

        
        private int[][] GetGridValuesInDirection(Direction dir)
        {
            throw new NotImplementedException();
        }

        private void CreatePartOfGrid(int xmin, int xmax, int ymin, int ymax, int[][] gridPartToCompare) {
            List<int> tempList = new List<int>();

            for (int row = ymin; row < ymax; row++) {
                for (int col = xmin; col < xmax; col++) {
                    tempList.Add(gridPartToCompare[row][col]);
                }
            }

            for (int i = 0; i < tempList.Count; i++) {
                int x = i % gridPartToCompare.Length;
                int y = i / gridPartToCompare.Length;
                gridPartToCompare[x][y] = tempList[i];
            }

        }

    }

}

