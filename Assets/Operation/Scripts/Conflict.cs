using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Operation {

    public class Conflict
    {
        public OperationUnit aggressor;
        public List<OperationUnit> targets;



        public Conflict(OperationUnit a, List<OperationUnit> b)
        {
            aggressor = a;
            targets = b;
        }


    }

}

