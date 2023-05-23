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
        public MoveType appliedMoveType = MoveType.REGULAR;
        public UnitStatus appliedStatus = UnitStatus.FRESH;
        public int subUnitIndex = 0;

        public GameObject bluforPrefab;
        public GameObject opforPrefab;

        [HideInInspector]
        public GameObject selectedUnitObject;
        [HideInInspector]
        public OperationManager opm;
        [HideInInspector]
        public OperationJsonManager opjsm;
        [HideInInspector]
        public GameObject selectedHex;

        public Material selectedBluforUnitMaterial;
        public Material selectedOpforUnitMaterial;
        public Material bluforUnitMaterial;
        public Material opforUnitMaterial;
        public Material plannedHexMaterial;



        private void Start()
        {
            opm = GetComponent<OperationManager>();
            opjsm = GetComponent<OperationJsonManager>();
        }

        void Update()
        {
            if (!ocmEnabled)
            {
                return;
            }

            if (selectedUnitObject != null && Input.GetKeyDown(KeyCode.X)) {
                var selectedUnit = selectedUnitObject.GetComponent<OperationUnitData>().ou;
                
                if (opm.currentTimeSegment.plannedMovement.ContainsKey(selectedUnit)) {
                    opm.currentTimeSegment.plannedMovement.Remove(selectedUnit);
                    Debug.Log("Cleared Planned Movement For: "+selectedUnit.unitName);
                }
                UnselectUnitColor(selectedUnitObject);
                SelectUnitColor(selectedUnitObject);
            }

            if (!Input.GetMouseButtonDown(0)) {
                return;
            }

            var hitObject = GetClickedObject();

            ResolveHitObject(hitObject);


        }

        private void ResolveHitObject(GameObject hitObject) {
            if (hitObject.tag == "Hex")
            {
                ClickHex(hitObject);

                if (selectedUnitObject != null)
                    SelectUnitColor(selectedUnitObject);
                else
                {
                    selectedHex = hitObject;
                    Debug.Log("Selected Hex: " + HexCord.GetHexCord(selectedHex).GetCord());
                }
            }
            else if (hitObject.tag == "Unit" && selectedUnitObject != null && selectedUnitObject == hitObject)
            {
                UnselectUnitColor(selectedUnitObject);
                selectedUnitObject = null;
                selectedHex = null;
            }
            else if (hitObject.tag == "Unit")
            {
                if (selectedUnitObject != null)
                    UnselectUnitColor(selectedUnitObject);
                selectedUnitObject = hitObject;
                selectedUnitObject.GetComponent<OperationUnitData>().ou.Output(opm);
                SelectUnitColor(selectedUnitObject);
                selectedHex = null;
            }
            else if (selectedUnitObject != null)
            {
                UnselectUnitColor(selectedUnitObject);
                selectedUnitObject = null;
                selectedHex = null;
            }
        }

        public void DeseletUnit() {
            if (selectedUnitObject == null)
                return;
            UnselectUnitColor(selectedUnitObject);
            selectedUnitObject = null;
        }

        private void UnselectUnitColor(GameObject selectedUnitObject) {
            var ou = selectedUnitObject.GetComponent<OperationUnitData>().ou;

            if (ou.side == Side.BLUFOR)
            {
                selectedUnitObject.GetComponent<Renderer>().material = bluforUnitMaterial;
            }
            else {
                selectedUnitObject.GetComponent<Renderer>().material = opforUnitMaterial;
            }

            foreach (var plannedHex in selectedUnitObject.GetComponent<OperationUnitData>().plannedHexMaterials) {
                plannedHex.Key.material = plannedHex.Value;
            }

            selectedUnitObject.GetComponent<OperationUnitData>().plannedHexMaterials.Clear();

        }

        private void SelectUnitColor(GameObject selectedUnitObject) {
            var ou = selectedUnitObject.GetComponent<OperationUnitData>().ou;

            if (ou.side == Side.BLUFOR)
            {
                selectedUnitObject.GetComponent<Renderer>().material = selectedBluforUnitMaterial;
            }
            else
            {
                selectedUnitObject.GetComponent<Renderer>().material = selectedOpforUnitMaterial;
            }

            if (!opm.currentTimeSegment.plannedMovement.ContainsKey(ou))
                return;

            SelectPlannedHexes();
            

        }

        private void SelectPlannedHexes() {
            var ou = selectedUnitObject.GetComponent<OperationUnitData>().ou;
            var plannedHexes = selectedUnitObject.GetComponent<OperationUnitData>().plannedHexMaterials;



            foreach (var plannedHex in opm.currentTimeSegment.plannedMovement[ou])
            {
                var hex = opm.hexes[plannedHex.x][plannedHex.y];
                if (hex.GetComponent<HexCord>() != null)
                {
                    var renderer = hex.GetComponent<Renderer>();
                    if (!plannedHexes.ContainsKey(renderer))
                    {
                        plannedHexes.Add(renderer, renderer.material);
                    }
                    renderer.material = plannedHexMaterial;
                }
                else
                {
                    var renderer = hex.GetComponentInChildren<Renderer>();
                    if (!plannedHexes.ContainsKey(renderer)) { 
                        plannedHexes.Add(renderer, renderer.material);
                    }
                    renderer.material = plannedHexMaterial;
                }
            }
        }

        public void CreateGame() {

            // Get hexes 
            // Get hex cords 
            // Create OPM 
            // Call set hexes on OPM

            opm.CreateOperation();

            var unitPref1 = Instantiate(bluforPrefab);
            unitPref1.transform.position = new Vector3(0, 0, 0);


            var unitPref2 = Instantiate(bluforPrefab);
            unitPref2.transform.position = new Vector3(0, 0, 0);


            var unitPref3 = Instantiate(opforPrefab);
            unitPref3.transform.position = new Vector3(0, 0, 0);


            var unitPref4 = Instantiate(opforPrefab);
            unitPref4.transform.position = new Vector3(0, 0, 0);

            OperationUnit ou = new OperationUnit("ou1", unitPref1, new Vector2Int(0, 0), Side.BLUFOR);

            ou.AddUnit(new Unit("plt1-g1"));
            ou.AddUnit(new Unit("plt1-g2"));
            ou.AddUnit(new Unit("plt1-g3"));
            ou.AddUnit(new Unit("plt1-g4"));

            OperationUnit ou2 = new OperationUnit("ou2", unitPref2, new Vector2Int(0, 3), Side.BLUFOR);

            ou2.AddUnit(new Unit("plt2-g1"));
            ou2.AddUnit(new Unit("plt2-g2"));
            ou2.AddUnit(new Unit("plt2-g3"));
            ou2.AddUnit(new Unit("plt2-g4"));

            OperationUnit ou3 = new OperationUnit("ou3", unitPref3, new Vector2Int(1, 1), Side.OPFOR);

            ou3.AddUnit(new Unit("plt1-c1"));
            ou3.AddUnit(new Unit("plt1-c2"));
            ou3.AddUnit(new Unit("plt1-c3"));
            ou3.AddUnit(new Unit("plt1-c4"));
            ou3.AddUnit(new Unit("plt1-c5"));
            ou3.AddUnit(new Unit("plt1-c6"));
            ou3.AddUnit(new Unit("plt1-c7"));

            OperationUnit ou4 = new OperationUnit("ou4", unitPref4, new Vector2Int(5, 3), Side.OPFOR);

            ou4.AddUnit(new Unit("plt2-c1"));
            ou4.AddUnit(new Unit("plt2-c2"));
            ou4.AddUnit(new Unit("plt2-c3"));
            ou4.AddUnit(new Unit("plt2-c4"));
            ou4.AddUnit(new Unit("plt2-c5"));
            ou4.AddUnit(new Unit("plt2-c6"));
            ou4.AddUnit(new Unit("plt2-c7"));

            

            unitPref1.name = ou.unitName;
            unitPref2.name = ou2.unitName;
            unitPref3.name = ou3.unitName;
            unitPref4.name = ou4.unitName;

            unitPref1.GetComponent<OperationUnitData>().ou = ou;
            unitPref2.GetComponent<OperationUnitData>().ou = ou2;
            unitPref3.GetComponent<OperationUnitData>().ou = ou3;
            unitPref4.GetComponent<OperationUnitData>().ou = ou4;

            opm.AddOU(ou);
            opm.AddOU(ou2);
            opm.AddOU(ou3);
            opm.AddOU(ou4);

            opm.SetHexes(MapGenerator.instance.hexes);

            MoveStartingUnit(ou, unitPref1, Clear(ou));
            MoveStartingUnit(ou2, unitPref2, Clear(ou2));
            MoveStartingUnit(ou3, unitPref3, Clear(ou3));
            MoveStartingUnit(ou4, unitPref4, Clear(ou4));


            opm.undoRedo.AddTurn();
        }

        private bool Clear(OperationUnit ou) { 
            return opm.hexCords[ou.hexPosition.x][ou.hexPosition.y].hexType == HexCord.HexType.CLEAR;
        }

        private void MoveStartingUnit(OperationUnit ou, GameObject unitPref, bool clear) {
            opm.gridMover.MoveUnit(ou, ou.hexPosition,
                    unitPref.transform.position, opm.hexes[ou.hexPosition.x][ou.hexPosition.y].transform.position, clear);
        }

        public void AdvanceTimeSegment()
        {
            opm.AdvanceTS();
            if (selectedUnitObject != null) { 
                UnselectUnitColor(selectedUnitObject);
                SelectUnitColor(selectedUnitObject);
            }
        }

        public void SetUnitStatus() {
            if (selectedUnitObject == null)
            {
                Debug.Log("Select a Operation Unit to set a subunit status.");
                return;
            }

            var selectedUnit = selectedUnitObject.GetComponent<OperationUnitData>().ou;

            if (subUnitIndex < 0 || subUnitIndex >= selectedUnit.GetUnits().Count) {
                Debug.Log("Subunit index out of bounds: "+subUnitIndex);
                return;
            }

            selectedUnit.GetUnit(subUnitIndex).unitStatus = appliedStatus;
            selectedUnit.UnitStatusUpdate();

        }

        public void SetUnitMoveType() {
            if (selectedUnitObject == null)
            {
                Debug.Log("Select a Operation Unit to change move type.");
                return;
            }

            var selectedUnit = selectedUnitObject.GetComponent<OperationUnitData>().ou;

            if (selectedUnit.moveType == MoveType.NONE && appliedMoveType != MoveType.NONE)
            {
                selectedUnit.moveType = appliedMoveType;
                Debug.Log("Set move type.");
            }
            else {
                Debug.Log("Could not set move type. Move type NONE or already chosen for this time unit.");
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
                AddPlannedMovement(clickedHex);
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

        private void AddPlannedMovement(GameObject clickedHex) {
            var hexCord = clickedHex.GetComponent<HexCord>() != null ? clickedHex.GetComponent<HexCord>() : clickedHex.GetComponentInChildren<HexCord>();
            var movingUnit = selectedUnitObject.GetComponent<OperationUnitData>().ou;

            Vector2Int lastPosition;

            if (opm.currentTimeSegment.plannedMovement.ContainsKey(movingUnit))
            {
                lastPosition = opm.currentTimeSegment.plannedMovement[movingUnit]
                    [opm.currentTimeSegment.plannedMovement[movingUnit].Count-1];
            }
            else {
                lastPosition = movingUnit.hexPosition;
            }

            OperationMovement.AddPlannedMovement(opm, movingUnit, hexCord, opm.hexCords, lastPosition);
            
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


