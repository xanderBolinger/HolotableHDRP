using Newtonsoft.Json;
using System;

[Serializable]
public class AircraftJammer
{
    [JsonProperty("jammer_strength_noise")]
    int _jammerStrengthNoise;
    [JsonProperty("jammer_strength_deception")]
    int _jammerStrengthDeception;

    public int jammerStrengthNoise { get { return _jammerStrengthNoise; } }
    public int jammerStrengthDeception { get { return _jammerStrengthDeception; } }

    [JsonConstructor]
    public AircraftJammer() { }

    public AircraftJammer(AircraftJammer aircraftJammer) {
        _jammerStrengthNoise = aircraftJammer.jammerStrengthNoise;
        _jammerStrengthDeception = aircraftJammer._jammerStrengthDeception;
    }

}
