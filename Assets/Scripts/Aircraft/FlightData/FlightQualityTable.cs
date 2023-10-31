using UnityEngine;

public class FlightQualityTable
{
    public enum FlightQuality { 
        Trained, Regular, Veteran, Ace
    }

    TwoWayTable table;

    public FlightQualityTable() {
        TextAsset csvFile = Resources.Load<TextAsset>("Aircraft/Tables/FlightQuality");
        table = new TwoWayTable(csvFile);
    }

    public int GetValue(FlightQuality quality) {

        var roll = DiceRoller.Roll(2, 20);

        return int.Parse(table.GetValue(quality.ToString(), roll));
    }

}
