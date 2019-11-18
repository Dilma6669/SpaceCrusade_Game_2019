using System.Collections.Generic;
using UnityEngine;

public class MapPieceBuilder : MonoBehaviour {

    ////////////////////////////////////////////////

    private static MapPieceBuilder _instance;

    ////////////////////////////////////////////////

    private static List<int[,]> _floors = new List<int[,]>();
    private static List<int[,]> _vents = new List<int[,]>();
    private static List<int[,]> _floorDataToReturn = new List<int[,]>();
    private static List<int[,]> _ventDataToReturn = new List<int[,]>();

    ////////////////////////////////////////////////

    private static List<CubeLocationScript> halfCubesWithPanels = new List<CubeLocationScript>(); 

    private static int[] neighbours;

    private static int[] worldNeighbours;

    private static int layerCount = -1;

    private static int nodeLayerCounter = 0;

    ////////////////////////////////////////////////

    public static List<int[,]> MapFloorData
    {
        get { return _floorDataToReturn; }
        set { _floorDataToReturn = value; }
    }

    public static List<int[,]> MapVentData
    {
        get { return _ventDataToReturn; }
        set { _ventDataToReturn = value; }
    }

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
    //////////////////////////////////////////////

    // trying to connection neighbours early so this is here
    public static void SetPanelsNeighbours()
    {
        foreach (CubeLocationScript script in halfCubesWithPanels)
        {
            script.AssignCubeNeighbours();
        }
        halfCubesWithPanels.Clear();
    }

    public static void SetWorldNodeNeighboursForDock(int[] worldNodes)
    {
        worldNeighbours = worldNodes;
    }

    //////////////////////////////////////////////

    public static void AttachMapPieceToNode<T>(T node) where T : BaseNode 
    {
        MapFloorData.Clear(); // storing data for serialzatino
        MapVentData.Clear();

        halfCubesWithPanels.Clear();

        neighbours = node.neighbours;
        layerCount = 2;

        nodeLayerCounter = 0;

        BuildMapsByIEnum(node);

    }


    private static void BuildMapsByIEnum<T>(T node) where T : BaseNode
    {
        Vector3 nodeLoc = node.NodeLocation;
        int rotations = node.NodeDirection;

        int startGridLocX = (int)nodeLoc.x - (int)Mathf.Floor(MapSettings.sizeOfMapPiecesXZ / 2);
        int startGridLocY = (int)nodeLoc.y - (int)Mathf.Floor(MapSettings.sizeOfMapPiecesY / 2);
        int startGridLocZ = (int)nodeLoc.z - (int)Mathf.Floor(MapSettings.sizeOfMapPiecesXZ / 2);

        Vector3 GridLoc;

        List<int[]> layers = GetMapPiece(node.NodeMapPiece);

        /*
           if (node.entrance)
           {
               KeyValuePair<int, int> mapAndRot = GetShipEntranceMap((int)mapPiece);
               mapPiece = mapAndRot.Key;
               rotation = mapAndRot.Value;
           }
           */


        int objectsCountX = startGridLocX;
        int objectsCountY = startGridLocY;
        int objectsCountZ = startGridLocZ;

        int cubeCounter = 0;
        for (int y = 0; y < MapSettings.sizeOfMapPiecesY; y++)
        {

            objectsCountX = startGridLocX;
            objectsCountZ = startGridLocZ;

           // floor = layers;


            // for an extra layer roof of the vents only appearing if no map piece above vent
            if (!node.entrance)
            {
                if (y == (layers.Count - 1) && neighbours[5] != -1)
                {
                    continue; // if so, skip last layer
                }
            }


            for (int r = 0; r < rotations; r++)
            {
            
                /*
                if(floorORRoof) // storing the map data for serilisation and other shit still to work out
                { // have to implement this at some stage
                    floorDataToReturn.Add(floor);
                }
                else
                {
                    ventDataToReturn.Add(floor);
                }
                */
            }

            for (int z = 0; z < MapSettings.sizeOfMapPiecesXZ; z++)
            {
                objectsCountX = startGridLocX;

                for (int x = 0; x < MapSettings.sizeOfMapPiecesXZ; x++)
                {
                    GridLoc = new Vector3(objectsCountX, objectsCountY, objectsCountZ);

                    if (layers != null && layers[cubeCounter] != null)
                    {
                        int[] cubeData = layers[cubeCounter];

                        //cubeType = FigureOutDoors(node, _mapType, cubeType, rotations);
                        int nodeLayercount = node.GetComponent<T>().NodeLayerCount + nodeLayerCounter;

                        CubeLocationScript cubeScript = CubeBuilder.CreateCubeObject(GridLoc, cubeData, rotations, nodeLayercount, node.gameObject.transform); // Create the cube // TIDY THIS UP

                        // A test to see if cube has panel to try make connecting neighbours easier
                        if (cubeScript && cubeScript.CubeIsPanel)
                        {
                            halfCubesWithPanels.Add(cubeScript);
                        }
                    }

                    cubeCounter++;

                    objectsCountX += 1;
                }
                objectsCountZ += 1;
            }

            if (y % 2 != 0)
            {
                nodeLayerCounter++;
            }
            objectsCountY += 1;
        }
    }
    
    ////////////////////////////////////////////////////

    private static KeyValuePair<int, int> GetShipEntranceMap(int mapCount)
    {
        if (mapCount == 0 || mapCount == 9 || mapCount == 18)
        {
            return GetMapPieceAndRotation(mapCount, -1, -1, new int[] { 1, 2 }, new int[] { 0, 0, 0, 3 });
        }
        if (mapCount == 1 || mapCount == 10 || mapCount == 19)
        {
            return GetMapPieceAndRotation(mapCount, 1, 0, new int[] { -1 }, new int[] { -1 });
        }
        if (mapCount == 2 || mapCount == 11 || mapCount == 20)
        {
            return GetMapPieceAndRotation(mapCount, -1, -1, new int[] { 1, 3 }, new int[] { 0, 1, 0, 1 });
        }
        if (mapCount == 3 || mapCount == 12 || mapCount == 21)
        {
            return GetMapPieceAndRotation(mapCount, 2, 3, new int[] { -1 }, new int[] { -1 });
        } 
        if (mapCount == 4) // middle Grnd // PLAYER SHIP IS PLACED HERE AND 13
        {
            return new KeyValuePair<int, int>( 0, 0 );
        }
        if (mapCount == 13 || mapCount == 22) // middle nothing/celing/empty space
        {
            return new KeyValuePair<int, int>(3, 0);
        }
        if (mapCount == 5 || mapCount == 14 || mapCount == 23)
        {
            return GetMapPieceAndRotation(mapCount, 3, 1, new int[] { -1 }, new int[] { -1 });
        }
        if (mapCount == 6 || mapCount == 15 || mapCount == 24)
        {
            return GetMapPieceAndRotation(mapCount, -1, -1, new int[] { 2, 4 }, new int[] { 1, 3, 3, 2 });
        }
        if (mapCount == 7 || mapCount == 16 || mapCount == 25)
        {
            return GetMapPieceAndRotation(mapCount, 4, 2, new int[] { -1 }, new int[] { -1 });
        }
        if (mapCount == 8 || mapCount == 17 || mapCount == 26)
        {
            return GetMapPieceAndRotation(mapCount, -1, -1, new int[] { 3, 4 }, new int[] { 2, 2, 1, 2 });
        }
        Debug.LogError("OPSALA SOMETHING WRONG HERE! mapCount: " + mapCount);
        return new KeyValuePair<int, int>(-1, -1);
    }

    private static KeyValuePair<int, int> GetMapPieceAndRotation(int nodeCount, int neigh1, int rot1, int[] neighs2, int[] rots2)
    {
        int mapPiece = -1; // either 0,1,2 : just floor/Floor and wall/floor and corner
        int rotation = -1;
        int var1 = -1; // 0 = no wall, 1 = wall closed door, 
        int var2 = -1; // 0 = no wall, 1 = wall closed door, 

        if (nodeCount == 1 || nodeCount == 3 || nodeCount == 5 || nodeCount == 7) // FOR THE GROUND FLOOR
        {
            // if no world neighbour is present, then just make a floor, else make a wall
            var1 = (worldNeighbours[neigh1] == -1) ? 0 : 1;

            if (var1 == 0)
            {
                mapPiece = 0; // just floor
                rotation = rot1; // does not matter
            }
            else if (var1 == 1)
            {
                mapPiece = 1; // Floor and wall
                rotation = rot1;
            }
            else
            {
                Debug.LogError("OPSALA SOMETHING WRONG HERE! map: ");
            }
        }
        else if (nodeCount == 10 || nodeCount == 12 || nodeCount == 14 || nodeCount == 16 || // FOR THE HIGHER FLOORS
                 nodeCount == 19 || nodeCount == 21 || nodeCount == 23 || nodeCount == 25)
        {
            // if no world neighbour is present, then just make a floor, else make a wall
            var1 = (worldNeighbours[neigh1] == -1) ? 0 : 1;

            if (var1 == 0)
            {
                mapPiece = 3; // just floor
                rotation = rot1; // does not matter
            }
            else if (var1 == 1)
            {
                mapPiece = 4; // Floor and wall
                rotation = rot1;
            }
            else
            {
                Debug.LogError("OPSALA SOMETHING WRONG HERE! map: ");
            }
        }
        else if (nodeCount == 0 || nodeCount == 2 || nodeCount == 6 || nodeCount == 8) // FOR THE GROUND FLOOR
        {
            var1 = (worldNeighbours[neighs2[0]] == -1) ? 0 : 1;
            var2 = (worldNeighbours[neighs2[1]] == -1) ? 0 : 1;


            if (var1 == 0 && var2 == 0)
            {
                mapPiece = 0; // just floor
                rotation = rots2[0]; // does not matter
            }
            else if (var1 == 1 && var2 == 1)
            {
                mapPiece = 2; // corner
                rotation = rots2[1]; 
            }
            else if (var1 == 1 && var2 == 0)
            {
                mapPiece = 1; // wall
                rotation = rots2[2]; 
            }
            else if (var1 == 0 && var2 == 1)
            {
                mapPiece = 1; // wall
                rotation = rots2[3];
            }
            else
            {
                Debug.LogError("OPSALA SOMETHING WRONG HERE! map: ");
            }
        }
        else if (nodeCount == 9 || nodeCount == 11 || nodeCount == 15 || nodeCount == 17 || // FOR THE HIGHER FLOORS
                nodeCount == 18 || nodeCount == 20 || nodeCount == 24 || nodeCount == 26)
        {
            var1 = (worldNeighbours[neighs2[0]] == -1) ? 0 : 1;
            var2 = (worldNeighbours[neighs2[1]] == -1) ? 0 : 1;


            if (var1 == 0 && var2 == 0)
            {
                mapPiece = 3; // just floor
                rotation = rots2[0]; // does not matter
            }
            else if (var1 == 1 && var2 == 1)
            {
                mapPiece = 5; // corner
                rotation = rots2[1];
            }
            else if (var1 == 1 && var2 == 0)
            {
                mapPiece = 4; // wall
                rotation = rots2[2];
            }
            else if (var1 == 0 && var2 == 1)
            {
                mapPiece = 4; // wall
                rotation = rots2[3];
            }
            else
            {
                Debug.LogError("OPSALA SOMETHING WRONG HERE! map: ");
            }
        }

        return new KeyValuePair<int, int>(mapPiece, rotation);
    }




    ///////
    private static int FigureOutDoors<T>(T node, int _mapType, int _cubeType, int rotations) where T : BaseNode
    {
        int originalCubeType = _cubeType;

        Dictionary<int, int> cubeTypeAndWallType = new Dictionary<int, int>()
        {
            { 50, 1 },
            { 51, 3 },
            { 52, 2 },
            { 53, 2 },
            { 54, 3 },
            { 55, 1 }
        };

        if (cubeTypeAndWallType.ContainsKey(_cubeType))
        {
            if (rotations == 1)
            { 
                if (_cubeType == 51)
                {
                    _cubeType = 53;
                }
                else if (_cubeType == 52)
                {
                    _cubeType = 51;
                }
                else if (_cubeType == 53)
                {
                    _cubeType = 54;
                }
                else if (_cubeType == 54)
                {
                    _cubeType = 52;
                }
            }
            else if (rotations == 2)
            {
                if (_cubeType == 51)
                {
                    _cubeType = 54;
                }
                else if (_cubeType == 52)
                {
                    _cubeType = 53;
                }
                else if (_cubeType == 53)
                {
                    _cubeType = 52;
                }
                else if (_cubeType == 54)
                {
                    _cubeType = 51;
                }
            }
            else if (rotations == 3)
            {
                if (_cubeType == 51)
                {
                    _cubeType = 52;
                }
                else if (_cubeType == 52)
                {
                    _cubeType = 54;
                }
                else if (_cubeType == 53)
                {
                    _cubeType = 51;
                }
                else if (_cubeType == 54)
                {
                    _cubeType = 53;
                }
            }
            int index = _cubeType - 50;

            //Debug.Log("fuceken jezus _cubeType: " + _cubeType);
            //Debug.Log("fuceken jezus index: " + index);

            // stay in here
            if (_mapType == MapSettings.MAPTYPE_SHIPPORT_FLOOR) // shipYard
            {
                if (worldNeighbours[index] != -1) // if world neighbour present
                {
                    if (neighbours[index] != -1) // if neighbour present
                    {
                        return 0; // return no wall
                    }
                    else
                    {
                        return cubeTypeAndWallType[originalCubeType]; // return wall
                    }
                }
                else // if world neighbour IS NOT present
                {
                    return cubeTypeAndWallType[originalCubeType]; // return wall
                }
            }
            ///////



            if (neighbours[index] == -1) // no neighbour (Space)
            {
                return cubeTypeAndWallType[originalCubeType]; // return wall
            }
            else
            {   // this is for the floors no connectors UP to cover the vents below it. kind of a special case really
                if (_mapType == MapSettings.MAPTYPE_CONNECT_UP_FLOOR && _cubeType == 50) 
                {
                    //if (worldNeighbours[0] != -1)
                    //{
                    //    int neighIndex = node.worldNodeParent.neighbours[0]; // need to know the vect of neighbours
                    //    //WorldNode worldBottomNeighbour =  .LocationManager._LocationLookup[neighIndex];
                    //}
                }
            }
        }
        return originalCubeType;
    }


    private static List<int[]> GetMapPiece(MapPieceTypes mapPieceType)
    {
        return LoadDataFromMapTextDoc(mapPieceType);
    }

    private static List<int[]> LoadDataFromMapTextDoc(MapPieceTypes mapPiece)
    {
        string textFileName = "MapData/" + mapPiece.ToString();

        TextAsset TextFile = Resources.Load(textFileName) as TextAsset;

        string[] words;
        List<int[]> array = new List<int[]>();

        if (TextFile != null)
        {
            // Add each line of the text file to
            // the array using a space
            // as the delimiter
            words = (TextFile.text.Split(','));

            for (int i = 0; i < words.Length; i++)
            {
                string line = words[i];

                string[] data = line.Split(' ');

                if (line.Length > 1)
                {
                    int[] dataArray = new int[6];

                    dataArray[0] = int.Parse(data[0]);
                    dataArray[1] = int.Parse(data[1]);
                    dataArray[2] = int.Parse(data[2]);
                    dataArray[3] = int.Parse(data[3]);
                    dataArray[4] = int.Parse(data[4]);
                    dataArray[5] = int.Parse(data[5]);

                    array.Add(dataArray);
                }
            }
        }

        return array;
    }
		
}
