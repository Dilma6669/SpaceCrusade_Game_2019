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

    public static void CreateMapNodesForWorldNode(WorldNode worldNode)
    {
        int maxNodeSizeX = 9;
        int maxNodeSizeY = 9;
        int maxNodeSizeZ = 9;

        int nodeDistanceX = MapSettings.MapNodeCountDistanceXZ;
        int nodeDistanceY = MapSettings.MapNodeCountDistanceY;
        int nodeDistanceZ = MapSettings.MapNodeCountDistanceXZ;

        int vectStartLocX = -(nodeDistanceX * 4);
        int vectStartLocY = -(nodeDistanceY * 4);
        int vectStartLocZ = -(nodeDistanceZ * 4);

        int vectLocX = vectStartLocX;
        int vectLocY = vectStartLocY;
        int vectLocZ = vectStartLocZ;

        Dictionary<int, int[]> mapPieces = worldNode.NodeData.worldNodeMapPieces;

        List<MapNode> mapNodes = new List<MapNode>();

        int locCounter = 1; // coz locations start at 1
        for (int y = 0; y < maxNodeSizeY; y++)
        {
            for (int z = 0; z < maxNodeSizeZ; z++)
            {
                for (int x = 0; x < maxNodeSizeX; x++)
                {
                    if (mapPieces.ContainsKey(locCounter))
                    {
                        MapNodeStruct mapData = new MapNodeStruct()
                        {
                            NodeID = new Vector3Int(vectLocX, vectLocY, vectLocZ),
                            mapPiece = (MapPieceTypes)mapPieces[locCounter][0],
                            location = new Vector3Int(vectLocX, vectLocY, vectLocZ),
                            rotation = new Vector3Int(0, mapPieces[locCounter][1], 0),
                            parentNode = worldNode.gameObject.transform,
                            spinny = mapPieces[locCounter][2]
                        };

                        MapNode mapNode = WorldBuilder._nodeBuilder.CreateMapNode(mapData);
                        mapNode.worldNodeParent = worldNode;
                        mapNodes.Add(mapNode);

                        if (!worldNode.entrance) // this could be better
                        {
                            CoverTypes coverType = CoverTypes.NormalCover;

                            if (mapNode.NodeMapPiece == MapPieceTypes.ConnectorPiece_Hor_Empty)
                            {
                                coverType = CoverTypes.ConnectorCover;
                            }
                            else if (mapNode.NodeMapPiece == MapPieceTypes.ConnectorPiece_Ver_Empty)
                            {
                                coverType = CoverTypes.ConnectorUPCover;
                            }

                            WorldBuilder._nodeBuilder.AttachCoverToNode(mapNode, mapNode.gameObject, coverType);

                        }
                    }

                    locCounter++;

                    vectLocX += nodeDistanceX;
                }
                vectLocX = vectStartLocX;

                vectLocZ += nodeDistanceZ;
            }
            vectLocX = vectStartLocX;
            vectLocZ = vectStartLocZ;

            vectLocY += nodeDistanceY;
        }

        worldNode.mapNodes = mapNodes;
    }
}
