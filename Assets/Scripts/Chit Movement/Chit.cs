using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Chit : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    
    Vector3 elevatedPosition;
    bool elevated;
    
    private void Update()
    {
        if (elevated) {
            //var step = speed * Time.deltaTime; // calculate distance to move
            //transform.position = Vector3.MoveTowards(transform.position, elevatedPosition, step);
            transform.position = elevatedPosition;
        }
    }

    private void OnMouseDown()
    {
        var newPos = transform.position;
        newPos.y += 1f;
        elevatedPosition = newPos;
        elevated = true;
    }

    private void OnMouseUp()
    {
        elevated = false;
    }

}
