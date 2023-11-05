using System.Collections.Generic;

public enum ElevationDistribution { 
    Smooth=1,Normal=2,Bumpy=3,VeryBumpy=4
}

public enum ElevationHeightRange { 
    Low=3,Regular=5,High=7,VeryHigh=10
}

public class ElevationCalculator
{

    public static List<List<int>> GetElevationMap(int width, int height,
        ElevationDistribution distribution, ElevationHeightRange heightRange) {

        var map = PerlinNoiseCalculator.GetNoiseMap(width, height, (int)distribution, true);
        var formattedMap = PerlinNoiseCalculator.FormatOutput(map, 0f, (int)heightRange);

        return formattedMap;

    }

}
