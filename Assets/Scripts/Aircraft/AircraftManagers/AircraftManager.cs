using System.Collections.Generic;
using UnityEngine;
using static AircraftLoader;
using static AircraftMovementData;
using static AircraftSpeedData;

public class AircraftManager : MonoBehaviour
{
    public static AircraftManager aircraftManager;

    [InspectorName("Add Flight Callsign")]
    public string testAddFlightCallsign;
    [InspectorName("Add at X")]
    public int testAddFlightX;
    [InspectorName("Add at Y")]
    public int testAddFlightY;
    [InspectorName("Add Hex Rough")]
    public bool testAddFlightRoughTerrain;

    AircraftLoader _aircraftLoader;
    List<AircraftFlight> _aircraftFlights;

    [HideInInspector]
    public List<string> testAircraftFlightDisplayList = new List<string>();
    [HideInInspector]
    public int selectedAircraftFlightIndex;

    public List<AircraftFlight> aircraftFlights { get { return _aircraftFlights; } }

    void Start()
    {
        Setup();
    }

    public void Setup() {
        aircraftManager = this;
        _aircraftFlights = new List<AircraftFlight>();
        _aircraftLoader = new AircraftLoader();
    }

    public void AddFlight(string flightCallsign)
    {
        if (!CanAddFlight(flightCallsign))
            return;


        testAircraftFlightDisplayList.Add(flightCallsign);
        Debug.Log("Add flight: " + flightCallsign);
    }

    public void AddAircraft(AircraftType aircraftType, string flightCallsign,
        string aircraftCallsign) {
        var aircraft = _aircraftLoader.LoadAircraft(aircraftType);


        var flight = FindFlight(flightCallsign);
        aircraft.SetupAircraft(aircraftCallsign, flight.GetSpeed(), flight.GetAltitude(), flight.GetLocation());
        flight.AddAircraft(aircraft);
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

