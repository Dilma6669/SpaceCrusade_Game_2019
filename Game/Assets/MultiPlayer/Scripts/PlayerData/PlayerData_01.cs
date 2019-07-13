
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_01 : BasePlayerData
{

    public PlayerData_01()
    {
        playerID = 1;
        name = "Boris";
        numUnits = 2;

        allUnitData = new List<UnitStruct>()
        {
            // can Climb walls, unitCombat Stats, starting Local loc
            new UnitStruct(){
                UnitModel = 0,
                UnitCanClimbWalls = true,
                UnitCombatStats = new int[2]{ 1, 4 }, // Rank, Movement
                UnitStartingLocalLoc = new Vector3(0, -7, 2)
            },
            new UnitStruct(){
                UnitModel = 0,
                UnitCanClimbWalls = true,
                UnitCombatStats = new int[2]{ 0, 4 },
                UnitStartingLocalLoc = new Vector3(0, -7, 4)
            }
        };

        shipSize = new Vector3Int(1, 1, 3);

        shipMapPieces = new List<MapPieceStruct>()
        {
            new MapPieceStruct()
            {
                nodeType = NodeTypes.MapNode,
                mapPiece = MapPieceTypes.MapPiece_Corridor_01
            },
            new MapPieceStruct()
            {
                nodeType = NodeTypes.MapNode,
                mapPiece = MapPieceTypes.MapPiece_Corridor_02
            },
            new MapPieceStruct()
            {
                nodeType = NodeTypes.MapNode,
                mapPiece = MapPieceTypes.MapPiece_Corridor_02
            },
            new MapPieceStruct()
            {
                nodeType = NodeTypes.MapNode,
                mapPiece = MapPieceTypes.MapPiece_Corridor_02
            },
            new MapPieceStruct()
            {
                nodeType = NodeTypes.MapNode,
                mapPiece = MapPieceTypes.MapPiece_Corridor_02
            },
            new MapPieceStruct()
            {
                nodeType = NodeTypes.MapNode,
                mapPiece = MapPieceTypes.MapPiece_Corridor_01
            },
        };
        /////////////////////////////////////////////////////////////////
    }
}