using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirToAirMoraleCalculator
{

    public static void MoraleCheckStandard(AircraftFlight flight,
        bool hasSurprise, bool disadvantage, int enemyCasualties, int friendlyCasualties) {

        var roll = DiceRoller.Roll(2, 20);

        var surpriseMod = hasSurprise ? 1 : 0;
        var disadvantageMod = disadvantage ? -1 : 0;
        var aggressionValueMod = flight.agressionValue;

        var modifiedRoll = surpriseMod + disadvantageMod + aggressionValueMod + enemyCasualties 
            - friendlyCasualties
            + roll;

        if (modifiedRoll >= 17)
        {
            Debug.Log("Morale Result(" + flight.flightCallsign + "): JETTISON CHECK");
        }
        else if (modifiedRoll >= 15)
        {
            Debug.Log("Morale Result(" + flight.flightCallsign + "): JETTISON CHECK, agression -1");
            flight.agressionValue--;
        }
        else if (modifiedRoll >= 9)
        {
            flight.flightStatus = AircraftFlight.FlightStatus.Disordered;
            Debug.Log("Morale Result(" + flight.flightCallsign + "): DISORDERED, agression -1");
            flight.agressionValue--;
        }
        else if (modifiedRoll >= 6)
        {
            flight.flightStatus = AircraftFlight.FlightStatus.Disordered;
            Debug.Log("Morale Result(" + flight.flightCallsign + "): DISORDERED, agression -2");
            flight.agressionValue-=2;
        }
        else {
            flight.flightStatus = AircraftFlight.FlightStatus.Aborted;
            Debug.Log("Morale Result(" + flight.flightCallsign + "): ABORT, agression -3");
            flight.agressionValue -= 3;
        }

    }


}