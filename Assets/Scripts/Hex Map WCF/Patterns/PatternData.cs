using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WaveFunctionCollapse {

    public class PatternData
    {
        private Pattern pattern;
        private int frequency = 1;
        private float frequencyRelative;
        private float frequencyRelativeLog2;

        public float FrequencyRelative { get => frequencyRelative; }
        public Pattern Pattern { get => pattern; }
        public float FrequencyRelativeLog2 { get => frequencyRelativeLog2; }

        public PatternData(Pattern pattern) {
            this.pattern = pattern;
        }

        public void AddToFrequency() {
            frequency++;
        }

        public void CalculateRelativeFrequency(int total) {
            frequencyRelative = (float)frequency / total;
            frequencyRelativeLog2 = Mathf.Log(frequencyRelative, 2);
        }

        public bool CompareGrid(Direction dir, PatternData data) {
            return pattern.ComparePatternToAnotherPattern(dir, data.Pattern);
        }

    }

}

