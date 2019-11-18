using UnityEngine;

public struct MapPieceStruct
{
    public Vector3 mapID;
    public NodeTypes nodeType;
    public MapPieceTypes mapPiece;
    public Vector3 location;
    public Vector3 rotation;
    public int direction;
    public Transform parentNode;
}