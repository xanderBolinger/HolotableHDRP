using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Operation {
    public class OperationUnitLoader
    {

        public static OperationUnit LoadJSON(string fileName)
        {
            TextAsset asset = Resources.Load("OperationUnits/"+fileName, typeof(TextAsset)) as TextAsset;
            string jsonString = asset.text;
            var ouJson = JsonConvert.DeserializeObject<OuRoot>(jsonString);
            

            return ouJson.GetOu();
        }
    }
    public class OuRoot
    {
        public string unitName { get; set; }
        public string side { get; set; }
        public List<JSONUnit> units { get; set; }

        public OperationUnit GetOu() {
            List<Unit> units = new List<Unit>();

            foreach (var item in this.units) {
                units.Add(item.GetUnit());
            }

            return new OperationUnit(unitName, side, units);
        }
    }

    public class JSONTrooper
    {
        public string identifier { get; set; }
        public string name { get; set; }
        public int sl { get; set; }

        public Trooper GetTrooper() {
            return new Trooper(identifier, name, sl);
        }

    }

    public class JSONUnit
    {
        public string name { get; set; }
        public string identifier { get; set; }
        public List<JSONTrooper> troopers { get; set; }
        public List<JSONVehicle> vehicles { get; set; }

        public Unit GetUnit()
        {
            List<Trooper> troopers = new List<Trooper>();
            List<Vehicle> vehicles = new List<Vehicle>();

            foreach (var item in this.troopers)
                troopers.Add(item.GetTrooper());
            foreach (var item in this.vehicles)
                vehicles.Add(item.GetVehicle());

            return new Unit(name, identifier, troopers, vehicles);
        }
    }

    public class JSONVehicle
    {
        public string callsign { get; set; }
        public string vehicleType { get; set; }
        public string vehicleClass { get; set; }
        public bool repulsorCraft { get; set; }
        public bool disabled { get; set; }
        public int transportCapacity { get; set; }
        public string identifier { get; set; }

        public Vehicle GetVehicle()
        {
            return new Vehicle(callsign, vehicleType, vehicleClass, repulsorCraft, disabled, transportCapacity, identifier);
        }
    }
}


