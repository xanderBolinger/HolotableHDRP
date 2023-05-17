using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HexCord;
using static Operation.OperationUnit;

namespace Operation {
    public class OperationMovement
    {

        public static double GetMovementCost(UnitType unitType, HexType hexType, bool repulsorEquipped)
        {
            int BASE = 0, REPULSOR = 1, TRACKED = 2, WHEELED = 3;

            Dictionary<HexType, List<double>> table = new Dictionary<HexType, List<double>>();

            List<double> clear = new List<double> { 1, 1, 1, 1 };
            List<double> woodedOrRough = new List<double> { 1, 2, 2, 2 };
            List<double> mountain = new List<double> { 2, -1, -1, -1 };
            List<double> town = new List<double> { 1, 1, 1, 1 };
            List<double> city = new List<double> { 1, 2, 2, 2 };
            List<double> road = new List<double> { 1, 0.5, 0.5, 0.5 };
            List<double> highway = new List<double> { 1, 0.333, 0.333, 0.333 };

            table.Add(HexType.CLEAR, clear);
            table.Add(HexType.WOODS, woodedOrRough);
            table.Add(HexType.MOUNTAIN, mountain);
            table.Add(HexType.TOWN, town);
            table.Add(HexType.CITY, city);
            table.Add(HexType.PATH, road);
            table.Add(HexType.HIGHWAY, highway);


            int index;

            switch (unitType) {

                case UnitType.INF:
                    index = BASE;
                    break;
                case UnitType.ARMOR:
                    index = TRACKED;
                    break;
                case UnitType.MECHANIZED:
                    index = TRACKED;
                    break;
                case UnitType.MOTORIZED:
                    index = WHEELED;
                    break;
                case UnitType.LIGHT_WALKER:
                    index = BASE;
                    break;
                case UnitType.HEAVY_WALKER:
                    index = BASE;
                    break;
                case UnitType.SPEEDER:
                    index = REPULSOR;
                    break;
                default:
                    throw new System.Exception("Unit type not found: "+unitType);
            
            }

            return table[hexType][repulsorEquipped ? REPULSOR : index];

        }

        public static bool AddPlannedMovement(OperationManager opm, OperationUnit operationUnit, HexCord hexCord, List<List<HexCord>> hexes,
            Vector2Int lastPosition) {
            MoveType moveType = operationUnit.moveType;

            var legalHexes = HexDirection.GetHexNeighbours(lastPosition);

            if (moveType == MoveType.NONE || !operationUnit.CanMoveDisabledVehicles() 
                || !legalHexes.Contains(new Vector2Int(hexCord.x, hexCord.y))) {
                Debug.Log("Cannot add planned movement for unit: "+operationUnit.unitName
                    +", Move Type: "+moveType+", Can Tow Disabled Vehicles: "+operationUnit.CanMoveDisabledVehicles()
                    +", Move to position: "+ new Vector2Int(hexCord.x, hexCord.y));
                return false;
            }

            if (moveType != MoveType.TACTICAL)
            {
                bool repulsorEquipped = operationUnit.RepulsorEquipped();
                double cost = GetMovementCost(operationUnit.unitType, hexCord.hexType, repulsorEquipped)
                    + GetPreviouslyPlannedMovmentCost(opm, operationUnit, hexes, repulsorEquipped);

                // extended movement not ending in enemy ZOC

                if (operationUnit.spentMPTS + cost <= (moveType == MoveType.REGULAR ? operationUnit.maxMPTS : operationUnit.maxMPTSExtended)
                    && operationUnit.spentMPTU + cost <= (moveType == MoveType.REGULAR ? operationUnit.maxMPTU : operationUnit.maxMPTUExtended))
                {
                    Debug.Log("Add planned hex: ("+hexCord.x+", "+hexCord.y+")");
                    AddPlannedHex(opm, operationUnit, hexCord);
                    return true;
                }
                
                return false;
            }
            else if(operationUnit.tacticalMovement + 1 <= 2) {
                Debug.Log("Add planned hex: (" + hexCord.x + ", " + hexCord.y + ")");
                AddPlannedHex(opm, operationUnit, hexCord);
                return true;
            }
            return false;

        }

        private static double GetPreviouslyPlannedMovmentCost(OperationManager opm, OperationUnit operationUnit, List<List<HexCord>> hexes, bool repulsorEquipped) {
            double total = 0;

            if (opm.currentTimeSegment.plannedMovement.ContainsKey(operationUnit))
            {
                foreach (var item in opm.currentTimeSegment.plannedMovement[operationUnit]) {
                    total += GetMovementCost(operationUnit.unitType, hexes[item.x][item.y].hexType, repulsorEquipped);
                }
            }

            return total;
        }

        private static void AddPlannedHex(OperationManager opm, OperationUnit operationUnit, HexCord hexCord) {
            if (opm.currentTimeSegment.plannedMovement.ContainsKey(operationUnit))
            {
                opm.currentTimeSegment.plannedMovement[operationUnit].Add(new Vector2Int(hexCord.x, hexCord.y));
            }
            else {
                opm.currentTimeSegment.plannedMovement.Add(operationUnit, 
                    new List<Vector2Int> { new Vector2Int(hexCord.x, hexCord.y) });
            }
        }

        private static List<Conflict> GetConflicts(OperationManager opm) {
            List<Conflict> conflicts = new List<Conflict>();

            var currentTimeSegment = opm.currentTimeSegment;




            return conflicts;
        }

        private static void AddConflictToList(Conflict conflict, List<Conflict> conflicts) { 
        
        }

    }
}


