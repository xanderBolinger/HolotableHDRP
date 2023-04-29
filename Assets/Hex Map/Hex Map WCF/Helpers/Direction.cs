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
    }
}

