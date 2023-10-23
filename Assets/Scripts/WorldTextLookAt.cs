using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTextLookAt : MonoBehaviour
{

    [SerializeField] Transform targetTransform;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _transform.LookAt(-targetTransform.position);
    }
}
