using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static UnitsManager _instance;

    ////////////////////////////////////////////////

    public static UnitBuilder _unitBuilder;

    ////////////////////////////////////////////////

    public static Dictionary<int, Dictionary<Vector3, UnitScript>> _unitObjectsByPlayerID;

    private static UnitScript _activeUnit = null;

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        _unitBuilder = transform.Find("UnitBuilder").GetComponent<UnitBuilder>();

        _unitObjectsByPlayerID = new Dictionary<int, Dictionary<Vector3, UnitScript>>();
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    public static void LoadPlayersUnits()
    {
        List<UnitStruct> units = PlayerManager.PlayerUnitData;

        Debug.Log("LoadPlayersUnits <<<<<<< for player " + PlayerManager.PlayerID);

        for (int i = 0; i < PlayerManager.PlayerNumUnits; i++)
        {
            UnitStruct unitData = units[i];

           PlayerManager.NetworkAgent.CmdTellServerToSpawnPlayerUnit(PlayerManager.PlayerAgent.NetworkInstanceID, unitData, PlayerManager.PlayerID);
        }
    }

    ////////////////////////////////////////////////

    public static void CreateUnitOnClient(NetworkNodeStruct networkNodeStruct, int playerID) // an attempt to make units like ships
    {
        Debug.Log("CreateUnitOnClient " + networkNodeStruct.unitData.UnitStartingNodeID);

        CubeLocationScript cubeScript = LocationManager.GetLocationScript_CLIENT(networkNodeStruct.NodeID);

        if (cubeScript != null)
        {
            GameObject prefab = UnitsManager._unitBuilder.GetUnitModel(networkNodeStruct.unitData.UnitModel);
            GameObject unit = Instantiate(prefab, cubeScript.gameObject.transform, false);
            unit.transform.SetParent(cubeScript.gameObject.transform);
            unit.transform.localPosition = networkNodeStruct.CurrLoc;
            GameObject unitContainer = unit.transform.FindDeepChild("UnitContainer").gameObject;
            unitContainer.transform.localEulerAngles = networkNodeStruct.CurrRot;

            UnitScript unitScript = unit.GetComponent<UnitScript>();

            unitScript.UnitData = networkNodeStruct.unitData;
            unitScript.UnitID = networkNodeStruct.NodeID;
            unitScript.PlayerControllerID = playerID;
            unitScript.UnitModel = networkNodeStruct.unitData.UnitModel;
            unitScript.UnitCanClimbWalls = networkNodeStruct.unitData.UnitCanClimbWalls;
            unitScript.UnitCombatStats = networkNodeStruct.unitData.UnitCombatStats;

            LocationManager.SetUnitOnCube_CLIENT(unit.GetComponent<UnitScript>(), unitScript.UnitID); //sets CubeUnitIsOn

            AddUnitToGame(playerID, unitScript.UnitID, unitScript); // add unit to generic unit manager pool
            PlayerManager.PlayerAgent.GetComponent<UnitsAgent>().AddUnitToUnitAgent(unitScript); // add unit to a more specific player unit pool

            if (PlayerManager.PlayerID == playerID)
            {
                if (networkNodeStruct.unitData.UnitCombatStats[0] == 1) // if rank is 'Captain'???? then make active
                {
                    SetUnitActive(true, playerID, unitScript.UnitID);
                    AssignCameraToActiveUnit();
                }
            }
            Debug.Log("Unit Succesfully created on CLIENT: " + unitScript.UnitID);
        }
        else
        {
            Debug.LogError("Got a problem here > " + networkNodeStruct.NodeID);
        }
    }

    ////////////////////////////////////////////////

    public static void AddUnitToGame(int playerContID, Vector3 unitID, UnitScript unit)
    {
        if (_unitObjectsByPlayerID.ContainsKey(playerContID))
        {
            Dictionary<Vector3, UnitScript> unitList = _unitObjectsByPlayerID[playerContID];

            //Debug.Log("unitID " + unitID);

            if (!unitList.ContainsKey(unitID))
            {
                unitList.Add(unitID, unit);
                _unitObjectsByPlayerID[playerContID] = unitList;
            }
            else
            {
                Debug.LogError("Trying to assign a unit twice");
            }
        }
        else
        {
            Dictionary<Vector3, UnitScript> unitList = new Dictionary<Vector3, UnitScript>();
            unitList.Add(unitID, unit);
            _unitObjectsByPlayerID.Add(playerContID, unitList);
        }
    }

    ////////////////////////////////////////////////

    public static void SetUnitActive(bool onOff, int playerContID, Vector3 unitID)
    {
        UnitScript unit = _unitObjectsByPlayerID[playerContID][unitID];

        if (unit == null)
        {
            Debug.LogError("SetUnitActive ERROR unit == null");
            return;
        }

        if (_activeUnit == null)
        {
            _activeUnit = unit;
        }

        if (onOff)
        {
            if (_activeUnit.UnitID != unit.UnitID)
            {
                _activeUnit.DeActivateUnit();
            }

            _activeUnit = unit;
            _activeUnit.ActivateUnit();

            // _locationManager.DebugTestPathFindingNodes(_activeUnit);
        }
        else
        {
            if (_activeUnit.UnitID != unit.UnitID)
            {
                Debug.LogError("should not be here Unit is active and another unit is tying to get turned off");
            }
            else
            {
                _activeUnit.DeActivateUnit();
                _activeUnit = null;
            }
        }
    }

    ////////////////////////////////////////////////

    public static void AssignCameraToActiveUnit()
    {
        CameraManager.SetCamToOrbitUnit(_activeUnit);
        LayerManager.ChangeCameraLayer(_activeUnit.CubeUnitIsOn);
    }

    ////////////////////////////////////////////////

    public static void MakeActiveUnitMove_CLIENT(Vector3 posToMoveTo)
    {
        if (_activeUnit)
        {
            List<Vector3> movePath = MovementManager.SetUnitsPath(_activeUnit, _activeUnit.CubeUnitIsOn.CubeID, posToMoveTo);

            //int[] pathInts = DataManipulation.ConvertVectorsIntoIntArray(movePath);

            if (movePath != null)
            {
                //int unitID = (int)_activeUnit.netId.Value;
                MovementManager.CreatePathFindingNodes_CLIENT(_activeUnit, _activeUnit.UnitID, movePath);

                //PlayerManager.NetworkAgent.CmdTellServerToMoveUnit(PlayerManager.PlayerAgent.NetworkInstanceID, _activeUnit.NetID, pathInts);

                _activeUnit.GetComponent<MovementScript>().MoveUnit(movePath);
            }

        }
    }

    ////////////////////////////////////////////////

}
