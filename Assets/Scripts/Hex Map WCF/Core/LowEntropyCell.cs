using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse {

    public class LowEntropyCell : IComparable<LowEntropyCell>, IEqualityComparer<LowEntropyCell>
    {

        public Vector2Int position { get; set; }
        public float entropy { get; set; }
        private float smallEntropyNoise;

        public LowEntropyCell(Vector2Int p, float e) {
            smallEntropyNoise = UnityEngine.Random.Range(0.001f, 0.005f);
            this.position = p;
            this.entropy = e+smallEntropyNoise;
        }

        public int CompareTo(LowEntropyCell other)
        {
            if (entropy > other.entropy)
                return 1;
            else if (entropy < other.entropy)
                return -1;
            else
                return 0;
        }

        public bool Equals(LowEntropyCell x, LowEntropyCell y)
        {
            return x.position.x == y.position.x && x.position.y == y.position.y;
        }

        public int GetHashCode(LowEntropyCell obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }

    }

}


