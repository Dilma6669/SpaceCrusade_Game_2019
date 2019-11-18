using UnityEngine;

public class BaseNode : MonoBehaviour {

    private Vector3 _nodeID;
    private NodeTypes _thisNodeType;
    private Vector3 _nodeLocation;
    private Vector3 _nodeRotation;
    private int _nodeDirection;
    private int _nodeSize;
    private int _nodeLayerCount;
    private MapPieceTypes _nodeMapPiece;

    private Vector3 _currLoc;
    private Vector3 _currRot;

    private Vector3 _targetLoc;
    private Vector3 _targetRot;

    public int[] neighbours;
    public bool entrance = false;

    public bool connectorUp = false;

    public WorldNode worldNodeParent;
    public GameObject _nodeCover;

    private bool _moveNode = false;
    private bool _thisClientInControl = false;
    private GameObject _networkNodeContainer;
    private int _linkedNetworkNodeIndex;

    private float _thrust = 0f;

    ////////////////////////////////////////////////

    public NodeTypes NodeType
    {
        get { return _thisNodeType; }
        set { _thisNodeType = value; }
    }
    public int NodeSize
    {
        get { return _nodeSize; }
        set { _nodeSize = value; }
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


    public Vector3 NodeRotation
    {
        get { return _nodeRotation; }
        set { _nodeRotation = value; }
    }
    public Vector3 NodeLocation
    {
        get { return _nodeLocation; }
        set { _nodeLocation = value; }
    }
    public int NodeDirection
    {
        get { return _nodeDirection; }
        set { _nodeDirection = value; }
    }


    public GameObject NodeCover
    {
        get { return _nodeCover; }
        set { _nodeCover = value; }
    }


    //////////////////////////////
    /// Network shit
    //////////////////////////////
    public Vector3 NetworkNodeID
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
            //transform.position = Vector3.MoveTowards(transform.position, TargetLoc, _thrust);

            //// Rotation
            //Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, TargetLoc.y, transform.eulerAngles.z);
            //transform.rotation = Quaternion.Euler(eulerRotation);

            // Rotation
            //Vector3 newDir = Vector3.RotateTowards(transform.forward, eulerRotation, (Time.deltaTime * 2f) * 5f, 0.0f);
            //transform.rotation = Quaternion.LookRotation(newDir);

            // Moving
            //transform.position = Vector3.MoveTowards(transform.position, TargetLoc, (Time.deltaTime * _thrust));


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
                    PlayerManager.NetworkAgent.CmdTellServerToUpdateWorldNodePosition(PlayerManager.PlayerAgent.NetworkInstanceID, NetworkNodeID, NodeLoc, NodeRot);
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
