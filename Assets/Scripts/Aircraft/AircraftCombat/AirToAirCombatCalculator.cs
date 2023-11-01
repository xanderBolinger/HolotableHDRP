using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftAirToAirCombatManager;

public class AirToAirCombatCalculator
{

    public static bool AttackerFacingDefender(AircraftFlight attacker, AircraftFlight defender)
    {
        var forward = HexDirection.GetHexSideFacingTarget(attacker.GetCord(), defender.GetCord());

        if (forward != attacker.GetFacing())
        {
            Debug.Log("Defender(" + defender.flightCallsign + ") outside attacker("
                + attacker.flightCallsign + ")'s forward arc");
            return false;
        }

        return true;
    }

    public static bool CanEngageBvr(AircraftFlight shooters)
    {
        return GetPylon(shooters, true) != null;
    }

    public static bool AdditionalUndepletedPylons(AircraftFlight shooters)
    {

        var pylon = GetPylon(shooters, false);
        var pylon2 = GetPylon(shooters, false, pylon);

        return pylon != null && pylon2 != null;
    }

    public static AirToAirPylon GetPylon(AircraftFlight shooters, bool bvr, AirToAirPylon existingPylon = null)
    {

        foreach (var aircraft in shooters.flightAircraft)
        {

            foreach (var pylon in aircraft.aircraftPayload.pylons)
            {
                var wep = aircraftCombatManager.weaponLoader.GetWeapon(pylon.weaponType);
                if ((bvr && wep.bvrRangeForward == 0) || pylon.depleted
                    || (existingPylon != null && existingPylon.weaponType == pylon.weaponType))
                    continue;
                return pylon;
            }

        }

        return null;
    }

    public static bool CanAttackStandard(AircraftFlight attacker, AircraftFlight defender)
    {

        if (!defender.Detected())
        {
            Debug.Log("Attack defender(" + defender.flightCallsign + ") not detected.");
            return false;
        }

        if (!AttackerFacingDefender(attacker, defender))
            return false;


        return true;
    }

    public static int ResolveShots(AircraftFlight shooters, int shots, AircraftFlight targetFlight, bool bvr)
    {
        var pylon = GetPylon(shooters, bvr);
        var undepletedWeapons = AdditionalUndepletedPylons(shooters);
        var wep = aircraftCombatManager.weaponLoader.GetWeapon(pylon.weaponType);
        var combatRating = bvr ? wep.bvrRating : wep.standardRating;

        int casualtiesInflicted = 0;

        for (int i = 0; i < shots; i++)
        {
            casualtiesInflicted += 
                AircraftDamageCalculator.ShotResolution(targetFlight, combatRating, undepletedWeapons) ? 1
                : 0;
        }

        return casualtiesInflicted;
    }

}
