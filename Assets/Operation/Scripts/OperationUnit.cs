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
        public int spentMP;

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
            spentMP = 0;

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
        }

        public void RemoveUnit(int index)
        {
            units.RemoveAt(index);
            DetermineUnitType();
        }

        public void RemoveUnit(Unit unit)
        {
            units.Remove(unit);
            DetermineUnitType();
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
            }
        }

    }

}

