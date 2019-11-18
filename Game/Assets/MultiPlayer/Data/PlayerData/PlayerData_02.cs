
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_02 : BasePlayerData {

    public PlayerData_02()
    {
        playerID = 2;
        name = "Fred";
        numUnits = 2;

        allUnitData = new List<UnitStruct>()
        {
            // can Climb walls, unitCombat Stats, starting Local loc
            new UnitStruct()
            {
                UnitModel = 0,
                UnitCanClimbWalls = false,
                UnitCombatStats = new int[2]{ 1, 4 }, // Rank, Movement
                UnitShipLoc = new Vector3(6, -2, 4),
                UnitRot = new Vector3(0, 90, 0),
            },

            new UnitStruct()
            {
                UnitModel = 0,
                UnitCanClimbWalls = false,
                UnitCombatStats = new int[2]{ 0, 4 },
                UnitShipLoc = new Vector3(4, -2, 6),
                UnitRot = new Vector3(0, 0, 0),
            }
        };

        shipSize = new Vector3(1, 1, 3);

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