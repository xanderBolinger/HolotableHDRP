using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftStandoffJammerCalculator
{

    public static int GetJammerStrength(AircraftFlight target, List<AircraftFlight> flights) {

        var strength = 0;

        var targetLocation = target.GetLocation();
        var targetCord = new Vector2Int(targetLocation.x, targetLocation.y);

        foreach (var flight in flights) {

            var flightLocation = flight.GetLocation();
            var flightCord = new Vector2Int(flightLocation.x, flightLocation.y);

            var dist = HexMap.GetDistance(targetLocation, flightLocation);
            var soj = flight.flightAircraft[0].aircraftJammerData.aircraftStandoffJammer;

            var jammerFacingToTarget = HexDirection.GetHexSideFacingTarget(flightCord, targetCord);

            if (soj == null || !soj.active || dist > soj.longRange || jammerFacingToTarget != soj.facing)
                continue;

            var targetFacingToJammer = HexDirection.GetHexSideFacingTarget(targetCord, flightCord);

            strength += GetJammerStrengthValue(dist, soj, jammerFacingToTarget, targetFacingToJammer);
        }


        return -strength;
    }

    static int GetJammerStrengthValue(int dist, AircraftStandoffJammer soj, 
        Direction jammerFacingToTarget, Direction targetFacingToJammer) {
        

        var opposite = HexDirection.GetOppositeDirectionTo(jammerFacingToTarget);

        bool facingIntoJammer = false;

        // Radar aiming into jammer
        if (targetFacingToJammer == opposite)
            facingIntoJammer = true;

        if (dist > soj.mediumRange)
            return facingIntoJammer ? soj.longRangeStrength : soj.longRangeStrength / 2;
        else if (dist > soj.shortRange)
            return facingIntoJammer ? soj.mediumRangeStrength : soj.mediumRangeStrength / 2;
        else
            return facingIntoJammer ? soj.shortRangeStrength : soj.shortRangeStrength / 2;

    }

}
