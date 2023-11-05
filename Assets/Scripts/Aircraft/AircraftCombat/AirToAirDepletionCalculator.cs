using UnityEngine;

public class AirToAirDepletionCalculator
{

    public static void DepletionCheck(AircraftFlight flight, int shots, AirToAirPylon pylon,
        bool bvr) {

        var dp = pylon.depletionPoints;

        var roll = DiceRoller.Roll(1, 10);

        var modifiedRoll = (shots-1) + roll;

        if (modifiedRoll > dp)
        {
            Debug.Log("Flight " + flight.flightCallsign + " weapon " + pylon.weaponType + " DEPLETED, Roll >= DP: " + dp + ", Roll: " + roll +
                ", Shots: " + shots + ", Modified Roll: " + modifiedRoll);

            DepleteFlight(flight, pylon);

            if (roll == 1 && !bvr) {
                var nextPylon = AirToAirCombatCalculator.GetPylon(flight, false, null);
                if (nextPylon != null)
                    DepleteFlight(flight, pylon);
            }

        }
        else {

            Debug.Log("Flight "+flight.flightCallsign+" weapon "+pylon.weaponType+" not depleted, Roll < DP: "+dp+", Roll: "+roll+
                ", Shots: "+shots+", Modified Roll: "+modifiedRoll);
        }

    }

    private static void DepleteFlight(AircraftFlight flight, AirToAirPylon pylon) {
        if (pylon == null)
            return;

        foreach (var aircraft in flight.flightAircraft) {
            foreach (var p in aircraft.aircraftPayload.pylons) {
                if (p.weaponType == pylon.weaponType)
                    p.depleted = true;
            }
        }

    }

    public static void DepleteFlightTest(AircraftFlight flight, AirToAirPylon pylon) {
        DepleteFlight(flight, pylon);
    }

}
