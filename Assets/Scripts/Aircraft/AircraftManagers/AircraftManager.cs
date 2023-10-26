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

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        _aircraftLoader = new AircraftLoader();
        aircraftManager = this;
    }

    public void RemoveAircraftFromFlight(AircraftFlight flight, string aircraftCallsign) {
        flight.RemoveAircraft(aircraftCallsign);
    }

    public void AddAircraftToFlight(AircraftFlight flight, string aircraftCallsign, AircraftType aircraftType,
        AircraftAltitude altitude, HexCord hexCord)
    {
        var aircraft = CreateAircraft(aircraftType, aircraftCallsign, altitude, hexCord);

        if (flight.flightAircraft.Count > 0)
            aircraft.SetupAircraft(aircraftCallsign, flight.GetSpeed(), flight.GetAltitude(), flight.GetLocation());

        
        flight.AddAircraft(aircraft);
    }

    Aircraft CreateAircraft(AircraftType aircraftType, string aircraftCallsign,
        AircraftAltitude altitude, HexCord hexCord)
    {
        var aircraft = _aircraftLoader.LoadAircraft(aircraftType);

        aircraft.SetupAircraft(aircraftCallsign, AircraftSpeed.Combat, altitude, hexCord);

        return aircraft;
    }

}

