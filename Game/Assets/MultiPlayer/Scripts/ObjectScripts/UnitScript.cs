using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    ////////////////////////////////////////////////

    // GamePlay data
    private List<CubeLocationScript> _pathFindingNodes;
    private bool _unitActive;
    // Visual
    private Renderer[] _rends;
    // for the player+camera to pivot around
    GameObject _playerPivot;

    private Vector3 _unitID;
    private Vector3 _startingWorldLoc;
    private int _playerControllerId;
    private Vector3 _nodeIDUnitIsOn;
    public CubeLocationScript _cubeUnitIsOn;
    private int _pathNodeCount;

    ////////////////////////////////////////////////

    // Unit stats
    private UnitStruct _unitData;
    private int _unitModel;
    private bool _unitCanClimbWalls;
    private int[] _unitCombatStats;

    ////////////////////////////////////////////////

    public Vector3 UnitID
    {
        get { return _unitID; }
        set { _unitID = value; }
    }

    ////////////////////////////////////////////////

    public int UnitModel
    {
        get { return _unitModel; }
        set { _unitModel = value; }
    }

    public bool UnitCanClimbWalls
    {
        get { return _unitCanClimbWalls; }
        set { _unitCanClimbWalls = value; }
    }

    public int[] UnitCombatStats
    {
        get { return _unitCombatStats; }
        set { _unitCombatStats = value; }
    }

    public CubeLocationScript CubeUnitIsOn
    {
        get { return _cubeUnitIsOn; }
        set { _cubeUnitIsOn = value; }
    }

    public int PlayerControllerID
    {
        get { return _playerControllerId; }
        set { _playerControllerId = value; }
    }

    public UnitStruct UnitData
    {
        get { return _unitData; }
        set { _unitData = value; }
    }

    public GameObject PlayerPivot
    {
        get { return _playerPivot; }
        set { _playerPivot = value; }
    }

    public bool UnitAcive
    {
        get { return _unitActive; }
        set { _unitActive = value; }
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    void Awake()
    {
        _pathFindingNodes = new List<CubeLocationScript>();
        _rends = GetComponentsInChildren<Renderer>();
        PlayerPivot = transform.Find("Player_Pivot").gameObject;

        if (_pathFindingNodes == null) { Debug.LogError("We got a problem here"); }
        if (_rends == null) { Debug.LogError("We got a problem here"); }
        if (PlayerPivot == null) { Debug.LogError("We got a problem here"); }
    }

    // Use this for initialization
    void Start()
    {
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    public void PanelPieceChangeColor(string color) {

        if (_rends == null) {
            _rends = GetComponentsInChildren<Renderer>();
        }

        foreach (Renderer rend in _rends) {
			switch (color) {
			case "Red":
				rend.material.color = Color.red;
				break;
			case "Black":
				rend.material.color = Color.black;
				break;
			case "White":
				rend.material.color = Color.white;
				break;
			case "Green":
				rend.material.color = Color.green;
				break;
			default:
				break;
			}
		}
	}


    public void ActivateUnit()
    {
        PanelPieceChangeColor("Red");
        _unitActive = true;
    }


    public void DeActivateUnit()
    {
        PanelPieceChangeColor("White");
        _unitActive = false;
    }


    public void AssignPathFindingNodes(List<CubeLocationScript> nodes)
    {
        ClearPathFindingNodes();
        _pathFindingNodes = nodes;
        SetUnitToNextLocation_CLIENT(_pathFindingNodes[_pathNodeCount].CubeID);
    }

    public void ClearPathFindingNodes()
    {
        foreach(CubeLocationScript node in _pathFindingNodes)
        {
            node.DestroyPathFindingNode();
        }
        _pathFindingNodes.Clear();
        _pathNodeCount = 0;
    }

    public void SetUnitToNextLocation_CLIENT(Vector3 vect)
    {
        //if (_pathNodeCount < _pathFindingNodes.Count)
        //{
            //Vector3 vect = _pathFindingNodes[_pathNodeCount].CubeID;
            CubeLocationScript script = LocationManager.GetLocationScript_CLIENT(vect);
            CubeUnitIsOn = script;
        //}
        _pathNodeCount++;
    }

}
