using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Operation.OperationUnit;

namespace Operation {
    public class OperationControlsManager : MonoBehaviour
    {

        public enum ControlsStatus { 
            RETREAT_ADVANCE,MOVE
        }

        public bool ocmEnabled = false;
        public ControlsStatus status;
        public UnitStatus appliedStatus = UnitStatus.FRESH;
        public int subUnitIndex = 0;

        GameObject selectedUnitObject;
        OperationManager opm;

        private void Start()
        {
            opm = GetComponent<OperationManager>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
                selectedUnitObject = null;
                return;
            }

            if (!Input.GetMouseButtonDown(0) || !ocmEnabled)
            {
                return;
            }

            var hitObject = GetClickedObject();

            if (hitObject.tag == "Hex")
            {
                ClickHex(hitObject);
            }
            else if (hitObject.tag == "Unit")
            {
                selectedUnitObject = hitObject;
            }
            else {
                selectedUnitObject = null;
            }


        }

        public void CreateGame() { 
        
        }

        public void AdvanceTimeSegment()
        {
            opm.AdvanceTS();
        }

        public void SetUnitStatus() {
            if (selectedUnitObject == null)
            {
                Debug.Log("Select a Operation Unit to set a subunit status.");
                return;
            }




        }

        private void ClickHex(GameObject clickedHex) {
            if (selectedUnitObject == null) {
                return;
            }

            if (status == ControlsStatus.RETREAT_ADVANCE)
            {
                RetreatAdvance(clickedHex);
            }
            else {
                AddPlannedMovement();
            }

        }

        private void RetreatAdvance(GameObject clickedHex) {
            var hexCord = clickedHex.GetComponent<HexCord>() != null ? clickedHex.GetComponent<HexCord>() : clickedHex.GetComponentInChildren<HexCord>();
            var cord = new Vector2Int(hexCord.x, hexCord.y);
            var movingUnit = selectedUnitObject.GetComponent<OperationUnitData>().ou;

            if (movingUnit.spentFreeConflictResultMovement) {
                Debug.Log("Already spent free conflict advance or retreat.");
                return;
            }

            foreach (var unit in opm.operationUnits) {
                if (unit.side == movingUnit.side)
                    continue;
                if (unit.hexPosition == cord) {
                    Debug.Log("Invalid Hex");
                    return;
                }
            }

            CombatResults.AdvanceOrRetreat(opm, movingUnit, cord, selectedUnitObject.transform.position, 
                opm.hexes[cord.x][cord.y].transform.position);
        }

        private void AddPlannedMovement() { 
        
        }

        private GameObject GetClickedObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                return hitInfo.collider.transform.gameObject;
            }

            return null;
        }



    }

}


