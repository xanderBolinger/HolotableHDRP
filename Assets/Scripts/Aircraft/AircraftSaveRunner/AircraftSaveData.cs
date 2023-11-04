using System;
using System.Collections.Generic;

[Serializable]
public class AircraftSaveData
{

    public List<AircraftFlight> flights;

    public AircraftSaveData(List<AircraftFlight> flights)
    {
        this.flights = flights;
    }
}
