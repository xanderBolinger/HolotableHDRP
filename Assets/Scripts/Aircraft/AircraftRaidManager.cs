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

        // Fuel depletion 
        // Remove markers

        var flights = aircraftFlightManager.aircraftFlights;

        foreach (var flight in flights) {
            if (flight.flightStatus == AircraftFlight.FlightStatus.Disordered)
                DisorderedRecovery(flight);
            flight.SpendFuel();
            flight.zoomClimb = false;
            flight.disengaing = false;
        }

    }

    public static void DisorderedRecovery(AircraftFlight flight) {

        var roll = DiceRoller.Roll(2, 20);

        var modifiedRoll = roll + flight.agressionValue;

        var recovered = modifiedRoll >= 15;

        Debug.Log("Flight("+flight.flightCallsign+") attempt disordered recovery roll: "
            +roll+", modified roll: "+modifiedRoll+", agression value: "+flight.agressionValue+"," +
            " Recovered: "+recovered.ToString().ToUpper());

    }


}
