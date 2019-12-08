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
    public static Dictionary<Vector3, CubeLocationScript> _LocationLookup = new Dictionary<Vector3, CubeLocationScript>();   // movable locations
    public static Dictionary<Vector3, CubeLocationScript> _LocationHalfLookup = new Dictionary<Vector3, CubeLocationScript>(); // not moveable locations BUT important for neighbour system

    public static Dictionary<Vector3, CubeLocationScript> _unitLocation = new Dictionary<Vector3, CubeLocationScript>(); // sever shit

    public static Dictionary<Vector3, BaseNode> _CLIENT_nodeLookup = new Dictionary<Vector3, BaseNode>();

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

    public static void SetCubeScriptToLocation_CLIENT(Vector3 vect, CubeLocationScript script)
    {
        if (!_LocationLookup.ContainsKey(vect))
        {
             //Debug.Log("Adding normalscript to vect: " + vect);
            _LocationLookup.Add(vect, script);
            return;
        }
        // Debug.LogError("trying to assign script to already taking location!!!"); // this now happens all the time because maps overlap eachother with new map system
    }

    public static void SetCubeScriptToHalfLocation_CLIENT(Vector3 vect, CubeLocationScript script)
    {
        if (!_LocationHalfLookup.ContainsKey(vect))
        {
             //Debug.Log("Adding normalscript to vect: " + vect);
            _LocationHalfLookup.Add(vect, script);
            return;
        }
        //Debug.LogError("trying to assign script to already taking location!!!"); // this now happens all the time because maps overlap eachother with new map system
    }

    ////////////////////////////////////////////////

    public static CubeLocationScript GetLocationScript_CLIENT(Vector3 loc)
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

    public static CubeLocationScript GetHalfLocationScript_CLIENT(Vector3 loc)
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
    

    public static CubeLocationScript CheckIfCanMoveToCube_CLIENT(CubeLocationScript node, Vector3 neighloc, bool unitIsAlien, bool recursiveCheck = false)
    {
        //Debug.Log("CheckIfCanMoveToCube_CLIENT loc: " + neighloc);

        CubeLocationScript neighCubeScript = GetLocationScript_CLIENT(neighloc);

        neighCubeScript.IS_HUMAN_MOVABLE = false;
        neighCubeScript.IS_ALIEN_MOVABLE = false;

        if (neighCubeScript == null)
        {
            Debug.LogError("FAIL move cubeScript == null: " + neighloc);
            return null;
        }

        if (neighCubeScript.CubeIsSlope)
        {
            Debug.LogError("FAIL move neighCubeScript._isSlope: " + neighloc);
            return null;
        }

        if (neighCubeScript.CubeOccupied)
        {
            Debug.LogError("FAIL move Cube is Occupied at vect:" + neighloc);
            return null;
        }


        // for the god damn slopes and moveing through panels (this is ugly but fuck off its working)
        if (node != null)
        {
            Vector3 nodevect = node.CubeID;

            // check for panel to stop going through panels
            if (node.CubeID.y > neighloc.y)
            {
                Vector3 neighHalf = new Vector3(nodevect.x, nodevect.y - 1, nodevect.z);
                CubeLocationScript neighscript = GetHalfLocationScript_CLIENT(neighHalf);
                if (neighscript.CubeIsPanel)
                {
                    //Debug.LogError("FAIL move neighscript.CubeIsPanel:" + neighloc);
                    return null;
                }
                // BOTTOM SLOPES //
                Vector3 neighSlope = new Vector3(nodevect.x, nodevect.y - 2, nodevect.z);
                neighscript = GetLocationScript_CLIENT(neighSlope);
                if (neighscript.CubeIsSlope)
                {
                    //Debug.LogError("FAIL move neighscript.CubeIsPanel:" + neighloc);
                    int slopeAngle = neighscript._panelScriptChild._panelYAxis;
                    if (slopeAngle == 90)
                    {
                        Vector3 neighBehindVect = node.NeighbourVects[3];
                        if (neighloc == neighBehindVect) { return null; }
                    }
                    else if (slopeAngle == 180)
                    {
                        Vector3 neighBehindVect = node.NeighbourVects[0];
                        if (neighloc == neighBehindVect) { return null; }
                    }
                    else if (slopeAngle == 270)
                    {
                        Vector3 neighBehindVect = node.NeighbourVects[1];
                        if (neighloc == neighBehindVect) { return null; }

                    }
                    else if (slopeAngle == 0)
                    {
                        Vector3 neighBehindVect = node.NeighbourVects[4];
                        if (neighloc == neighBehindVect) { return null; }
                    }
                }
            }
            else
            {
                Vector3 neighHalf = new Vector3(nodevect.x, nodevect.y + 1, nodevect.z);
                CubeLocationScript neighscript = GetHalfLocationScript_CLIENT(neighHalf);
                if (neighscript.CubeIsPanel)
                {
                    Debug.LogError("FAIL move neighscript.CubeIsPanel:" + neighloc);
                    return null;
                }
                // TOP SLOPES // THIS WILL NEED TO BE IMPLEMENTED AT SOME POINT FOR ALIENS
                Vector3 neighSlope = new Vector3(nodevect.x, nodevect.y + 2, nodevect.z);
                neighscript = GetLocationScript_CLIENT(neighSlope);
                if (neighscript.CubeIsSlope)
                {
                    Debug.LogError("FAIL move neighscript.CubeIsPanel:" + neighloc);
                    int slopeAngle = neighscript._panelScriptChild._panelYAxis;
                    if (slopeAngle == 90)
                    {
                        Vector3 neighBehindVect = node.NeighbourVects[3];
                        if (neighloc == neighBehindVect) { return null; }
                    }
                    else if (slopeAngle == 180)
                    {
                        Vector3 neighBehindVect = node.NeighbourVects[0];
                        if (neighloc == neighBehindVect) { return null; }
                    }
                    else if (slopeAngle == 270)
                    {
                        Vector3 neighBehindVect = node.NeighbourVects[1];
                        if (neighloc == neighBehindVect) { return null; }

                    }
                    else if (slopeAngle == 0)
                    {
                        Vector3 neighBehindVect = node.NeighbourVects[4];
                        if (neighloc == neighBehindVect) { return null; }
                    }
                }
            }
        }


        if (!neighCubeScript.CubePlatform)
        {
            if (recursiveCheck)
            {
                Debug.LogError("FAIL !neighCubeScript.CubePlatform: " + neighloc);
                return null;
            }

            // making climbable edges ///
            if (node != null)
            {
                List<Vector3> newNeighVects = node.NeighbourVects;
                foreach (Vector3 newVect in newNeighVects)
                {
                    //Debug.Log("RECURSIVE CheckIfCanMoveToCube_CLIENT loc: " + newVect);
                    CubeLocationScript script = CheckIfCanMoveToCube_CLIENT(node, newVect, unitIsAlien, true);
                    if (script != null)
                    {
                        //Debug.Log("SUCCES move to loc: " + newVect);
                        return neighCubeScript; // if climable neighboursing node return true
                    }
                }
            }
            ///////

            //Debug.LogError("FAIL move cubeScript not CubeMoveable: " + neighloc);
            return null;
        }


        if (!unitIsAlien)  // if human
        {
            if (neighCubeScript.IsHumanWalkable == false && neighCubeScript.IsHumanClimbable == false && neighCubeScript.IsHumanJumpable == false)
            {
                Debug.LogError("FAIL move error Human walkable/climable/jumpable: " + neighloc);
                Debug.LogFormat("FAIL walkable/climable/jumpable: {0} , {1} , {2}", neighCubeScript.IsHumanWalkable, neighCubeScript.IsHumanClimbable, neighCubeScript.IsHumanJumpable);
                return null;
            }
            neighCubeScript.IS_HUMAN_MOVABLE = true;
        }
        else if (unitIsAlien) // alien
        {
            if (neighCubeScript.IsAlienWalkable == false && neighCubeScript.IsAlienClimbable == false && neighCubeScript.IsAlienJumpable == false)
            {
               Debug.LogError("FAIL move error ALIEN walkable/climable/jumpable: " + neighloc);
                return null;
            }
            neighCubeScript.IS_ALIEN_MOVABLE = true;
        }

        //Debug.Log("Unit CAN move to loc: " + neighloc);
        return neighCubeScript;
    }


    ///////////////////////////////////////////

    public static bool SetUnitOnCube_CLIENT(UnitScript unitScript, Vector3 loc)
    {

        Debug.Log("SetUnitOnCube_CLIENT " + loc);

        Vector3 unitId = unitScript.UnitID;
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
            Debug.Log("SetUnitOnCube loc:  " + loc);
            return true;
        }
        else
        {
            Debug.LogError("Unit cannot move to a location " + loc);
            return false;
        }
    }

    //////////////////////////////////////////////

    public static void SaveNodeTo_CLIENT(Vector3 vect, BaseNode node)
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


    public static BaseNode GetNodeFrom_Client(Vector3 loc)
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

    public static void SetCubeActive_CLIENT(bool onOff, Vector3 location)
    {
        if (_activeCube)
        {
            _activeCube.GetComponent<CubeLocationScript>().CubeActive(false);
            _activeCube = null;
        }

        if (onOff)
        {
            if (GetLocationScript_CLIENT(location) != null)
            {
                _activeCube = GetLocationScript_CLIENT(location);
                _activeCube.GetComponent<CubeLocationScript>().CubeActive(true);
            }
            else if (GetHalfLocationScript_CLIENT(location) != null)
            {
                _activeCube = GetHalfLocationScript_CLIENT(location);
                _activeCube.GetComponent<CubeLocationScript>().CubeActive(true);
            }

           UnitsManager.MakeActiveUnitMove_CLIENT(location); // this just checks is a unit is currently selected and then does the moving shit
        }
    }

    // tries to spawn visual nodes in all current moveable locations for a unit
    public static void DebugTestPathFindingNodes_CLIENT(UnitScript unitScript)
    {
        foreach (KeyValuePair<Vector3, CubeLocationScript> element in _LocationLookup)
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
