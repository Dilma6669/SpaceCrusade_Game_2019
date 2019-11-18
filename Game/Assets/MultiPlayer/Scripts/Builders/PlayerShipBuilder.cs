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
    public static void CreatePlayerShip(NetworkNodeStruct networkNodeStruct, int playerID)
    {
        MapPieceStruct mapData = new MapPieceStruct()
        {
            mapPiece = MapPieceTypes.MapPiece_Corridor_01,
            nodeType = NodeTypes.WorldNode,
            location = networkNodeStruct.CurrLoc,
            rotation = networkNodeStruct.CurrRot,
            direction = 0,
            parentNode = WorldManager._WorldContainer.transform
        };

        WorldNode worldNode = WorldBuilder._nodeBuilder.CreateNode<WorldNode>(mapData);

        LocationManager.SaveNodeTo_CLIENT(networkNodeStruct.NodeID, worldNode);

        BasePlayerData playerData = PlayerManager.GetPlayerData(playerID);

        Debug.Log("fuck worldNode >>>>> " + worldNode.transform.position);

        worldNode.transform.eulerAngles = new Vector3(0, 90, 0);

        Dictionary<WorldNode, List<MapNode>> worldNodeAndWrapperNodes = MapNodeBuilder.CreateMapNodes(new List<WorldNode>() { worldNode }, true, playerData.shipSize, playerData.shipMapPieces);

        //if (playerID == PlayerManager.PlayerID)
        //{
            foreach (KeyValuePair<WorldNode, List<MapNode>> element in worldNodeAndWrapperNodes)
            {
                List<MapNode> mapNodes = element.Value;

                foreach (MapNode mapNode in mapNodes)
                {
                    mapNode.ActivateMapPiece(true);
                }
            }
        //}

        //worldNode.transform.SetParent(networkNode.transform);
        //worldNode.transform.position = networkNode.transform.position;
        //worldNode.transform.localEulerAngles = new Vector3(0, 0, 0);
        worldNode.GetComponent<WorldNode>().NetworkNodeID = networkNodeStruct.NodeID;

        if (playerData.playerID == PlayerManager.PlayerID)
        {
            MoveShip(playerData, worldNode);
            PlayerManager.PlayersShipLoaded();
        }
    }


    public static void MoveShip(BasePlayerData playerData, WorldNode worldNode)
    {
        Vector3 testLocVect = PlayerManager.GetTESTShipMovementPositions(playerData.playerID);
        Vector3 testRotVect = Vector3.zero;
        float thrust = 5;

        worldNode.MakeNodeMoveToLoc(testLocVect, testRotVect, true);
    }

}
