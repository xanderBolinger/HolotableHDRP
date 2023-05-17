using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Operation {

    public class Conflict
    {
        public Dictionary<OperationUnit, List<Vector2Int>> participants;

        public Conflict() {
            participants = new Dictionary<OperationUnit, List<Vector2Int>>();
        }

        public void AddParticipant(OperationUnit ou, List<Vector2Int> cords) {
            participants.Add(ou, cords);
        }

    }

}

