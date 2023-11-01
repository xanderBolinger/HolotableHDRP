using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class AirToAirPylon {

    [JsonProperty("depletion_points")]
    private int _depletionPoints;
    [JsonProperty("weapon_type"), JsonConverter(typeof(StringEnumConverter))]
    AirToAirWeaponType _weaponType;

    public int depletionPoints { get { return _depletionPoints; } }

    public AirToAirWeaponType weaponType { get { return _weaponType; } }

    public bool depleted;

    [JsonConstructor]
    public AirToAirPylon() { }

    public AirToAirPylon(AirToAirPylon other)
    {
        _weaponType = other.weaponType;
        _depletionPoints = other._depletionPoints;
        depleted = other.depleted;
    }

    public override string ToString()
    {
        return weaponType + " Depleted: " + depleted + ", DP: " + depletionPoints;
    }

}
