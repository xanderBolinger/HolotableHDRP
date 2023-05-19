using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Operation {
    public class OperationUnitData : MonoBehaviour
    {

        public OperationUnit ou;

        public Vector3 destination;
        float speed = 0.5f;

        public void Update()
        {
            if(transform.localPosition != destination) 
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, Time.deltaTime * speed);
        }

    }
}

