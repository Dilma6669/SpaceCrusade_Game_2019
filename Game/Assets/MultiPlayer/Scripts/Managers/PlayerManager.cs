using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    ////////////////////////////////////////////////

    public static int forcePlayer = 0; // to force a particular player to load rather than than always player 0

    ////////////////////////////////////////////////

    private static PlayerManager _instance;

    ////////////////////////////////////////////////

    private static PlayerAgent _playerAgent;
    private static CameraAgent _cameraAgent;
    private static UnitsAgent _unitsAgent;
    private static NetworkAgent _networkAgent;

    ////////////////////////////////////////////////

    private static BasePlayerData _playerData;

    ////////////////////////////////////////////////

    private static int _playerID = 0;
    private static string _playerName = "???";
    private static int _numUnits = 0;

    private static int _totalPlayers = -1;
    private static int _seed = -1;

    ////////////////////////////////////////////////

    public static PlayerAgent PlayerAgent
    {
        get { return _playerAgent; }
        set { _playerAgent = value; }
    }

    public static CameraAgent CameraAgent
    {
        get { return _cameraAgent; }
        set { _cameraAgent = value; }
    }

    public static UnitsAgent UnitsAgent
    {
        get { return _unitsAgent; }
        set { _unitsAgent = value; }
    }

    public static NetworkAgent NetworkAgent
    {
        get { return _networkAgent; }
        set { _networkAgent = value; }
    }

    ////////////////////////////////////////////////

    public static BasePlayerData PlayerData
    {
        get { return _playerData; }
        set { _playerData = value; }
    }

    ////////////////////////////////////////////////

    public static int PlayerID
    {
        get { return _playerID; }
        set { _playerID = value; }
    }

    public static string PlayerName
    {
        get { return _playerData.name; }
    }

    public static int PlayerNumUnits
    {
        get { return _playerData.numUnits; }
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

    public static int TotalPlayers
    {
        get { return _totalPlayers; }
        set { _totalPlayers = value; }
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

        if (forcePlayer != 0)
        {
            PlayerID = forcePlayer;
        }


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
                return new KeyValuePair<Vector3Int, Vector3Int>(new Vector3Int(-120, 478, 890), new Vector3Int(0, 90, 0));
            case 1:
                return new KeyValuePair<Vector3Int, Vector3Int>(new Vector3Int(10, 478, -890), new Vector3Int(0, 270, 0));
            case 2:
                return new KeyValuePair<Vector3Int, Vector3Int>(new Vector3Int(-954, 480, -70), new Vector3Int(0, 0, 0));
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

    public static void PlayerLoaded()
    {
        LoadPlayersShip();
    }


    public static void LoadPlayersShip()
    {
        KeyValuePair<Vector3Int, Vector3Int> playerPosRot = PlayerManager.GetPlayerStartPosition(PlayerID);
        PlayerManager.NetworkAgent.CmdTellServerToSpawnPlayersShip(PlayerAgent.NetworkInstanceID, playerPosRot.Key, playerPosRot.Value, PlayerID); // Create the Network Node
    }

    public static void PlayersShipLoaded()
    {
        LoadPlayersUnits();
    }


    public static void LoadPlayersUnits()
    {
       UnitsManager.LoadPlayersUnits();
    }

    public static void PlayersUnitsLoaded()
    {
       
    }

}
