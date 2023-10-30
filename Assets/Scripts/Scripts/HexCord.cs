using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HexCord : MonoBehaviour
{
    public int x;
    public int y;
    public int elevation { get { return elevation; } set { UpdateText(value);  } }
    public bool roadHex = false;
    public bool urbanHex = false;
    public HexType hexType;

    [SerializeField]
    GameObject hexElevation;
    [SerializeField]
    TextMeshProUGUI text;

    [Serializable]
    public enum HexType { 
        Clear,HeavyWoods,MediumWoods,LightWoods,Brush,HeavyBrush,MOUNTAIN,Building,BigBuilding,PATH,HIGHWAY
    }

    void UpdateText(int value) {
        elevation = value;
        text.SetText(value.ToString());
    }

    public void HideText() {
        hexElevation.SetActive(false);
    }

    public void ShowText() {
        hexElevation.SetActive(true);
    }

    public Vector2Int GetCord() {
        return new Vector2Int(x, y);
    }

    public static HexCord GetHexCord(GameObject hex) {
        var comp = hex.GetComponent<HexCord>();
        var compInChildren = hex.GetComponentInChildren<HexCord>();
        return comp != null ? comp : compInChildren;
    }

    public static bool operator ==(HexCord b1, HexCord b2)
    {
        if (b1 is null)
            return b2 is null;

        return b1.Equals(b2);
    }

    public static bool operator !=(HexCord b1, HexCord b2)
    {
        return !(b1 == b2);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        return obj is HexCord b2 ? (x == b2.x &&
                               y == b2.y) : false;
    }

    public bool Rough() {
        return hexType == HexType.MOUNTAIN || hexType == HexType.HeavyWoods || hexType == HexType.BigBuilding;
    }



}
