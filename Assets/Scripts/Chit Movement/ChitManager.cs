using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitManager : MonoBehaviour
{
    public static ChitManager instance;
    public Chit selectedChit;

    private void Awake()
    {
        instance = this;
    }

}
