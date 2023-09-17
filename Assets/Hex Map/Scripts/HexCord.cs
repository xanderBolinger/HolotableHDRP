using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCord : MonoBehaviour
{
    public int x;
    public int y;
    public int elevation;
    public bool roadHex = false;
    public bool urbanHex = false;
    public HexType hexType;

    [Serializable]
    public enum HexType { 
        Clear,HeavyWoods,MediumWoods,LightWoods,Brush,HeavyBrush,MOUNTAIN,Building,BigBuilding,PATH,HIGHWAY
    }

    public Vector2Int GetCord() {
        return new Vector2Int(x, y);
    }

    public static HexCord GetHexCord(GameObject hex) {
        var comp = hex.GetComponent<HexCord>();
        var compInChildren = hex.GetComponentInChildren<HexCord>();
        return comp != null ? comp : compInChildren;
    }

}
