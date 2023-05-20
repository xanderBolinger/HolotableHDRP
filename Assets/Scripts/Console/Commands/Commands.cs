using IngameConsole;
using Operation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecutableFromConsole]
public class Commands : MonoBehaviour
{
    public static bool consoleActive = false;

    public OperationControlsManager operationControlsManager;

    private BaseWriter _writer = new FormattedWriter();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            consoleActive = !consoleActive;
            operationControlsManager.ocmEnabled = !operationControlsManager.ocmEnabled;
        }

        if (!consoleActive)
            return;

    }

    [ConsoleMethod("create_ou", "Deletes selected OU removing it from the operation")]
    public void CreateOu([ConsoleParameter("name for created OU")] string ouName, [ConsoleParameter("side{BLUFOR or OPFOR}")] string side) {

        if (side.ToUpper() != "BLUFOR" && side.ToUpper() != "OPFOR") {
            _writer.NextLine();
            _writer.Write("Side: "+side+", invalid needs to be BLUFOR or OPFOR");
            return;
        }

        var (ou, ouExistsFlag) = GetOperationUnit(ouName, true);
        if (ouExistsFlag)
            return;
       
        var hexObject = operationControlsManager.selectedHex;
        var hexCord = HexCord.GetHexCord(hexObject);

        if (hexObject == null) {
            _writer.NextLine();
            _writer.Write("Select a hex for deployment.");
            return;
        }

        CreateOU(hexObject, hexCord, ouName, side);
    }

    private void CreateOU(GameObject hexObject, HexCord hexCord, string ouName, string side) {
        var opm = operationControlsManager.opm;

        var unitGameObject = Instantiate(side.ToUpper() == "BLUFOR" ? operationControlsManager.bluforPrefab : operationControlsManager.opforPrefab);

        unitGameObject.transform.position = hexObject.transform.position;

        OperationUnit ou = new OperationUnit(ouName, unitGameObject, hexCord.GetCord(),
            side.ToUpper() == "BLUFOR" ? OperationUnit.Side.BLUFOR : OperationUnit.Side.OPFOR);

        var unitDataComponent = unitGameObject.GetComponent<OperationUnitData>();
        unitDataComponent.ou = ou;

        opm.AddOU(ou);

        opm.gridMover.MoveUnit(ou, ou.hexPosition, unitGameObject.transform.position,
            hexObject.transform.position,
            hexCord.hexType == HexCord.HexType.CLEAR);
    }

    [ConsoleMethod("delete_ou", "Deletes selected OU removing it from the operation")]
    public void DeleteOU() {
        
        // There must be a selected OU
        var (selectedOU, validSelectedOU) = SelectedOU();
        if (!validSelectedOU)
            return;
        operationControlsManager.DeseletUnit();
        operationControlsManager.opm.RemoveOU(selectedOU);
        Destroy(selectedOU.unitGameobject);
        _writer.NextLine();
        _writer.Write("Successfully deleted Operation Unit;");
    }

    [ConsoleMethod("transfer_unit", "transfers one unit by name from selected OU to another OU")]
    public void TransferSubunit([ConsoleParameter("Subunit name/callsign from selected OU")] string subUnitName, 
        [ConsoleParameter("Target OU that unit will be transfered to. Must be in same hex as selected unit.")] string targetOuName)
    {

        // There must be a selected OU
        var (selectedOU, validSelectedOU) = SelectedOU();
        if (!validSelectedOU)
            return;

        // Subunit name must be a unit within the selected OU 
        var (subUnit, validTargetOU) = GetSubunit(selectedOU, subUnitName);
        if (!validTargetOU)
            return;

        // Target OU name must be valid 
        var (targetOu, validOU) = GetOperationUnit(targetOuName);
        if (!validOU) 
            return;

        // Selected OU must be in same hex as target OU 
        if (selectedOU.hexPosition != targetOu.hexPosition) {
            _writer.NextLine();
            _writer.Write("In order to transfer subunits selected OU and target OU must be in the same hex");
            return;
        }

        selectedOU.RemoveUnit(subUnit);
        targetOu.AddUnit(subUnit);

        _writer.NextLine();
        _writer.Write("Successfully transfered subunit: "+subUnit.name+" to OU: "+targetOu.unitName);
    }

    private (OperationUnit, bool) SelectedOU() {
        if (operationControlsManager.selectedUnitObject == null)
        {
            _writer.NextLine();
            _writer.Write("Must select a operation unit to use this command...");
            return (null, false);
        }
        return (operationControlsManager.selectedUnitObject.GetComponent<OperationUnitData>().ou, true);
    }

    private (Unit, bool) GetSubunit(OperationUnit ou, string subunitName) {

        foreach (var unit in ou.GetUnits())
        {
            if (unit.name == subunitName)
                return (unit, true);
        }

        _writer.NextLine();
        _writer.Write("Invalid Subunit name...");
        return (null, false);

    }

    private (OperationUnit, bool) GetOperationUnit(string OUName, bool checkExists=false) {

        foreach (var ou in operationControlsManager.opm.operationUnits) {
            if (ou.unitName == OUName && !checkExists)
                return (ou, true);
            else if (ou.unitName == OUName && checkExists) {
                _writer.NextLine();
                _writer.Write("Invalid target OU name, the OU name already exists...");
                return (null, true);
            }
        }

        _writer.NextLine();
        _writer.Write("Invalid target OU name...");
        return (null, false);
    }


    /*[ConsoleMethod("cube_rotate", "Rotates the cube by to the given angle.")]
    public void RotateCubeBy([ConsoleParameter("Factor. 1 is default")] float degrees)
    {
        cube.transform.Rotate(Vector3.up, degrees, Space.World);
    }

    [ConsoleMethod("cube_rotation", "Prints the current rotation of the cube.")]
    public void GetRotation()
    {
        _writer.NextLine();
        _writer.Write("Current rotation is: ");
        _writer.WriteBold(cube.transform.rotation.eulerAngles.ToString());
    }*/

    // [...]
}