using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WaveFunctionCollapse {

    public class PatternManager
    {
        Dictionary<int, PatternData> patternDataIndexDictionary;
        Dictionary<int, PatternNeighbours> patternPossibleNeighboursDictionary;
        int patternSize = -1;
        IFindNeighbourStrategy strategy;
         

    }

}


