using System.Collections.Generic;
using UnityEngine;

public class ConnectorNodeBuilder : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static ConnectorNodeBuilder _instance;

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

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    // Get Connector Vects /////////////////////////////////////////////////////
    public static Dictionary<WorldNode, List<KeyValuePair<Vector3Int, int>>> GetConnectorVects(List<WorldNode> worldNodes)
    {
        Dictionary<WorldNode, List<KeyValuePair<Vector3Int, int>>> connectorVectsAndRotations = new Dictionary<WorldNode, List<KeyValuePair<Vector3Int, int>>>();

        // int floorBounds = MapSettings.worldSizeX * MapSettings.worldSizeZ; dont need this now for some weird reason
        int roofBounds = ((MapSettings.worldSizeX * MapSettings.worldSizeZ) * MapSettings.worldSizeY);

        foreach (WorldNode worldNode in worldNodes)
        {
            int nodeCount = worldNode.worldNodeCount;
            int[] worldNeighbours = worldNode.neighbours;
            List<KeyValuePair<Vector3Int, int>> vectList = new List<KeyValuePair<Vector3Int, int>>();

            if (worldNode.NodeSize == 1)
            {
                foreach (int worldNeigh in worldNeighbours)
                {
                    if (worldNeigh != -1) // out of bounds check
                    {
                        if (worldNeigh < roofBounds) // keeping nodes inside bounds
                        {
                            WorldNode neighbour = worldNodes[worldNeigh];
                            KeyValuePair<Vector3Int, int> vectorAndRot = GetVectsAndRotation(worldNode, neighbour, worldNeigh);
                            if (vectorAndRot.Value != -1) // weird issue still not sure why
                            {
                                vectList.Add(vectorAndRot);
                            }
                        }
                    }
                }
                connectorVectsAndRotations.Add(worldNode, vectList);
            }
        }
        return connectorVectsAndRotations;
    }

    public static KeyValuePair<Vector3Int, int> GetVectsAndRotation(WorldNode node0, WorldNode node1, int neighCount)
    {
        Vector3Int connectionVect = new Vector3Int();

        bool initialSmaller = false;
        WorldNode smallerNode = null;
        WorldNode biggerNode = null;

        if (node0.NodeSize <= node1.NodeSize)
        {
            initialSmaller = true;
            smallerNode = node0;
            biggerNode = node1;
        }
        else if (node0.NodeSize > node1.NodeSize)
        {
            initialSmaller = false;
            smallerNode = node1;
            biggerNode = node0;
        }
        else
        {
            Debug.LogError("Something went wrong here");
        }


       int direction = -1;

        Vector3Int finalVect;
        Vector3Int seperateAxis; // this is to seperate what axis x,y,z neighbour is

        if (initialSmaller)
        {
            seperateAxis = (biggerNode.NodeLocation - smallerNode.NodeLocation);
        }
        else
        {
            seperateAxis = (smallerNode.NodeLocation - biggerNode.NodeLocation);
        }


        finalVect = node0.NodeLocation;

        if (seperateAxis.x != 0 && seperateAxis.y == 0 && seperateAxis.z == 0)
        {
            if (seperateAxis.x > 0)
            {
                direction = 1;
                finalVect = new Vector3Int(finalVect.x + (MapSettings.sizeOfMapPiecesXZ), finalVect.y, finalVect.z);
            }
            else if (seperateAxis.x < 0)
            {
                direction = 3;
                finalVect = new Vector3Int(finalVect.x - (MapSettings.sizeOfMapPiecesXZ), finalVect.y, finalVect.z);
            }
        }
        else if (seperateAxis.x == 0 && seperateAxis.y != 0 && seperateAxis.z == 0)
        {
            if (seperateAxis.y > 0)
            {
                direction = 4; // these are the bastards making the connectors go UP
                finalVect = new Vector3Int(finalVect.x, finalVect.y + (MapSettings.sizeOfMapPiecesY), finalVect.z);
            }
            else if (seperateAxis.y < 0)
            {
                direction = 4;// these are the bastards making the connectors go UP
                finalVect = new Vector3Int(finalVect.x, finalVect.y - (MapSettings.sizeOfMapPiecesY), finalVect.z);
            }
        }
        else if (seperateAxis.x == 0 && seperateAxis.y == 0 && seperateAxis.z != 0)
        {
            if (seperateAxis.z > 0)
            {
                direction = 0;
                finalVect = new Vector3Int(finalVect.x, finalVect.y, finalVect.z + (MapSettings.sizeOfMapPiecesXZ));
            }
            else if (seperateAxis.z < 0)
            {
                direction = 2;
                finalVect = new Vector3Int(finalVect.x, finalVect.y, finalVect.z - (MapSettings.sizeOfMapPiecesXZ));
            }
        }
        else
        {
            //Debug.LogError("SOMETHING WRONG HERE direction: " + direction);
            //Debug.LogFormat("initialSmaller: {0} -node0: {1} -node1: {2}", initialSmaller, node0.nodeLocation, node1.nodeLocation);
            return new KeyValuePair<Vector3Int, int>(new Vector3Int(-1, -1, -1), -1);
        }

        connectionVect = new Vector3Int(finalVect.x - 1, finalVect.y, finalVect.z - 1); // -1's to fix annoying postiioning issue

        return new KeyValuePair<Vector3Int, int>(connectionVect, direction);
    }
    ////////////////////////////////////////////////////////////////////////////



    // Create Connector Nodes ////////////////////////////////////////////////
    public static Dictionary<WorldNode, List<ConnectorNode>> CreateConnectorNodes(Dictionary<WorldNode, List<KeyValuePair<Vector3Int, int>>> connectorVects)
    {
        // Wrap map Nodes around around Initial
        Dictionary<WorldNode, List<ConnectorNode>> worldNodeAndConnectorNodes = new Dictionary<WorldNode, List<ConnectorNode>>();

        foreach (WorldNode worldNode in connectorVects.Keys)
        {
            List<ConnectorNode> connectorNodes = new List<ConnectorNode>();
            List<KeyValuePair<Vector3Int, int>> vectsAndRot = connectorVects[worldNode];

            foreach (KeyValuePair<Vector3Int, int> pair in vectsAndRot)
            {
                Vector3Int location = pair.Key;
                int direction = pair.Value;

                int nodeLayerCount = -1;

                MapPieceStruct mapData = new MapPieceStruct()
                {
                    nodeType = NodeTypes.ConnectorNode,
                    mapPiece = MapPieceTypes.ConnectorPiece_01,
                    location = pair.Key,
                    rotation = Quaternion.identity,
                    direction = pair.Value,
                    parentNode = worldNode.gameObject.transform
                };

                ConnectorNode connectorNode = WorldBuilder._nodeBuilder.CreateNode<ConnectorNode>(mapData);
                WorldBuilder._nodeBuilder.AttachCoverToNode(connectorNode, connectorNode.gameObject, CoverTypes.ConnectorCover, new Vector3(0, direction * 90, 0));
                connectorNode.NodeSize = 1;
                connectorNode.neighbours = new int[6];
                for (int i = 0; i < connectorNode.neighbours.Length; i++)
                {
                    connectorNode.neighbours[i] = 1;
                }
                connectorNode.worldNodeParent = worldNode;

                connectorNodes.Add(connectorNode);

                // Up connector
                if (direction == 4) // DONT LIKE THIS CATCH ITS DUMB
                {
                    connectorNode.connectorUp = true;
                    connectorNode.NodeMapPiece = MapPieceTypes.MapPiece_Corridor_Up_01;
                    connectorNode.transform.localEulerAngles = new Vector3(0, direction * 90, 0);

                    Transform nodeCover = connectorNode.transform.Find("ConnectorCoverPrefab(Clone)");

                    nodeCover.localPosition = new Vector3(0, 0, 0); // to fix annoying vertical connector
                    nodeCover.localScale = new Vector3(8, 8, 8);
                    nodeCover.Find("Select").transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }


                if (worldNode.NodeSize == 1)
                {
                    nodeLayerCount = worldNode.NodeLayerCount + 4;  // 4 total layers in 1 map and vent piece
                }
                else
                {
                    Debug.LogError("something weird here");
                }
                connectorNode.NodeLayerCount = nodeLayerCount;

                LayerManager.AddNodeToLayer(connectorNode); // for camera layers

            }
            worldNode.connectorNodes = connectorNodes;
            worldNodeAndConnectorNodes.Add(worldNode, connectorNodes);


            ///// Connector Neighbours << This is a bit annoying is only using worldnode neighbours atm, might be cool to just use map neighbours instead... something to do
            int[] worldNodeNeighbours = worldNode.neighbours;

            if (worldNode.NodeSize == 1)
            {
                foreach (ConnectorNode connectorNode in worldNode.connectorNodes)
                {
                    int[] connectorNeighbours = connectorNode.neighbours;

                    for (int i = 0; i < worldNodeNeighbours.Length; i++)
                    {
                        if (worldNodeNeighbours[i] != -1)
                        {
                            connectorNeighbours[i] = 1;
                        }
                        else
                        {
                            connectorNeighbours[i] = -1;
                            //connectorNode.entranceSides.Add(i);
                        }
                    }
                }
            }
            ////////
        }
        return worldNodeAndConnectorNodes;
    }
    ////////////////////////////////////////////////////////////////////////////
}
