using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftStandardCombat
{

    public void Engagement(AircraftFlight agressor, AircraftFlight target, bool daytime) {

        bool attackerEngages = AircraftCombatEngagement.Engagement(agressor, target, false, daytime);
        bool defenderEngages = AircraftCombatEngagement.Engagement(target, agressor, false, daytime);
    }

    public void Manuever() { 
    
    }

    public void ShotResolution() { 
    
    }

}
