using System.Collections.Generic;
using UnityEngine;
using static AircraftMovementData;
using static AircraftSpeedData;

public class AircraftManager : MonoBehaviour
{
    [InspectorName("Add Flight Callsign")]
    public string testAddFlightCallsign;
    [InspectorName("Add at X")]
    public int testAddFlightX;
    [InspectorName("Add at Y")]
    public int testAddFlightY;
    [InspectorName("Add Hex Rough")]
    public bool testAddFlightRoughTerrain;

    [HideInInspector]
    public List<string> testAircraftFlightDisplayList = new List<string>();
    [HideInInspector]
    public int selectedAircraftFlightIndex;

    List<AircraftFlight> _aircraftFlights;

    public List<AircraftFlight> aircraftFlights { get { return _aircraftFlights; } }

    public static AircraftManager aircraftManager;

    void Start()
    {
        Setup();
    }

    public void Setup() {
        aircraftManager = this;
        _aircraftFlights = new List<AircraftFlight>();
    }

    public void AddFlight(string flightCallsign)
    {
        if (!CanAddFlight(flightCallsign))
            return;

        var cord = AircraftMovementManager.CreateTestHexCord(testAddFlightRoughTerrain, testAddFlightX, testAddFlightY);
        var flight = new AircraftFlight(flightCallsign);
        var v19 = AircraftLoader.LoadAircraftJson("V19");
        v19.SetupAircraft("hitman", AircraftSpeed.Combat, AircraftAltitude.VERY_HIGH, cord);
        var v192 = AircraftLoader.LoadAircraftJson("V19");
        v192.SetupAircraft("hitman2", AircraftSpeed.Combat, AircraftAltitude.VERY_HIGH,cord);
        flight.AddAircraft(v19);
        flight.AddAircraft(v192);


        _aircraftFlights.Add(flight);
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

