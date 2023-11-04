using System;
using System.Collections.Generic;

[Serializable]
public class ForceSide
{

    List<ForceSide> _friendlySides;
    string _displayName;

    public string displayName { get { return _displayName; } }

    public ForceSide(string displayName) { 
        _displayName = displayName;
        _friendlySides = new List<ForceSide>();
    }

    public void AddFriendly(ForceSide side) {
        _friendlySides.Add(side);
    }

    public void RemoveFriendly(ForceSide side) {
        _friendlySides.Remove(side);
    }

    public bool FriendlyTowards(ForceSide side) { 
        return _friendlySides.Contains(side) || side.displayName == displayName;
    }

    public override string ToString()
    {
        return "Side: "+displayName+", "+GetSidesString(true)/*+", "+GetSidesString(false)*/;
    }

    private string GetSidesString(bool friendly) {

        //var sides = friendly ? _friendlySides : _enemySides;
        var sides = _friendlySides;
        string sideString = friendly ? "Friendly" : "Hostile";
        sideString += " towards [";

        foreach(var s in sides) 
            sideString += s.displayName + (sides[sides.Count-1] != s ? ", " : "");

        return sideString+"]";

    }

}
