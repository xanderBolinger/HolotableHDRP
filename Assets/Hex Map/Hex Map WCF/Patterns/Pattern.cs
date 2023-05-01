using Helpers;
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
            int[][] gridPartToCompare;
            switch (dir)
            {
                case Direction.A:
                    //gridPartToCompare = MyCollectableExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 1, grid.Length, gridPartToCompare);
                    break;
                case Direction.D:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 0, grid.Length - 1, gridPartToCompare);
                    break;
                case Direction.F:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(0, grid.Length - 1, 0, grid.Length, gridPartToCompare);
                    break;
                case Direction.E:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(0, grid.Length - 1, 0, grid.Length, gridPartToCompare);
                    break;
                case Direction.B:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(1, grid.Length, 0, grid.Length, gridPartToCompare);
                    break;
                case Direction.C:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(1, grid.Length, 0, grid.Length, gridPartToCompare);
                    break;
                default:
                    return grid;
            }

            return gridPartToCompare;
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

