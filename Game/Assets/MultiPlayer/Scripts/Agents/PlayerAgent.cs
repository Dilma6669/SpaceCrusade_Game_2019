using UnityEngine;
using UnityEngine.Networking;


public class PlayerAgent : NetworkBehaviour
{
    ////////////////////////////////////////////////

    NetworkInstanceId _networkID;
    int _totalPlayers = -1;

    ////////////////////////////////////////////////

    public NetworkInstanceId NetworkInstanceID
    {
        get { return _networkID; }
        set { _networkID = value; }
    }

    public int TotalPlayers
    {
        get { return _totalPlayers; }
        set { _totalPlayers = value; }
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    // Use this for initialization
    void Awake()
    {
    }

    // Need this Start()
    void Start()
    {
        transform.SetParent(GameManager._PlayerManager.transform);

        if (!isLocalPlayer) return;
        PlayerManager.PlayerAgent = this;
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    public override void OnStartLocalPlayer()
    {
        Start();
        if (!isLocalPlayer) return;
        Debug.Log("A network Player object has been created");

        PlayerManager.PlayerAgent = GetComponent<PlayerAgent>();
        PlayerManager.CameraAgent = GetComponent<CameraAgent>();
        PlayerManager.UnitsAgent = GetComponent<UnitsAgent>();
        PlayerManager.NetworkAgent = GetComponent<NetworkAgent>();

        PlayerManager.SetUpPlayer();

        NetworkInstanceID = GetComponent<NetworkIdentity>().netId;
        GetComponent<NetworkAgent>().CmdAddPlayerToSession(NetworkInstanceID);

        UIManager.SetUpPlayersGUI(PlayerManager.PlayerID);
        CameraManager.SetUpCameraAndLayers(PlayerManager.PlayerID, GetComponent<CameraAgent>());

        WorldManager.BuildWorldForClient();
    }


    public void UpdatePlayerCount(int count)
    {
        TotalPlayers = count;
        UIManager.UpdateTotalPlayersGUI(count);

        if (!isLocalPlayer) return;

        PlayerManager.PlayerLoaded();
    }

    public void SetUpPlayerStartPosition(Vector3 camPos, Quaternion camRot)
    {
        transform.position = camPos;
        transform.rotation = camRot;
    }


    //////////////

}
