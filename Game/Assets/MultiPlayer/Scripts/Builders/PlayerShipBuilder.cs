using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBuilder : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static PlayerShipBuilder _instance;

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
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////
    public static void CreatePlayerShip(BasePlayerData playerData, Vector3Int startPos, Vector3Int startRot)
    {
        MapPieceStruct mapData = new MapPieceStruct()
        {
            mapPiece = MapPieceTypes.MapPiece_Corridor_01,
            nodeType = NodeTypes.WorldNode,
            location = startPos,
            rotation = Quaternion.Euler(0, 180, 0),
            direction = playerData.playerID,
            parentNode = WorldManager._WorldContainer.transform
        };

        WorldNode worldNode = WorldBuilder._nodeBuilder.CreateNode<WorldNode>(mapData);
        LocationManager.SetNodeScriptToLocation_CLIENT(startPos, worldNode);

        Dictionary<WorldNode, List<MapNode>> worldNodeAndWrapperNodes = MapNodeBuilder.CreateMapNodes(new List<WorldNode>() { worldNode }, true, playerData.shipSize, playerData.shipMapPieces);

        if (NetWorkManager.NetworkAgent._isLocalPlayer)
        {
            foreach (KeyValuePair<WorldNode, List<MapNode>> element in worldNodeAndWrapperNodes)
            {
                List<MapNode> mapNodes = element.Value;

                foreach (MapNode mapNode in mapNodes)
                {
                    mapNode.ActivateMapPiece(true);
                }
            }
        }

        KeyValuePair<Vector3Int, Vector3Int> posRot = PlayerManager.GetPlayerStartPosition(playerData.playerID);
        worldNode.transform.rotation = Quaternion.Euler(posRot.Value);
    }


}
