using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Operation.OperationUnit;

namespace Operation {
    [Serializable]
    public class Unit
    {
        public string name;
        public bool spotted;

        public UnitType unitType;
        public UnitStatus unitStatus;
        
        public string identifier;

        private List<Trooper> troopers;
        private List<Vehicle> vehicles;

        public Unit(string name, string identifier, List<Trooper> troopers, List<Vehicle> vehicles)
        {
            this.name = name;
            this.identifier = identifier;
            this.troopers = troopers;
            this.vehicles = vehicles;
        }

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

        public List<Trooper> GetTroopers() {
            return troopers;
        }

        public Vehicle GetVehicle(int index)
        {
            return vehicles[index];
        }

        public List<Vehicle> GetVehicles() {
            return vehicles;
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

        public string GetOutput()
        {
            string output = name + ", Unit Type: "+unitType+", Unit Status: "+unitStatus+"\n";

            int trooperCount = 1;
            output += "     -Troopers: \n";
            foreach (var trooper in troopers) {
                output += "         "+trooperCount+": "+trooper.name +", SL:"+trooper.sl+ "\n";
                trooperCount++;
            }
            output += "     -Vehicles: \n";
            int vehicleCount = 1;
            foreach (var vehicle in vehicles)
            {
                output += "         " +vehicleCount+": " +vehicle.callsign + ", Class: " + vehicle.vehicleClass+", Type: "+vehicle.vehicleType+", Disabled: "+vehicle.disabled
                    + ", Repulsor Craft: " + vehicle.repulsorCraft+", Transport Capacity: "+ vehicle.transportCapacity +  "\n";
                vehicleCount++;
            }

            return output;
        }



    }
}


