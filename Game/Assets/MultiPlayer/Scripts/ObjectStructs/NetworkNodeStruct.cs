using UnityEngine;
using UnityEngine.Networking;


// This might need to be seriously tidyied up if the new internet movement thing works
public struct NetworkNodeStruct
{
    public Vector3 NodeID;
    public int StructIndex;
    public NetworkInstanceId ClientNetID;
    public Vector3 CurrLoc;
    public Vector3 CurrRot;
    public int PlayerID;
    public UnitStruct unitData;
}