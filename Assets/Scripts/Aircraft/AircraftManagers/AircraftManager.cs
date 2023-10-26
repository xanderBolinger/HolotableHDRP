using System.Collections.Generic;
using UnityEngine;
using static AircraftLoader;
using static AircraftMovementData;
using static AircraftSpeedData;

public class AircraftManager : MonoBehaviour
{
    public static AircraftManager aircraftManager;

    [SerializeField]
    AircraftType testCreateAircraftType;
    [SerializeField]
    string testCreateAircraftCallsign;

    AircraftLoader _aircraftLoader;

    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        aircraftManager = this;
        _aircraftLoader = new AircraftLoader();
    }

    public Aircraft CreateAircraft(AircraftType aircraftType, string aircraftCallsign,
        AircraftAltitude altitude, HexCord hexCord)
    {
        var aircraft = _aircraftLoader.LoadAircraft(aircraftType);

        aircraft.SetupAircraft(aircraftCallsign, AircraftSpeed.Combat, altitude, hexCord);

        return aircraft;
    }

}

