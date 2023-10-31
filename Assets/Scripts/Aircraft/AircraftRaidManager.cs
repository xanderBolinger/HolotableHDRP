using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftRaidManager : MonoBehaviour
{
    [SerializeField]
    AircraftDetectionManager aircraftDetectionManager;
    [SerializeField]
    AircraftFlightManager aircraftFlightManager;

    public void NextTurn() {
        aircraftDetectionManager.UndetectFlights(aircraftFlightManager.aircraftFlights);
        aircraftDetectionManager.DetectFlights(aircraftFlightManager.aircraftFlights);
        AdminPhase();
    }

    public void AdminPhase() { 
    
    }

}
