using Newtonsoft.Json;

public class AirToAirPayload {

    [JsonProperty("depletion_points")]
    private int _depletionPoints;
    public int depletionPoints { get { return _depletionPoints; } }

    public bool depleted;

    [JsonConstructor]
    public AirToAirPayload() { }

    public AirToAirPayload(AirToAirPayload other)
    {

        _depletionPoints = other._depletionPoints;
        depleted = other.depleted;
    }
}
