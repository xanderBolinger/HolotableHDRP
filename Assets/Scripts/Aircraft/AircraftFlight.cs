using System.Collections.Generic;

public class AircraftFlight
{
    List<Aircraft> _flightAircraft;
    string _flightCallsign;

    public string flightCallsign { get { return _flightCallsign; } }
    public List<Aircraft> flightAircraft { get { return _flightAircraft; } }

    public AircraftFlight(string flightCallsign) {
        _flightCallsign = flightCallsign;
        _flightAircraft = new List<Aircraft>();
    }

    public void AddAircraft(Aircraft aircraft) { 
        if(!CanAddAircraft(aircraft))
            throw new System.Exception("Aircraft could not be added because aircraft type: " + aircraft.aircraftName
                    + " does not match aircraft type in flight or aircraft already in flight: "+_flightAircraft.Contains(aircraft));

        _flightAircraft.Add(aircraft);

    }

    public bool CanAddAircraft(Aircraft aircraft) {
        if (_flightAircraft.Contains(aircraft))
            return false;

        foreach (var a in _flightAircraft)
            if (a.aircraftName != aircraft.aircraftName)
                return false;

        return true;
    }

}
