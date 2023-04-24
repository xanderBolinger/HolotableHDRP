using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexFrequency
{

    public enum Frequency { 
        None,VeryLow,LowDense,Low,MediumThin,Medium,High   
    }

    public int margin;
    public float magnificationUpper;
    public float magnifiactionLower;
    public int densityUpper;
    public int densityLower;


    public HexFrequency(Frequency freq) {

        switch (freq) {
            case Frequency.None:
                margin = 1000000;
                magnificationUpper = 10f;
                magnifiactionLower = 8f;
                densityUpper = 10;
                densityLower = 10;
                break;
            case Frequency.VeryLow:
                margin = 9;
                magnificationUpper = 7f;
                magnifiactionLower = 7f;
                densityUpper = 10;
                densityLower = 10;
                break;
            case Frequency.LowDense:
                margin = 9;
                magnificationUpper = 8.75f;
                magnifiactionLower = 8.75f;
                densityUpper = 10;
                densityLower = 10;
                break;
            case Frequency.Low:
                margin = 2;
                magnificationUpper = 7.25f;
                magnifiactionLower = 7.25f;
                densityUpper = 3;
                densityLower = 3;
                break;
            case Frequency.MediumThin:
                margin = 2;
                magnificationUpper = 5.25f;
                magnifiactionLower = 5.25f;
                densityUpper = 4;
                densityLower = 4;
                break;
            case Frequency.Medium:
                margin = 2;
                magnificationUpper = 7.25f;
                magnifiactionLower = 7.25f;
                densityUpper = 4;
                densityLower = 4;
                break;
            case Frequency.High:
                margin = 1;
                magnificationUpper = 8.25f;
                magnifiactionLower = 8.25f;
                densityUpper = 3;
                densityLower = 3;
                break;

        }
    }



}
