using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
namespace WaveFunctionCollapse {
    public class PatternDataResults
    {
        private int[][] patternIndicesGrid;
        
        public Dictionary<int, PatternData> patternIndexDictionary { get; internal set; }



        public PatternDataResults(int[][] patternIndicesGrid, Dictionary<int, PatternData> patternIndexDictionary)
        {
            this.patternIndicesGrid = patternIndicesGrid;
            this.patternIndexDictionary = patternIndexDictionary;
        }

        public int GetGridLengthX() {
            return patternIndicesGrid[0].Length;
        }

        public int GetGridLengthY() {
            return patternIndicesGrid.Length;
        }

        public int GetIndexAt(int x, int y) {
           // Debug.Log("Get Index At: "+x+", "+y);

            if (x < 0 || y < 0 || y >= patternIndicesGrid.Length || x >= patternIndicesGrid[0].Length) {
                throw new System.Exception("Get Index at out of bounds, Length: " + patternIndicesGrid.Length + ", x: " + x + ", y: " + y);
            }

            return patternIndicesGrid[y][x];
        }

        public int GetNeighbourInDirection(int x, int y, Direction dir) {
            //Debug.Log("Get Neighor in Direction: "+x+", "+y+", Dir: "+dir);

            if (!patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x, y)) {
                return -1;
            }

            bool even = x % 2 == 0 ? true : false;

            //dir = DirectionHelper.GetOppositeDirectionTo(dir);



            switch (dir)
            {
                case Direction.Up:
                    if (patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x, y + 1))
                    {
                        return GetIndexAt(x, y + 1);
                    }
                    return -1;
                case Direction.Down:
                    if (patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x, y - 1))
                    {
                        return GetIndexAt(x, y - 1);
                    }
                    return -1;
                case Direction.Left:
                    if (patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x - 1, y))
                    {
                        return GetIndexAt(x - 1, y);
                    }
                    return -1;
                case Direction.Right:
                    if (patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x + 1, y))
                    {
                        return GetIndexAt(x + 1, y);
                    }
                    return -1;
                default:
                    return -1;

                    /*case Direction.A:
                        if (valid(x, y - 1))
                            return GetIndexAt(x, y - 1);
                        return -1;
                    case Direction.B:
                        if (even && valid(x + 1, y - 1))
                            return GetIndexAt(x + 1, y - 1);
                        else if (!even && valid(x + 1, y))
                            return GetIndexAt(x + 1, y);
                        return -1;
                    case Direction.C:
                        if (even && valid(x + 1, y))
                            return GetIndexAt(x + 1, y);
                        else if (!even && valid(x + 1, y + 1))
                            return GetIndexAt(x + 1, y + 1);
                        return -1;
                    case Direction.D:
                        if (valid(x, y + 1))
                            return GetIndexAt(x, y + 1);
                        return -1;
                    case Direction.E:
                        if (even && valid(x - 1, y))
                            return GetIndexAt(x - 1, y);
                        else if (!even && valid(x - 1, y + 1))
                            return GetIndexAt(x - 1, y + 1);
                        return -1;
                    case Direction.F:
                        if (even && valid(x - 1, y - 1))
                            return GetIndexAt(x - 1, y - 1);
                        else if (!even && valid(x-1, y))
                            return GetIndexAt(x-1, y);
                        return -1;
                    default:
                        return -1;*/

            }

            /*switch (dir) {

                case Direction.A:
                    if (valid(x - 1, y))
                        return GetIndexAt(x - 1, y);
                    return -1; 
                case Direction.B:
                    if (even && valid(x - 1, y + 1))
                        return GetIndexAt(x - 1, y + 1);
                    else if (!even && valid(x, y+1))
                        return GetIndexAt(x, y+1);
                    return -1;
                case Direction.C:
                    if (even && valid(x, y + 1))
                        return GetIndexAt(x, y + 1);
                    else if (!even && valid(x+1, y + 1))
                        return GetIndexAt(x+1, y + 1);
                    return -1;
                case Direction.D:
                    if (valid(x - 1, y))
                        return GetIndexAt(x - 1, y);
                    return -1;
                case Direction.E:
                    if (even && valid(x, y - 1))
                        return GetIndexAt(x, y - 1);
                    else if (!even && valid(x + 1, y - 1))
                        return GetIndexAt(x + 1, y - 1);
                    return -1;
                case Direction.F:
                    if (even && valid(x - 1, y - 1))
                        return GetIndexAt(x - 1, y - 1);
                    else if (!even && valid(x, y - 1))
                        return GetIndexAt(x, y - 1);
                    return -1;
                default:
                    return -1;

            }*/

        }

        private bool valid(int x, int y) {
            return patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x, y);
        }

    }
}


