using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum AirToAirWeaponType
{
    Aim260Jatm
}

public class AirToAirWeaponLoader
{

    List<AirToAirWeaponData> airToAirWeaponList;

    public AirToAirWeaponLoader() {
        airToAirWeaponList = new List<AirToAirWeaponData>
        {
            LoadWeapon("Aim260Jatm")
        };

    }

    AirToAirWeaponData LoadWeapon(string weaponFileName)
    {
        TextAsset asset = Resources.Load("Aircraft/AirToAirWeapons/" + weaponFileName, typeof(TextAsset)) as TextAsset;
        string jsonString = asset.text;
        return JsonConvert.DeserializeObject<AirToAirWeaponData>(jsonString);
    }

    public AirToAirWeaponData GetWeapon(AirToAirWeaponType type) {

        foreach(var wep in airToAirWeaponList)
            if(wep.weaponType == type)
                return wep;

        throw new System.Exception("Weapon not found in list for type: "+type);
    }


    public override string ToString()
    {

        string rslt = "";

        foreach (var weapon in airToAirWeaponList) {
            rslt += weapon.ToString() + "\n";
        }

        return rslt;

    }

}
