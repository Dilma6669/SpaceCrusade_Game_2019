using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static UnitsManager _instance;

    ////////////////////////////////////////////////

    public static UnitBuilder _unitBuilder;

    ////////////////////////////////////////////////

    public static Dictionary<int, Dictionary<Vector3Int, UnitScript>> _unitObjectsByPlayerID;

    private static UnitScript _activeUnit = null;

    public static UnitScript ActiveUnit
    {
        get { return _activeUnit; }
    }

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

        _unitObjectsByPlayerID = new Dictionary<int, Dictionary<Vector3Int, UnitScript>>();
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

    public static void CreateUnitOnClient(NetworkNodeStruct nodeStruct, int playerID) // an attempt to make units like ships
    {
        Vector3Int nodeID = new Vector3Int(Mathf.FloorToInt(nodeStruct.NodeID.x), Mathf.FloorToInt(nodeStruct.NodeID.y), Mathf.FloorToInt(nodeStruct.NodeID.z));

        //Debug.Log("CreateUnitOnClient UnitStartingNodeID " + nodeStruct.UnitData.UnitStartingNodeID);
        //Debug.Log("CreateUnitOnClient nodeID " + nodeID);

        CubeLocationScript cubeScript = LocationManager.GetLocationScript_CLIENT(nodeID);

        //cubeScript.MapNodeParent.ActivateMapPiece(true);

        if (cubeScript != null)
        {
            GameObject prefab = _unitBuilder.GetUnitModel(nodeStruct.UnitData.UnitModel);
            GameObject unit = Instantiate(prefab, cubeScript.gameObject.transform, false);
            unit.transform.SetParent(cubeScript.gameObject.transform);
            unit.transform.localPosition = nodeStruct.CurrLoc;
            GameObject unitContainer = unit.transform.FindDeepChild("UnitContainer").gameObject;
            unitContainer.transform.localEulerAngles = nodeStruct.CurrRot;

            UnitScript unitScript = unit.GetComponent<UnitScript>();

            unitScript.UnitData = nodeStruct.UnitData;
            unitScript.UnitID = nodeID;
            unitScript.PlayerControllerID = playerID;
            unitScript.UnitModel = nodeStruct.UnitData.UnitModel;
            unitScript.UnitCanClimbWalls = nodeStruct.UnitData.UnitCanClimbWalls;
            unitScript.UnitCombatStats = nodeStruct.UnitData.UnitCombatStats;

            LocationManager.SetUnitOnCube_CLIENT(unit.GetComponent<UnitScript>(), unitScript.UnitID); //sets CubeUnitIsOn

            AddUnitToGame(playerID, unitScript.UnitID, unitScript); // add unit to generic unit manager pool
            PlayerManager.PlayerAgent.GetComponent<UnitsAgent>().AddUnitToUnitAgent(unitScript); // add unit to a more specific player unit pool

            if (PlayerManager.PlayerID == playerID)
            {
                if (nodeStruct.UnitData.UnitCombatStats[0] == 1) // if rank is 'Captain'???? then make active
                {
                    SetUnitActive(true, playerID, unitScript.UnitID);
                }
            }
            Debug.Log("Unit Succesfully created on CLIENT: " + unitScript.UnitID);
        }
        else
        {
            Debug.LogError("Got a problem here > " + nodeID);
        }
    }

    ////////////////////////////////////////////////

    public static void AddUnitToGame(int playerContID, Vector3Int unitID, UnitScript unit)
    {
        if (_unitObjectsByPlayerID.ContainsKey(playerContID))
        {
            Dictionary<Vector3Int, UnitScript> unitList = _unitObjectsByPlayerID[playerContID];

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
            Dictionary<Vector3Int, UnitScript> unitList = new Dictionary<Vector3Int, UnitScript>();
            unitList.Add(unitID, unit);
            _unitObjectsByPlayerID.Add(playerContID, unitList);
        }
    }

    ////////////////////////////////////////////////

    public static void SetUnitActive(bool onOff, int playerContID, Vector3Int unitID)
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

            _activeUnit.ClearPathFindingNodes();
            _activeUnit = unit;
            _activeUnit.ActivateUnit();
            AssignCameraToActiveUnit();

            Debug.Log("Unit: " + unitID + " : SetActive");

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
        //LayerManager.ChangeCameraLayer(_activeUnit.CubeUnitIsOn);
    }

    ////////////////////////////////////////////////

}
