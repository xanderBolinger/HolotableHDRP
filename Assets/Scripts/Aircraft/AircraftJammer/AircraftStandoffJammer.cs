using HexMapper;
using Newtonsoft.Json;

public class AircraftStandoffJammer
{

    public Direction facing;
    public bool active;

    [JsonProperty("short_range")]
    private int _shortRange;
    [JsonProperty("medium_range")]
    private int _mediumRange;
    [JsonProperty("long_range")]
    private int _longRange;

    [JsonProperty("short_range_strength")]
    private int _shortRangeStrength;
    [JsonProperty("medium_range_strength")]
    private int _mediumRangeStrength;
    [JsonProperty("long_range_strength")]
    private int _longRangeStrength;

    public int shortRange { get { return _shortRange; } }
    public int mediumRange { get { return _mediumRange; } }
    public int longRange { get { return _longRange; } }
    public int shortRangeStrength { get { return _shortRangeStrength; } }
    public int mediumRangeStrength { get { return _mediumRangeStrength; } }
    public int longRangeStrength { get { return _longRangeStrength; } }

    [JsonConstructor]
    public AircraftStandoffJammer() { }

    public AircraftStandoffJammer(AircraftStandoffJammer other)
    {
        _shortRange = other._shortRange;
        _mediumRange = other._mediumRange;
        _longRange = other._longRange;
        _shortRangeStrength = other._shortRangeStrength;
        _mediumRangeStrength = other._mediumRangeStrength;
        _longRangeStrength = other._longRangeStrength;
        facing = other.facing;
        active = other.active;
    }

}
