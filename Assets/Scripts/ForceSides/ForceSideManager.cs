using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSideManager : MonoBehaviour
{
    public static ForceSideManager forceSideManager;


    [HideInInspector]
    public List<string> inspectorSideDisplayList = new List<string>();
    [HideInInspector]
    public int inspectorSelectedSideIndex;

    List<ForceSide> _sides;
    
    private void Start()
    {
        
        Setup();
    }

    public void Setup() {
        forceSideManager = this;
        _sides = new List<ForceSide>();

        var blufor = new ForceSide("blufor");
        inspectorSideDisplayList.Add("blufor");
        var opfor = new ForceSide("opfor");
        inspectorSideDisplayList.Add("opfor");
        var indfor = new ForceSide("indfor");
        inspectorSideDisplayList.Add("indfor");

        blufor.AddFriendly(indfor);
        indfor.AddFriendly(blufor);

        _sides.Add(blufor);
        _sides.Add(opfor);
        _sides.Add(indfor);
    }

    public ForceSide GetInspectorSelectedSide() {
        return GetSide(inspectorSelectedSideIndex);
    }

    public ForceSide GetSide(int index) {
        return _sides[index];
    }

    public void PrintSides() {
        string sideString = "Sides: \n";

        foreach(var side in _sides)
        {
            sideString += side.ToString() + "\n";
        }

        Debug.Log(sideString);
    }

}
