using HexMapper;
using System;

[Serializable]
public class AircraftFlightJammerControls
{

    AircraftFlight _flight;

    public AircraftFlightJammerControls(AircraftFlight flight) {
        _flight = flight;
    }

    public AircraftStandoffJammer GetSoj() {
        return GetSoj(_flight);
    }

    public static AircraftStandoffJammer GetSoj(AircraftFlight flight) {
        return GetSoj(flight.flightAircraft[0]);
    }

    public static AircraftStandoffJammer GetSoj(Aircraft aircraft)
    {
        return aircraft.aircraftJammerData.aircraftStandoffJammer;
    }

    public bool SojActive() {
        return GetSoj().active;
    }

    public void ToggleSojs() {
        foreach (var aircraft in _flight.flightAircraft) {
            var soj = GetSoj(aircraft);
            soj.active = !soj.active;
        }      
    }

    public void SetSojFacing(Direction facing) {
        foreach (var aircraft in _flight.flightAircraft)
            GetSoj(aircraft).facing = facing;
    }

}
