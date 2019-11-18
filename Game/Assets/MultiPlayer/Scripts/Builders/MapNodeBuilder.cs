using System.Collections.Generic;
using UnityEngine;

public class MapNodeBuilder : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static MapNodeBuilder _instance;

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

    // Get Map Vects ////////////////////////////////////////////////////////////
    private static List<Vector3> GetMapVects(WorldNode nodeScript, bool customShip, Vector3 customSize)
    {
        Vector3 worldNodeloc = nodeScript.NodeLocation;

        List<Vector3> nodeVects = new List<Vector3>();

        int sizeX;
        int sizeZ;
        int sizeY;

        int multiplierX;
        int multiplierZ;
        int multiplierY;

        int countX;
        int countZ;
        int countY;

        if (!customShip)
        {
            sizeX = nodeScript.NodeSize;
            sizeZ = nodeScript.NodeSize;
            sizeY = nodeScript.NodeSize;
        }
        else
        {
            sizeX = (int)customSize.x;
            sizeZ = (int)customSize.z;
            sizeY = (int)customSize.y;
        }

        int mapOffset = -1;// this is for the maps overlapping each other with thenew system (might be fucking my self over here)

        multiplierX = (int)Mathf.Floor(sizeX / 2) * (MapSettings.sizeOfMapPiecesXZ + mapOffset);
        multiplierZ = (int)Mathf.Floor(sizeZ / 2) * (MapSettings.sizeOfMapPiecesXZ + mapOffset);
        multiplierY = (int)Mathf.Floor(sizeY / 2) * (MapSettings.sizeOfMapPiecesY + mapOffset);

        countX = (int)worldNodeloc.x - multiplierX;
        countZ = (int)worldNodeloc.z - multiplierZ;
        countY = (int)worldNodeloc.y - multiplierY;

        for (int y = 0; y < sizeY; y++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                for (int x = 0; x < sizeX; x++) 
                {
                    // Debug.Log("Vector3 (gridLoc): x: " + x + " y: " + y + " z: " + z);
                    nodeVects.Add(new Vector3(countX, countY, countZ));
                    countX += (MapSettings.sizeOfMapPiecesXZ + mapOffset);
                }
                countX = (int)worldNodeloc.x - multiplierX;
                countZ += (MapSettings.sizeOfMapPiecesXZ + mapOffset);
            }
            countX = (int)worldNodeloc.x - multiplierX;
            countZ = (int)worldNodeloc.z - multiplierZ;
            countY += (MapSettings.sizeOfMapPiecesY + mapOffset);
        }

        return nodeVects;
    }
    ////////////////////////////////////////////////////////////////////////////


    // Create Map Nodes ////////////////////////////////////////////////////////
    public static Dictionary<WorldNode, List<MapNode>> CreateMapNodes(List<WorldNode> worldNodes, bool customShip, Vector3 shipSize, List<MapPieceStruct> customShipMapPieces = null)
    {
        // Wrap map Nodes around around Initial
        Dictionary<WorldNode, List<MapNode>> worldNodeAndWrapperNodes = new Dictionary<WorldNode, List<MapNode>>();

        foreach (WorldNode worldNode in worldNodes)
        {
            List<Vector3> mapVects = (customShip == false) ? GetMapVects(worldNode, false, Vector3.zero) : GetMapVects(worldNode, true, shipSize);

            List<MapNode> mapNodes = new List<MapNode>();

            int mapCount = 1;
            int layerCount = 0;
            int nodeLayerCount = -1;

            foreach (Vector3 location in mapVects)
            {
                MapPieceStruct mapData = new MapPieceStruct();

                if (customShip == false)
                {
                    MapPieceTypes randomMapPiece = MapPieceTypes.MapPiece_Corridor_01;

                    int random = Random.Range(0, 3);
                    if (random == 0)
                    {
                        randomMapPiece = MapPieceTypes.MapPiece_Corridor_01;
                    }
                    if (random == 1)
                    {
                        randomMapPiece = MapPieceTypes.MapPiece_Corridor_02;
                    }
                    if (random == 2)
                    {
                        randomMapPiece = MapPieceTypes.MapPiece_Corridor_03;
                    }

                    mapData = new MapPieceStruct()
                    {
                        nodeType = NodeTypes.MapNode,
                        mapPiece = randomMapPiece,
                        location = location,
                        rotation = Vector3.zero,
                        direction = 0,
                        parentNode = worldNode.gameObject.transform
                    };
                }
                else
                {
                    MapPieceStruct customMapData = customShipMapPieces[mapCount - 1];

                    mapData = new MapPieceStruct()
                    {
                        nodeType = customMapData.nodeType,
                        mapPiece = customMapData.mapPiece,
                        location = location,
                        rotation = Vector3.zero,
                        direction = 0,
                        parentNode = worldNode.gameObject.transform
                    };
                }


                MapNode mapNode = WorldBuilder._nodeBuilder.CreateNode<MapNode>(mapData);
                mapNode.NodeSize = 1;
                mapNode.worldNodeParent = worldNode;
                mapNodes.Add(mapNode);

                ///////////
                // NOT TO FUTURE SELF THIS SECTION IS NOT WORKING BECAUSE THERE IS NO LONGER A CENTRAL NODE LAYER COUNT SYSTEM BECAUSE OF CUSTOM SHIPS ALL RUNNNIG THROUGH HERE
                if (worldNode.NodeSize == 1)
                {
                    nodeLayerCount = worldNode.NodeLayerCount + 4; // 4 total layers in 1 map and vent piece
                }
                else if (worldNode.NodeSize == 3)
                {
                    nodeLayerCount = worldNode.NodeLayerCount + layerCount;

                    if (mapCount % 9 == 0)
                    {
                        layerCount = layerCount + 4;  // 4 total layers in 1 map and vent piece
                    }
                }
                else
                {
                    Debug.LogError("something weird here because a custom ship isnt defined by NodeSize");
                }
                mapNode.NodeLayerCount = nodeLayerCount;

                LayerManager.AddNodeToLayer(mapNode); // for camera layers // THIS IS GOING TO BE ALLLLL FUCKED WITH SHIPS MAP PIECES
                ////////////////////
                ///////////


                if (!worldNode.entrance)
                {
                    WorldBuilder._nodeBuilder.AttachCoverToNode(mapNode, mapNode.gameObject, CoverTypes.NormalCover, new Vector3(0, 0, 0));
                }


                mapNode.neighbours = new int[6];
                for (int i = 0; i < mapNode.neighbours.Length; i++)
                {
                    mapNode.neighbours[i] = 1;
                }


                mapCount++;
            }
            worldNode.mapNodes = mapNodes;
            worldNodeAndWrapperNodes.Add(worldNode, mapNodes);

            ////////////////


            if (customShip == false)
            {
                //// Map Neighbours

                int[] worldNodeNeighbours = worldNode.neighbours;
                if (worldNode.NodeSize == 1)
                {
                    MapNode mapNode = worldNode.mapNodes[0];
                    int[] mapNeighbours = mapNode.neighbours;

                    for (int i = 0; i < worldNodeNeighbours.Length; i++)
                    {
                        if (worldNodeNeighbours[i] != -1)
                        {
                            mapNeighbours[i] = 1;
                        }
                        else
                        {
                            mapNeighbours[i] = -1;
                            //mapNode.entranceSides.Add(i);
                        }
                    }
                }
                ////////
                if (worldNode.NodeSize == 3)
                {
                    // bottom
                    SetMapNeighboursWithMultipleLinks(worldNode, 0, 4, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
                    // Front
                    SetMapNeighboursWithMultipleLinks(worldNode, 1, 10, new int[] { 0, 1, 2, 9, 10, 11, 18, 19, 20 });
                    // Left
                    SetMapNeighboursWithMultipleLinks(worldNode, 2, 12, new int[] { 0, 3, 6, 9, 12, 15, 18, 21, 24 });
                    // Right
                    SetMapNeighboursWithMultipleLinks(worldNode, 3, 14, new int[] { 2, 5, 8, 11, 14, 17, 20, 23, 26 });
                    // Back
                    SetMapNeighboursWithMultipleLinks(worldNode, 4, 16, new int[] { 6, 7, 8, 15, 16, 17, 24, 25, 26 });
                    // Top
                    SetMapNeighboursWithMultipleLinks(worldNode, 5, 22, new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26 });
                }
            }
        }

        return worldNodeAndWrapperNodes;
    }
    ////////////////////////////////////////////////////////////////////////////

    // SetUp MapNode Connections to neighbours ////////////////////////////////////////////////////////
    private static void SetMapNeighboursWithMultipleLinks(WorldNode worldNode, int worldNeighCount, int singleLinkCount, int[] multipleLinkCounts)
    {
        int[] worldNodeNeighbours = worldNode.neighbours;

        if (worldNodeNeighbours[worldNeighCount] != -1)
        {
            WorldNode worldNeighbour = WorldNodeBuilder.GetWorldNode(worldNodeNeighbours[worldNeighCount]);

            if (worldNeighbour.NodeSize == 1)
            {
                foreach (int link in multipleLinkCounts)
                {
                    worldNode.mapNodes[link].neighbours[worldNeighCount] = -1;
                }
                worldNode.mapNodes[singleLinkCount].neighbours[worldNeighCount] = 1; // for the middle front connector
            }
            if (worldNeighbour.NodeSize == 3)
            {
                foreach (int link in multipleLinkCounts)
                {
                    worldNode.mapNodes[link].neighbours[worldNeighCount] = 1;
                }
            }
        }
        else
        {
            foreach (int link in multipleLinkCounts)
            {
                worldNode.mapNodes[link].neighbours[worldNeighCount] = -1;
                //worldNode.mapNodes[link].entranceSides.Add(worldNeighCount);
            }
        }
    }
    ////////////////////////////////////////////////////////////////////////////
}
