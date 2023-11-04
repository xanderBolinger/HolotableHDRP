using System.Collections.Generic;
using UnityEngine;
using static AirToAirCombatCalculator;
using static AirToAirMoraleCalculator;
using static AirToAirDepletionCalculator;
using static AircraftDamageCalculator;
using HexMapper;

public class AircraftAirToAirCombatManager : MonoBehaviour
{
    public bool night;
    public static AircraftAirToAirCombatManager aircraftCombatManager;
    public AirToAirWeaponLoader weaponLoader;

    AircraftStandardCombat standardAirToAir;

    void Start()
    {
        SetUp();
    }

    public void SetUp() {
        weaponLoader = new AirToAirWeaponLoader();
        standardAirToAir = new AircraftStandardCombat();
        aircraftCombatManager = this;

    }

    public void BvrAirToAir(AircraftFlight attacker, AircraftFlight defender) {
        var pylon = GetPylon(attacker, false);
        if (pylon == null)
        {
            Debug.Log("Attacker " + attacker.flightCallsign + " has no undepleted weapons.");
            return;
        }

        Debug.Log("Begin BVR Attack " + attacker.flightCallsign + " targeting " + defender.flightCallsign);

        var dist = HexMap.GetDistance(attacker.GetCord(), defender.GetCord());
        var wep = weaponLoader.GetWeapon(pylon.weaponType);
        
        if (dist > GetBvrRange(attacker, defender, wep)) {
            Debug.Log("Range to target greater than weapon("+wep.weaponDisplayName+") BVR range.");
            return;
        }

        var (attackerYes, defenderYes) =
                AircraftStandardCombat.EngagementResults(attacker, defender, !night);

        if (!attackerYes)
        {
            Debug.Log("Attacker could not engage defender.");
            return;
        }

        var (attackerShots, defenderShots) = standardAirToAir.GetShots(attacker, defender, !night,
            attackerYes, defenderYes, true);

        Debug.Log("BVR Air to Air Engagement, Attacker(" + attacker.flightCallsign + "), Defender(" +
            defender.flightCallsign + ") shots:" + attackerShots);

        if (attackerShots <= 0)
            return;

        var attackerCasualtiesInflicted = ResolveShots(attacker, attackerShots, defender, true);
        DepletionCheck(attacker, attackerShots, pylon, true);

        MoraleCheckStandard(defender, false, false,
           0, attackerCasualtiesInflicted);
        defender.bvrAvoid = true;

        RemoveDestroyedAircraft();

    }

    int GetBvrRange(AircraftFlight attacker, AircraftFlight defender, AirToAirWeaponData data) {

        var forward = HexDirection.GetHexSideFacingTarget(attacker.GetCord()
            , defender.GetCord()) == attacker.GetFacing();
        var rear = HexDirection.Rear(attacker.GetCord(), defender.GetCord(), defender.GetFacing());

        if (rear) {
            Debug.Log("BVR Rear Range: "+ data.bvrRangeRear);
            return data.bvrRangeRear;
        }
        else if (forward) {
            Debug.Log("BVR Forward Range: " + data.bvrRangeForward);
            return data.bvrRangeForward;
        }
        else {
            Debug.Log("BVR Beam Range: " + data.bvrRangeBeam);
            return data.bvrRangeBeam;
        }

    }

    public void StandardAirToAir(AircraftFlight attacker, AircraftFlight defender) {
        var dist = HexMap.GetDistance(attacker.GetCord(), defender.GetCord());

        if (dist > 1) {
            Debug.Log("Attacker out of range from defender, dist: "+dist);
            return;
        }
        
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

        Debug.Log("Standard Air to Air Engagement, Attacker(" + attacker.flightCallsign + "), Defender(" +
            defender.flightCallsign + ") shots (" + attackerShots + ", " + defenderShots + ")");

        var attackerCasualtiesInflicted = ResolveShots(attacker, attackerShots, defender, false);
        DepletionCheck(attacker, attackerShots, pylon, false);

        var defenderPylon = GetPylon(defender, false);

        int defenderCasualtiesInflicted = 0;

        if (defenderPylon != null)
        {
            defenderCasualtiesInflicted = ResolveShots(defender, defenderShots, attacker, false);
            DepletionCheck(defender, defenderShots, defenderPylon, false);
        }
        else 
            Debug.Log("Defender "+defender.flightCallsign+" cannot shoot back, has no undepleted weapons");

        attacker.SpendFuel();
        defender.SpendFuel();

        MoraleCheckStandard(attacker, attackerYes && !defenderYes, !attackerYes && defenderYes, 
            attackerCasualtiesInflicted, defenderCasualtiesInflicted);

        MoraleCheckStandard(defender, false, attackerYes && !defenderYes,
            defenderCasualtiesInflicted, attackerCasualtiesInflicted);

        RemoveDestroyedAircraft();

        UpdateCountersStandard(attacker);
        UpdateCountersStandard(defender);

    }

    void UpdateCountersStandard(AircraftFlight flight) {

        if(!flight.disengaing)
            flight.manueverMarker = true;
        flight.bvrAvoid = false;
        flight.climbed = false;
        flight.zoomClimb = false;

    }

}
