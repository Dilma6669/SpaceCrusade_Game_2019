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
    public static void CreatePlayerShip(NetworkNodeStruct nodeStruct, int playerID)
    {
        Vector3Int nodeID = new Vector3Int(Mathf.FloorToInt(nodeStruct.NodeID.x), Mathf.FloorToInt(nodeStruct.NodeID.y), Mathf.FloorToInt(nodeStruct.NodeID.z));

        WorldNodeStruct worldNodeData = new WorldNodeStruct()
        {
            NodeID = nodeID,
            CurrLoc = new Vector3Int(Mathf.FloorToInt(nodeStruct.CurrLoc.x), Mathf.FloorToInt(nodeStruct.CurrLoc.y), Mathf.FloorToInt(nodeStruct.CurrLoc.z)),
            CurrRot = new Vector3Int(Mathf.FloorToInt(nodeStruct.CurrRot.x), Mathf.FloorToInt(nodeStruct.CurrRot.y), Mathf.FloorToInt(nodeStruct.CurrRot.z)),
        };

        WorldNode worldNode = WorldBuilder._nodeBuilder.CreateWorldNode(worldNodeData);

        LocationManager.SaveNodeTo_CLIENT(nodeID, worldNode);

        BasePlayerData playerData = PlayerManager.GetPlayerData(playerID);

        worldNode.transform.eulerAngles = new Vector3Int(0, 90, 0);

        worldNode.NodeData.worldNodeMapPieces = playerData.shipMapPieces;

        MapNodeBuilder.CreateMapNodesForWorldNode(worldNode);

        Dictionary<Vector3Int, WorldNode> dict = new Dictionary<Vector3Int, WorldNode>();
        dict.Add(nodeID, worldNode);
        //LayerManager.AssignLayerCountsToWorldAndMapNodes(dict);

        //if (playerID == PlayerManager.PlayerID)
        //{
        foreach (MapNode mapNode in worldNode.mapNodes)
        {
            mapNode.ActivateMapPiece(true);
        }
        //}

        if (playerData.playerID == PlayerManager.PlayerID)
        {
           MoveShip(playerData, worldNode);

            worldNode.transform.Find("RotationalNode").GetComponent<RotationalNode>()._rotate = true;
            PlayerManager.PlayersShipLoaded();
        }
    }


    public static void MoveShip(BasePlayerData playerData, WorldNode worldNode)
    {
        Vector3Int testLocVect = PlayerManager.GetTESTShipMovementPositions(playerData.playerID);
        Vector3Int testRotVect = Vector3Int.zero;
        //float thrust = 50;

        worldNode.MakeNodeMoveToLoc(testLocVect, testRotVect, true);

    }

}
