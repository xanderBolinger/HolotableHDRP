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

}
