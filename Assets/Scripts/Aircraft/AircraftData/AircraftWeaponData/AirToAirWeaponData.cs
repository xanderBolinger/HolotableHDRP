using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AircraftLoader;
using static AirToAirWeaponLoader;

public class AirToAirWeaponData
{
    [JsonProperty("radar_guided")]
    private bool _radarGuided;

    [JsonProperty("standard_rating")]
    private int _standardRating;

    [JsonProperty("bvr_rating")]
    private int _bvrRating;

    [JsonProperty("bvr_range_forward")]
    private int _bvrRangeForward;

    [JsonProperty("bvr_range_beam")]
    private int _bvrRangeBeam;

    [JsonProperty("bvr_range_rear")]
    private int _bvrRangeRear;

    [JsonProperty("weapon_display_name")]
    private string _weaponDisplayName;

    [JsonProperty("weapon_type"), JsonConverter(typeof(StringEnumConverter))]
    AirToAirWeaponType _weaponType;

    public bool radarGuided { get { return _radarGuided; } }

    public int standardRating { get { return _standardRating; } }

    public int bvrRating { get { return _bvrRating; } }

    public int bvrRangeForward { get { return _bvrRangeForward; } }

    public int bvrRangeBeam { get { return _bvrRangeBeam; } }

    public int bvrRangeRear { get { return _bvrRangeRear; } }

    public string weaponDisplayName { get { return _weaponDisplayName; } }

    public AirToAirWeaponType weaponType { get { return _weaponType; } }

    [JsonConstructor]
    public AirToAirWeaponData() { }

    // Copy constructor
    public AirToAirWeaponData(AirToAirWeaponData other)
    {
        _radarGuided = other._radarGuided;
        _standardRating = other._standardRating;
        _bvrRating = other._bvrRating;
        _bvrRangeForward = other._bvrRangeForward;
        _bvrRangeBeam = other._bvrRangeBeam;
        _bvrRangeRear = other._bvrRangeRear;
        _weaponDisplayName = other._weaponDisplayName;
        _weaponType = other._weaponType;
    }

    public override string ToString()
    {
        return weaponDisplayName+" Radar Guided: "+radarGuided+", Standard Rating: "+standardRating
            +", BVR Rating: "+bvrRating+", BVR Range Forward: "+_bvrRangeForward+", BVR Range Beam: "+bvrRangeBeam
            +", BVR Range Rear: "+bvrRangeRear; 
    }

}
