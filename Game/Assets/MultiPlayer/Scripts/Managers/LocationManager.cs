using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static LocationManager _instance;

    ////////////////////////////////////////////////
    // SERVER

    ////////////////////////////////////////////////
    // CLIENT
    public static Dictionary<Vector3Int, CubeLocationScript> _LocationLookup = new Dictionary<Vector3Int, CubeLocationScript>();   // movable locations
    public static Dictionary<Vector3Int, CubeLocationScript> _LocationHalfLookup = new Dictionary<Vector3Int, CubeLocationScript>(); // not moveable locations BUT important for neighbour system

    public static Dictionary<Vector3Int, CubeLocationScript> _unitLocation = new Dictionary<Vector3Int, CubeLocationScript>(); // sever shit

    public static Dictionary<Vector3Int, BaseNode> _CLIENT_nodeLookup = new Dictionary<Vector3Int, BaseNode>();

    ////////////////////////////////////////////////

    private static CubeLocationScript _activeCube = null; // hmmm dont know if should be here

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    ////////////////////////////////////////////////
    ////// SERVER FUNCTIONS ////////////////////////
    ////////////////////////////////////////////////


    ////////////////////////////////////////////////
    ////// CLIENT FUNCTIONS ////////////////////////
    ////////////////////////////////////////////////

    public static void SetCubeScriptToLocation_CLIENT(Vector3Int vect, CubeLocationScript script)
    {
        if (!_LocationLookup.ContainsKey(vect))
        {
            //if(vect == new Vector3(-128, 468, 882))
            //Debug.Log("Adding FULL normalscript to vect: " + vect);
            _LocationLookup.Add(vect, script);
            return;
        }
        // Debug.LogError("trying to assign script to already taking location!!!"); // this now happens all the time because maps overlap eachother with new map system
    }

    public static void SetCubeScriptToHalfLocation_CLIENT(Vector3Int vect, CubeLocationScript script)
    {
        if (!_LocationHalfLookup.ContainsKey(vect))
        {
             //Debug.Log("Adding HALF normalscript to vect: " + vect);
            _LocationHalfLookup.Add(vect, script);
            return;
        }
        //Debug.LogError("trying to assign script to already taking location!!!"); // this now happens all the time because maps overlap eachother with new map system
    }

    ////////////////////////////////////////////////

    public static CubeLocationScript GetLocationScript_CLIENT(Vector3Int loc)
    {
        //Debug.Log("GetLocationScript");

        if (_LocationLookup.ContainsKey(loc))
        {
            return _LocationLookup[loc];
        }
        //Debug.LogError("LOCATION DOSENT EXIST Loc: " + loc);
        //Debug.LogError("_LocationLookup.Count: " + _LocationLookup.Count);
        return null;
    }

    public static CubeLocationScript GetHalfLocationScript_CLIENT(Vector3Int loc)
    {
        //Debug.Log("GetLocationScript");

        if (_LocationHalfLookup.ContainsKey(loc))
        {
            return _LocationHalfLookup[loc];
        }
        //Debug.LogError("LOCATION DOSENT EXIST Loc: " + loc);
        return null;
    }

    ////////////////////////////////////////////////

    private static bool debugStatements = false; //   turn this on to have fail attempts on move to
    public static CubeLocationScript CheckIfCanMoveToCube_CLIENT(CubeLocationScript node, Vector3Int neighloc, bool unitIsAlien, bool recursiveCheck = false)
    {
        //Debug.Log("CheckIfCanMoveToCube_CLIENT loc: " + neighloc);

        CubeLocationScript neighCubeScript = GetLocationScript_CLIENT(neighloc);

        if (neighCubeScript == null)
        {
            if (debugStatements) { Debug.LogError("FAIL move cubeScript == null: " + neighloc); };
            return null;
        }

        //if (neighCubeScript.CubeIsSlope)
        //{
        //    if (debugStatements) { Debug.LogError("FAIL move neighCubeScript._isSlope: " + neighloc); };
        //    return null;
        //}

        if (neighCubeScript.CubeOccupied)
        {
            if (debugStatements) { Debug.LogError("FAIL move Cube is Occupied at vect:" + neighloc); };
            return null;
        }

        //if (node != null) // For panels inbetween cube Platforms
        //{
        //    Vector3 result = (node.CubeID - neighloc);
        //    Vector3Int midDiff = new Vector3Int(Mathf.FloorToInt(result.x * 0.5f), Mathf.FloorToInt(result.y * 0.5f), Mathf.FloorToInt(result.z * 0.5f));
        //    Vector3Int midHalfVect = node.CubeID - midDiff;


        //    Debug.Log("node.CubeID >> " + node.CubeID);
        //    Debug.Log("neighloc >> " + neighloc);
        //    Debug.Log("midDiff >> " + midDiff);
        //    Debug.Log("midHalfVect >> " + midHalfVect);

        //    CubeLocationScript midHalfnode = GetHalfLocationScript_CLIENT(midHalfVect);

        //    if (midHalfnode != null)
        //    {
        //        if (midHalfnode.CubeIsPanel)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL mid Half Cube is Panel at vect:" + neighloc); };
        //            return null;
        //        }
        //    }
        //}

        //// FOR THE FUCKEN SLOPES and moveing through panels (this is ugly but fuck off its working) // NOTE MIGHT NEED TO REMOVE STRAIGHT UP AND DOWN FROM THESE NEXT CHECKS
        //if (node != null)
        //{
        //    Vector3Int nodevect = node.CubeID;

        //    if (node.CubeID.y > neighloc.y)
        //    {
        //        // check for panel to stop going through panels
        //        Vector3Int neighHalf = new Vector3Int(nodevect.x, nodevect.y - 1, nodevect.z);
        //        CubeLocationScript neighscript = GetHalfLocationScript_CLIENT(neighHalf);
        //        if (neighscript == null)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL move neighscript == null:" + neighloc); };
        //            return null;
        //        }
        //        if (neighscript.CubeIsPanel)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL move neighscript.CubeIsPanel:" + neighloc); };
        //            return null;
        //        }

        //        // BOTTOM SLOPES //
        //        Vector3Int neighSlope = new Vector3Int(nodevect.x, nodevect.y - 2, nodevect.z);
        //        neighscript = GetLocationScript_CLIENT(neighSlope);
        //        if (neighscript == null)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL move neighscript == null:" + neighloc); };
        //            return null;
        //        }
        //        if (neighscript == null)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL move neighscript.CubeIsPanel:" + neighloc); };
        //            return null;
        //        }
        //        if (neighscript.CubeIsSlope)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL move neighscript.CubeIsPanel:" + neighloc); };
        //            int slopeAngle = neighscript._panelScriptChild._panelYAxis;
        //            if (slopeAngle == 90)
        //            {
        //                Vector3Int neighBehindVect = node.NeighbourVects[3];
        //                if (neighloc == neighBehindVect) { return null; }
        //            }
        //            else if (slopeAngle == 180)
        //            {
        //                Vector3Int neighBehindVect = node.NeighbourVects[0];
        //                if (neighloc == neighBehindVect) { return null; }
        //            }
        //            else if (slopeAngle == 270)
        //            {
        //                Vector3Int neighBehindVect = node.NeighbourVects[1];
        //                if (neighloc == neighBehindVect) { return null; }

        //            }
        //            else if (slopeAngle == 0)
        //            {
        //                Vector3Int neighBehindVect = node.NeighbourVects[4];
        //                if (neighloc == neighBehindVect) { return null; }
        //            }
        //        }
        //    }
        //    else if (node.CubeID.y < neighloc.y)
        //    {
        //        // check for panel to stop going through panels
        //        Vector3Int neighHalf = new Vector3Int(nodevect.x, nodevect.y + 1, nodevect.z);
        //        CubeLocationScript neighscript = GetHalfLocationScript_CLIENT(neighHalf);
        //        if(neighscript == null)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL move neighscript == null:" + neighloc); };
        //            return null;
        //        }
        //        if (neighscript.CubeIsPanel)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL move neighscript.CubeIsPanel:" + neighloc); };
        //            return null;
        //        }

        //        // TOP SLOPES // THIS WILL NEED TO BE IMPLEMENTED AT SOME POINT FOR ALIENS
        //        Vector3Int neighSlope = new Vector3Int(nodevect.x, nodevect.y + 2, nodevect.z);
        //        neighscript = GetLocationScript_CLIENT(neighSlope);
        //        if (neighscript == null)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL move neighscript == null:" + neighloc); };
        //            return null;
        //        }
        //        if (neighscript.CubeIsSlope)
        //        {
        //            if (debugStatements) { Debug.LogError("FAIL move neighscript.CubeIsPanel:" + neighloc); };
        //            int slopeAngle = neighscript._panelScriptChild._panelYAxis;
        //            if (slopeAngle == 90)
        //            {
        //                Vector3Int neighBehindVect = node.NeighbourVects[3];
        //                if (neighloc == neighBehindVect) { return null; }
        //            }
        //            else if (slopeAngle == 180)
        //            {
        //                Vector3Int neighBehindVect = node.NeighbourVects[0];
        //                if (neighloc == neighBehindVect) { return null; }
        //            }
        //            else if (slopeAngle == 270)
        //            {
        //                Vector3Int neighBehindVect = node.NeighbourVects[1];
        //                if (neighloc == neighBehindVect) { return null; }

        //            }
        //            else if (slopeAngle == 0)
        //            {
        //                Vector3Int neighBehindVect = node.NeighbourVects[4];
        //                if (neighloc == neighBehindVect) { return null; }
        //            }
        //        }
        //    }
        //}

        //if (!neighCubeScript.CubePlatform)
        //{
        //    if (recursiveCheck)
        //    {
        //        if (debugStatements) { Debug.LogError("RECURSIVE FAIL !neighCubeScript.CubePlatform: " + neighloc); };
        //        return null;
        //    }

        //    // making climbable edges ///
        //    if (node != null)
        //    {
        //        List<Vector3Int> newNeighVects = node.NeighbourVects;
        //        foreach (Vector3Int newVect in newNeighVects)
        //        {
        //            if (debugStatements) { Debug.Log("RECURSIVE CheckIfCanMoveToCube_CLIENT loc: " + newVect); };
        //            CubeLocationScript script = CheckIfCanMoveToCube_CLIENT(node, newVect, unitIsAlien, true);
        //            if (script != null)
        //            {
        //                //Debug.Log("SUCCES move to loc: " + newVect);
        //                return neighCubeScript; // if climable neighboursing node return true
        //            }
        //        }
        //    }
        //    ///////

        //    if (debugStatements) { Debug.LogError("FAIL move cubeScript not CubeMoveable: " + neighloc); };
        //    return null;
        //}


        //Debug.Log("Unit CAN move to loc: " + neighloc);
        return neighCubeScript;
    }


    ///////////////////////////////////////////

    public static bool SetUnitOnCube_CLIENT(UnitScript unitScript, Vector3Int loc)
    {
        Vector3Int unitId = unitScript.UnitID;
        CubeLocationScript cubescript = CheckIfCanMoveToCube_CLIENT(unitScript.CubeUnitIsOn, loc, unitScript.UnitCanClimbWalls);
        if (cubescript != null)
        {
            if (!_unitLocation.ContainsKey(unitId))
            {
                _unitLocation.Add(unitId, cubescript);
            }
            else
            {
                CubeLocationScript oldCubescript = _unitLocation[unitId];
                oldCubescript.CubeOccupied = false;
                _unitLocation[unitId] = cubescript;
            }
            cubescript.CubeOccupied = true;

            unitScript.CubeUnitIsOn = cubescript;

            //if (unitScript.CubeUnitIsOn != null && unitScript.CubeUnitIsOn._platform_Panel_Cube != null)
            //{
            //    cubescript._platform_Panel_Cube._panelScriptChild.PanelPieceChangeColor("Pink");
            //}
            return true;
        }
        else
        {
            Debug.LogError("Unit cannot move to a location " + loc);
            return false;
        }
    }

    //////////////////////////////////////////////

    public static void SaveNodeTo_CLIENT(Vector3Int vect, BaseNode node)
    {
        if (!_CLIENT_nodeLookup.ContainsKey(vect))
        {
            //Debug.Log("fucken adding normalscript to vect: " + vect + " script: " + script);
            _CLIENT_nodeLookup.Add(vect, node);
        }
        else
        {
            Debug.LogError("trying to assign script to already taking location!!!");
        }
    }


    public static BaseNode GetNodeFrom_Client(Vector3Int loc)
    {
        //Debug.Log("GetLocationScript");

        if (_CLIENT_nodeLookup.ContainsKey(loc))
        {
            return _CLIENT_nodeLookup[loc];
        }
        //Debug.LogError("LOCATION DOSENT EXIST Loc: " + loc);
        return null;
    }

    //////////////////////////////////////////////

    // tries to spawn visual nodes in all current moveable locations for a unit
    public static void DebugTestPathFindingNodes_CLIENT(UnitScript unitScript)
    {
        foreach (KeyValuePair<Vector3Int, CubeLocationScript> element in _LocationLookup)
        {
            Debug.Log("DebugTestPathFindingNodes_CLIENT ");
            CubeLocationScript script = CheckIfCanMoveToCube_CLIENT(unitScript.CubeUnitIsOn, element.Key, unitScript.UnitCanClimbWalls);

            if (script != null)
            {
                if (script.CubeMoveable)
                {
                    script.CreatePathFindingNodeInCube(unitScript.UnitID);
                }
            }
        }
    }

    public static void RemoveUnNeededCubes()
    {
        foreach(CubeLocationScript script in _LocationLookup.Values)
        {
            script.DestroyCubeIfNotImportant();
        }
    }
}
