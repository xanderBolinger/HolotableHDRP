﻿using System.Collections.Generic;
using UnityEngine;
using static AircraftLoader;
using static AircraftMovementData;
using static FlightQualityTable;

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

    [HideInInspector]
    public List<string> testTargetAircraftFlightDisplayList = new List<string>();
    [HideInInspector]
    public int selectedTargetAircraftFlightIndex;

    public FlightQuality inspectorFlightQuality = FlightQuality.Regular;

    public List<AircraftFlight> aircraftFlights { get { return _aircraftFlights; } }

    FlightQualityTable flightQualityTable;

    void Awake()
    {
        Setup();
    }

    public void Setup() {
        aircraftFlightManager = this;
        _aircraftFlights = new List<AircraftFlight>();
        flightQualityTable = new FlightQualityTable();
    }

    public void AddFlight(string flightCallsign, ForceSide forceSide, FlightQuality quality=FlightQuality.Regular)
    {
        if (!CanAddFlight(flightCallsign))
            return;

        AircraftFlight flight = new AircraftFlight(flightCallsign, flightQualityTable.GetValue(quality));
        flight.side = forceSide;
        AddFlight(flight);
    }

    void AddFlight(AircraftFlight flight) {
        var flightCallsign = flight.flightCallsign;
        aircraftFlights.Add(flight);
        testTargetAircraftFlightDisplayList.Add(flightCallsign);
        testAircraftFlightDisplayList.Add(flightCallsign);
        Debug.Log("Add flight: " + flightCallsign);
    }

    public void LoadFlights(AircraftSaveData data) {
        aircraftFlights.Clear();
        testTargetAircraftFlightDisplayList.Clear();
        testAircraftFlightDisplayList.Clear();

        for (int i = 0; i < data.flights.Count; i++) {

            var flight = data.flights[i];
            var cordData = data.cords[i];
            var cord = AircraftHexCordManager.CreateHexCord(cordData.rough, cordData.x, cordData.y);
            foreach (var aircraft in flight.flightAircraft)
                aircraft.movementData.location = cord;
            AddFlight(flight);
        }

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
        testTargetAircraftFlightDisplayList.Remove(flightCallsign);
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