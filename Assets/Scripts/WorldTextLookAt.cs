using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class WorldTextLookAt : MonoBehaviour
{

    Transform cameraTransform;

    Transform _transform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _transform.LookAt(_transform.position - (cameraTransform.position - _transform.position));
    }
}
