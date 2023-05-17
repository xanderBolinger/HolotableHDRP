using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Operation.TimeSegment;

namespace Operation {

    public class Operation : MonoBehaviour
    {

        public List<TimeSegment> timeSegments;
        public List<OperationUnit> operationUnits;

        public void CreateOperation() {

            for (int i = 1; i <= 24; i++) {

                TimeUnit timeUnit = TimeUnit.AM;

                if (i >= 19 && i <= 4)
                {
                    timeUnit = TimeUnit.NIGHT;
                }
                else if (i >= 12 && i < 19) {
                    timeUnit = TimeUnit.PM;
                }

                timeSegments.Add(new TimeSegment(i, timeUnit));
            }

            operationUnits = new List<OperationUnit>();
        }

        public void AddOU(OperationUnit ou) {
            operationUnits.Add(ou);
        }

        public void AdvanceTS() { 

        }


    }
}



