using UnityEngine;
using UnityEngine.Networking;

public class SyncedVars : NetworkBehaviour
{
    /////////////////////////////////////////////////////////////////////
    ////////////////// VERY BIG IMPORTANT NOTE //////////////////////////
    /////////////////////////////////////////////////////////////////////
    /// All syncVars syncLists need to be in here 
    /// ( I think because its an object thats in scene and not on the player object )
    /// But Im not sure but seems to be working....
    /////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////

    [SyncVar]
	public int _globalSeed = -1;

    [SyncVar]
    public int _playerCount = -1;

    ////////////////////////////////////

    public class SyncListNetworkNode : SyncListStruct<NetworkNodeStruct> { }

    public SyncListInt _network_Ships_Indexs = new SyncListInt();
    public SyncListNetworkNode _network_Ships_Container = new SyncListNetworkNode();

    /////////////////////////////////////////////////////////////////////

    public SyncListInt _network_Units_Indexs = new SyncListInt();
    public SyncListNetworkNode _network_Units_Container = new SyncListNetworkNode();

    /////////////////////////////////////////////////////////////////////

    public int GlobalSeed
	{
		get { return _globalSeed; }
		set { _globalSeed = value; }
	}

	public int PlayerCount
	{
		get { return _playerCount; }
		set { _playerCount = _playerCount + value; }
	}

    /////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////

    void Start()
    {
        //if (isServer)
        //{
        //    for (int i = 0; i < 20; i++)
        //    {
        //        _network_Nodes_Container.Add(new NetworkNodeStruct());
        //    }
        //}
    }

}
