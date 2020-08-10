using UnityEngine;
using UnityEngine.Networking;

// This might need to be seriously tidyied up if the new internet movement thing works
public struct WorldNodeStruct
{
    public Vector3 NodeID; // this is the beginng Global location of the Node
    public int StructIndex;
    public NetworkInstanceId ClientNetID;
    public Vector3 CurrLoc;
    public Vector3 CurrRot;
    public int PlayerID;
}