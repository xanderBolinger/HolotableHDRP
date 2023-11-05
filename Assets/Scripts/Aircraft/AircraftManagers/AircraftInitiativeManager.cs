using System.Collections.Generic;
using UnityEngine;

public class AircraftInitiativeManager : MonoBehaviour
{


    public void ActivateFlights() {

        var chitPull = GetChitPull();

        Debug.Log("Initative: "+chitPull);

    }

    public int GetChitPull() {

        List<int> chitPool = new List<int>();
        chitPool.Add(0);
        chitPool.Add(0);
        chitPool.Add(1);
        chitPool.Add(1);
        chitPool.Add(1);
        chitPool.Add(2);
        chitPool.Add(2);
        chitPool.Add(2);
        chitPool.Add(3);

        return chitPool[DiceRoller.Roll(0, chitPool.Count - 1)];
    }


}