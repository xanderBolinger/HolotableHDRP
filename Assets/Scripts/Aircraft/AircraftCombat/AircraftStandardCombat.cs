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

    public (int, int) GetShots(AircraftFlight agressor, AircraftFlight target, bool daytime,
        bool attackerYes, bool defenderYes,bool bvr =false) {

        Debug.Log("Attacker engages: "+attackerYes+", Defender engages: "+defenderYes);

        var manueverRollAttacker = CalculateManueverAttacker(agressor, target, !daytime, 
            attackerYes && !defenderYes, bvr);
        var attackerShots = bvr ? combatManueverTable.GetValueBvr(agressor.UndamagedAircraft(),
            manueverRollAttacker) : combatManueverTable.GetValueStandard(agressor.UndamagedAircraft(),
            manueverRollAttacker);

        int defenderShots = 0;

        if (!bvr) {
            var manueverRollDefender = CalculateManueverDefender(agressor, target, !daytime,
            attackerYes && !defenderYes);
            defenderShots = combatManueverTable.GetValueStandard(target.UndamagedAircraft(),
                manueverRollDefender);
        }

        return (attackerShots, defenderShots);
    }


    public static (bool, bool) EngagementResults(AircraftFlight agressor, AircraftFlight target, bool daytime) {

        bool attackerEngages = Engagement(agressor, target, false, daytime);
        bool defenderEngages = Engagement(target, agressor, false, daytime);


        return (attackerEngages, defenderEngages);
    }

    public static int CalculateManueverAttacker(AircraftFlight attacker, AircraftFlight defender,
        bool night, bool attackerSurprise, bool bvr)
    {
        var aircraftManueverMod = !bvr ? attacker.flightAircraft[0].movementData.GetManueverRating() -
            defender.flightAircraft[0].movementData.GetManueverRating() : 0;
        var surpriseMod = attackerSurprise ? 3 : 0;
        var nightMod = night ? -3 : 0;
        var disorderedMod = defender.flightStatus == FlightStatus.Disordered ? 1 : 0;
        var enemyDisengaging = defender.disengaing ? -2 : 0;
        var rear = HexDirection.Rear(defender.GetCord(), attacker.GetCord(), defender.GetFacing());

        var flightManueverMod = !bvr ? FlightManueverRating(attacker) - FlightManueverRating(defender) : 0;

        var geometry = rear ? 1 : 0;

        var roll = DiceRoller.Roll(2, 20);
        var modifiedRoll = roll + geometry + enemyDisengaging + disorderedMod
            + nightMod + surpriseMod + aircraftManueverMod + (!night ? flightManueverMod : 0);

        Debug.Log("Attacker Manuever("+attacker.flightCallsign+")" + " roll: "+roll +", Modified Roll: "
            +modifiedRoll+", "+ "Geometry Mod: "+geometry + ", Enemy Disengaging: "+enemyDisengaging
            +", Enemy Disordered Mod: "+ disorderedMod+", Aircraft Manuever Rating Mod: "+aircraftManueverMod
            + ", Flight Manuever Mod(Day only): " + (!night ? flightManueverMod : 0)+", Night Mod: "+nightMod+", "+
            ", Surprise Mod: "+surpriseMod);

        return modifiedRoll;
    }

    public static int CalculateManueverDefender(AircraftFlight attacker, AircraftFlight defender,
        bool night, bool defenderDisadvantaged) {

        var aircraftManueverMod = defender.flightAircraft[0].movementData.GetManueverRating() -
            attacker.flightAircraft[0].movementData.GetManueverRating();

        var nightMod = night ? -3 : 0;
        
        var disadvantage = defenderDisadvantaged ? -1 : 0;
       
        var rear = HexDirection.Rear(defender.GetCord(), attacker.GetCord(), defender.GetFacing());

        var flightManueverMod = FlightManueverRating(defender) - FlightManueverRating(attacker);

        var geometry = rear ? -1 : 0;

        var roll = DiceRoller.Roll(2, 20);

        var modifiedRoll = roll + geometry + disadvantage
            + nightMod + aircraftManueverMod + (!night ? flightManueverMod : 0);

        Debug.Log("Defender Manuever(" + defender.flightCallsign + ")" + " roll: " + roll + " Modified Roll: "
            + modifiedRoll + ", "+ "Geometry Mod: " + geometry + ", Aircraft Manuever Rating Mod: " + aircraftManueverMod
            + ", Flight Manuever Mod(Day only): " + (!night ? flightManueverMod : 0) + ", Night Mod: " + nightMod 
            + ", Disadvantage Mod: "+ disadvantage);

        return modifiedRoll;

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

        var undamagedAircraft = flight.UndamagedAircraft();

        if (undamagedAircraft != flight.flightAircraft.Count 
            && undamagedAircraft < flight.flightAircraft.Count / 2)
            rating -= 2;
        else if (undamagedAircraft != flight.flightAircraft.Count)
            rating -= 1;

        return rating;

    }

}
