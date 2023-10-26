using System.Collections.Generic;
using UnityEngine;
using static AircraftLoader;
using static AircraftMovementData;
using static AircraftSpeedData;

public class AircraftManager : MonoBehaviour
{
    public static AircraftManager aircraftManager;

    [InspectorName("Aircraft Type")]
    public AircraftType testCreateAircraftType;
    [InspectorName("Starting Altitude")]
    public AircraftAltitude testStartingAircraftAltitude;
    [InspectorName("Aircraft Callsign")]
    public string testCreateAircraftCallsign;

    AircraftLoader _aircraftLoader;

    public void Setup()
    {
        _aircraftLoader = new AircraftLoader();
        aircraftManager = this;
    }

    public Aircraft CreateAircraft(AircraftType aircraftType, string aircraftCallsign,
        AircraftAltitude altitude, HexCord hexCord)
    {
        var aircraft = _aircraftLoader.LoadAircraft(aircraftType);

        aircraft.SetupAircraft(aircraftCallsign, AircraftSpeed.Combat, altitude, hexCord);

        return aircraft;
    }

}

