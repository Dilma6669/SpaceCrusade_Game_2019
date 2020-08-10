using System.Collections.Generic;

public class WorldNodeData_1X1 : BaseWorldNodeData
{
    public WorldNodeData_1X1()
    {
        worldNodeMapPieces = new Dictionary<int, int[]>()
        {
           // LocID            Maptype                                        Rotation  other info
            { 284 , new int[] { (int)MapPieceTypes.ConnectorPiece_Ver_Empty,    0,      0,0 } },
            { 356 , new int[] { (int)MapPieceTypes.ConnectorPiece_Hor_Empty,    0,      0,0 } },
            { 364 , new int[] { (int)MapPieceTypes.ConnectorPiece_Hor_Empty,    90,     0,0 } },
            { 365 , new int[] { (int)MapPieceTypes.MapPiece_Room_Random,        0,      0,0 } },
            { 366 , new int[] { (int)MapPieceTypes.ConnectorPiece_Hor_Empty,    90,     0,0 } },
            { 374 , new int[] { (int)MapPieceTypes.ConnectorPiece_Hor_Empty,    0,      0,0 } },
            { 446 , new int[] { (int)MapPieceTypes.ConnectorPiece_Ver_Empty,    0,      0,0 } },
        };
    }
}