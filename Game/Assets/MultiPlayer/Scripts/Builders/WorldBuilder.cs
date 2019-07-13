using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static WorldBuilder _instance;

    ////////////////////////////////////////////////

    public static NodeBuilder _nodeBuilder;

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
        _nodeBuilder = transform.Find("NodeBuilder").GetComponent<NodeBuilder>();
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////


    public static void BuildWorldNodes()
    {
        //NetWorkManager.NetworkAgent.CmdTellServerToSpawnNetworkNodeContainer(Vector3Int.zero);

        List<Vector3Int> worldVects;
        //Dictionary<WorldNode, List<MapNode>> worldAndWrapperNodes;
        Dictionary<WorldNode, List<ConnectorNode>> worldAndconnectorNodes;
        Dictionary<WorldNode, List<KeyValuePair<Vector3Int, int>>> connectorVectsAndRotations;

        List<List<Vector3Int>> container = WorldNodeBuilder.GetWorld_Outer_DockingVects();
        worldVects = container[0];

        WorldNodeBuilder.CreateWorldNodes(worldVects);
        List<WorldNode> worldNodes = WorldNodeBuilder.GetWorldNodes();
        WorldNodeBuilder.GetWorldNodeNeighbours();
        MapNodeBuilder.CreateMapNodes(worldNodes, false, Vector3Int.zero);

        connectorVectsAndRotations = ConnectorNodeBuilder.GetConnectorVects(worldNodes);
        worldAndconnectorNodes = ConnectorNodeBuilder.CreateConnectorNodes(connectorVectsAndRotations);
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    public static void AttachMapToNode<T>(T node) where T : BaseNode
    {
        if (node.NodeType == NodeTypes.WorldNode)
        {
            WorldNode worldNode = node as WorldNode;
            AttachMapsToWorldNode(worldNode);
        }

        if (node.NodeType == NodeTypes.MapNode)
        {
            MapNode mapNode = node as MapNode;
            AttachMapToMapNode(mapNode);
        }

        if (node.NodeType == NodeTypes.ConnectorNode)
        {
            ConnectorNode connectNode = node as ConnectorNode;
            AttachMapToConnectorNode(connectNode);
        }
       // Debug.Log("AttachMapToNode FINISHED");
    }

    ////////////////////////////////////////////////

    private static void AttachMapsToWorldNode(WorldNode worldNode)
    {
        int mapCount = 0;
        foreach (MapNode mapNode in worldNode.mapNodes)
        {
            mapNode.entrance = true;
            
            GridBuilder.BuildLocationGrid(mapNode.NodeLocation);
            MapPieceBuilder.SetWorldNodeNeighboursForDock(worldNode.neighbours); // for the ship docks
            MapPieceBuilder.AttachMapPieceToNode(mapNode);
            MapPieceBuilder.SetPanelsNeighbours();
            //mapNode.mapFloorData = MapPieceBuilder.MapFloorData;
            //mapNode.mapVentData = MapPieceBuilder.MapVentData;
            mapCount++;
        }

        GameManager.StartGame(worldNode.NodeLocation); // <<<<<< start the fucking game bitch
    }


    private static void AttachMapToMapNode(MapNode mapNode)
    {
        GridBuilder.BuildLocationGrid(mapNode.NodeLocation);
        MapPieceBuilder.AttachMapPieceToNode(mapNode);
        MapPieceBuilder.SetPanelsNeighbours();
        //mapNode.RemoveDoorPanels();
        //mapNode.mapFloorData = MapPieceBuilder.MapFloorData;
        //mapNode.mapVentData = MapPieceBuilder.MapVentData;
    }


    private static void AttachMapToConnectorNode(ConnectorNode connectNode)
    {
        GridBuilder.BuildLocationGrid(connectNode.NodeLocation);
        MapPieceBuilder.AttachMapPieceToNode(connectNode);
        MapPieceBuilder.SetPanelsNeighbours();
        //connectNode.mapFloorData = _mapPieceBuilder.GetMapFloorData();
        //connectNode.mapVentData = _mapPieceBuilder.GetMapVentData();
    }

}
