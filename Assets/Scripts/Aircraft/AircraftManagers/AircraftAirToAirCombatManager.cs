using System.Collections.Generic;
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

        Debug.Log("Standard Air to Air Engagement, Dttacker(" + attacker.flightCallsign + "), Defender(" +
            defender.flightCallsign + ") shots (" + attackerShots + ", " + defenderShots + ")");

        ResolveShots(attacker, attackerShots, defender, false);
        AirToAirDepletionCalculator.DepletionCheck(attacker, attackerShots, pylon, false);

        var defenderPylon = GetPylon(attacker, false);

        if (defenderPylon != null)
        {
            ResolveShots(defender, defenderShots, attacker, false);
            AirToAirDepletionCalculator.DepletionCheck(defender, defenderShots, defenderPylon, false);
        }
        else 
            Debug.Log("Defender "+defender.flightCallsign+" cannot shoot back, has no undepleted weapons");

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
