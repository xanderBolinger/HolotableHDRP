using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public enum Direction
    {
        A,
        B,
        C,
        D,
        E,
        F
    }

    public static class DirectionHelper
    {
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
                default:
                    return direction;
            }
        }

       

        public static List<Vector2Int> GetHexNeighbours(Vector2Int pos) {
            var neighbors = new List<Vector2Int>();

            // A
            neighbors.Add(new Vector2Int(pos.x - 1, pos.y));

            // B
            if (pos.y % 2 == 0)
                neighbors.Add(new Vector2Int(pos.x - 1, pos.y + 1));
            else
                neighbors.Add(new Vector2Int(pos.x, pos.y + 1));

            // C 
            if (pos.y % 2 == 0)
                neighbors.Add(new Vector2Int(pos.x, pos.y + 1));
            else
                neighbors.Add(new Vector2Int(pos.x + 1, pos.y + 1));

            // D
            neighbors.Add(new Vector2Int(pos.x + 1, pos.y));

            // E
            if (pos.y % 2 == 0)
                neighbors.Add(new Vector2Int(pos.x, pos.y - 1));
            else
                neighbors.Add(new Vector2Int(pos.x + 1, pos.y - 1));

            // F
            if (pos.y % 2 == 0)
                neighbors.Add(new Vector2Int(pos.x - 1, pos.y - 1));
            else
                neighbors.Add(new Vector2Int(pos.x, pos.y - 1));

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

