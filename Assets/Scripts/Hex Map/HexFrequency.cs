using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexFrequency
{

    public enum Frequency { 
        None,VeryLow,LowDense,Low,MediumThin,Medium,High,Test,Test2,Test3
    }

    public int margin;
    public float magnificationUpper;
    public float magnifiactionLower;
    public int densityUpper;
    public int densityLower;


    public HexFrequency(Frequency freq) {

        switch (freq) {
            case Frequency.Test:
                margin = MapGenerator.instance.testMargin;
                magnificationUpper = MapGenerator.instance.testMagnification;
                magnifiactionLower = MapGenerator.instance.testMagnification;
                densityUpper = MapGenerator.instance.testDensity;
                densityLower = MapGenerator.instance.testDensity;
                break;
            case Frequency.Test2:
                margin = MapGenerator.instance.testMargin2;
                magnificationUpper = MapGenerator.instance.testMagnification2;
                magnifiactionLower = MapGenerator.instance.testMagnification2;
                densityUpper = MapGenerator.instance.testDensity2;
                densityLower = MapGenerator.instance.testDensity2;
                break;
            case Frequency.Test3:
                margin = MapGenerator.instance.testMargin3;
                magnificationUpper = MapGenerator.instance.testMagnification3;
                magnifiactionLower = MapGenerator.instance.testMagnification3;
                densityUpper = MapGenerator.instance.testDensity3;
                densityLower = MapGenerator.instance.testDensity3;
                break;
            case Frequency.None:
                margin = 1000000;
                magnificationUpper = 10f;
                magnifiactionLower = 8f;
                densityUpper = 10;
                densityLower = 10;
                break;
            case Frequency.VeryLow:
                margin = 5;
                magnificationUpper = 2.5f;
                magnifiactionLower = 2.5f;
                densityUpper = 5;
                densityLower = 5;
                break;
            case Frequency.LowDense:
                margin = 9;
                magnificationUpper = 8.75f;
                magnifiactionLower = 8.75f;
                densityUpper = 10;
                densityLower = 10;
                break;
            case Frequency.Low:
                margin = 4;
                magnificationUpper = 2.5f;
                magnifiactionLower = 2.5f;
                densityUpper = 5;
                densityLower = 5;
                break;
            case Frequency.MediumThin:
                margin = 2;
                magnificationUpper = 5.25f;
                magnifiactionLower = 5.25f;
                densityUpper = 4;
                densityLower = 4;
                break;
            case Frequency.Medium:
                margin = 3;
                magnificationUpper = 2.5f;
                magnifiactionLower = 2.5f;
                densityUpper = 5;
                densityLower = 5;
                break;
            case Frequency.High:
                margin = 3;
                magnificationUpper = 3.5f;
                magnifiactionLower = 3.5f;
                densityUpper = 5;
                densityLower = 5;
                break;

        }
    }



}
