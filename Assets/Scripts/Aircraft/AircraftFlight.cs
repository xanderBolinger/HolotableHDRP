using HexMapper;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AircraftDetectionSuitMethods;
using static AircraftMovementData;
using static AircraftSpeedData;

[Serializable]
public class AircraftFlight
{
    [Serializable]
    public enum FlightStatus {
        Fresh, Disordered, Aborted
    }

    List<Aircraft> _flightAircraft;
    string _flightCallsign;
    AircraftDetectionSuit _aircraftDetectionSuit;

    public AircraftFlightJammerControls jammerControls;

    public FlightStatus flightStatus;
    public ForceSide side;
    public int quality;
    public int agressionValue;

    public bool manueverMarker;
    public bool bvrAvoid;
    public bool climbed;
    public bool zoomClimb;
    public bool disengaing;

    public string flightCallsign { get { return _flightCallsign; } }
    public List<Aircraft> flightAircraft { get { return _flightAircraft; } }

    public AircraftFlight(string flightCallsign, int quality = 0) {
        _aircraftDetectionSuit = GetSuit();
        _flightCallsign = flightCallsign;
        _flightAircraft = new List<Aircraft>();
        jammerControls = new AircraftFlightJammerControls(this);
        this.quality = quality;
        agressionValue = quality;
        flightStatus = FlightStatus.Fresh;
    }

    public bool DisorderdOrAborted() {
        if (flightStatus == FlightStatus.Disordered ||
                flightStatus == FlightStatus.Aborted)
            return true;
        return false;
    }

    public void SpendFuel() {
        foreach (var aircraft in flightAircraft)
            aircraft.movementData.SpendFuel();
    }
    public Vector2Int GetCord() {
        var location = GetLocation();
        return new Vector2Int(location.x, location.y);
    }
    public Direction GetFacing() {
        return flightAircraft[0].movementData.facing;
    }

    public AircraftSpeed GetSpeed() {
        return flightAircraft[0].movementData.speed;
    }

    public AircraftAltitude GetAltitude() {
        return flightAircraft[0].movementData.altitude;
    }

    public AircraftDetectionSuit GetDetectionSuit() {
        return _aircraftDetectionSuit;
    }

    public bool Detected() {
        return flightAircraft[0].aircraftDetectionData.detected;
    }

    public HexCord GetLocation() {
        return flightAircraft[0].movementData.location;
    }

    public void RemoveAircraft(string callsign) {
        if (!InFlight(callsign)) {
            Debug.Log("Could not remove aircraft from flight " + flightCallsign + ", aircraft not in flight.");
            return;
        }

        _flightAircraft.Remove(GetAircraftInFlight(callsign));
        Debug.Log("Removed aircraft " + callsign + " from flight " + flightCallsign);
    }

    public int UndamagedAircraft() {
        int goodAircraft = 0;

        foreach (var aircraft in flightAircraft)
            if (aircraft.damaged || aircraft.crippled)
                continue;

        return goodAircraft;
    }

    public void DamageAircraft(string callsign)
    {
        if (!InFlight(callsign))
        {
            Debug.Log("Could not damage aircraft from flight " + flightCallsign + ", aircraft not in flight.");
            return;
        }
        Debug.Log("Damaged Aircraft: " + callsign + " in flight " + flightCallsign);
        GetAircraftInFlight(callsign).damaged = true;
    }

    public void CrippleAircraft(string callsign)
    {
        if (!InFlight(callsign))
        {
            Debug.Log("Could not cripple aircraft from flight " + flightCallsign + ", aircraft not in flight.");
            return;
        }
        Debug.Log("Crippled Aircraft: " + callsign + " in flight " + flightCallsign);
        GetAircraftInFlight(callsign).crippled = true;
    }

    public void DestroyAircraft(string callsign)
    {
        if (!InFlight(callsign))
        {
            Debug.Log("Could not destroy aircraft from flight " + flightCallsign + ", aircraft not in flight.");
            return;
        }
        Debug.Log("Destroyed Aircraft: "+callsign+" in flight "+flightCallsign);
        GetAircraftInFlight(callsign).destroyed = true;
    }

    Aircraft GetAircraftInFlight(string callsign) {
        foreach (var a in flightAircraft)
            if (a.callsign == callsign)
                return a;

        return null;
    }

    public void AddAircraft(Aircraft aircraft) {
        if (InFlight(aircraft.callsign)) {
            Debug.Log("Could not add aircraft to flight "+flightCallsign+" because aircraft with that callsign already exists.");
            return;
        }
        else if (!CanAddAircraft(aircraft))
        {
            Debug.Log("Aircraft could not be added because aircraft type: " + aircraft.aircraftDisplayName
                    + " does not match aircraft type in flight or aircraft already in flight: " + _flightAircraft.Contains(aircraft));
            return;
        }
        
        _flightAircraft.Add(aircraft);
        Debug.Log("Added aircraft " + aircraft.callsign + " to flight " + flightCallsign);
    }

    public bool InFlight(string callsign) {
        foreach(var aircraft in _flightAircraft)
            if(aircraft.callsign == callsign)
                return true;

        return false;
    }

    public bool CanAddAircraft(Aircraft aircraft) {
        if (_flightAircraft.Contains(aircraft))
            return false;

        foreach (var a in _flightAircraft)
            if (a.aircraftDisplayName != aircraft.aircraftDisplayName)
                return false;

        return true;
    }

    public void ToggleRadar() {
        foreach (var aircraft in flightAircraft)
            aircraft.aircraftDetectionData.aircraftRadar.active = !aircraft.aircraftDetectionData.aircraftRadar.active;
    }

    public override string ToString()
    {
        return AircraftFlightOutput.ToString(this);
    }

}
