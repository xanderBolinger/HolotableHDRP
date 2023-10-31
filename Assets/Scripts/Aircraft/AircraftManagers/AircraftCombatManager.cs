using HexMapper;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AircraftCombatManager : MonoBehaviour
{

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
                + attacker.flightCallsign + "'s forward arc");
            return false;
        }

        return true;
    }

}
