using UnityEngine;
using UnityEngine.Networking;

public struct NetworkNodeStruct
{
    public Vector3 NodeID;
    public int StructIndex;
    public NetworkInstanceId ClientNetID;
    public int PlayerID;
    public Vector3 CurrLoc;
    public Vector3 CurrRot;
    public UnitStruct UnitData;
}