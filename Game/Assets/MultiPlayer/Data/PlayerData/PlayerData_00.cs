
using System.Collections.Generic;
using UnityEngine;

public class PlayerData_00 : BasePlayerData
{

    public PlayerData_00()
    {
        playerID = 0;
        name = "Kate";
        numUnits = 2;


        allUnitData = new List<UnitStruct>()
        {
            // can Climb walls, unitCombat Stats, starting Local loc
            new UnitStruct()
            {
                UnitModel = 0,
                UnitCanClimbWalls = true,
                UnitCombatStats = new int[2]{ 1, 4 }, // Rank, Movement
                UnitShipLoc = new Vector3Int(-8, -10, -8),
                UnitRot = new Vector3Int(0, 0, 0),
            },

            new UnitStruct()
            {
                UnitModel = 0,
                UnitCanClimbWalls = true,
                UnitCombatStats = new int[2]{ 0, 4 },
                UnitShipLoc = new Vector3Int(-8, -10, -6),
                UnitRot = new Vector3Int(0, 0, 0),
            }
        };


        shipMapPieces = new Dictionary<int, int[]>()
        {
            // LocID            Maptype                                Rotation  SpinForGravity other info
           // { 122 , new int[] { (int)MapPieceTypes.Simple_Room_Doors,    0,         1,           0,    0 } },
           // { 203 , new int[] { (int)MapPieceTypes.Simple_Room_Doors,    0,         1,           0,    0 } },
            { 284 , new int[] { (int)MapPieceTypes.Simple_Room_Doors,    0,         1,           0,    0 } },
            { 356 , new int[] { (int)MapPieceTypes.Simple_Room_Doors,    0,         0,           0,    0 } },
            { 365 , new int[] { (int)MapPieceTypes.Simple_Room_Doors,    0,         1,           0,    0 } },
            { 374 , new int[] { (int)MapPieceTypes.Simple_Room_Doors,    0,         0,           0,    0 } },
            { 446 , new int[] { (int)MapPieceTypes.Simple_Room_Doors,    0,         1,           0,    0 } },
           // { 527 , new int[] { (int)MapPieceTypes.Simple_Room_Doors,    0,         1,           0,    0 } },
           // { 608 , new int[] { (int)MapPieceTypes.Simple_Room_Doors,    0,         1,           0,    0 } },
        };
    }
}