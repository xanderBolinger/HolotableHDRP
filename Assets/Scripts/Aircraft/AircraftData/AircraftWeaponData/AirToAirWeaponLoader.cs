using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirToAirWeaponLoader
{

    public enum AirToAirWeaponType { 
        Aim260Jatm
    }


    List<AirToAirWeaponData> airToAirWeaponList;

    public AirToAirWeaponLoader() {
        airToAirWeaponList = new List<AirToAirWeaponData>
        {
            LoadWeapon("Aim260Jatm")
        };

    }

    public AirToAirWeaponData GetWeapon(AirToAirWeaponType type) {

        foreach(var wep in airToAirWeaponList)
            if(wep.weaponType == type)
                return wep;

        throw new System.Exception("Weapon not found in list for type: "+type);
    }

    public AirToAirWeaponData LoadWeapon(string weaponFileName) {
        TextAsset asset = Resources.Load("Aircraft/AirToAirWeapons" + weaponFileName, typeof(TextAsset)) as TextAsset;
        string jsonString = asset.text;
        return JsonConvert.DeserializeObject<AirToAirWeaponData>(jsonString);
    }


}
