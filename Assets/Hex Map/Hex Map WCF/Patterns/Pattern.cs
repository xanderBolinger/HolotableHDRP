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
            //Debug.Log("Grid,  Rows: "+grid.Length+", Cols: "+grid[0].Length);

            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[0].Length; col++)
                {
                    //Debug.Log("Value("+row+", "+col+"): "+grid[row][col]);
                }
            }


            //gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length);
            //CreatePartOfGrid(0, grid.Length, 0, grid.Length, gridPartToCompare);
            switch (dir)
            {
                case Direction.Up:
                    //gridPartToCompare = MyCollectableExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 1, grid.Length, gridPartToCompare);
                    break;
                case Direction.Down:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 0, grid.Length - 1, gridPartToCompare);
                    break;
                case Direction.Left:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(0, grid.Length - 1, 0, grid.Length, gridPartToCompare);
                    break;
                case Direction.Right:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(1, grid.Length, 0, grid.Length, gridPartToCompare);
                    break;
                default:
                    return grid;
                    /*case Direction.A:
                        //gridPartToCompare = MyCollectableExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                        //Debug.Log("Direction A");
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                        CreatePartOfGrid(0, grid.Length, 1, grid.Length, gridPartToCompare);
                        break;
                    case Direction.D:
                        //Debug.Log("Direction D");
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                        CreatePartOfGrid(0, grid.Length, 0, grid.Length - 1, gridPartToCompare);
                        break;
                    case Direction.F:
                        //Debug.Log("Direction F");
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length-1);
                        CreatePartOfGrid(0, grid.Length - 1, 1, grid.Length, gridPartToCompare);
                        break;
                    case Direction.E:
                        //Debug.Log("Direction E");
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length-1);
                        CreatePartOfGrid(0, grid.Length - 1, 0, grid.Length-1, gridPartToCompare);
                        break;
                    case Direction.B:
                        //Debug.Log("Direction B");
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length-1);
                        CreatePartOfGrid(1, grid.Length, 1, grid.Length, gridPartToCompare);
                        break;
                    case Direction.C:
                        //Debug.Log("Direction C");
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length-1);
                        CreatePartOfGrid(1, grid.Length, 0, grid.Length-1, gridPartToCompare);
                        break;
                    default:
                        return grid;*/
            }
            /*switch (dir)
            {
                case Direction.A:
                    //gridPartToCompare = MyCollectableExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    //Debug.Log("Direction A");
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 1, grid.Length, gridPartToCompare);
                    break;
                case Direction.D:
                    //Debug.Log("Direction D");
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 0, grid.Length - 1, gridPartToCompare);
                    break;
                case Direction.F:
                    //Debug.Log("Direction F");
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(0, grid.Length - 1, 0, grid.Length, gridPartToCompare);
                    break;
                case Direction.E:
                    //Debug.Log("Direction E");
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(0, grid.Length - 1, 0, grid.Length, gridPartToCompare);
                    break;
                case Direction.B:
                    //Debug.Log("Direction B");
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(1, grid.Length, 0, grid.Length, gridPartToCompare);
                    break;
                case Direction.C:
                    //Debug.Log("Direction C");
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(1, grid.Length, 0, grid.Length, gridPartToCompare);
                    break;
                default:
                    return grid;
            }*/

            return gridPartToCompare;
        }

        private void CreatePartOfGrid(int xmin, int xmax, int ymin, int ymax, int[][] gridPartToCompare) {
            List<int> tempList = new List<int>();

            for (int row = ymin; row < ymax; row++) {
                for (int col = xmin; col < xmax; col++) {
                   // Debug.Log("Row: " + row + ", Col: " + col);
                    //Debug.Log("Xmin: "+xmin+", Xmax: "+xmax+", Ymin:"+ymin+", Ymax: "+ymax+", Rows: "+gridPartToCompare.Length+", Cols: "+gridPartToCompare[0].Length);
                    tempList.Add(this.grid[row][col]);
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

