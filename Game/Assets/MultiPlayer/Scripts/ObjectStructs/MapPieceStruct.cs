using UnityEngine;

public struct MapPieceStruct
{
    public NodeTypes nodeType;
    public MapPieceTypes mapPiece;
    public Vector3Int location;
    public Quaternion rotation;
    public int direction;
    public Transform parentNode;
}