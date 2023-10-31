using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftStandoffJammerCalculator
{

    public static int GetJammerStrength(AircraftFlight spotter, HexCord target, List<AircraftFlight> flights) {

        var strength = 0;

        var spotterLocation = spotter.GetLocation();
        var spotterCord = new Vector2Int(spotterLocation.x, spotterLocation.y);
        var targetCord = new Vector2Int(target.x, target.y);

        foreach (var flight in flights) {
            if (flight == spotter)
                continue;

            var flightLocation = flight.GetLocation();
            var flightCord = new Vector2Int(flightLocation.x, flightLocation.y);

            var dist = HexMap.GetDistance(spotterLocation, flightLocation);
            var soj = flight.flightAircraft[0].aircraftJammerData.aircraftStandoffJammer;
            
            var jammerFacingToSpotter = HexDirection.GetHexSideFacingTarget(flightCord, spotterCord);

            if (soj == null || !soj.active || dist > soj.longRange || jammerFacingToSpotter != soj.facing)
                continue;

            var spotterFacingToTarget = HexDirection.GetHexSideFacingTarget(spotterCord, targetCord);
            strength += GetJammerStrengthValue(dist, soj, jammerFacingToSpotter == 
                HexDirection.GetOppositeDirectionTo(spotterFacingToTarget));
        }

        return -strength;
    }

    static int GetJammerStrengthValue(int dist, AircraftStandoffJammer soj, 
        bool facingIntoJammer)
    { 

        if (dist > soj.mediumRange)
            return facingIntoJammer ? soj.longRangeStrength : soj.longRangeStrength / 2;
        else if (dist > soj.shortRange)
            return facingIntoJammer ? soj.mediumRangeStrength : soj.mediumRangeStrength / 2;
        else
            return facingIntoJammer ? soj.shortRangeStrength : soj.shortRangeStrength / 2;

    }

}
