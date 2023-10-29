using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftRadar
{
    //public bool active = true;

    [JsonProperty("detection_class")]
    char _detectionClass;
    [JsonProperty("radar_range")]
    int _radarRange;

    public int radarRange { get { return _radarRange; } }
    public char detectionClass { get { return _detectionClass; } }

}
