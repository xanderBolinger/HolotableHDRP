using HexMapper;
using System.Collections.Generic;
using UnityEngine;
using static AircraftMovementData;
using static AircraftSpeedData;

public class AircraftFlight
{
    List<Aircraft> _flightAircraft;
    string _flightCallsign;


    public ForceSide side;

    public string flightCallsign { get { return _flightCallsign; } }
    public List<Aircraft> flightAircraft { get { return _flightAircraft; } }

    public AircraftFlight(string flightCallsign) {
        _flightCallsign = flightCallsign;
        _flightAircraft = new List<Aircraft>();
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


    public override string ToString()
    {
        string aircraft = "";

        foreach (var a in _flightAircraft) {
            aircraft += "- " + a.ToString()+"\n";
        }

        string aircraftMoveData = "";

        if (_flightAircraft.Count > 0) {
            aircraftMoveData += "Facing: " + GetFacing() + "\n";
            aircraftMoveData += "Cord: "+GetLocation().GetCord()+"\n";
            aircraftMoveData += "alt: cmbt/dash/manvr\n";

            var a = _flightAircraft[0];

            aircraftMoveData += GetAircraftSpeedString(a.movementData);

        }

        string alt = _flightAircraft.Count > 0 ? _flightAircraft[0].movementData.altitude.ToString() : "N/A";
        string spd = _flightAircraft.Count > 0 ? _flightAircraft[0].movementData.speed.ToString() : "N/A";

        return "Flight: "+flightCallsign +", Alt: "+alt+", Spd: "+spd + "\n" + aircraft + aircraftMoveData;
    }

    private string GetAircraftSpeedString(AircraftMovementData md)
    {

        string speedString = "";

        // Unladen
        speedString += GetSingleSpeedString(md, false) + "\nLaden: \n";

        // Laden
        speedString += GetSingleSpeedString(md, true);

        return speedString;

    }

    private string GetSingleSpeedString(AircraftMovementData md, bool laden)
    {

        string speedLow = GetSpeed(md, AircraftAltitude.LOW, laden);
        string speedDeck = GetSpeed(md, AircraftAltitude.DECK, laden);

        return "VH: " + GetSpeed(md, AircraftAltitude.VERY_HIGH, laden) + "\n H: " + GetSpeed(md, AircraftAltitude.HIGH, laden)
                + "\n M: " + GetSpeed(md, AircraftAltitude.MEDIUM, laden)
                + (speedLow == speedDeck ? "\n L/D: " + speedLow : "\n L: " + speedLow + "\n D: " + speedDeck);
    }

    private string GetSpeed(AircraftMovementData md, AircraftAltitude altitude, bool laden)
    {
        int cmd = md.GetSpeed(AircraftSpeed.Combat, altitude, laden);
        int dash = md.GetSpeed(AircraftSpeed.Dash, altitude, laden);
        int man = md.GetSpeed(AircraftSpeed.Manuever, altitude, laden);

        return cmd + "/" + dash + "/" + man;
    }

}
