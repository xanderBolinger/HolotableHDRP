using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCord : MonoBehaviour
{
    public int x;
    public int y;
    public bool roadHex = false;
    public bool urbanHex = false;
    public HexType hexType;

    public enum HexType { 
        CLEAR,WOODS,MOUNTAIN,TOWN,CITY,PATH,HIGHWAY
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
