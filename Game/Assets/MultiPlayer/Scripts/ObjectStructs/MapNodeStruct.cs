using UnityEngine;

public struct MapNodeStruct
{
    public Vector3Int NodeID;
    public NodeTypes nodeType; // Not sure if need this either map piece or connector piece
    public MapPieceTypes mapPiece;
    public Vector3Int location; // This is the localPosition relative to the world node this map is sittin inside
    public Vector3Int rotation; // for the connector pieces
    public Transform parentNode;
    public int spinny; // if mapnode follows gravitational spin of spinny node
}