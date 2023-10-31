using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCombatManuever
{

    public TwoWayTable table;

    public AircraftCombatManuever() {
        TextAsset csvFile = Resources.Load<TextAsset>("Aircraft/Tables/ManueverTable");
        table = new TwoWayTable(csvFile);
    }

    public int GetValueBvr(int numberOfAircraft, int roll) {

        if (roll >= 10 && roll <= 12)
            roll = 11;
        else if (roll >= 13 && roll <= 16)
            roll = 13;
        else if (roll >= 17)
            roll = 17;

        return GetValueStandard(numberOfAircraft, roll);
    }

    public int GetValueStandard(int numberOfAircraft, int roll) {
        return int.Parse(table.GetValue(numberOfAircraft, roll));
    }

}
