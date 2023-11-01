using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftBailoutTable
{

    public enum BailoutResult { 
        KIA,Parachute,ParachuteDrift
    }

    public static BailoutResult GetResult() {

        var roll = DiceRoller.Roll(1, 10);

        if (roll <= 4)
            return BailoutResult.KIA;
        else if (roll <= 9)
            return BailoutResult.Parachute;
        else
            return BailoutResult.ParachuteDrift;

    }

}
