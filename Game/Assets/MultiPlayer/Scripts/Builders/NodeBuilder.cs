using UnityEngine;

public class NodeBuilder : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static NodeBuilder _instance;

    ////////////////////////////////////////////////

    public GameObject _defaultCubeObject; // THE CUBES EVERYWHERE
    public GameObject _gridObjectPrefab; // Debugging purposes
    public GameObject _worldNodePrefab; // object that shows Map nodes
    public GameObject _mapNodePrefab; // object that shows Map nodes

    public GameObject _pathFindingPrefab; // strangely for pathfinding 
    public GameObject _networkNodePrefab; //a container on the server to put client world nodes into to make everything inside the network node move

    // COVERS
    public GameObject _normalCoverPrefab;
    public GameObject _openCoverPrefab;
    public GameObject _largeGarageCoverPrefab;
    public GameObject _connectorCoverPrefab;
    public GameObject _connectorUPCoverPrefab;

    public GameObject _motherShip;

    public GameObject _panelPrefab;

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

        if (_defaultCubeObject == null) { Debug.LogError("OOPSALA we have an ERROR!"); }
        if (_gridObjectPrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }
        if (_worldNodePrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }
        if (_mapNodePrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }
        if (_pathFindingPrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }

        if (_normalCoverPrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }
        if (_openCoverPrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }
        if (_largeGarageCoverPrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }
        if (_connectorCoverPrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }
        if (_connectorUPCoverPrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }

        if (_motherShip == null) { Debug.LogError("OOPSALA we have an ERROR!"); }

        if (_panelPrefab == null) { Debug.LogError("OOPSALA we have an ERROR!"); }
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    public GameObject CreateDefaultCube(Vector3Int localGridLoc, Transform parent)
    {
        GameObject cubeObject = Instantiate(GetNodePrefab(NodeTypes.CubeObject), parent, false); // empty cube
        cubeObject.transform.SetParent(parent);
        cubeObject.transform.localPosition = localGridLoc;

        CubeLocationScript cubeScript = cubeObject.GetComponent<CubeLocationScript>();

        cubeScript.CubeMoveable = false;

        if (Mathf.Abs(localGridLoc.y) % 2 == 0) // I fucken HATE THIS Has caused lots of issues
        {
            if (Mathf.Abs(localGridLoc.z) % 2 == 0)
            {
                if (Mathf.Abs(localGridLoc.x) % 2 == 0)
                {
                    cubeScript.CubeMoveable = true;
                }
            }
        }

        Vector3 cubeGlobalPos = cubeObject.transform.position;
        Vector3Int globalGridLoc = new Vector3Int(Mathf.FloorToInt(cubeGlobalPos.x), Mathf.FloorToInt(cubeGlobalPos.y), Mathf.FloorToInt(cubeGlobalPos.z));

        cubeScript.CubeID = globalGridLoc;

        //if (parent.GetComponent<MapNode>().Spinney == true) // for the spinny map experimental thing
        //{
        //    cubeScript.MapNodeParent = parent.parent.GetComponent<MapNode>();
        //}
        //else
        //{
            cubeScript.MapNodeParent = parent.GetComponent<MapNode>();
        //}

        cubeScript.CubeLayerID = parent.GetComponent<MapNode>().NodeLayerCount;

        return cubeObject;
    }

    ////////////////////////////////////////////////

    // Create Generic Node /////////////////////////////////////////////////////
    public WorldNode CreateWorldNode(WorldNodeStruct mapData)
    {
        //Debug.Log("Vector3Int (gridLoc): x: " + vect.x + " y: " + vect.y + " z: " + vect.z);
        GameObject nodeObject = Instantiate(GetNodePrefab(NodeTypes.WorldNode), WorldManager._WorldContainer.transform, false);
        nodeObject.transform.SetParent(WorldManager._WorldContainer.transform);
        nodeObject.transform.localPosition = Vector3Int.zero;
        nodeObject.transform.position = mapData.CurrLoc;

        WorldNode nodeScript = nodeObject.GetComponent<WorldNode>();
        nodeScript.NodeLayerCount = 0;
        nodeScript.NodeID = new Vector3Int((int)mapData.NodeID.x, (int)mapData.NodeID.y, (int)mapData.NodeID.z);
        return nodeScript;
    }


    public MapNode CreateMapNode(MapNodeStruct mapData)
    {
        //Debug.Log("Vector3Int (gridLoc): x: " + vect.x + " y: " + vect.y + " z: " + vect.z);
        GameObject nodeObject = Instantiate(GetNodePrefab(NodeTypes.MapNode), mapData.parentNode, false);
        Transform parentTransform = mapData.parentNode.transform;
        if (mapData.spinny == 1) // for the spinny map experimental thing
            parentTransform = mapData.parentNode.transform.Find("RotationalNode").transform;

        nodeObject.transform.SetParent(parentTransform);
        nodeObject.transform.localPosition = mapData.location; // Dontbe fooled! this is different to worldNode method above
        nodeObject.transform.localEulerAngles = mapData.rotation; // this might need to be looked at later

        MapNode nodeScript = nodeObject.GetComponent<MapNode>();

        if (mapData.spinny == 1) // for the spinny map experimental thing
            nodeScript.Spinney = true;

        nodeScript.NodeMapPiece = mapData.mapPiece;
        nodeScript.NodeLayerCount = 0;
        nodeScript.NodeID = mapData.NodeID;
        return nodeScript;
    }

    ////////////////////////////////////////////////


    public GameObject CreatePanelObject(CubeLocationScript cubeParent)
    {
        GameObject panelObject = Instantiate(_panelPrefab, cubeParent.gameObject.transform, false);
        panelObject.transform.SetParent(cubeParent.gameObject.transform);
        panelObject.transform.localPosition = Vector3Int.zero;

        PanelPieceScript panelScript = panelObject.gameObject.GetComponent<PanelPieceScript>();
        cubeParent.GetCubePanel = panelScript;
        cubeParent.CubeIsPanel = true;
        panelObject.name = "ERROR HERE";

        panelScript.cubeScriptParent = cubeParent;

        return panelObject;
    }

    ////////////////////////////////////////////////

    public GameObject CreatePathFindingNode(Transform parent, Vector3Int unitID)
    {
        //Debug.Log("Vector3Int (gridLoc): x: " + gridLocX + " y: " + gridLocY + " z: " + gridLocZ);
        GameObject nodeObject = Instantiate(GetNodePrefab(NodeTypes.PathFindingNode), parent, false);
        nodeObject.transform.SetParent(parent);
        nodeObject.transform.localPosition = Vector3Int.zero;
        nodeObject.transform.localEulerAngles = Vector3Int.zero;
        nodeObject.GetComponent<PathFindingNode>().UnitControllerID = unitID;
        return nodeObject;
    }

    ////////////////////////////////////////////////

    private GameObject GetNodePrefab(NodeTypes node)
    {
        switch (node)
        {
            case NodeTypes.CubeObject:
                return _defaultCubeObject;
            case NodeTypes.GridNode:
                return _gridObjectPrefab;
            case NodeTypes.WorldNode:
                return _worldNodePrefab;
            case NodeTypes.MapNode:
                return _mapNodePrefab;
            case NodeTypes.PathFindingNode:
                return _pathFindingPrefab;
            default:
                Debug.Log("OPPSALA WE HAVE AN ISSUE HERE");
                return null;
        }
    }

    ////////////////////////////////////////////////

    public void AttachCoverToNode(MapNode nodeType, GameObject node, CoverTypes _cover)
    {
        //Debug.Log("Vector3Int (gridLoc): x: " + gridLocX + " y: " + gridLocY + " z: " + gridLocZ);
        GameObject cover = Instantiate(GetCoverPrefab(_cover), node.transform, false);
        cover.transform.SetParent(node.transform);
        cover.transform.localScale = GetCoverSize(_cover);
        cover.GetComponent<NodeCover>().parentNode = nodeType;
        nodeType.NodeCover = cover;
    }


    private GameObject GetCoverPrefab(CoverTypes cover)
    {
        switch (cover)
        {
            case CoverTypes.NormalCover:
                return _normalCoverPrefab;
            case CoverTypes.OpenCover:
                return _openCoverPrefab; 
            case CoverTypes.LargeGarageCover:
                return _largeGarageCoverPrefab;
            case CoverTypes.ConnectorCover:
                return _connectorCoverPrefab;
            case CoverTypes.ConnectorUPCover:
                return _connectorUPCoverPrefab;
            default:
                Debug.Log("OPPSALA WE HAVE AN ISSUE HERE");
                return null;
        }
    }


    private Vector3Int GetCoverSize(CoverTypes cover) // All these magic numbers are stupid but couldnt figure out a better way at this stage, any type of equation doesnt work
    {
        switch (cover)
        {
            case CoverTypes.NormalCover:
                return new Vector3Int(34, 18, 34);
            case CoverTypes.OpenCover:
                return new Vector3Int(102, 54, 102);
            case CoverTypes.ConnectorCover:
                return new Vector3Int(11, 11, 34);
            case CoverTypes.ConnectorUPCover:
                return new Vector3Int(11, 18, 11);
            default:
                Debug.Log("OPPSALA WE HAVE AN ISSUE HERE");
                return new Vector3Int(0,0,0);
        }
    }

    ////////////////////////////////////////////////

}
