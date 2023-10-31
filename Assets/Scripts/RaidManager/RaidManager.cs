using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidManager : MonoBehaviour
{

    [SerializeField]
    AircraftRaidManager aircraftRaidManager;

    public int turn;

    public void NextTurn() {
        turn++;
        aircraftRaidManager.NextTurn();
    }



}
