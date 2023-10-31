using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftStandoffJammerCalculator;
using static AircraftFlightManager;

public class AircraftDetectionCalculator
{



    // Within 2 hexes at night
    // Within 4 hexes in daylight
    // Visual detection class D 
    public static int VisualDetectionRoll(bool night, int range, AircraftFlight spotter, AircraftFlight target) {
        var rangeMod = night && range == 2 ? -2 : range >= 3 ? -2 : 0;
        var altMod = spotter.GetAltitude() != target.GetAltitude() ? -1 : 0;
        var dashMod = night && target.GetSpeed() == AircraftSpeedData.AircraftSpeed.Dash ? 2 : 0;

        var mods = rangeMod + altMod + dashMod;
        var roll = DiceRoller.Roll(2, 20) + mods;

        return roll;
    }

    // Infra red search and track
    // Good IRST range 18 hexes front 25 hexes rear 
    // Launch detection at %65 of max range
    // Detection class E
    public static int IrstDetectionRoll(AircraftFlight spotter, AircraftFlight target, bool rear) {
        
        var rearMod = rear ? 3 : 0;

        return DiceRoller.Roll(2, 20) + rearMod;
    }

    // V19 base 250 mi active enemy radar range 
    // V19 120 mi on enemy that are not transmitting radar
    public static int RadarDetectionRoll(AircraftFlight spotter, AircraftFlight target, int distance, bool chaffCorridor) {
        var radar = spotter.flightAircraft[0].aircraftDetectionData.aircraftRadar;
        bool targetRadarActive = target.flightAircraft[0].aircraftDetectionData.aircraftRadar.active;

        var altitudeBandMod = spotter.GetAltitude() != target.GetAltitude() ? -1 : 0;
        var targetAtDeckMod = target.GetAltitude() == AircraftMovementData.AircraftAltitude.DECK ?
            (target.GetLocation().Rough() ? -3 : -2) : 0;
        var radarRangeMod = distance >= (targetRadarActive ? radar.radarMediumRangeActive : radar.radarMediumRange) ? -2 :
            distance >= (targetRadarActive ? radar.radarShortRangeActive : radar.radarShortRange) ? -1 : 0;

        var locationSpotter = spotter.GetLocation();
        var locationTarget = target.GetLocation();

        var chaffMod = chaffCorridor ? -3 : 0;

        var beamMod = HexDirection.Beam(new Vector2Int(locationSpotter.x, locationSpotter.y), 
            new Vector2Int(locationTarget.x, locationTarget.y),
            target.GetFacing()) ? - 2 : 0;

        var standoffJammingMod = GetJammerStrength(spotter, aircraftFlightManager.aircraftFlights);

        var roll = DiceRoller.Roll(2, 20);

        return roll + altitudeBandMod + targetAtDeckMod + radarRangeMod + beamMod + chaffMod + standoffJammingMod;
    }

}
