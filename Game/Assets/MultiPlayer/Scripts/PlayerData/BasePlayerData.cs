using System.Collections.Generic;
using UnityEngine;

public class BasePlayerData
{
    public int playerID;

    public string name;

    public Vector3Int shipSize = new Vector3Int(0,0,0);

    public List<MapPieceStruct> shipMapPieces = new List<MapPieceStruct>();

    public int numUnits;

    public List<UnitStruct> allUnitData = new List<UnitStruct>(){};

}

