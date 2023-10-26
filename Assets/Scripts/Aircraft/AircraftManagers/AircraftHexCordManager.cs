using UnityEngine;

public class AircraftHexCordManager : MonoBehaviour
{
    [InspectorName("X Cord")]
    public int xCord;
    [InspectorName("Y Cord")]
    public int yCord;
    [InspectorName("Add Hex Rough")]
    public bool hexRough;

    public static HexCord CreateHexCord(bool roughTerrain, int x, int y)
    {
        var testSetHexCord = new GameObject().AddComponent<HexCord>();
        testSetHexCord.x = x;
        testSetHexCord.y = y;
        testSetHexCord.hexType = roughTerrain ? HexCord.HexType.MOUNTAIN : HexCord.HexType.Clear;
        return testSetHexCord;
    }

}

