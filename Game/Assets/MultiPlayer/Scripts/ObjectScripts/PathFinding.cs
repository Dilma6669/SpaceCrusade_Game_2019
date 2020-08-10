using UnityEngine;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static PathFinding _instance;

    ////////////////////////////////////////////////

    public static bool _debugPathfindingNodes = false;

    ////////////////////////////////////////////////

    private static List<CubeLocationScript> _previousNodes = new List<CubeLocationScript>();

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

    public static List<Vector3Int> FindPath(UnitScript unit, Vector3Int startVect, Vector3Int targetVect)
    {
        ResetPath();

        //if (!LocationManager.CheckIfCanMoveToCube_CLIENT(null, targetVect, unit.UnitCanClimbWalls, false))
        //{
        //    Debug.Log("SHIT NO WAY OF GETTING TO THAT SPOT! targetVect ");
        //    return null;
        //}

        CubeLocationScript cubeStartScript = LocationManager.GetLocationScript_CLIENT(startVect);
        CubeLocationScript cubeTargetScript = LocationManager.GetLocationScript_CLIENT(targetVect);

        CubeLocationScript cubeHALFTargetScript = LocationManager.GetHalfLocationScript_CLIENT(targetVect);

        if (cubeHALFTargetScript != null && cubeHALFTargetScript._cubeIsHalfScriptPretendingToBeFullForSlopes)
            cubeTargetScript = cubeHALFTargetScript;

        if (cubeTargetScript == null)
            return null;

        List<CubeLocationScript> openSet = new List<CubeLocationScript>();
        openSet.Clear();
        HashSet<CubeLocationScript> closedSet = new HashSet<CubeLocationScript>();
        closedSet.Clear();

        openSet.Add(cubeStartScript);
        _previousNodes.Add(cubeStartScript);

        int maxcount = 100;

        while (openSet.Count > 0 && openSet.Count < maxcount)
        {
            CubeLocationScript node = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == cubeTargetScript)
                return RetracePath(cubeStartScript, cubeTargetScript);

            List<Vector3Int> neighVects = node.NeighbourVects;

            foreach (Vector3Int vect in neighVects)
            {
                // personal checks
                if (LocationManager.CheckIfCanMoveToCube_CLIENT(node, vect, unit.UnitCanClimbWalls) == null)
                    continue;


                CubeLocationScript neightbourScript = LocationManager.GetLocationScript_CLIENT(vect);


                if (closedSet.Contains(neightbourScript))
                    continue;


                if (_debugPathfindingNodes)
                    neightbourScript.CreatePathFindingNodeInCube(unit.UnitID);


                _previousNodes.Add(neightbourScript);

                int newCostToNeighbour = node.gCost + GetDistance(node, neightbourScript);
                if (newCostToNeighbour < neightbourScript.gCost || !openSet.Contains(neightbourScript))
                {
                    neightbourScript.gCost = newCostToNeighbour;
                    neightbourScript.hCost = GetDistance(neightbourScript, cubeTargetScript);
                    neightbourScript._parentPathFinding = node;

                    if (!openSet.Contains(neightbourScript))
                        openSet.Add(neightbourScript);
                }

                if (vect == targetVect)
                    break;
            }
        }
        Debug.Log("SHIT NO WAY OF GETTING TO THAT SPOT!");
        ResetPath();
        return null;
    }

	private static List<Vector3Int> RetracePath(CubeLocationScript startNode, CubeLocationScript endNode) {
		List<Vector3Int> path = new List<Vector3Int>();
		CubeLocationScript currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode.CubeID);
			currentNode = currentNode._parentPathFinding;
		}
		path.Reverse();
        ResetPath();

        return path;
	}



	private static int GetDistance(CubeLocationScript nodeA, CubeLocationScript nodeB) {

        int dstX = Mathf.FloorToInt(Mathf.Abs(nodeA.CubeID.x - nodeB.CubeID.x));
        int dstY = Mathf.FloorToInt(Mathf.Abs(nodeA.CubeID.y - nodeB.CubeID.y));
        int dstZ = Mathf.FloorToInt(Mathf.Abs(nodeA.CubeID.z - nodeB.CubeID.z));

        if (dstX > dstZ)
        {
            return 14 * dstZ + 10 * (dstX - dstZ) + 10 * dstY;
        }

        return 14 * dstX + 10 * (dstZ - dstX) + 10 * dstY;
    }

    private static void ResetPath()
    {
        foreach (CubeLocationScript node in _previousNodes)
        {
            //Debug.Log("pathfinding DestroyPathFindingNode: ");
            node.DestroyPathFindingNode();
        }
        _previousNodes.Clear();
    }
}