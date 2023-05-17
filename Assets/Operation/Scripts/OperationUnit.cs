using HexMapper;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using static HexCord;

namespace Operation {
    public class OperationUnit
    {
        public enum Side { 
            BLUFOR,OPFOR,IND
        }
        public enum UnitStatus { 
            FRESH,DISORDERED,FRACTURED,ROUTED
        }
        public enum UnitType { 
            INF,ARMOR,MECHANIZED,MOTORIZED,LIGHT_WALKER,HEAVY_WALKER,SPEEDER
        }
        public enum MoveType { 
            NONE,REGULAR,EXTENDED,TACTICAL
        }

        public string unitName;
        public GameObject unitGameobject;
        public Vector2Int hexPosition;

        public bool inConflict;
        public bool avoidConflict;
        public bool advancingCombat;
        public bool spentFreeConflictResultMovement;

        public Side side;
        public UnitStatus unitStatus;
        public UnitType ouType;
        public MoveType moveType;

        public HexType GetTerrainType(List<List<HexCord>> hexes) {

            return hexes[hexPosition.x][hexPosition.y].hexType;

        }

        public List<Vector2Int> GetZoneOfControl() {
            if (unitStatus == UnitStatus.FRESH || unitStatus == UnitStatus.ROUTED) {
                return new List<Vector2Int>();
            }

            return HexDirection.GetHexNeighbours(hexPosition);
        }

    }

}

