using UnityEngine;

public class AircraftHexCordManager : MonoBehaviour
{
    [InspectorName("X Cord")]
    public int xCord;
    [InspectorName("Y Cord")]
    public int yCord;
    [InspectorName("Add Hex Rough")]
    public bool hexRough;

    [SerializeField]
    Transform hexCordContainer;

    public static AircraftHexCordManager aircraftHexCordManager;

    private void Start()
    {
        aircraftHexCordManager = this;
    }

    public HexCord TestCreateHexCord() {
        return CreateHexCord(hexRough, xCord, yCord);
    }

    public static HexCord CreateHexCord(bool roughTerrain, int x, int y)
    {
        var obj = new GameObject();
        obj.name = "Hex Cord("+x+", "+y+")";

        if (aircraftHexCordManager != null && aircraftHexCordManager.hexCordContainer != null)
            obj.transform.parent = aircraftHexCordManager.hexCordContainer;

        var testSetHexCord = obj.AddComponent<HexCord>();
        testSetHexCord.x = x;
        testSetHexCord.y = y;
        testSetHexCord.hexType = roughTerrain ? HexCord.HexType.MOUNTAIN : HexCord.HexType.Clear;
        return testSetHexCord;
    }

}

