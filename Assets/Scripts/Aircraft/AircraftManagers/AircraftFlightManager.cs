using System.Collections.Generic;
using UnityEngine;
using static AircraftLoader;
using static AircraftMovementData;

[RequireComponent(typeof(AircraftManager))]
public class AircraftFlightManager : MonoBehaviour
{
    public static AircraftFlightManager aircraftFlightManager;

    [InspectorName("Add Flight Callsign")]
    public string testAddFlightCallsign;

    List<AircraftFlight> _aircraftFlights;

    [HideInInspector]
    public List<string> testAircraftFlightDisplayList = new List<string>();
    [HideInInspector]
    public int selectedAircraftFlightIndex;

    public List<AircraftFlight> aircraftFlights { get { return _aircraftFlights; } }

    void Awake()
    {
        Setup();
    }

    public void Setup() {
        aircraftFlightManager = this;
        _aircraftFlights = new List<AircraftFlight>();
    }

    public void AddFlight(string flightCallsign, ForceSide forceSide)
    {
        if (!CanAddFlight(flightCallsign))
            return;

        AircraftFlight flight = new AircraftFlight(flightCallsign);
        flight.side = forceSide;
        aircraftFlights.Add(flight);

        testAircraftFlightDisplayList.Add(flightCallsign);
        Debug.Log("Add flight: " + flightCallsign);
    }

    

    public bool CanAddFlight(string flightCallsign) {

        return !FlightExists(flightCallsign);
        
    }

    public void RemoveFlight(string flightCallsign)
    {
        if (!FlightExists(flightCallsign)) {
            Debug.Log("Flight with that callsign does not exist in aircraft manager.");
            return;
        }

        _aircraftFlights.Remove(FindFlight(flightCallsign));
        testAircraftFlightDisplayList.Remove(flightCallsign);
        Debug.Log("Remove flight: " + flightCallsign);
    }


    AircraftFlight FindFlight(string flightCallsign)
    {
        AircraftFlight flight = null;

        foreach (var f in _aircraftFlights)
        {
            if (f.flightCallsign == flightCallsign)
            {
                flight = f;
                break;
            }
        }

        return flight;
    }

    bool FlightExists(string flightCallsign)
    {
        foreach (var flight in _aircraftFlights)
        {
            if (flight.flightCallsign == flightCallsign)
            {
                Debug.Log("Flight with that callsign already exists in aircraft manager.");
                return true;
            }
        }

        
        return false;
    }

    public void PrintFlight(string callsign) {

        var flight = FindFlight(callsign);

        Debug.Log(flight.ToString());
    }

    public void PrintFlights() {
        foreach (var flight in _aircraftFlights)
            Debug.Log(flight.ToString()+"\n");
    }

}

