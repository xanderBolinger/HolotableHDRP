
public class AircraftCombatEngagement
{

    public static bool Engagement(AircraftFlight engager, AircraftFlight target, 
        bool bvr, bool daytime) {

        var roll = DiceRoller.Roll(2, 20);

        var altitudeMod = engager.GetAltitude() != target.GetAltitude() ? -1 : 0;

        roll += altitudeMod + engager.agressionValue;

        if (bvr)
        {
            return roll >= 9;
        }
        else if (target.Detected())
        {
            return daytime ? roll >= 10 : roll >= 14;
        }
        else {
            return daytime ? roll >= 14 : roll >= 16;
        }

    }
    
}
