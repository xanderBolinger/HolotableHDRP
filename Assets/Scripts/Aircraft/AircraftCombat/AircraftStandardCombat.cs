using HexMapper;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftFlight;
using static AircraftCombatEngagementTable;

public class AircraftStandardCombat
{
    AircraftCombatManueverTable combatManueverTable;

    public AircraftStandardCombat() {
        combatManueverTable = new AircraftCombatManueverTable();
    }

    public (int, int) GetShots(AircraftFlight agressor, AircraftFlight target, bool daytime) {

        var (attackerYes, defenderYes) = EngagementResults(agressor, target, daytime);

        if (!attackerYes && !defenderYes) {
            Debug.Log("Neither defender nor attacker engaged each other.");
            return (0,0);
        }

        var manueverRollAttacker = CalculateManueverAttacker(agressor, target, !daytime, 
            attackerYes && !defenderYes);
        var manueverRollDefender = CalculateManueverDefender(agressor, target, !daytime, 
            attackerYes && !defenderYes);

        var attackerShots = combatManueverTable.GetValueStandard(agressor.flightAircraft.Count,
            manueverRollAttacker);
        var defenderShots = combatManueverTable.GetValueStandard(target.flightAircraft.Count,
            manueverRollDefender);

        return (attackerShots, defenderShots);
    }


    public static (bool, bool) EngagementResults(AircraftFlight agressor, AircraftFlight target, bool daytime) {

        bool attackerEngages = Engagement(agressor, target, false, daytime);
        bool defenderEngages = Engagement(target, agressor, false, daytime);


        return (attackerEngages, defenderEngages);
    }

    public static int CalculateManueverAttacker(AircraftFlight attacker, AircraftFlight defender,
        bool night, bool attackerSurprise)
    {
        var aircraftManueverMod = attacker.flightAircraft[0].movementData.manueverRating -
            defender.flightAircraft[0].movementData.manueverRating;
        var surpriseMod = attackerSurprise ? 3 : 0;
        var nightMod = night ? -3 : 0;
        var disorderedMod = defender.flightStatus == FlightStatus.Disordered ? 1 : 0;
        var enemyDisengaging = defender.disengaing ? -2 : 0;
        var beam = HexDirection.Beam(defender.GetCord(), attacker.GetCord(), defender.GetFacing());
        var rear = HexDirection.Rear(defender.GetCord(), attacker.GetCord(), defender.GetFacing());

        var flightManueverMod = FlightManueverRating(attacker) - FlightManueverRating(defender);

        var geometry = beam && rear ? 1 : 0;

        var roll = DiceRoller.Roll(2, 20);

        return roll + geometry + enemyDisengaging + disorderedMod
            + nightMod + surpriseMod + aircraftManueverMod + (!night ? flightManueverMod : 0);
    }

    public static int CalculateManueverDefender(AircraftFlight attacker, AircraftFlight defender,
        bool night, bool defenderDisadvantaged) {

        var aircraftManueverMod = defender.flightAircraft[0].movementData.manueverRating -
            attacker.flightAircraft[0].movementData.manueverRating;

        var nightMod = night ? -3 : 0;
        
        var disadvantage = defenderDisadvantaged ? -1 : 0;
       
        var beam = HexDirection.Beam(defender.GetCord(), attacker.GetCord(), defender.GetFacing());
        var rear = HexDirection.Rear(defender.GetCord(), attacker.GetCord(), defender.GetFacing());

        var flightManueverMod = FlightManueverRating(defender) - FlightManueverRating(attacker);

        var geometry = beam && rear ? -1 : 0;

        var roll = DiceRoller.Roll(2, 20);

        return roll + geometry + disadvantage
            + nightMod + aircraftManueverMod + (!night ? flightManueverMod : 0);

    }

    static int FlightManueverRating(AircraftFlight flight)
    {

        var rating = 0;

        if (flight.bvrAvoid)
            rating -= 2;
        if (flight.manueverMarker)
            rating -= 2;
        if (flight.zoomClimb)
            rating -= 2;
        if (flight.climbed)
            rating -= 1;

        return rating;

    }

}
