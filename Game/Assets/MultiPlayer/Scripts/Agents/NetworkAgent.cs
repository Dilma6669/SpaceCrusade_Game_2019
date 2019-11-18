using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkAgent : NetworkBehaviour
{
    /////////////////////////////////////////////////////

    SyncedVars _syncedVars = null;

    public bool _isLocalPlayer = false;

    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////

    // Use this for initialization
    void Awake()
    {
        if (!isServer) return;

        if (!isLocalPlayer) return;
        _isLocalPlayer = true;

        PlayerManager.NetworkAgent = this;
    }

    // Need this Start()
    void Start()
    {
        if (!isLocalPlayer) return;
        _isLocalPlayer = true;

        PlayerManager.NetworkAgent = this;
    }

    /////////////////////////////////////////
    // HELPER FUNCIONS:
    /////////////////////////////////////////

    private NetworkConnection TargetSpecificClient(NetworkInstanceId clientNetID)
    {
        return ClientScene.FindLocalObject(clientNetID).GetComponent<NetworkIdentity>().connectionToClient;
    }


    private NetworkInstanceId GetNetworkObjectInstanceID(GameObject networkNode)
    {
        return networkNode.GetComponent<NetworkIdentity>().netId;
    }

    private void CheckSyncVars()
    {
        if (_syncedVars == null)
        {
            _syncedVars = GameObject.Find("SyncedVars").GetComponent<SyncedVars>(); // needs to be here, function runs before awake
        }
        if (_syncedVars == null) { Debug.LogError("We got a problem here"); }
    }



    /////////////////////////////////////////
    // TELL SERVER: CREATE NEW PLAYER OBJECT + ADD NEW PLAYER COUNT TO OTHER CLIENTS
    /////////////////////////////////////////

    [Command]
    public void CmdAddPlayerToSession(NetworkInstanceId clientNetID)
    {
        CheckSyncVars();

        RpcUpdatePlayerCountOnClient(_syncedVars.PlayerCount + 1);
    }

    [ClientRpc]
    void RpcUpdatePlayerCountOnClient(int count)
    {
        GetComponent<PlayerAgent>().UpdatePlayerCount(count);
    }





    /////////////////////////////////////////
    // TELL SERVER: CREATE NETWORK NODE FOR WORLD NODE (SHIP TO MOVE TOWARDS)
    /////////////////////////////////////////

    [Command]
    public void CmdTellServerToInitializeLists()
    {
        CheckSyncVars();
    }

    [Command]
    public void CmdTellServerToSpawnPlayersShip(NetworkInstanceId clientNetID, Vector3 loc, Vector3 rot, int playerID) // this player ID is dumb and needs the player ship info (thats all its here for) to be fed in here rather than pulled from a script on the clients machines, beacuse that cant happen if people are joining games on the fly
    {
        if (!isServer) return;

        CheckSyncVars();

        NetworkNodeStruct nodeStruct = new NetworkNodeStruct()
        {
            NodeID = loc,
            StructIndex = _syncedVars._network_Ships_Indexs.Count,
            ClientNetID = clientNetID,
            PlayerID = playerID,
            CurrLoc = loc,
            CurrRot = rot
        };

        // tell all clients to load new ship
        RpcTellAllClientsToSpawnPlayerShip(nodeStruct, playerID);

        // now load old SHIPS into this client
        foreach (NetworkNodeStruct node in _syncedVars._network_Ships_Container)
        {
            NetworkConnection netConnect = TargetSpecificClient(clientNetID);
            TargetTellClientToSpawnExistingShips(netConnect, node);
        }

        // AND now load old UNITS into this client
        foreach (NetworkNodeStruct unit in _syncedVars._network_Units_Container)
        {
            TargetTellClientToLoadExistingUnits(TargetSpecificClient(clientNetID), unit);
        }

        // two lists, first is the NetworkNodeIDIndex that links to the second nodestruct, basicly a shitty dictionary, to get the list index to 'then' use to get the nodestruct
        _syncedVars._network_Ships_Indexs.Add(DataManipulation.ConvertVectorIntoInt(nodeStruct.NodeID));
        _syncedVars._network_Ships_Container.Add(nodeStruct);
    }


    [ClientRpc]
    void RpcTellAllClientsToSpawnPlayerShip(NetworkNodeStruct nodeStruct, int playerID)
    {
        TellClientToSpawnOtherClientShip(nodeStruct, playerID);
    }
    [TargetRpc]
    public void TargetTellClientToSpawnExistingShips(NetworkConnection target, NetworkNodeStruct nodeStruct)
    {
        TellClientToSpawnOtherClientShip(nodeStruct, nodeStruct.PlayerID);
    }


    private void TellClientToSpawnOtherClientShip(NetworkNodeStruct nodeStruct, int playerID)
    {
        PlayerShipBuilder.CreatePlayerShip(nodeStruct, playerID);
    }

    ////////////////////////////////////////
    /////////////////////////////////////////

    [Command]
    public void CmdTellServerToUpdateWorldNodePosition(NetworkInstanceId clientNetID, Vector3 networkNodeIndex, Vector3 locPos, Vector3 locRot)
    {
        if (!isServer) return;

        CheckSyncVars();
        int index = _syncedVars._network_Ships_Indexs.IndexOf(DataManipulation.ConvertVectorIntoInt(networkNodeIndex));
        NetworkNodeStruct nodeStruct = _syncedVars._network_Ships_Container[index]; // this is not good and will be horribly innefficient
        nodeStruct.CurrLoc = locPos;
        nodeStruct.CurrRot = locRot;

        // tell all clients to move ship
        RpcTellOtherClientsToMoveClientShip(clientNetID, nodeStruct);
    }
    [ClientRpc]
    void RpcTellOtherClientsToMoveClientShip(NetworkInstanceId clientIDWhoOwnsShip, NetworkNodeStruct nodeStruct)
    {
        if (clientIDWhoOwnsShip != PlayerManager.PlayerAgent.NetworkInstanceID) // to stop sending data to the clients OWN ship thats already moving being told to move
        {
            if (LocationManager.GetNodeFrom_Client(nodeStruct.NodeID) != null)
            {
                BaseNode worldNode = LocationManager.GetNodeFrom_Client(nodeStruct.NodeID);

                float dist = Vector3.Distance(worldNode.transform.position, nodeStruct.CurrLoc);
                if (dist > 10f)
                {
                    worldNode.transform.position = nodeStruct.CurrLoc;
                    worldNode.transform.localEulerAngles = nodeStruct.CurrRot;
                }

                worldNode.MakeNodeMoveToLoc(nodeStruct.CurrLoc, nodeStruct.CurrRot, false);
            }
        }
    }

    /////////////////////////////////////////
    // TELL SERVER: SPAWN SINGLE PLAYER UNIT + ASSIGN PROPERTIES TO CLIENTS COPY OF UNIT
    /////////////////////////////////////////

    [Command]
    public void CmdTellServerToSpawnPlayerUnit(NetworkInstanceId clientNetID, UnitStruct unitData, int playerID)
    {
        if (!isServer) return;

        // figure out what Cube to put unit in
        KeyValuePair<Vector3Int, Vector3Int> playerPosRot = PlayerManager.GetPlayerStartPosition(playerID);
        Vector3Int unitShipLoc = new Vector3Int((int)unitData.UnitShipLoc.x, (int)unitData.UnitShipLoc.y, (int)unitData.UnitShipLoc.z);

        Vector3Int unitStartLoc = new Vector3Int((unitShipLoc.x + playerPosRot.Key.x), (unitShipLoc.y + playerPosRot.Key.y), (unitShipLoc.z + playerPosRot.Key.z)); // This is nessicary, We need to get the cube ID
        unitData.UnitStartingNodeID = unitStartLoc;

        NetworkNodeStruct nodeStruct = new NetworkNodeStruct()
        {
            NodeID = unitStartLoc,
            StructIndex = _syncedVars._network_Units_Indexs.Count,
            ClientNetID = clientNetID,
            PlayerID = playerID,
            CurrLoc = Vector3.zero,
            CurrRot = unitData.UnitRot,
            unitData = unitData
        };

        // tell all clients to load new unit
        RpcTellAllClientsToSpawnPlayerUnit(nodeStruct, playerID);

        // two lists, first is the NetworkNodeIDIndex that links to the second nodestruct, basicly a shitty dictionary, to get the list index to 'then' use to get the nodestruct
        _syncedVars._network_Units_Indexs.Add(DataManipulation.ConvertVectorIntoInt(nodeStruct.NodeID));
        _syncedVars._network_Units_Container.Add(nodeStruct);
    }


    [ClientRpc]
    void RpcTellAllClientsToSpawnPlayerUnit(NetworkNodeStruct nodeStruct, int playerID)
    {
        TellClientToLoadOtherClientUnit(nodeStruct, playerID);
    }
    [TargetRpc]
    public void TargetTellClientToLoadExistingUnits(NetworkConnection target, NetworkNodeStruct nodeStruct)
    {
        TellClientToLoadOtherClientUnit(nodeStruct, nodeStruct.PlayerID);
    }

    private void TellClientToLoadOtherClientUnit(NetworkNodeStruct nodeStruct, int playerID)
    {
        UnitsManager.CreateUnitOnClient(nodeStruct, playerID);
    }


    ////////////////////////////////////////
    /////////////////////////////////////////

    [Command]
    public void CmdTellServerToUpdateUnitPosition(NetworkInstanceId clientNetID, Vector3 nodeID, int networkNodeIndex, Vector3 locPos, Vector3 locRot)
    {
        if (!isServer) return;

        CheckSyncVars();
        NetworkNodeStruct nodeStruct = _syncedVars._network_Units_Container[networkNodeIndex];
        nodeStruct.CurrLoc = locPos;
        nodeStruct.CurrRot = locRot;

        // tell all clients to move unit
        RpcTellOtherClientsToMoveUnit(clientNetID, nodeStruct);
    }
    [ClientRpc]
    void RpcTellOtherClientsToMoveUnit(NetworkInstanceId clientIDWhoOwnsUnit, NetworkNodeStruct nodeStruct)
    {
        if (clientIDWhoOwnsUnit != PlayerManager.PlayerAgent.NetworkInstanceID) // to stop sending data to the clients OWN ship thats already moving being told to move
        {
            //GameObject unit = LocationManager.GetUnitFrom_Client(nodeStruct.NodeID);

            //float dist = Vector3.Distance(unit.transform.position, nodeStruct.CurrLoc);
            //if (dist > 10f)
            //{
            //    unit.transform.position = nodeStruct.CurrLoc;
            //    unit.transform.localEulerAngles = nodeStruct.CurrRot;
            //}

            //unit.MakeNodeMoveToLoc(nodeStruct.CurrLoc, nodeStruct.CurrRot, false); this needs to be made, using the ships lerp thingy
        }
    }


    ////////////////////////////////////////////////
    ///
}

