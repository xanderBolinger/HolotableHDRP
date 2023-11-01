using HexMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static AirToAirCombatCalculator;

public class AircraftAirToAirCombatManager : MonoBehaviour
{
    public bool night;
    public static AircraftAirToAirCombatManager aircraftCombatManager;
    public AirToAirWeaponLoader weaponLoader;

    AircraftStandardCombat standardAirToAir;
    AircraftBailoutTable bailoutTable;


    void Start()
    {
        SetUp();
    }

    public void SetUp() {
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
            AircraftFlightManager.aircraftFlightManager.RemoveFlight(flight.flightCallsign);

    }

    

}
