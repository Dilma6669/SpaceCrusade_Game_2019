using System.Collections.Generic;
using UnityEngine;

public class WorldNodeBuilder : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static WorldNodeBuilder _instance;

    ////////////////////////////////////////////////

    private static List<Vector3Int> _WorldNodeVects;
    private static Dictionary<Vector3Int, WorldNode> _WorldNodes;

    private static int _0X0 = 00;
    private static int _1X1 = 01;
    private static int _3X3 = 03;

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

    public static WorldNode GetWorldNode(Vector3Int index)
    {
        if (!_WorldNodes.ContainsKey(index))
            Debug.Log("fuck Got an invalid world node access location here >> " + index);

        return _WorldNodes[index];
    }

    public static Dictionary<Vector3Int, WorldNode> GetWorldNodes()
    {
        return _WorldNodes;
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    // Get World Node Vects ////////////////////////////////////////////
    public static void CalculateWorldNodeVects()
    {
        _WorldNodeVects = new List<Vector3Int>();

        int startX = -(MapSettings.worldSizeX * MapSettings.WorldNodeCountDistanceXZ) / 2;
        int startZ = -(MapSettings.worldSizeZ * MapSettings.WorldNodeCountDistanceXZ) / 2;
        int startY = 0;

        for (int y = 0; y < MapSettings.worldSizeY; y++)
        {
            for (int z = 0; z < MapSettings.worldSizeZ; z++)
            {
                for (int x = 0; x < MapSettings.worldSizeX; x++)
                {
                    int resultX = startX + (x * MapSettings.WorldNodeCountDistanceXZ);
                    int resultZ = startZ + (z * MapSettings.WorldNodeCountDistanceXZ);
                    int resultY = startY + (y * MapSettings.WorldNodeCountDistanceY);

                    _WorldNodeVects.Add(new Vector3Int(resultX, resultY, resultZ));
                }
            }
        }
    }
    ////////////////////////////////////////////////////////////////////////////


    // Create World Nodes ///////////////////////////////////////////////////
    public static void CreateWorldNodes()
    {
        // build inital map Node
        _WorldNodes = new Dictionary<Vector3Int, WorldNode>();

        int rowMultipler = MapSettings.worldSizeX;
        int colMultiplier = MapSettings.worldSizeZ;

        int totalMultiplier = MapSettings.worldSizeX * MapSettings.worldSizeZ;

        int countFloorX = 0; // complicated to explain to future me
        int countFloorZ = 0;
        int countFloorY = 1;

        int count = 1;
        foreach (Vector3Int location in _WorldNodeVects)
        {
            WorldNodeStruct worldNodeData = new WorldNodeStruct()
            {
                NodeID = location,
                CurrLoc = location,
                CurrRot = Vector3Int.zero,
                PlayerID = -1
            };

            WorldNode nodeScript = WorldBuilder._nodeBuilder.CreateWorldNode(worldNodeData);

            // for the specified map structures
            if (MapSettings.LOADPREBUILT_STRUCTURE)
            {
                StructureData_01 worldStructure = new StructureData_01();
                List<int[,]> worldStructureFloors = worldStructure.floors;

                int[,] structureFloor = worldStructureFloors[countFloorY - 1];
                if (structureFloor[countFloorX, countFloorZ] == 01)
                {
                    nodeScript.NodeData = GetWorldNodeData(new int[] { _1X1, _3X3 });
                }
                else
                {
                    nodeScript.NodeData = GetWorldNodeData(new int[] { _0X0 });
                }
            }
            else
            {
                nodeScript.NodeData = GetWorldNodeData(new int[] { _0X0, _1X1, _3X3 });
            }


            int shipEntranceProbablity = 20;

            //if (nodeScript.NodeData.GetType() == typeof(WorldNodeData_3X3) && Random.Range(0, shipEntranceProbablity) == 0)
            //{
            //    WorldBuilder._nodeBuilder.AttachCoverToNode(nodeScript, nodeScript.gameObject, CoverTypes.LargeGarageCover); // REALLLY DONT LIKE THIS HERE
            //    nodeScript.entrance = true;
            //}

            //Debug.Log("fuck adding worldNode to Vect " + location);

            _WorldNodes.Add(location, nodeScript);

            // for counting, best not to change, even tho its ugly
            countFloorX++;
            if (count % totalMultiplier == 0)
            {
                countFloorY++;
            }
            if (countFloorX % rowMultipler == 0)
            {
                countFloorX = 0;
                countFloorZ++;
            }
            if (countFloorZ % colMultiplier == 0)
            {
                countFloorZ = 0;
            }

            // for the dynamic grid experiment
            LocationManager.SaveNodeTo_CLIENT(location, nodeScript);
            //LocationManager.SetNodeScriptToLocation_SERVER(vect, nodeScript); // this needs to do this in the server

            count++;
        }
    }
    ////////////////////////////////////////////////////////////////////////////

    private static BaseWorldNodeData GetWorldNodeData(int[] mapPieces)
    {
        int randomIndex = mapPieces[Random.Range(0, mapPieces.Length)];
        switch (randomIndex)
        {
            case 00:
                return null;
            case 01:
                return new WorldNodeData_1X1();
            case 03:
                return new WorldNodeData_3X3();
            default:
                Debug.Log("OPPSALA WE HAVE AN ISSUE HERE");
                return null;
        }
    }



    // Get World Node Neighbours ///////////////////////////////////////////////
    public static void CalculateWorldNodeNeighbours()
    {
        // build inital map Node
        List<WorldNode> worldNodes = new List<WorldNode>();
        bool left = true;
        bool right = false;
        bool front = false;
        bool back = true;
        bool bottom = true;
        bool top = true;

        int rowMultipler = MapSettings.worldSizeX;
        int colMultiplier = MapSettings.worldSizeZ;

        int totalMultiplier = MapSettings.worldSizeX * MapSettings.worldSizeZ;

        int countFloorY = 1;

        int count = 1;
        foreach (WorldNode worldNode in _WorldNodes.Values)
        {
            // for neighbours
            bottom = (countFloorY < 1) ? false : true;
            right = (count % rowMultipler == 0) ? false : true;
            left = (count == 1 || ((count - 1) % rowMultipler == 0)) ? false : true;
            front = ((count + MapSettings.worldSizeX) > (totalMultiplier * countFloorY) && count <= (totalMultiplier * countFloorY)) ? false : true;
            back = (count >= ((totalMultiplier + 1) * (countFloorY - 1)) && count <= (totalMultiplier * (countFloorY - 1)) + MapSettings.worldSizeX) ? false : true;
            top = (countFloorY > MapSettings.worldSizeY) ? false : true;

            GetWorldNodeNeighbours(worldNode, (count - 1), left, right, front, back, bottom, top);

            // for counting, best not to change, even tho its ugly
            if (count % totalMultiplier == 0)
            {
                countFloorY++;
            }
            count++;
        }
    }



    private static void GetWorldNodeNeighbours(WorldNode worldNode, int count, bool left, bool right, bool front, bool back, bool bottom, bool top)
    {
        int nodeDistanceXZ = MapSettings.WorldNodeCountDistanceXZ;
        int nodeDistanceY = MapSettings.WorldNodeCountDistanceY;

        worldNode.neighbourVects.Add((bottom) ? new Vector3Int(worldNode.NodeID.x, worldNode.NodeID.y - nodeDistanceY, worldNode.NodeID.z)  : new Vector3Int(-1, -1, -1));                                        //(x, y - 1, z)
        worldNode.neighbourVects.Add((back)   ? new Vector3Int(worldNode.NodeID.x, worldNode.NodeID.y, worldNode.NodeID.z - nodeDistanceXZ) : new Vector3Int(-1, -1, -1)); //(x, y, z - 1)
        worldNode.neighbourVects.Add((left)   ? new Vector3Int(worldNode.NodeID.x - nodeDistanceXZ, worldNode.NodeID.y, worldNode.NodeID.z) : new Vector3Int(-1, -1, -1)); //(x - 1, y, z)
        worldNode.neighbourVects.Add((right)  ? new Vector3Int(worldNode.NodeID.x + nodeDistanceXZ, worldNode.NodeID.y, worldNode.NodeID.z) : new Vector3Int(-1, -1, -1)); //(x + 1, y, z)
        worldNode.neighbourVects.Add((front)  ? new Vector3Int(worldNode.NodeID.x, worldNode.NodeID.y, worldNode.NodeID.z + nodeDistanceXZ) : new Vector3Int(-1, -1, -1)); //(x, y, z + 1)
        worldNode.neighbourVects.Add((top)    ? new Vector3Int(worldNode.NodeID.x, worldNode.NodeID.y + nodeDistanceY, worldNode.NodeID.z)  : new Vector3Int(-1, -1, -1));                                        //(x, y + 1, z)
    }
    ////////////////////////////////////////////////////////////////////////////
}
