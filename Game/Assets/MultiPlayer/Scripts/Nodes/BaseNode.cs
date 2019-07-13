using UnityEngine;

public class BaseNode : MonoBehaviour {

    private Vector3Int _nodeID;
    private NodeTypes _thisNodeType;
    private Vector3Int _nodeLocation;
    private Quaternion _nodeRotation;
    private int _nodeDirection;
    private int _nodeSize;
    private int _nodeLayerCount;
    private MapPieceTypes _nodeMapPiece;

    public int[] neighbours;
    public bool entrance = false;

    public bool connectorUp = false;

    public WorldNode worldNodeParent;
    public GameObject _nodeCover;

    private bool _followNetworkNode = false;
    private GameObject _networkNodeContainer;

    private float _thrust = 0f;

    ////////////////////////////////////////////////

    public Vector3Int NodeID
    {
        get { return _nodeID; }
        set { _nodeID = value; }
    }
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


    public Quaternion NodeRotation
    {
        get { return _nodeRotation; }
        set { _nodeRotation = value; }
    }
    public Vector3Int NodeLocation
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


    public GameObject NetworkNodeContainer
    {
        get { return _networkNodeContainer; }
        set { _networkNodeContainer = value; }
    }

    void Awake()
    {
    }

    void Update()
    {
        if (_followNetworkNode)
        {
            // Rotation
            //transform.rotation = Vector3.RotateTowards(transform.position, _networkNode.transform.position, (Time.deltaTime * 50f), 0.0f);
            //transform.rotation = Quaternion.Euler(NetworkNodeContainer.transform.rotation.eulerAngles);

            // Moving
            transform.position = Vector3.MoveTowards(transform.position, NetworkNodeContainer.transform.position, (Time.deltaTime * _thrust));

        
            Vector3 targetDir = NetworkNodeContainer.transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * 0.1f, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    public virtual bool ActivateMapPiece(bool coverActive = false)
    {
        WorldBuilder.AttachMapToNode(this);
        //Vector3 testLocVect = new Vector3(200, 0, 0);
        //Vector3 testRotVect = new Vector3(0, 0, 0);
        //float thrust = 5;
        //NetWorkManager.NetworkAgent.CmdTellServerToMoveWorldNode(NodeID, testLocVect, testRotVect, thrust);

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


    public void MakeNodeFollowNetworkNode(float thrust)
    {
        _thrust = thrust;
        _followNetworkNode = true;
    }
}
