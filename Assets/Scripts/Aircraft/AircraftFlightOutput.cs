using static AircraftMovementData;
using static AircraftSpeedData;

public class AircraftFlightOutput
{
    public static string ToString(AircraftFlight aircraftFlight)
    {
        string aircraft = "";

        foreach (var a in aircraftFlight.flightAircraft)
        {
            aircraft += "- " + a.ToString() + "\n";
        }

        string aircraftMoveData = "";

        if (aircraftFlight.flightAircraft.Count > 0)
        {
            aircraftMoveData += aircraftFlight.side.ToString() + "\n";
            aircraftMoveData += "Flight Status: " + aircraftFlight.flightStatus + ", Disengaging: " + aircraftFlight.disengaing +"\n";
            aircraftMoveData += "Markers: " + (aircraftFlight.bvrAvoid ? "BVR Avoid, " : "")
                + (aircraftFlight.climbed ? "Climbed, " : "")
                + (aircraftFlight.zoomClimb ? "Zoom Climbed, " : "")
                + (aircraftFlight.manueverMarker ? "Manuever, " : "")
                + "\n";
            aircraftMoveData += "Flight Quality: " + aircraftFlight.quality;
            aircraftMoveData += ", Radar Active: " + aircraftFlight.flightAircraft[0].aircraftDetectionData.aircraftRadar.active + "\n";
            aircraftMoveData += "Suit: " + aircraftFlight.GetDetectionSuit() + ", ";
            aircraftMoveData += "Detected: " + aircraftFlight.Detected() + "\n";
            aircraftMoveData += "Facing: " + aircraftFlight.GetFacing() + ", ";
            aircraftMoveData += "Cord: " + aircraftFlight.GetLocation().GetCord() + "\n";
            aircraftMoveData += "alt: cmbt/dash/manvr\n";

            var a = aircraftFlight.flightAircraft[0];

            aircraftMoveData += GetAircraftSpeedString(a.movementData);

        }

        string alt = aircraftFlight.flightAircraft.Count > 0 ? aircraftFlight.flightAircraft[0].movementData.altitude.ToString() : "N/A";
        string spd = aircraftFlight.flightAircraft.Count > 0 ? aircraftFlight.flightAircraft[0].movementData.speed.ToString() : "N/A";

        return "Flight: " + aircraftFlight.flightCallsign + ", Alt: " + alt + ", Spd: " + spd + "\n" + aircraft + aircraftMoveData;
    }

    private static string GetAircraftSpeedString(AircraftMovementData md)
    {

        string speedString = "";

        // Unladen
        speedString += GetSingleSpeedString(md, false) + "\nLaden: \n";

        // Laden
        speedString += GetSingleSpeedString(md, true);

        return speedString;

    }

    private static string GetSingleSpeedString(AircraftMovementData md, bool laden)
    {

        string speedLow = GetSpeed(md, AircraftAltitude.LOW, laden);
        string speedDeck = GetSpeed(md, AircraftAltitude.DECK, laden);

        return "VH: " + GetSpeed(md, AircraftAltitude.VERY_HIGH, laden) + "\n H: " + GetSpeed(md, AircraftAltitude.HIGH, laden)
                + "\n M: " + GetSpeed(md, AircraftAltitude.MEDIUM, laden)
                + (speedLow == speedDeck ? "\n L/D: " + speedLow : "\n L: " + speedLow + "\n D: " + speedDeck);
    }

    private static string GetSpeed(AircraftMovementData md, AircraftAltitude altitude, bool laden)
    {
        int cmd = md.GetSpeed(AircraftSpeed.Combat, altitude, laden);
        int dash = md.GetSpeed(AircraftSpeed.Dash, altitude, laden);
        int man = md.GetManueverRating(altitude, laden);

        return cmd + "/" + dash + "/" + man;
    }
}
