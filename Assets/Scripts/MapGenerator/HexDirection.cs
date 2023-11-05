using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexMapper {
    [Serializable]
    public enum Direction
    {
        A,AB,
        B,BC,
        C,CD,
        D,DE,
        E,EF,
        F,FA
    }

    public static class HexDirection
    {

        public static bool Beam(Vector2Int start, Vector2Int target, Direction targetFacing) {
            if (Rear(start, target, targetFacing) || GetHexSideFacingTarget(target, start) == targetFacing)
                return false;
            return true;
        }

        public static bool Rear(Vector2Int start, Vector2Int target, Direction targetFacing) {
            var dirToTarget = GetHexSideFacingTarget(start, target);

            if (dirToTarget == GetOppositeDirectionTo(targetFacing))
                return true;

            return false;
        }

        private static Direction GetHexSideFacingTargetBaseCases(Vector2Int start, Vector2Int target, int startDistance)
        {

            if (startDistance == 0)
            {
                return Direction.A;
            }
            else if (startDistance == 1)
            {

                var adjacentTiles = GetHexNeighbours(start);

                for (int i = 0; i < 6; i++)
                {
                    if (adjacentTiles[i] == target)
                    {
                        if (i == 0)
                            return Direction.A;
                        if (i == 1)
                            return Direction.B;
                        if (i == 2)
                            return Direction.C;
                        if (i == 3)
                            return Direction.D;
                        if (i == 4)
                            return Direction.E;
                        if (i == 5)
                            return Direction.F;
                    }

                }

            }

            throw new Exception("Could not find base case direction: "+start+", "+target+", Dist: "+startDistance);

        }

        public static Direction GetHexSideFacingTarget(Vector2Int start, Vector2Int target)
        {

            var startDistance = HexMap.GetDistance(start, target);
            if (startDistance <= 1)
            {
                var baseCase = GetHexSideFacingTargetBaseCases(start, target, startDistance);
                return baseCase;
            }


            Direction closestDir = Direction.A;
            var closestDist = GetDistanceInDirection(start, target, startDistance, Direction.A);

            foreach(Direction dir in Enum.GetValues(typeof(Direction)))
            {
                var newDist = GetDistanceInDirection(start, target, startDistance, dir);

                if (newDist < closestDist)
                {
                    closestDist = newDist;
                    closestDir = dir;
                }

            }

            return closestDir;

            // if x2 < x1 and y2 == y1 than A
            // if getDistanceInDirection(A) < getDistanceInDirection(B) than A elif getDistanceInDirection(A) > getDistanceInDirection(B) than B, otherwise A/B
            // if x2 <= x1 , y2 > y1 than B

            // if x2 >= x1 and y2 > y1 than C 

            // if x2 > x1 and y2 == y1 than D

            // if x2 >= x1 and y2 < y1 than E

            // if x2 <= x1 and y2 < y1 thanF

            //throw new Exception("Direction not found for cords: ("+start.toString()+") to ("+target.toString()+")");
        }
        public static int GetDistanceInDirection(Vector2Int start, Vector2Int target, int distance, Direction dir)
        {

            Vector2Int newCord = start;
            bool clockwise = true;
            for (int i = 0; i < distance; i++)
            {
                newCord = GetHexInDirection(dir, newCord, clockwise);
                clockwise = !clockwise;
            }

            return HexMap.GetDistance(target, newCord);
        }

        public static Vector2Int GetHexInDirection(Direction dir, Vector2Int pos, bool clockwise)
        {
            List<Vector2Int> neighbors = GetHexNeighbours(pos);

            switch (dir)
            {
                case Direction.A:
                    return neighbors[0];
                case Direction.B:
                    return neighbors[1];
                case Direction.C:
                    return neighbors[2];
                case Direction.D:
                    return neighbors[3];
                case Direction.E:
                    return neighbors[4];
                case Direction.F:
                    return neighbors[5];
                case Direction.AB:
                    return !clockwise ? neighbors[0] : neighbors[1];
                case Direction.BC:
                    return !clockwise ? neighbors[1] : neighbors[2];
                case Direction.CD:
                    return !clockwise ? neighbors[2] : neighbors[3];
                case Direction.DE:
                    return !clockwise ? neighbors[3] : neighbors[4];
                case Direction.EF:
                    return !clockwise ? neighbors[4] : neighbors[5];
                case Direction.FA:
                    return !clockwise ? neighbors[5] : neighbors[0];
                default:
                    throw new Exception("Hex direction not found, dir: " + dir + ", pos: " + pos);
            }
        }

        public static Direction GetOppositeDirectionTo(this Direction direction)
        {
            switch (direction)
            {
                    case Direction.A:
                        return Direction.D;
                    case Direction.B:
                        return Direction.E;
                    case Direction.C:
                        return Direction.F;
                    case Direction.D:
                        return Direction.A;
                    case Direction.E:
                        return Direction.B;
                    case Direction.F:
                        return Direction.C;
                    case Direction.AB:
                        return Direction.DE;
                    case Direction.BC:
                        return Direction.EF;
                    case Direction.CD:
                        return Direction.FA;
                    case Direction.DE:
                        return Direction.AB;
                    case Direction.EF:
                        return Direction.BC;
                    case Direction.FA:
                        return Direction.CD;
                default:
                        return direction;
            }
        }


        public static Direction GetFaceInDirection(Direction facing, bool clockwise)
        {

            switch (facing)
            {

                case Direction.A:
                    return clockwise ? Direction.AB : Direction.FA;
                case Direction.AB:
                    return clockwise ? Direction.B : Direction.A;
                case Direction.B:
                    return clockwise ? Direction.BC : Direction.AB;
                case Direction.BC:
                    return clockwise ? Direction.C : Direction.B;
                case Direction.C:
                    return clockwise ? Direction.CD : Direction.BC;
                case Direction.CD:
                    return clockwise ? Direction.D : Direction.C;
                case Direction.D:
                    return clockwise ? Direction.DE : Direction.CD;
                case Direction.DE:
                    return clockwise ? Direction.E : Direction.D;
                case Direction.E:
                    return clockwise ? Direction.EF : Direction.DE;
                case Direction.EF:
                    return clockwise ? Direction.F : Direction.E;
                case Direction.F:
                    return clockwise ? Direction.FA : Direction.EF;
                case Direction.FA:
                    return clockwise ? Direction.A : Direction.F;
                default:
                    break;

            }

            throw new System.Exception("Direction not found for clockwise: "+clockwise+", start dir: "+facing);

        }

        public static Vector2Int GetHexInDirection(Vector2Int originalPos, Direction direction) {

            var neighbours = GetHexNeighbours(originalPos);


            switch (direction)
            {
                case Direction.A:
                    return neighbours[0];
                case Direction.B:
                    return neighbours[1];
                case Direction.C:
                    return neighbours[2];
                case Direction.D:
                    return neighbours[3];
                case Direction.E:
                    return neighbours[4];
                case Direction.F:
                    return neighbours[5];
                default:
                    return neighbours[0];
            }
        }

        public static List<Vector2Int> GetHexNeighbours(Vector2Int pos) {
            var neighbors = new List<Vector2Int>();

            // A
            neighbors.Add(new Vector2Int(pos.x - 1, pos.y));

            // B
            if (pos.y % 2 != 0)
                neighbors.Add(new Vector2Int(pos.x, pos.y + 1));
            else
                neighbors.Add(new Vector2Int(pos.x - 1, pos.y + 1));

            // C
            if (pos.y % 2 != 0)
                neighbors.Add(new Vector2Int(pos.x + 1, pos.y + 1));
            else
                neighbors.Add(new Vector2Int(pos.x, pos.y + 1));

            // D
            neighbors.Add(new Vector2Int(pos.x + 1, pos.y));

            // E
            if (pos.y % 2 != 0)
                neighbors.Add(new Vector2Int(pos.x - 1, pos.y - 1));
            else
                neighbors.Add(new Vector2Int(pos.x, pos.y - 1));

            // F
            if (pos.y % 2 != 0)
                neighbors.Add(new Vector2Int(pos.x, pos.y - 1));
            else
                neighbors.Add(new Vector2Int(pos.x - 1, pos.y - 1));

            return neighbors;
        }

        public static Vector2Int GetHexInDirection(Direction dir, Vector2Int pos) {

            var neighbours = GetHexNeighbours(pos);

            switch (dir)
            {
                case Direction.A:
                    return neighbours[0];
                case Direction.B:
                    return neighbours[1];
                case Direction.C:
                    return neighbours[2];
                case Direction.D:
                    return neighbours[3];
                case Direction.E:
                    return neighbours[4];
                case Direction.F:
                    return neighbours[5];
                default:
                    throw new System.Exception("Hex direction not found, dir: "+dir+", pos: "+pos);
            }
        }

    }
}


