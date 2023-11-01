using HexMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AircraftCombatManager : MonoBehaviour
{
    public bool night;
    public static AircraftCombatManager aircraftCombatManager;

    AircraftStandardCombat standardAirToAir;
    AircraftBailoutTable bailoutTable;

    AirToAirWeaponLoader weaponLoader;

    void Start()
    {
        Setup();
    }

    void Setup() {
        weaponLoader = new AirToAirWeaponLoader();
        standardAirToAir = new AircraftStandardCombat();
        bailoutTable = new AircraftBailoutTable();
        aircraftCombatManager = this;
    }

    public void StandardAirToAir(AircraftFlight attacker, AircraftFlight defender) {
        var pylon = GetPylon(attacker, false);

        if (pylon == null) {
            Debug.Log("Attacker "+attacker.flightCallsign+" has no undepleted weapons.");
            return;
        }

        if(!CanAttackStandard(attacker, defender)) return;

        var (attackerYes, defenderYes) = 
            AircraftStandardCombat.EngagementResults(attacker, defender, !night);

        if (!attackerYes && !defenderYes)
        {
            Debug.Log("Neither defender nor attacker engaged each other.");
            return;
        }

        var (attackerShots, defenderShots) = standardAirToAir.GetShots(attacker, defender, !night,
            attackerYes, defenderYes);
        ResolveShots(attacker, attackerShots, defender, false);
        ResolveShots(defender, defenderShots, attacker, false);
        Debug.Log("Standard Air to Air Engagement, Dttacker("+attacker.flightCallsign+"), Defender("+
            defender.flightCallsign+") shots ("+attackerShots+", "+defenderShots+")");

        attacker.SpendFuel();
        defender.SpendFuel();
        RemoveDestroyedAircraft();
    }

    void RemoveDestroyedAircraft() {

        var flights = AircraftFlightManager.aircraftFlightManager.aircraftFlights;

        // remove aircraft 
        foreach (var flight in flights) {
            List<Aircraft> removeAircraft = new List<Aircraft>();
            foreach (var aircraft in flight.flightAircraft)
                if (aircraft.destroyed)
                    removeAircraft.Add(aircraft);
            foreach (var aircraft in removeAircraft)
                flight.flightAircraft.Remove(aircraft);
        }


        // remove empty flights 
        List<AircraftFlight> removeFlights = new List<AircraftFlight>();

        foreach (var flight in flights) {
            if (flight.flightAircraft.Count == 0)
                removeFlights.Add(flight);
        }

        foreach (var flight in removeFlights)
            flights.Remove(flight);

    }

    void ResolveShots(AircraftFlight shooters, int shots, AircraftFlight targetFlight, bool bvr) {
        var pylon = GetPylon(shooters, bvr);
        var undepletedWeapons = AdditionalUndepletedPylons(shooters);
        var wep = weaponLoader.GetWeapon(pylon.weaponType);
        var combatRating = bvr ? wep.bvrRating : wep.standardRating;
        for (int i = 0; i < shots; i++) {
            AircraftDamageCalculator.ShotResolution(targetFlight, combatRating, undepletedWeapons);
        }

    }

    bool CanEngageBvr(AircraftFlight shooters)
    {
        return GetPylon(shooters, true) != null;
    }

    bool AdditionalUndepletedPylons(AircraftFlight shooters)
    {

        var pylon = GetPylon(shooters, false);
        var pylon2 = GetPylon(shooters, false, pylon);

        return pylon != null && pylon2 != null;
    }

    AirToAirPylon GetPylon(AircraftFlight shooters, bool bvr, AirToAirPylon existingPylon = null)
    {

        foreach (var aircraft in shooters.flightAircraft)
        {

            foreach (var pylon in aircraft.aircraftPayload.pylons)
            {
                var wep = weaponLoader.GetWeapon(pylon.weaponType);
                if ((bvr && wep.bvrRangeForward == 0) || pylon.depleted
                    || (existingPylon != null && existingPylon.weaponType == pylon.weaponType))
                    continue;
                return pylon;
            }

        }

        return null;
    }

    public bool CanAttackStandard(AircraftFlight attacker, AircraftFlight defender) {

        if (!defender.Detected()) {
            Debug.Log("Attack defender("+defender.flightCallsign+") not detected.");
            return false;
        }

        if (!AttackerFacingDefender(attacker, defender))
            return false;


        return true;
    }

    bool AttackerFacingDefender(AircraftFlight attacker, AircraftFlight defender) {
        var forward = HexDirection.GetHexSideFacingTarget(attacker.GetCord(), defender.GetCord());

        if (forward != attacker.GetFacing())
        {
            Debug.Log("Defender(" + defender.flightCallsign + ") outside attacker("
                + attacker.flightCallsign + ")'s forward arc");
            return false;
        }

        return true;
    }

}
