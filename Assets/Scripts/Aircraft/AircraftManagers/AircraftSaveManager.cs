using UnityEngine;

public class AircraftSaveManager : MonoBehaviour
{

    [SerializeField]
    string fileName;

    public void SaveAircraft() {
        var flights = AircraftFlightManager.aircraftFlightManager.aircraftFlights;
        var data = new AircraftSaveData(flights);
        AircraftSaveRunner.SaveAircraft(data, fileName);
    }

    public void LoadAircraft() { 
        var data = AircraftSaveRunner.LoadAircraft(fileName);
        AircraftFlightManager.aircraftFlightManager.LoadFlights(data);
    }

}
