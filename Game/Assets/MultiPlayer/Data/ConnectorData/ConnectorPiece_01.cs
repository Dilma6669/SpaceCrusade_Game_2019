﻿public class ConnectorPiece_01
{
    void Awake()
    {

        //		case 0: Empty
        //		case 1: Floor
        //		case 2: Left
        //		case 3: Front
        //		case 4: Right
        //		case 5: Back
        //		case 6: Ceiling
        //		case 7: Floor Left Back
        //		case 8: Floor Left
        //		case 9: Floor Left Front
        //		case 10: Floor front
        //		case 11: Floor Front Right
        //		case 12: Floor Right 
        //		case 13: Floor Right Back
        //		case 14: Floor Back
        //		case 15: Left Back
        //		case 16: Left Front
        //		case 17: Front Right
        //		case 18: Right Back


        // LOOK AT THESE AS UNDERNEATH MAPS
        // First Floor
        //floors.Add(new int[,] {
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 01, 00, 01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }
        //});
        //// third Floor
        //floors.Add(new int[,] {
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 05, 00, 00, 00, 00, 00, 04, 02, 00, 00, 00, 00, 00, 00, 00 }
        //});
        //// fourth Floor
        //floors.Add(new int[,] {
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
        //});
        //// third Floorw
        //floors.Add(new int[,] {
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00, 02, 00, 00, 00, 00, 00, 00, 00 }
        //});
        //// fourth Floor
        //floors.Add(new int[,] {
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 }
        //});
        //// third Floor
        //floors.Add(new int[,] {
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00 },
        //    { 00, 00, 00, 00, 00, 00, 00, 02, 09, 00, 00, 00, 00, 00, 08, 02, 00, 00, 00, 00, 00, 00, 00 }
        //});
    }
}
