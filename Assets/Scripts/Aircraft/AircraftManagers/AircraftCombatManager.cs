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

    AircraftStandardCombat standardAirToAir;
    AircraftBailoutTable bailoutTable;


    void Start()
    {
        Setup();
    }

    void Setup() {
        standardAirToAir = new AircraftStandardCombat();
        bailoutTable = new AircraftBailoutTable();
    }

    public void StandardAirToAir(AircraftFlight attacker, AircraftFlight defender) {
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

        Debug.Log("Standard Air to Air Engagement, Dttacker("+attacker.flightCallsign+"), Defender("+
            defender.flightCallsign+") shots ("+attackerShots+", "+defenderShots+")");

        attacker.SpendFuel();
        defender.SpendFuel();

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
