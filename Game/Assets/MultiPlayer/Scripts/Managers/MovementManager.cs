using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static MovementManager _instance;


    public static bool _gravityActivated = false; // This will need to change per ship section and not just a general global, cause units are already accessing it// fuck thats gunna be a shit show later on
    public static bool _gravityBoots_ON = true; // This will need to change per player, for now its a shitty global

    private static List<Vector3Int> _movePath = new List<Vector3Int>();

    private static CubeLocationScript _currActiveCube = null; // Just a check to stop re-calculations of pathfinding to be done only once when panel mouse moved over and not repeatedly

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

    public static void MakeActiveUnitMove_CLIENT()
    {
        UnitScript activeUnit = UnitsManager.ActiveUnit;

        if (activeUnit)
        {
            if (_movePath != null)
            {
                activeUnit.GetComponent<MovementScript>().MoveUnit(_movePath);
                //PlayerManager.NetworkAgent.CmdTellServerToMoveUnit(PlayerManager.PlayerAgent.NetworkInstanceID, _activeUnit.NetID, pathInts);
            }
        }
    }

    ////////////////////////////////////////////////

    public static bool BuildMovementPath(CubeLocationScript endLocScript)
    {
        if (_currActiveCube == endLocScript)
        {
            return true;
        }
        else
        {
            _currActiveCube = endLocScript;
        }

        UnitScript activeUnit = UnitsManager.ActiveUnit;
        if (activeUnit != null)
        {
            UnitsManager.ActiveUnit.ClearPathFindingNodes();
        }

        _movePath = SetUnitsPath(endLocScript);
        if (_movePath != null)
        {
            CreatePathFindingNodes_CLIENT(UnitsManager.ActiveUnit, _movePath);
            return true;
        }
        else
        {
            //_currActiveCube = null;
            return false;
        }
    }

    ////////////////////////////////////////////////

    public static List<Vector3Int> SetUnitsPath(CubeLocationScript endLocScript)
    {
        List<Vector3Int> path = null;

        UnitScript activeUnit = UnitsManager.ActiveUnit;
        if (activeUnit != null && activeUnit.CubeUnitIsOn != null && endLocScript != null && endLocScript.CubeID != null)
        {
            Vector3Int startLoc = activeUnit.CubeUnitIsOn.CubeID;
            Vector3Int endLoc = endLocScript.CubeID;

            path = PathFinding.FindPath(activeUnit, startLoc, endLoc);
        }

        return path;
    }


    public static void StopUnits() {


	}

    public static void CreatePathFindingNodes_CLIENT(UnitScript unitScript, List<Vector3Int> path)
    {
        List<CubeLocationScript> scriptList = new List<CubeLocationScript>();

        foreach(Vector3Int vect in path)
        {
            CubeLocationScript script = LocationManager.GetLocationScript_CLIENT(vect);
            script.CreatePathFindingNodeInCube(unitScript.UnitID);
            scriptList.Add(script);
            //Debug.Log("pathfinding VISUAL node set at vect: " + vect);
        }

        unitScript.AssignPathFindingNodesToUnit(scriptList);
    }

    ////////////////////////////////////////////////

}
