using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Operation {
    public class Vehicle
    {
        public enum VehicleClass { 
            ATTE,
            
            AAT
        }

        public enum VehicleType
        {
            ARMOR, MECHANIZED, MOTORIZED, LIGHT_WALKER, HEAVY_WALKER,SPEEDER
        }

        public string callsign;
        public VehicleType vehicleType;
        public string vehicleClass;
        public bool repulsorCraft;
        public bool disabled;
        public int transportCapacity;
        public string identifier;

        public Vehicle(string callsign, VehicleType vehicleType, string vehicleClass, bool repulsorCraft, int transportCapacity)
        {
            this.callsign = callsign;
            this.vehicleType = vehicleType;
            this.vehicleClass = vehicleClass;
            this.repulsorCraft = repulsorCraft;
            this.transportCapacity = transportCapacity;
            this.identifier = Identifier.GenerateIdentifier();
            disabled = false;
        }

        public Vehicle(VehicleClass vicClass, string callsign) {

            switch (vicClass) {
                case VehicleClass.ATTE:
                    this.callsign = callsign;
                    vehicleType = VehicleType.HEAVY_WALKER;
                    vehicleClass = "ATTE";
                    repulsorCraft = false;
                    transportCapacity = 27;
                    break;
                case VehicleClass.AAT:
                    this.callsign = callsign;
                    vehicleType = VehicleType.ARMOR;
                    vehicleClass = "AAT";
                    repulsorCraft = true;
                    transportCapacity = 3;
                    break;
            }


            this.identifier = Identifier.GenerateIdentifier();
        }

    }
}


