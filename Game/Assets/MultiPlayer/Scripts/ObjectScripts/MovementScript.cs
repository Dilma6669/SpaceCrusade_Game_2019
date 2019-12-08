using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    ////////////////////////////////////////////////

    public UnitScript unitScript;

    private bool moveInProgress = false;

    private Vector3 _unitCurrPos;

    private List<Vector3> _nodes;

    private Vector3 _currTargetCubeID;                 // current target ID

    private CubeLocationScript _currTargetScript;      // current target script
    public Vector3 _currTargetLoc;                     // vector of the moving cube Object
    private PanelPieceScript _currTargetPanelScript;

    public Vector3 _finalTargetLoc;                     // vector of the moving cube Object
    private CubeLocationScript _finalTargetScript;      // final target script


    private int locCount;

    private int _unitsSpeed;

    private bool _newPathSet = false;
    private List<Vector3> _newPathNodes;

    private Animator[] _animators;

    private Transform unitContainerAngles; // for rotations
    private Transform unitContainerTransform; // for rotations

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    void Awake()
    {
        unitContainerAngles = transform.FindDeepChild("UnitAngleContainer");
        unitContainerTransform = transform.FindDeepChild("UnitContainer");
        _animators = GetComponentsInChildren<Animator>();
    }

    void Start()
    {
        unitScript = GetComponent<UnitScript>();

        _nodes = new List<Vector3>();
        _newPathNodes = new List<Vector3>();
    }


    // Use this for initialization
    void FixedUpdate()
    {

        if (moveInProgress)
        {
            StartMoving();
        }
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    private void StartMoving()
    {
        if (locCount < _nodes.Count)
        {
            _unitCurrPos = unitContainerTransform.position;

            _currTargetLoc = _currTargetScript.transform.position;

            if (_unitCurrPos != _currTargetLoc)
            {
                UnitMoveTowardsTarget();
            }
            else
            {
                UnitReachedTarget();
            }
        }
    }

    ////////////////////////////////////////////////

    private void UnitMoveTowardsTarget()
    {
        // Moving
        transform.position = Vector3.MoveTowards(transform.position, _currTargetLoc, Time.deltaTime * _unitsSpeed);

        // Rotation
        unitContainerTransform.LookAt(_currTargetLoc, Vector3.up);
        unitContainerTransform.localEulerAngles = new Vector3(0, unitContainerTransform.localEulerAngles.y, 0);

        if (MovementManager._gravityActivated == false)
        {
            switch (_currTargetPanelScript.name)
            {
                case "Panel_Floor":
                    unitContainerAngles.localEulerAngles = new Vector3(0, unitContainerAngles.localEulerAngles.y, 0);
                    break;
                case "Panel_Wall":
                    //itContainerTransform.localEulerAngles = new Vector3(0, 0, unitContainerTransform.localEulerAngles.z);
                    break;
                case "Panel_Angle": // angles put in half points
                    int panelYaxis = _currTargetPanelScript._panelYAxis;

                    if (panelYaxis == 0)
                    {
                        unitContainerAngles.localEulerAngles = new Vector3(45, 0, 0);
                    }
                    else if (panelYaxis == 90)
                    {
                        unitContainerAngles.localEulerAngles = new Vector3(0, 0, -45);
                    }
                    else if (panelYaxis == 180)
                    {
                        unitContainerAngles.localEulerAngles = new Vector3(-45, 0, 0);
                    }
                    else if (panelYaxis == 270)
                    {
                        unitContainerAngles.localEulerAngles = new Vector3(0, 0, 45);
                    }
                    break;
                default:
                    Debug.LogError("FUCK got error here " + _currTargetPanelScript.name);
                    break;
            }
        }
    }

    ////////////////////////////////////////////////

    private void UnitReachedTarget()
    {
        //Debug.Log("UnitReachedTarget!");

        locCount += 1;

        transform.SetParent(_currTargetScript.transform);
        transform.localPosition = Vector3.zero;

        if (_newPathSet) // has a new location been clicked
        {
            SetNewpath();
        }
        else if (locCount < _nodes.Count) // move to next target
        {
            SetNextTarget();
        }
        else
        {
            FinishMoving();
        }
    }

    ////////////////////////////////////////////////

    private void SetNewpath()
    {
        Reset();
        _nodes = _newPathNodes;

        SetNextTarget();
    }

    ////////////////////////////////////////////////

    private void FinishMoving()
    {
        //Debug.Log("FINISHED MOVING!");

        AnimationManager.SetAnimatorBool(_animators, "Combat_Walk", false);
        moveInProgress = false;
        Reset();
    }

    ////////////////////////////////////////////////

    private void SetNextTarget()
    {
        _currTargetCubeID = _nodes[locCount];
        _currTargetScript = LocationManager.GetLocationScript_CLIENT(_currTargetCubeID);
        _currTargetPanelScript = _currTargetScript._platform_Panel_Cube._panelScriptChild;

        if (!LocationManager.SetUnitOnCube_CLIENT(GetComponent<UnitScript>(), _currTargetCubeID))
        {
            Debug.LogWarning("units movement interrupted >> recalculating");
            moveInProgress = false;
            Reset();
            UnitsManager.MakeActiveUnitMove_CLIENT(_finalTargetLoc);
        }
    }

    ////////////////////////////////////////////////

    public void MoveUnit(List<Vector3> path)
    {
        AnimationManager.SetAnimatorBool(_animators, "Combat_Walk", true);

        _unitsSpeed = gameObject.transform.GetComponent<UnitScript>().UnitCombatStats[1]; // movement

        _finalTargetLoc = path[path.Count-1];
        _finalTargetScript = LocationManager.GetLocationScript_CLIENT(_finalTargetLoc);

        if (path.Count > 0)
        {
            if (moveInProgress)
            {
                _newPathNodes = path;
                _newPathSet = true;
            }
            else
            {
                Reset();
                _nodes = path;
                moveInProgress = true;
                SetNextTarget();
            }
        }
    }

    ////////////////////////////////////////////////

    private void Reset()
    {
        locCount = 0;
        //_dynamicTargetLocVect = Vector3.zero;
        //_staticTargetVect = new KeyValuePair<Vector3, Vector3>();
        //_staticFinalTargetVect = new KeyValuePair<Vector3, Vector3>();
        _newPathSet = false;
        //foreach (Vector3 nodeVect in _nodes)
        //{
        //    CubeLocation_SERVER script = LocationManager.GetLocationScript_SERVER(nodeVect);
        //    script.DestroyPathFindingNode();
        //}
        _nodes.Clear();
    }
}
