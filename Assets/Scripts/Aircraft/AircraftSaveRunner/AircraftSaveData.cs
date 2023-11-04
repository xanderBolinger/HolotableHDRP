using System;
using System.Collections.Generic;

[Serializable]
public class AircraftSaveData
{

    public List<AircraftFlight> flights;
    public List<FlightHexCordSaveData> cords;

    public AircraftSaveData(List<AircraftFlight> flights)
    {
        this.flights = flights;
        cords = new List<FlightHexCordSaveData>();

        foreach (var flight in flights) {
            cords.Add(new FlightHexCordSaveData(flight.GetLocation()));
        }

    }

    [Serializable]
    public class FlightHexCordSaveData {
        public int x;
        public int y;
        public bool rough;

        public FlightHexCordSaveData(HexCord hexCord) {
            x = hexCord.x;
            y = hexCord.y;
            rough = hexCord.Rough();
        }

    }

}


