using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitManager : MonoBehaviour
{
    public static ChitManager instance;
    public List<Chit> selectedChits;

    private void Awake()
    {
        instance = this;
        selectedChits = new List<Chit>();
    }

}
