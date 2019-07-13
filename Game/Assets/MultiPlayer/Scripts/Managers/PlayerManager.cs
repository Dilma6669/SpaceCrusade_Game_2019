using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static PlayerManager _instance;

    ////////////////////////////////////////////////

    private static PlayerAgent _playerAgent;
    private static BasePlayerData _playerData;

    ////////////////////////////////////////////////

    private static int _playerID = 0;
    private static string _playerName = "???";
    private static int _totalPlayers = -1;
    private static int _seed = -1;

    ////////////////////////////////////////////////

    public static PlayerAgent PlayerAgent
    {
        get { return _playerAgent; }
        set { _playerAgent = value; }
    }

    public static BasePlayerData PlayerData
    {
        get { return _playerData; }
        set { _playerData = value; }
    }

    public static int PlayerID
    {
        get { return _playerID; }
        set { _playerID = value; }
    }

    public static int TotalPlayers
    {
        get { return _totalPlayers; }
        set { _totalPlayers = value; }
    }

    public static string PlayerName
    {
        get { return _playerData.name; }
    }

    public static List<UnitStruct> PlayerUnitData
    {
        get { return _playerData.allUnitData; }
    }

    public static Vector3 PlayerShipSize
    {
        get { return _playerData.shipSize; }
    }

    public static List<MapPieceStruct> PlayerShipMapPieces
    {
        get { return _playerData.shipMapPieces; }
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
    ////////////////////////////////////////////////

    public static void SetUpPlayer()
    {
        SyncedVars _syncedVars = GameObject.Find("SyncedVars").GetComponent<SyncedVars>(); // needs to be here, function runs before awake
        if (_syncedVars == null) { Debug.LogError("We got a problem here"); }

        PlayerID = _syncedVars.PlayerCount;
        PlayerData = GetPlayerData(PlayerID);
    }


    public static BasePlayerData GetPlayerData(int playerID)
    {
        BasePlayerData data = null;

        switch (playerID)
        {
            case 0:
                data = new PlayerData_00();
                break;
            case 1:
                data = new PlayerData_01();
                break;
            case 2:
                data = new PlayerData_02();
                break;
            case 3:
                data = new PlayerData_03();
                break;
            default:
                Debug.Log("SOMETHING WENT WRONG HERE: playerID: " + playerID);
                break;
        }
        return data;
    }

    public static KeyValuePair<Vector3Int, Vector3Int> GetPlayerStartPosition(int playerID)
    {
        switch (playerID)
        {
            case 0:
                return new KeyValuePair<Vector3Int, Vector3Int>(new Vector3Int(-124, 475, 895), new Vector3Int(0, 90, 0));
            case 1:
                return new KeyValuePair<Vector3Int, Vector3Int>(new Vector3Int(11, 572, -879), new Vector3Int(0, 270, 0));
            case 2:
                return new KeyValuePair<Vector3Int, Vector3Int>(new Vector3Int(-955, 489, -71), new Vector3Int(0, 0, 0));
            case 3:
                return new KeyValuePair<Vector3Int, Vector3Int>(new Vector3Int(738, 344, -210), new Vector3Int(0, 180, 0));
            default:
                Debug.Log("SOMETHING WENT WRONG HERE: playerID: " + playerID);
                return new KeyValuePair<Vector3Int, Vector3Int>(new Vector3Int(0, 0, 0), new Vector3Int(0, 0, 0));
        }
    }


    public static Vector3 GetTESTShipMovementPositions(int playerID)
    {
        switch (playerID)
        {
            case 0:
                return new Vector3(600, 700, 0);
            case 1:
                return new Vector3(-600, 700, 0);
            case 2:
                return new Vector3(0, 700, 600);
            case 3:
                return new Vector3(0, 700, -600);
            default:
                Debug.Log("SOMETHING WENT WRONG HERE: playerID: " + playerID);
                return Vector3.zero;
        }
    }


    public static void LoadPlayersShip(int playerID)
    {
        KeyValuePair<Vector3Int, Vector3Int> playerPosRot = PlayerManager.GetPlayerStartPosition(playerID);

        NetWorkManager.NetworkAgent.CmdTellServerToSpawnShipWorldNodeOnClients(playerID, playerPosRot.Key);

        NetWorkManager.NetworkAgent.CmdTellServerToSpawnNetworkNodeContainer(playerPosRot.Key);

        Vector3 testLocVect = GetTESTShipMovementPositions(playerID);
        Vector3 testRotVect = new Vector3(0, 0, 0);
        float thrust = 5;
        NetWorkManager.NetworkAgent.CmdTellServerToMoveWorldNode(playerPosRot.Key, testLocVect, testRotVect, thrust);
    }
}
