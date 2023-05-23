using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Operation {
    public class OperationUnitData : MonoBehaviour
    {

        public OperationUnit ou;

        public Vector3 destination { get; private set; }
        float t;
        Vector3 startPosition;
        const float timeToReachTarget = 1f;

        public Dictionary<Renderer, Material> plannedHexMaterials = new Dictionary<Renderer, Material>();


        public void Update()
        {
            
            /*if(transform.localPosition != destination) 
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, Time.deltaTime * speed);*/
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, destination, t);
        }

        public void SetDestination(Vector3 destination) {
            t = 0;
            startPosition = transform.position;
            this.destination = destination;
        }

    }
}

