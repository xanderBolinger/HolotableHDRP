using Newtonsoft.Json;
using System;

[Serializable]
public class AircraftRadar
{
    public bool active = false;

    [JsonProperty("detection_class")]
    string _detectionClass;

    [JsonProperty("radar_max_range_active")]
    int _radarMaxRangeActive;
    [JsonProperty("radar_medium_range_active")]
    int _radarMediumRangeActive;
    [JsonProperty("radar_short_range_active")]
    int _radarShortRangeActive;

    [JsonProperty("radar_max_range")]
    int _radarMaxRange;
    [JsonProperty("radar_medium_range")]
    int _radarMediumRange;
    [JsonProperty("radar_short_range")]
    int _radarShortRange;

    [JsonProperty("irst_front")]
    int _irstFront;
    [JsonProperty("irst_rear")]
    int _irstRear;

    public int radarMaxRangeActive { get { return _radarMaxRangeActive; } }
    public int radarMediumRangeActive { get { return _radarMediumRangeActive; } }
    public int radarShortRangeActive { get { return _radarShortRangeActive; } }

    public int radarMaxRange { get { return _radarMaxRange; } }
    public int radarMediumRange { get { return _radarMediumRange; } }
    public int radarShortRange { get { return _radarShortRange; } }

    public string detectionClass { get { return _detectionClass; } }

    public int irstFront { get { return _irstFront; } }
    public int irstRear { get { return _irstRear; } }

    [JsonConstructor]
    public AircraftRadar() { }

    public AircraftRadar(AircraftRadar radar) {
        _detectionClass = radar.detectionClass;

        _radarMaxRangeActive = radar._radarMaxRangeActive;
        _radarMediumRangeActive = radar._radarMediumRangeActive;
        _radarShortRangeActive = radar._radarShortRangeActive;

        _radarMaxRange = radar.radarMaxRange;
        _radarMediumRange = radar.radarMediumRange;
        _radarShortRange = radar.radarShortRange;

        _irstFront = radar.irstFront;
        _irstRear = radar.irstRear;

        active = radar.active;
    }
}
