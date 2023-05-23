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

        public int tacticalMovement;
        public double spentMPTU;
        public double spentMPTS;
        public double maxMPTS;
        public double maxMPTSExtended;
        public double maxMPTU;
        public double maxMPTUExtended;
        public bool inConflict;
        public bool avoidConflict;
        public bool advancingCombat;
        public bool spentFreeConflictResultMovement;

        public Side side;
        public UnitStatus unitStatus;
        public UnitType unitType;
        public MoveType moveType;
        private List<Unit> units;
        private string side1;

        public OperationUnit(string unitName, GameObject unitGameobject, Vector2Int hexPosition, Side side) {
            this.unitName = unitName;
            this.unitGameobject = unitGameobject;
            this.hexPosition = hexPosition;
            this.side = side;

            unitStatus = UnitStatus.FRESH;
            moveType = MoveType.NONE;
            inConflict = false;
            avoidConflict = false;
            advancingCombat = true;
            spentFreeConflictResultMovement = false;
            unitType = UnitType.INF;
            spentMPTS = 0;
            spentMPTU = 0;
            tacticalMovement = 0;

            units = new List<Unit>();
        }

        // Copy consturctor 
        public OperationUnit(OperationUnit original)
        {
            unitName = original.unitName;
            unitGameobject = original.unitGameobject;
            hexPosition = original.hexPosition;

            tacticalMovement = original.tacticalMovement;
            spentMPTU = original.spentMPTU;
            spentMPTS = original.spentMPTS;

            maxMPTS = original.maxMPTS;
            maxMPTSExtended = original.maxMPTSExtended;

            maxMPTU = original.maxMPTU;
            maxMPTUExtended = original.maxMPTUExtended;

            inConflict = original.inConflict;
            avoidConflict = original.avoidConflict;
            advancingCombat = original.advancingCombat;
            spentFreeConflictResultMovement = original.spentFreeConflictResultMovement;

            side = original.side;
            unitStatus = original.unitStatus;
            unitType = original.unitType;
            moveType = original.moveType;

            units = new List<Unit>(original.units);
        }

        // Empty constructor for testing
        public OperationUnit()
        {
        }

        public OperationUnit(string unitName, string side1, List<Unit> units)
        {
            this.unitName = unitName;
            this.side1 = side1;
            this.units = units;
        }

        public List<Unit> GetUnits()
        {
            return units;
        }

        public Unit GetUnit(int index)
        {
            return units[index];
        }

        public Unit GetUnit(Unit lookUpUnit)
        {
            string identifier = lookUpUnit.identifier;
            foreach (Unit unit in units)
            {
                if (unit.identifier == identifier)
                    return unit;
            }

            return null;
        }

        public Unit GetUnitByName(string unitName)
        {
            foreach (Unit unit in units)
            {
                if (unit.name == unitName)
                    return unit;
            }

            return null;
        }

        public Unit GetUnit(string identifier)
        {
            foreach (Unit unit in units)
            {
                if (unit.identifier == identifier)
                    return unit;
            }

            return null;
        }

        public void AddUnit(Unit unit)
        {
            units.Add(unit);
            DetermineUnitType();
            DetermineMP();
            UnitStatusUpdate();
        }

        public void RemoveUnit(int index)
        {
            units.RemoveAt(index);
            DetermineUnitType();
            DetermineMP();
            UnitStatusUpdate();
        }

        public void RemoveUnit(Unit unit)
        {
            units.Remove(unit);
            DetermineUnitType();
            DetermineMP();
            UnitStatusUpdate();
        }

        public void UnitStatusUpdate()
        {
            int fresh = 0, disoriented = 0, fractured = 0, routed = 0;

            foreach (var subUnit in GetUnits())
            {
                switch (subUnit.unitStatus)
                {

                    case UnitStatus.FRESH:
                        fresh++;
                        break;
                    case UnitStatus.DISORDERED:
                        disoriented++;
                        break;
                    case UnitStatus.FRACTURED:
                        fractured++;
                        break;
                    case UnitStatus.ROUTED:
                        routed++;
                        break;

                }
            }

            int max4 = System.Math.Max(System.Math.Max(fresh, disoriented), System.Math.Max(fractured, routed));

            if (max4 == fresh)
            {
                unitStatus = UnitStatus.FRESH;
            }
            else if (max4 == disoriented)
            {
                unitStatus = UnitStatus.DISORDERED;
            }
            else if (max4 == fractured)
            {
                unitStatus = UnitStatus.FRACTURED;
            }
            else if (max4 == routed)
            {
                unitStatus = UnitStatus.ROUTED;
            }

        }

        public HexType GetTerrainType(List<List<HexCord>> hexes)
        {

            return hexes[hexPosition.x][hexPosition.y].hexType;

        }

        public List<Vector2Int> GetZoneOfControl()
        {
            if (unitStatus == UnitStatus.FRESH || unitStatus == UnitStatus.ROUTED)
            {
                return new List<Vector2Int>();
            }

            return HexDirection.GetHexNeighbours(hexPosition);
        }

        public bool RepulsorEquipped()
        {
            foreach (var unit in units)
            {
                foreach (var victor in unit.GetVehicles())
                {
                    if (!victor.repulsorCraft)
                        return false;
                }
            }

            return unitType == UnitType.INF ? false : true;
        }

        public bool CanMoveDisabledVehicles() {
            int disabledVehicles = 0;
            int towingVehicles = 0;

            foreach (var unit in units)
            {
                foreach (var victor in unit.GetVehicles())
                {
                    if (victor.disabled)
                        disabledVehicles++;
                    else if (victor.vehicleType == Vehicle.VehicleType.ARMOR
                        || victor.vehicleType == Vehicle.VehicleType.HEAVY_WALKER
                        || victor.vehicleType == Vehicle.VehicleType.MECHANIZED
                        || victor.vehicleType == Vehicle.VehicleType.MOTORIZED) {
                        towingVehicles++;
                    }

                }
            }

            return towingVehicles >= disabledVehicles;
        }

        private void DetermineUnitType()
        {
            if(units.Count == 0) {
                unitType = UnitType.INF;
            }

            foreach (Unit unit in units)
            {
                if (unit.unitType == UnitType.ARMOR)
                {
                    unitType = UnitType.ARMOR;
                }
                else if (unit.unitType == UnitType.HEAVY_WALKER
                    && unitType != UnitType.ARMOR)
                {
                    unitType = UnitType.HEAVY_WALKER;
                }
                else if (unit.unitType == UnitType.MECHANIZED && unitType != UnitType.ARMOR && unitType != UnitType.HEAVY_WALKER)
                {
                    unitType = UnitType.MECHANIZED;
                }
                else if (unit.unitType == UnitType.MOTORIZED && unitType != UnitType.MECHANIZED
                    && unitType != UnitType.ARMOR && unitType != UnitType.HEAVY_WALKER)
                {
                    unitType = UnitType.MOTORIZED;
                }
                else if (unit.unitType == UnitType.LIGHT_WALKER && unitType != UnitType.MECHANIZED
                    && unitType != UnitType.ARMOR && unitType != UnitType.HEAVY_WALKER
                    && unitType != UnitType.MOTORIZED)
                {
                    unitType = UnitType.LIGHT_WALKER;
                }
                else if (unit.unitType == UnitType.SPEEDER && unitType != UnitType.MECHANIZED
                    && unitType != UnitType.ARMOR && unitType != UnitType.HEAVY_WALKER
                    && unitType != UnitType.MOTORIZED && unitType != UnitType.LIGHT_WALKER)
                {
                    unitType = UnitType.SPEEDER;
                }
                else if (unit.unitType == UnitType.INF && unitType != UnitType.MECHANIZED
                    && unitType != UnitType.ARMOR && unitType != UnitType.HEAVY_WALKER
                    && unitType != UnitType.MOTORIZED && unitType != UnitType.LIGHT_WALKER && unitType != UnitType.SPEEDER)
                {
                    unitType = UnitType.INF;
                }
            }

            if (!CanTransportAll())
                unitType = UnitType.INF;

        }

        private void DetermineMP()
        {

            

            switch (unitType)
            {
                case UnitType.INF:
                    maxMPTS = 1;
                    maxMPTSExtended = 2;
                    maxMPTU = 4;
                    maxMPTUExtended = 6;
                    break;
                case UnitType.SPEEDER:
                    maxMPTS = 5;
                    maxMPTSExtended = 6;
                    maxMPTU = 18;
                    maxMPTUExtended = 24;
                    break;
                case UnitType.LIGHT_WALKER:
                    maxMPTS = 2;
                    maxMPTSExtended = 3;
                    maxMPTU = 6;
                    maxMPTUExtended = 9;
                    break;
                case UnitType.HEAVY_WALKER:
                    maxMPTS = 2;
                    maxMPTSExtended = 3;
                    maxMPTU = 5;
                    maxMPTUExtended = 7;
                    break;
                case UnitType.ARMOR:
                    maxMPTS = 2;
                    maxMPTSExtended = 3;
                    maxMPTU = 5;
                    maxMPTUExtended = 7;
                    break;
                case UnitType.MECHANIZED:
                    maxMPTS = 2;
                    maxMPTSExtended = 3;
                    maxMPTU = 6;
                    maxMPTUExtended = 9;
                    break;
                case UnitType.MOTORIZED:
                    maxMPTS = 2;
                    maxMPTSExtended = 4;
                    maxMPTU = 8;
                    maxMPTUExtended = 12;
                    break;

            }
        }

        private bool CanTransportAll() {

            int totalInf = 0;
            int totalTransportCapacity = 0;

            foreach (var unit in units)
            {
                totalInf += unit.GetTroopers().Count;
                foreach (var victor in unit.GetVehicles())
                {
                    totalTransportCapacity += victor.transportCapacity;
                }
            }

            return totalInf <= totalTransportCapacity;
        }


        public void Output(OperationManager opm) {

            string output = "Unit Output: \n";
            output += side+":: " + unitName + ", Status: " + unitStatus + ", Unit Type: " +unitType+", Move Type: " + moveType+ "\n";

            output += "used free move: " + spentFreeConflictResultMovement + " \n";

            output += "Spent MP " + spentMPTS + "/" + spentMPTU + ", Max MP: " + maxMPTS + "+" + (maxMPTSExtended - maxMPTS) + "/" +
                +maxMPTU + "+" + (maxMPTUExtended - maxMPTU) + "\n";

            if (opm.currentTimeSegment.plannedMovement.ContainsKey(this))
                output += "Planned Movement: " + opm.currentTimeSegment.plannedMovement[this].ToString()+"\n";
            else
                output += "No planned movement this TS\n";

            int unitCount = 1;
            foreach (var unit in units) {
                output += unitCount + ": "+ unit.GetOutput() + "\n";
                unitCount++;
            }

            
            Debug.Log(output);

        }

    }

}

