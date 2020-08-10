using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    ////////////////////////////////////////////////

    public UnitScript unitScript;

    private bool moveInProgress = false;


    private Vector3 _unitCurrPos;
    private CubeLocationScript _currCubeScript;      // current Cube script
    private PanelPieceScript _currPanelScript;
    private ObjectScript _currObjectScript;

    public Vector3 _TARGETLoc;                     // vector of the moving cube Object
    private Vector3Int _TARGETCubeID;                 // current target ID
    private CubeLocationScript _TARGETCubeScript;      // current target script
    private PanelPieceScript _TARGETPanelScript;
    private ObjectScript _TARGETObjectScript;

    public Vector3Int _finalTargetLoc;                     // vector of the moving cube Object
    private CubeLocationScript _finalTargetScript;      // final target script

    private List<Vector3Int> _nodes;
    private int locCount;

    private int _unitsSpeed;

    private bool _newPathSet = false;
    private List<Vector3Int> _newPathNodes;

    private Animator[] _animators;

    private Transform unit_GlobalRotation; // for rotations
    private Transform unit_LocalYAxisRotation; // for rotations
    private Transform camera_Pivot; // for camera

    public int _lastPanelYAxis = -1;
    public bool changeWallAngle = false;
    public int beenUpsideDownCount = 0;

    private bool rotateInProgress = false;
    private Vector3 _unitTargetRotation = Vector3.zero;
    private Vector3 _cameraTargetRotation = Vector3.zero;

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    void Awake()
    {
        _animators = GetComponentsInChildren<Animator>();
    }

    void Start()
    {
        unit_GlobalRotation = transform.FindDeepChild("UnitAngleContainer");
        unit_LocalYAxisRotation = transform.FindDeepChild("UnitContainer");
        camera_Pivot = transform.FindDeepChild("Player_Pivot");

        unitScript = GetComponent<UnitScript>();

        _nodes = new List<Vector3Int>();
        _newPathNodes = new List<Vector3Int>();
    }


    // Use this for initialization
    void FixedUpdate()
    {
        if (moveInProgress)
            StartMoving();

        if(rotateInProgress)
            StartRotating();
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    private void StartMoving()
    {
        if (locCount < _nodes.Count)
        {
            _unitCurrPos = transform.position;

            Vector3 currTargetPos = _TARGETCubeScript.transform.position;
            Vector3 moveOffset = _TARGETCubeScript.GetCubeMovementOffset();

            _TARGETLoc = new Vector3(currTargetPos.x + moveOffset.x, currTargetPos.y + moveOffset.y, currTargetPos.z + moveOffset.z);

            if (_unitCurrPos == _TARGETLoc)
            {
                UnitReachedTarget();
                return;
            }
            else
            {
                UnitMoveTowardsTarget();
            }
        }
    }

    ////////////////////////////////////////////////

    private void UnitMoveTowardsTarget()
    {
        _TARGETPanelScript.PanelIsActive();

        // Moving
        transform.position = Vector3.MoveTowards(transform.position, _TARGETLoc, Time.deltaTime * _unitsSpeed);
        //transform.localPosition = Vector3.zero;

        // Rotation
        unit_LocalYAxisRotation.LookAt(_TARGETLoc, Vector3.up); // All the Y rotation we need

        Vector3 temp = unit_LocalYAxisRotation.localEulerAngles;
        unit_LocalYAxisRotation.localEulerAngles = new Vector3(0, temp.y, 0);
    }

    ////////////////////////////////////////////////

    private void UnitReachedTarget()
    {
        //Debug.Log("UnitReachedTarget!");

        locCount += 1;

        _currCubeScript = unitScript.CubeUnitIsOn;
        _currPanelScript = _currCubeScript._platform_Panel_Cube.GetCubePanel;
        _currObjectScript = _currPanelScript.gameObject.GetComponent<ObjectScript>();

        _currPanelScript.PanelIsDEActive();

        transform.SetParent(_TARGETCubeScript.transform);

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
    ////////////////////////////////////////////////

    private void StartRotating()
    {
        Vector3 unitCurrentAngle = unit_GlobalRotation.localEulerAngles;

        if (Vector3.Distance( unitCurrentAngle , _unitTargetRotation ) > 0)
            UnitRotateTowardsTarget();

    }

    ////////////////////////////////////////////////


    ////////////////////////////////////////////////
    ////////////////////////////////////////////////
    ////////////////////////////////////////////////
    /// vvvvvvvvvv This is the current rotation state. its wonky as fuck, but Im gunna leave as is, coz I need to do sother stuff before I kill myself
    ////////////////////////////////////////////////
    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    private void UnitRotateTowardsTarget()
    {
        _TARGETPanelScript.PanelIsActive();

        StartCoroutine("RotateTowards");
    }

    IEnumerator RotateTowards()
    {
        Vector3 unitCurrentAngle = unit_GlobalRotation.localEulerAngles;
        Vector3 cameraAngles = camera_Pivot.localEulerAngles;

        // Rotation
        while (unitCurrentAngle != _unitTargetRotation)
        {
            unitCurrentAngle = unit_GlobalRotation.localEulerAngles;
            //unit_GlobalRotation.localEulerAngles = new Vector3(unitCurrentAngle.x, 0, unitCurrentAngle.z);
            unit_GlobalRotation.localEulerAngles = Vector3.MoveTowards(unitCurrentAngle, _unitTargetRotation, Time.deltaTime * _unitsSpeed);

            //int currPanelYAxis = _currPanelScript._panelYAxis;

            //if (currPanelYAxis == 90)
            //    unit_GlobalRotation.localEulerAngles = new Vector3(unitCurrentAngle.x, 0, 0);

            //if (currPanelYAxis == 0)
            //    unit_GlobalRotation.localEulerAngles = new Vector3(0, 0, unitCurrentAngle.z);

            //cameraAngles = camera_Pivot.localEulerAngles;
            //camera_Pivot.localEulerAngles = Vector3.MoveTowards(cameraAngles, _cameraTargetRotation, Time.deltaTime * _unitsSpeed);

            yield return null;
        }

    }


    ////////////////////////////////////////////////

    private void ChangeUnitsRotation()
    {
        if (rotateInProgress)
        {
            Vector3 localAngles = _TARGETPanelScript.transform.localEulerAngles;
            Vector3 newRotation = localAngles;

            Vector3 unitCurrentAngle = unit_GlobalRotation.localEulerAngles;

            int currPanelYAxis = _currPanelScript._panelYAxis;
            int targetPanelYAxis = _TARGETPanelScript._panelYAxis;

            bool activeLeft = _TARGETPanelScript.ActivePanelSideIsLeft();

            Debug.Log("fuck _TARGETObjectScript.objectType " + _TARGETObjectScript.objectType);

            switch (_TARGETObjectScript.objectType)
            {
                case CubeObjectTypes.Panel_Floor:
                    if (activeLeft) // Upsidedown
                    {
                        if(currPanelYAxis == 0)
                            newRotation = new Vector3(0, 0, 180); //newRotation = new Vector3(0, 0, 180);

                        if (currPanelYAxis == 90)
                            newRotation = new Vector3(180, 0, 0); //newRotation = new Vector3(0, 0, 180);

                    }
  
                    if (!activeLeft) // Normal
                        newRotation = new Vector3(0, 0, 0);
                    break;
                case CubeObjectTypes.Panel_Wall:

                    if (targetPanelYAxis == 90)
                    {
                        if (activeLeft)
                            newRotation = new Vector3(-90, 0, 0); //newRotation = new Vector3(0, 90, -90);
                        if (!activeLeft)
                            newRotation = new Vector3(90, 0, 0);// newRotation = new Vector3(0, 90, 90);
                    }

                    if (targetPanelYAxis == 0)
                    {
                        if (activeLeft)
                            newRotation = new Vector3(0, 0, 270);
                        if (!activeLeft)
                            newRotation = new Vector3(0, 0, 90);
                    }
                    break;
                case CubeObjectTypes.Panel_Angle:
                    // angles put in half points
                    break;
                default:
                    Debug.LogError("FUCK got error here " + _TARGETPanelScript.name);
                    break;
            }

            _unitTargetRotation = newRotation;
            camera_Pivot.localEulerAngles = Vector3.zero;
        }
    }

    private void ChangeCameraRotation()
    {
        Vector3 localAngles = _TARGETPanelScript.transform.localEulerAngles;
        Vector3 newRotation = localAngles;

        Vector3 unitCurrentAngle = unit_GlobalRotation.localEulerAngles;

        int targetPanelYAxis = _TARGETPanelScript._panelYAxis;

        bool activeLeft = _TARGETPanelScript.ActivePanelSideIsLeft();


        int currPanelYAxis = _currPanelScript._panelYAxis;
        Vector3 cameraAngles = camera_Pivot.localEulerAngles;
        Vector3 cameraRotation = cameraAngles;


        // seems to be fine if coming from wall -> floor
        // has issues when coming from floor -> floor, inparticular from the roof


        switch (_TARGETObjectScript.objectType)
        {
            case CubeObjectTypes.Panel_Floor: // Seems to be good
                if (activeLeft) // Upsidedown
                    cameraRotation = new Vector3(0, 0, 0);

                if (!activeLeft) // Normal
                    cameraRotation = new Vector3(0, 0, 0);

                break;
            case CubeObjectTypes.Panel_Wall:

                if (targetPanelYAxis == 90)
                {
                    if (activeLeft)
                        cameraRotation = new Vector3(0, 0, 0);
                    if (!activeLeft)
                        cameraRotation = new Vector3(0, 0, 0);
                }

                if (targetPanelYAxis == 0)
                {
                    if (activeLeft)
                        cameraRotation = new Vector3(0, 0, 0);
                    if (!activeLeft)
                        cameraRotation = new Vector3(0, 0, 0);
                }
                break;
            case CubeObjectTypes.Panel_Angle: // angles put in half points
                break;
            default:
                Debug.LogError("FUCK got error here " + _TARGETPanelScript.name);
                break;
        }
        _cameraTargetRotation = cameraRotation;
        //camera_Pivot.localEulerAngles = cameraRotation;
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////
    ////////////////////////////////////////////////
    /// ^^^^^^^^ This is the current rotation state. its wonky as fuck, but Im gunna leave as is, coz I need to do sother stuff before I kill myself
    ////////////////////////////////////////////////
    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    private void SetNewpath()
    {
        ResetValues();
        _nodes = _newPathNodes;

        SetNextTarget();
    }

    ////////////////////////////////////////////////

    private void FinishMoving()
    {
        //Debug.Log("FINISHED MOVING!");

        AnimationManager.SetAnimatorBool(_animators, "Combat_Walk", false);
        moveInProgress = false;
        ResetValues();
    }

    ////////////////////////////////////////////////

    //private void AnnoyingFuckingAngleMovementThing() // this is to fix the fucking camera spinning around in weird angles 10% of the time when a unit moves onto a diff sided panel
    //{
    //    if (_TARGETObjectScript.objectType == CubeObjectTypes.Panel_Wall)
    //    {
    //        if (_lastPanelYAxis == -1)
    //        {
    //            _lastPanelYAxis = _TARGETPanelScript._panelYAxis;
    //            changeWallAngle = false;
    //        }
    //        else
    //        {
    //            if (_lastPanelYAxis != _TARGETPanelScript._panelYAxis)
    //            {
    //                changeWallAngle = true;
    //            }
    //            else
    //            {
    //                changeWallAngle = false;
    //            }

    //            _lastPanelYAxis = _TARGETPanelScript._panelYAxis;

    //        }
    //        Debug.Log("fuck changeWallAngle " + changeWallAngle);
    //    }
    //}

    ////////////////////////////////////////////////

    private void SetNextTarget()
    {
        _TARGETCubeID = _nodes[locCount];
        _TARGETCubeScript = LocationManager.GetLocationScript_CLIENT(_TARGETCubeID);
        if (_TARGETCubeScript._platform_Panel_Cube.GetCubePanel != null)
        {
            _TARGETPanelScript = _TARGETCubeScript._platform_Panel_Cube.GetCubePanel;
            _TARGETObjectScript = _TARGETPanelScript.gameObject.GetComponent<ObjectScript>();

            //AnnoyingFuckingAngleMovementThing();

            if (MovementManager._gravityActivated)
            {
                //    unitContainerTransformLOCAL.localEulerAngles = new Vector3(0, unitContainerTransformLOCAL.localEulerAngles.y, 0);
                //    unitContainerAnglesGLOBAL.localEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                rotateInProgress = false;

                if (_TARGETObjectScript.objectType != _currObjectScript.objectType)
                    rotateInProgress = true;

                if (_TARGETObjectScript.objectType == _currObjectScript.objectType)
                {
                    int currPanelYAxis = _currPanelScript._panelYAxis;
                    int targetPanelYAxis = _TARGETPanelScript._panelYAxis;

                    if (currPanelYAxis != targetPanelYAxis)
                        rotateInProgress = true;
                }
                ChangeUnitsRotation();
                ChangeCameraRotation();
            }


            if (!LocationManager.SetUnitOnCube_CLIENT(GetComponent<UnitScript>(), _TARGETCubeID))
            {
                Debug.LogWarning("units movement interrupted >> recalculating");
                moveInProgress = false;
                ResetValues();
                if (MovementManager.BuildMovementPath(_finalTargetScript))
                {
                    MovementManager.MakeActiveUnitMove_CLIENT();
                }
            }
        }
    }

    ////////////////////////////////////////////////

    public void MoveUnit(List<Vector3Int> path)
    {
        AnimationManager.SetAnimatorBool(_animators, "Combat_Walk", true);

        _unitsSpeed = gameObject.transform.GetComponent<UnitScript>().UnitCombatStats[1]; // movement

        _finalTargetLoc = path[path.Count-1];
        _finalTargetScript = LocationManager.GetLocationScript_CLIENT(_finalTargetLoc);

        _currCubeScript = unitScript.CubeUnitIsOn;
        _currPanelScript = _currCubeScript._platform_Panel_Cube.GetCubePanel;
        _currObjectScript = _currPanelScript.gameObject.GetComponent<ObjectScript>();

        if (path.Count > 0)
        {
            if (moveInProgress)
            {
                foreach (Vector3Int nodeVect in _nodes)
                {
                    LocationManager.GetLocationScript_CLIENT(nodeVect)._platform_Panel_Cube.GetCubePanel.PanelIsDEActive();
                }
                _newPathNodes = path;
                _newPathSet = true;
            }
            else
            {
                ResetValues();
                _nodes = path;
                moveInProgress = true;
                foreach(Vector3Int nodeVect in _nodes)
                {
                    LocationManager.GetLocationScript_CLIENT(nodeVect)._platform_Panel_Cube.GetCubePanel.PanelIsActive();
                }
                SetNextTarget();
            }
        }
    }

    ////////////////////////////////////////////////

    private void ResetValues()
    {
        locCount = 0;
        //_dynamicTargetLocVect = Vector3Int.zero;
        //_staticTargetVect = new KeyValuePair<Vector3Int, Vector3Int>();
        //_staticFinalTargetVect = new KeyValuePair<Vector3Int, Vector3Int>();
        _newPathSet = false;
        foreach (Vector3Int nodeVect in _nodes)
        {
            CubeLocationScript script = LocationManager.GetLocationScript_CLIENT(nodeVect);
            script.DestroyPathFindingNode();
        }
        _nodes.Clear();
    }
}
