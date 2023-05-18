using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HexCord;
using static Operation.OperationUnit;

namespace Operation {
    public class OperationMovement
    {

        public static void MoveUnits(OperationManager opm, TimeSegment currentTimeSegment, GridMover gridMover)
        {
            if (gridMover == null)
                return;
            var order = GetMoveOrder(currentTimeSegment);
            foreach (var unit in order) {
                foreach (var cord in currentTimeSegment.plannedMovement[unit]) {
                    if (!CheckZOC(opm, unit, cord))
                        continue;
                    bool clear = opm.hexCords[cord.x][cord.y].hexType == HexType.CLEAR ? true : false;
                    gridMover.MoveUnit(unit, cord, unit.unitGameobject.transform.position, opm.hexes[cord.x][cord.y].transform.position,
                        clear);
                    if (unit.moveType != MoveType.TACTICAL)
                    {
                        unit.spentMPTS += GetMovementCost(unit.unitType, opm.hexCords[cord.x][cord.y].hexType, unit.RepulsorEquipped());
                        unit.spentMPTU += GetMovementCost(unit.unitType, opm.hexCords[cord.x][cord.y].hexType, unit.RepulsorEquipped());
                    }
                    else {
                        unit.tacticalMovement++;
                    }

                }
            }

        }

        private static List<OperationUnit> GetMoveOrder(TimeSegment currentTimeSegment) {
            List<OperationUnit> order = new List<OperationUnit>();
            List<OperationUnit> freshUnits = new List<OperationUnit>();
            List<OperationUnit> disorientedUnits = new List<OperationUnit>();
            List<OperationUnit> fracturedUnits = new List<OperationUnit>();
            List<OperationUnit> routedUnits = new List<OperationUnit>();

            foreach (var dict in currentTimeSegment.plannedMovement)
            {
                var unit = dict.Key;

                switch (unit.unitStatus)
                {
                    case UnitStatus.FRESH:
                        freshUnits.Add(unit);
                        break;
                    case UnitStatus.DISORDERED:
                        disorientedUnits.Add(unit);
                        break;
                    case UnitStatus.FRACTURED:
                        fracturedUnits.Add(unit);
                        break;
                    case UnitStatus.ROUTED:
                        routedUnits.Add(unit);
                        break;
                }

            }

            foreach (var unit in ShuffleList(freshUnits))
                order.Add(unit);
            foreach (var unit in ShuffleList(disorientedUnits))
                order.Add(unit);
            foreach (var unit in ShuffleList(fracturedUnits))
                order.Add(unit);
            foreach (var unit in ShuffleList(routedUnits))
                order.Add(unit);
            return order;
        }

        private static List<T> ShuffleList<T>(List<T> alpha) {
            for (int i = 0; i < alpha.Count; i++)
            {
                var temp = alpha[i];
                int randomIndex = Random.Range(i, alpha.Count);
                alpha[i] = alpha[randomIndex];
                alpha[randomIndex] = temp;
            }
            return alpha;
        }

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
            bool repulsorEquipped = operationUnit.RepulsorEquipped();
            double cost = GetMovementCost(operationUnit.unitType, hexCord.hexType, repulsorEquipped)
                + GetPreviouslyPlannedMovmentCost(opm, operationUnit, hexes, repulsorEquipped);

            if (moveType == MoveType.NONE || !operationUnit.CanMoveDisabledVehicles() 
                || !legalHexes.Contains(new Vector2Int(hexCord.x, hexCord.y))
                || !CheckZOC(opm, operationUnit, new Vector2Int(hexCord.x, hexCord.y))
                || (cost <= 0 && moveType != MoveType.TACTICAL)) {
                Debug.Log("Cannot add planned movement for unit: "+operationUnit.unitName
                    +", Move Type: "+moveType+", Can Tow Disabled Vehicles: "+operationUnit.CanMoveDisabledVehicles()
                    +", Move to position: "+ new Vector2Int(hexCord.x, hexCord.y));
                return false;
            }

            if (moveType != MoveType.TACTICAL)
            {
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

            Debug.LogError("Could not add planned hex for uncaught reason.");
            return false;

        }

        private static bool CheckZOC(OperationManager opm, OperationUnit operationUnit, Vector2Int cord) {

            if (operationUnit.moveType == MoveType.TACTICAL)
                return true;

            foreach (var unit in opm.operationUnits) {
                if (unit.side == operationUnit.side || unit.unitStatus == UnitStatus.FRACTURED 
                    || unit.unitStatus == UnitStatus.ROUTED
                    || unit.avoidConflict) {
                    continue;
                }

                if (HexDirection.GetHexNeighbours(unit.hexPosition).Contains(cord))
                    return false;

            } 

            return true;
        }

        private static double GetPreviouslyPlannedMovmentCost(OperationManager opm, OperationUnit operationUnit, 
            List<List<HexCord>> hexes, bool repulsorEquipped) {
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

        public static List<Conflict> GetConflicts(OperationManager opm) {
            List<Conflict> conflicts = new List<Conflict>();


            // There are two cases in which a conflict can be created 
            // When two opposing units have overlapping zone of control, both units have to not be avoiding conflict.
            // Or when one OU is projecting a zone of control and not avoiding conflict 
            foreach (var unit in opm.operationUnits)
            {
                if (unit.avoidConflict || unit.moveType == MoveType.EXTENDED)
                    continue;

                var zoc = HexDirection.GetHexNeighbours(unit.hexPosition);

                List<OperationUnit> targets = new List<OperationUnit>();

                foreach (var compareUnit in opm.operationUnits)
                {
                    if (compareUnit.side == unit.side)
                        continue;

                    if (zoc.Contains(compareUnit.hexPosition))
                    {
                        targets.Add(compareUnit);
                    }
                    else if (!compareUnit.avoidConflict 
                        && OverlappingZOC(zoc, HexDirection.GetHexNeighbours(compareUnit.hexPosition)))
                    {
                        targets.Add(compareUnit);
                    }

                }

                if (targets.Count > 0) {
                    conflicts.Add(new Conflict(unit, targets));
                }

            }


            return conflicts;
        }

        private static bool OverlappingZOC(List<Vector2Int> a, List<Vector2Int> b) {
            foreach(var item in a) {
                if (b.Contains(item))
                    return true;
            }

            return false;
        }

    }
}


