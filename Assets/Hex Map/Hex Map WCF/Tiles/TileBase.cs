using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WaveFunctionCollapse {
    public class TileBase
    {
        public HexCord.HexType hexType;

        public TileBase(HexCord.HexType hexType) {
            this.hexType = hexType;
        }
    }
}


