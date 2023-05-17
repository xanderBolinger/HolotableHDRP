using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Operation.OperationUnit;

namespace Operation {
    public class Unit
    {
        public string name;
        public bool spotted;
        public UnitType unitType;
        public UnitStatus unitStatus;
        public string identifier;
        
        List<Trooper> troopers;
        List<Vehicle> vehicles;

        public Unit(string name) {
            this.name = name;
            this.identifier = Identifier.GenerateIdentifier();
            unitType = UnitType.INF;
            unitStatus = UnitStatus.FRESH;
            spotted = false;
            troopers = new List<Trooper>();
            vehicles = new List<Vehicle>();
        }

        public void DetermineUnitType()
        {
            if (vehicles.Count == 0) { 
                unitType = UnitType.INF;
            }

            foreach (Vehicle vic in vehicles) {
                if (vic.vehicleType == Vehicle.VehicleType.ARMOR)
                {
                    unitType = UnitType.ARMOR;
                }
                else if (vic.vehicleType == Vehicle.VehicleType.HEAVY_WALKER 
                    && unitType != UnitType.ARMOR)
                {
                    unitType = UnitType.HEAVY_WALKER;
                }
                else if (vic.vehicleType == Vehicle.VehicleType.MECHANIZED && unitType != UnitType.ARMOR && unitType != UnitType.HEAVY_WALKER) {
                    unitType = UnitType.MECHANIZED;
                }
                else if (vic.vehicleType == Vehicle.VehicleType.MOTORIZED && unitType != UnitType.MECHANIZED 
                    && unitType != UnitType.ARMOR && unitType != UnitType.HEAVY_WALKER) {
                    unitType = UnitType.MOTORIZED;
                }
                else if (vic.vehicleType == Vehicle.VehicleType.LIGHT_WALKER && unitType != UnitType.MECHANIZED 
                    && unitType != UnitType.ARMOR && unitType != UnitType.HEAVY_WALKER
                    && unitType != UnitType.MOTORIZED)
                {
                    unitType = UnitType.LIGHT_WALKER;
                }
                else if (vic.vehicleType == Vehicle.VehicleType.SPEEDER && unitType != UnitType.MECHANIZED 
                    && unitType != UnitType.ARMOR && unitType != UnitType.HEAVY_WALKER
                    && unitType != UnitType.MOTORIZED && unitType != UnitType.LIGHT_WALKER)
                {
                    unitType = UnitType.SPEEDER;
                }
            }

        }

        public Trooper GetTrooper(int index) {
            return troopers[index];
        }

        public Trooper GetTrooper(string identifier) {
            foreach (Trooper trooper in troopers) {
                if (trooper.identifier == identifier)
                    return trooper;
            }

            return null;
        }

        public void AddTrooper(Trooper trooper) {
            troopers.Add(trooper);
            DetermineUnitType();
        }

        public void RemoveTrooper(int index)
        {
            troopers.RemoveAt(index);
            DetermineUnitType();
        }

        public void RemoveTrooper(Trooper trooper) {
            troopers.Remove(trooper);
            DetermineUnitType();
        }

        public Vehicle GetVehicle(int index)
        {
            return vehicles[index];
        }

        public Vehicle GetVehicle(string identifier)
        {
            foreach (Vehicle vic in vehicles)
            {
                if (vic.identifier == identifier)
                    return vic;
            }

            return null;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
            DetermineUnitType();
        }

        public void RemoveVehicle(int index)
        {
            vehicles.RemoveAt(index);
            DetermineUnitType();
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            vehicles.Remove(vehicle);
            DetermineUnitType();
        }

    }
}


