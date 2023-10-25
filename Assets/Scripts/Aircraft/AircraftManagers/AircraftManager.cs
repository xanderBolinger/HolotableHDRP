using System.Collections.Generic;
using UnityEngine;

public class AircraftManager : MonoBehaviour
{
    [HideInInspector]
    public List<string> testAircraftFlightDisplayList = new List<string>();
    [HideInInspector]
    public int selectedAircraftFlightIndex;

    List<AircraftFlight> _aircraftFlights;

    void Start()
    {
        _aircraftFlights = new List<AircraftFlight>();
    }

    public void AddFlight(string flightName) {
        if (!CanAddFlight(flightName))
            return;

        _aircraftFlights.Add(new AircraftFlight(flightName));
    }

    public bool CanAddFlight(string flightName) {


        return true;
    }

}

