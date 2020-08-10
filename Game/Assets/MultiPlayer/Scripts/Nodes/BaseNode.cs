using System.Collections.Generic;
using UnityEngine;

public class BaseNode : MonoBehaviour {

    private Vector3Int _nodeID;
    private NodeTypes _thisNodeType;

    private BaseWorldNodeData _nodeData = new BaseWorldNodeData();
    public int _nodeLayerCount;
    private MapPieceTypes _nodeMapPiece;

    private Vector3 _currLoc;
    private Vector3 _currRot;

    private Vector3 _targetLoc;
    private Vector3 _targetRot;

    private bool _spinny; // to spin with spinny ship parts

    public List<Vector3Int> neighbourVects;
    public bool entrance = false;

    public WorldNode worldNodeParent; // not used but could come in handy
    public GameObject _nodeCover;

    private bool _moveNode = false;
    private bool _thisClientInControl = false;

    private float _thrust = 0f;

    ////////////////////////////////////////////////

    public NodeTypes NodeType
    {
        get { return _thisNodeType; }
        set { _thisNodeType = value; }
    }
    public BaseWorldNodeData NodeData
    {
        get { return _nodeData; }
        set { _nodeData = value; }
    }
    public int NodeLayerCount
    {
        get { return _nodeLayerCount; }
        set { _nodeLayerCount = value; }
    }
    public MapPieceTypes NodeMapPiece
    {
        get { return _nodeMapPiece; }
        set { _nodeMapPiece = value; }
    }

    public bool Spinney
    {
        get { return _spinny; }
        set { _spinny = value; }
    }

    //////////////////////////////

    public GameObject NodeCover
    {
        get { return _nodeCover; }
        set { _nodeCover = value; }
    }


    //////////////////////////////
    /// Network shit
    //////////////////////////////
    public Vector3Int NodeID
    {
        get { return _nodeID; }
        set { _nodeID = value; }
    }
    //////////////////////////////


    public Vector3 TargetLoc
    {
        get { return _targetLoc; }
        set { _targetLoc = value; }
    }

    public Vector3 TargetRot
    {
        get { return _targetRot; }
        set { _targetRot = value; }
    }


    //////////////////////////////
    public Vector3 NodeLoc
    {
        get { return _currLoc; }
        set { _currLoc = value; }
    }

    public Vector3 NodeRot
    {
        get { return _currRot; }
        set { _currRot = value; }
    }


    void Awake()
    {
    }

    void Update()
    {

        if (_moveNode)
        {
            _thrust = 10f;

            // Moving
            //transform.position = Vector3Int.MoveTowards(transform.position, TargetLoc, _thrust);

            //// Rotation
            //Vector3Int eulerRotation = new Vector3Int(transform.eulerAngles.x, TargetLoc.y, transform.eulerAngles.z);
            //transform.rotation = Quaternion.Euler(eulerRotation);

            // Rotation
            //Vector3Int newDir = Vector3Int.RotateTowards(transform.forward, eulerRotation, (Time.deltaTime * 2f) * 5f, 0.0f);
            //transform.rotation = Quaternion.LookRotation(newDir);

            // Moving
            //transform.position = Vector3Int.MoveTowards(transform.position, TargetLoc, (Time.deltaTime * _thrust));


            // UMM THIS IS WORKING REALLY FICKEING WELL
            // Rotation
            Vector3 targetDir = TargetLoc - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * (_thrust * 0.005f), 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            // Moving
            transform.position += transform.forward * Time.deltaTime * (_thrust * 5f);
            ///////////////////////////////


            if (_thisClientInControl)
            {
                if (NodeLoc != transform.position || NodeRot != transform.eulerAngles)
                {
                    NodeLoc = transform.position;
                    NodeRot = transform.eulerAngles;
                    PlayerManager.NetworkAgent.CmdTellServerToUpdateWorldNodePosition(PlayerManager.PlayerAgent.NetworkInstanceID, NodeID, NodeLoc, NodeRot);
                }
            }
        }
    } 


    public virtual bool ActivateMapPiece(bool coverActive = false)
    {
        WorldBuilder.AttachMapToNode(this);
        LocationManager.RemoveUnNeededCubes();

        if (coverActive)
        {
            NodeCover.SetActive(false); // this turns the cover off
            return false; // this is not a fail, this is deactivation
        }
        else
        {
            NodeCover.SetActive(true);  // this turns the cover On
            return true;
        }
    }

    public void MakeNodeMoveToLoc(Vector3 loc, Vector3 rot, bool thisClientInControl = false)
    {
        _thisClientInControl = thisClientInControl;

        NodeLoc = transform.position;
        NodeRot = transform.eulerAngles;

        TargetLoc = loc;
        TargetRot = rot;
        _moveNode = true;
    }
}
