

public class AircraftUtility { 


    public static int Distance(AircraftFlight flight, AircraftFlight flight2) {

        return HexMap.GetDistance(flight.GetLocation(), flight2.GetLocation());

    }

}