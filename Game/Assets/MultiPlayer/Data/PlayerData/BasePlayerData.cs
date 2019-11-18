using System.Collections.Generic;
using UnityEngine;

public class BasePlayerData
{
    public int playerID;

    public string name;

    public Vector3 shipSize = new Vector3(0,0,0);

    public List<MapPieceStruct> shipMapPieces = new List<MapPieceStruct>();

    public int numUnits;

    public List<UnitStruct> allUnitData = new List<UnitStruct>(){};

}

