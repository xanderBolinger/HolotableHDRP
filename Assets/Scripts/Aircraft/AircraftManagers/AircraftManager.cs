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

    private void Awake()
    {
        Setup();
    }

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

    public void RemoveAircraftFromFlight(AircraftFlight flight, string aircraftCallsign) {
        if (!AircraftExistInFlight(flight, aircraftCallsign)) {
            Debug.Log("Could not remove aircraft with callsign: "+aircraftCallsign
                +", because aircraft not in flight: "+flight.flightCallsign);
            return;
        }

        var aircraft = GetAircraftInFlight(flight, aircraftCallsign);
        flight.flightAircraft.Remove(aircraft);
        Debug.Log("Removed aircraft " + aircraft.callsign + " from flight " + flight.flightCallsign);
    }

    public void AddAircraftToFlight(AircraftFlight flight, string aircraftCallsign, AircraftType aircraftType,
        AircraftAltitude altitude, HexCord hexCord)
    {
        if (AircraftExistInFlight(flight, aircraftCallsign))
        {
            Debug.Log("Aircraft with same callsign already in flight.");
            return;
        }

        var aircraft = CreateAircraft(aircraftType, aircraftCallsign, altitude, hexCord);

        if (flight.flightAircraft.Count > 0)
            aircraft.SetupAircraft(aircraftCallsign, flight.GetSpeed(), flight.GetAltitude(), flight.GetLocation());

        Debug.Log("Added aircraft " + aircraft.callsign + " to flight " + flight.flightCallsign);
        flight.AddAircraft(aircraft);
    }

    bool AircraftExistInFlight(AircraftFlight flight, string callsign) {
        return GetAircraftInFlight(flight, callsign) != null;
    }

    Aircraft GetAircraftInFlight(AircraftFlight flight, string callsign) {
        foreach (var a in flight.flightAircraft)
            if (a.callsign == callsign)
                return a;

        return null;
    }

}

