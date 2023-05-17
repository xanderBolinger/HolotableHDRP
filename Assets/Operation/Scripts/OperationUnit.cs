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

        List<Unit> units;

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

        public OperationUnit()
        {
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
        }

        public void RemoveUnit(int index)
        {
            units.RemoveAt(index);
            DetermineUnitType();
            DetermineMP();
        }

        public void RemoveUnit(Unit unit)
        {
            units.Remove(unit);
            DetermineUnitType();
            DetermineMP();
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

        
    }

}

