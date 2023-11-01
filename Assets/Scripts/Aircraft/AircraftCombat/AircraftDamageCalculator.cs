using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftDamageCalculator
{

    public static void ShotResolution(AircraftFlight targetFlight, int combatValue, bool additionalUndepletedWeapons) {

        var undepletedMod = additionalUndepletedWeapons ? 1 : 0;

        var roll = DiceRoller.Roll(2, 20);

        var modifiedRoll = roll + combatValue + undepletedMod;

        var aircraft = GetRandomAircraft(targetFlight);

        if (modifiedRoll <= 13)
        {
            Debug.Log("Shot Resolution, Roll: "+roll+", Modified Roll: "+modifiedRoll
                +", Combat Value: "+combatValue+", Undepleted Mod: "+undepletedMod+", NO EFFECT");
        }
        else if (modifiedRoll <= 14)
        {
            Debug.Log("Shot Resolution, Roll: " + roll + ", Modified Roll: " + modifiedRoll
                + ", Combat Value: " + combatValue + ", Undepleted Mod: " + undepletedMod + ", One Aircraft Damaged");
            aircraft.damaged = true;
        }
        else if (modifiedRoll <= 15)
        {
            Debug.Log("Shot Resolution, Roll: " + roll + ", Modified Roll: " + modifiedRoll
                + ", Combat Value: " + combatValue + ", Undepleted Mod: " + undepletedMod + ", One Aircraft Crippled");
            aircraft.crippled = true;
        }
        else if (modifiedRoll <= 19)
        {
            Debug.Log("Shot Resolution, Roll: " + roll + ", Modified Roll: " + modifiedRoll
                + ", Combat Value: " + combatValue + ", Undepleted Mod: " + undepletedMod + ", One Aircraft Shotdown");
            aircraft.destroyed = true;
        }
        else if (modifiedRoll <= 23)
        {
            Debug.Log("Shot Resolution, Roll: " + roll + ", Modified Roll: " + modifiedRoll
                + ", Combat Value: " + combatValue + ", Undepleted Mod: " + undepletedMod + ", One Aircraft Damaged");
            aircraft.damaged = true;
        }
        else {
            Debug.Log("Shot Resolution, Roll: " + roll + ", Modified Roll: " + modifiedRoll
                    + ", Combat Value: " + combatValue + ", Undepleted Mod: " + undepletedMod + ", NO EFFECT");
        }

    }

    private static Aircraft GetRandomAircraft(AircraftFlight targetFlight) {
        return targetFlight.flightAircraft[DiceRoller.Roll(0, 
            targetFlight.flightAircraft.Count - 1)];
    }

}
