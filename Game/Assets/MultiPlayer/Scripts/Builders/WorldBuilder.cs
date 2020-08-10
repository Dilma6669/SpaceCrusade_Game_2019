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


    public static void BuildEnvironmentNodes()
    {
        WorldNodeBuilder.CalculateWorldNodeVects();
        WorldNodeBuilder.CreateWorldNodes();
        WorldNodeBuilder.CalculateWorldNodeNeighbours();

        foreach (WorldNode worldNode in WorldNodeBuilder.GetWorldNodes().Values)
        {
            if (worldNode.NodeData != null)
            {
                MapNodeBuilder.CreateMapNodesForWorldNode(worldNode);
            }
        }
       // LayerManager.AssignLayerCountsToWorldAndMapNodes(WorldNodeBuilder.GetWorldNodes());
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

        if (node.NodeType == NodeTypes.MapNode || node.NodeType == NodeTypes.ConnectorNode)
        {
            MapNode mapNode = node as MapNode;
            AttachMapToMapNode(mapNode);
        }
    }

    ////////////////////////////////////////////////

    private static void AttachMapsToWorldNode(WorldNode worldNode)
    {
        Debug.Log("fuck wtf AttachMapsToWorldNode");
        int mapCount = 0;
        foreach (MapNode mapNode in worldNode.mapNodes)
        {
            mapNode.entrance = true;
            
            MapPieceBuilder.SetWorldNodeNeighboursForDock(worldNode.neighbourVects); // for the ship docks
            AttachMapToMapNode(mapNode);
            mapCount++;
        }
    }


    private static void AttachMapToMapNode(MapNode mapNode)
    {
        MapPieceBuilder.BuildMap(mapNode);
        MapPieceBuilder.SetPanelsNeighbours();
    }

}
