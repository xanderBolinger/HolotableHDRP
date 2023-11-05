using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseCalculator
{
    public static List<List<float>> GetNoiseMap(int width, int height, float scale, bool randomOrigin, 
        float inputXOrg=0, float inputYOrg =0)
    {
        List<List<float>> list = new List<List<float>>();

        float y = 0.0F;

        float xOrg = randomOrigin ? DiceRoller.Roll(0, 10) : inputXOrg;
        float yOrg = randomOrigin ? DiceRoller.Roll(0, 10) : inputYOrg;

        while (y < height)
        {
            var row = new List<float>();

            float x = 0.0F;
            while (x < width)
            {
                float xCoord = xOrg + x / width * scale;
                float yCoord = yOrg + y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                row.Add(sample);
                
                x++;
            }
            y++;

            list.Add(row);
        }

        return list;
    }

    public static List<List<int>> FormatOutput(List<List<float>> floatMap, float minRange, float maxRange) {

        var output = new List<List<int>>();

        foreach (var row in floatMap)
        {
            var outputRow = new List<int>();

            foreach (var elem in row)
                outputRow.Add(ConvertRange(0f, 1f, minRange, maxRange, elem));

            output.Add(outputRow);
        }

        return output;
    }

    public static void PrintMap(List<List<float>> list, float rangeMin, float rangeMax) {
        string output = "Output: ";
        foreach (var row in list)
        {
            output += "[";
            foreach (var elem in row)
            {
                output += ConvertRange(0f, 1f, rangeMin, rangeMax, elem)
                    + (elem != row[row.Count - 1] ? ", " : "]");
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    private static int ConvertRange(float _input_range_min,
        float _input_range_max, float _output_range_min,
        float _output_range_max, float _input_value_tobe_converted)
    {
        float diffOutputRange = Mathf.Abs((_output_range_max - _output_range_min));
        float diffInputRange = Mathf.Abs((_input_range_max - _input_range_min));
        float convFactor = (diffOutputRange / diffInputRange);
        return (int)(_output_range_min + (convFactor * (_input_value_tobe_converted - _input_range_min)));
    }

}
