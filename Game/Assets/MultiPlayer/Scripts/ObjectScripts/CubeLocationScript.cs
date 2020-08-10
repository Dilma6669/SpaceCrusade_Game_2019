using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeLocationScript : MonoBehaviour {

    // Cube info
    public Vector3Int _cubeUniqueID;
    int _cubeType;
    int _cubeLayerID;
    public bool _cubeMovable; // this is all movable cubes everywhere, including in the air
    public bool _cubePlatform; // this is all movable cubes only with panels to walk on, not in air
    public bool _cubeIsSlope;
    public bool _cubeIsPanel;

    private MapNode _mapNodeParent; // the parent of the object

    public CubeLocationScript _platform_Panel_Cube; // this is the neighbouring cube that has a panel in it

    public bool _cubeIsHalfScriptPretendingToBeFullForSlopes;

    bool _cubeVisible;
    bool _cubSelected;
    public bool _cubeOccupied; // If a guy is on square

    // panel objects
    private PanelPieceScript _panelScriptChild;

    // Pathfinding
    public GameObject _pathFindingNode;
    public CubeLocationScript _parentPathFinding; //important // used to retrace pathfinding nodes
    public int fCost;
    public int hCost;
    public int gCost;

    // neighbour Cubes
    public List<Vector3Int> _neighVects = new List<Vector3Int>();
    public List<Vector3Int> _neighHalfVects = new List<Vector3Int>();
    public bool _neighboursSet = false;
    bool[] _neighBools = new bool[27];

    // _movement Offset
    public Vector3Int _movementOffset_Left;
    public Vector3Int _movementOffset_Right;

    public Vector3Int CubeID
    {
        get { return _cubeUniqueID; }
        set { _cubeUniqueID = value; }
    }

    public MapNode MapNodeParent
    {
        get { return _mapNodeParent; }
        set { _mapNodeParent = value; }
    }

    public bool CubeMoveable
    {
        get { return _cubeMovable; }
        set { _cubeMovable = value; }
    }

    public bool CubePlatform
    {
        get { return _cubePlatform; }
        set { _cubePlatform = value; }
    }

    public int CubeType
    {
        get { return _cubeType; }
        set { _cubeType = value; }
    }

    public bool CubeIsVisible
    {
        get { return _cubeVisible; }
        set { _cubeVisible = value; }
    }

    public bool CubeSelected
    {
        get { return _cubSelected; }
        set { _cubSelected = value; }
    }

    public bool CubeOccupied
    {
        get { return _cubeOccupied; }
        set { _cubeOccupied = value; }
    }

    public bool CubeIsSlope
    {
        get { return _cubeIsSlope; }
        set { _cubeIsSlope = value; }
    }

    public bool CubeIsPanel
    {
        get { return _cubeIsPanel; }
        set { _cubeIsPanel = value; }
    }

    public PanelPieceScript GetCubePanel
    {
        get { return _panelScriptChild; }
        set { _panelScriptChild = value; }
    }


    public int CubeLayerID
    {
        get { return _cubeLayerID; }
        set { _cubeLayerID = value; }
    }


    // Neighbours
    public bool NeighboursSet
    {
        get { return _neighboursSet; }
        set { _neighboursSet = value; }
    }

    public List<Vector3Int> NeighbourVects
    {
        get { return _neighVects; }
        set { _neighVects = value; }
    }

    public List<Vector3Int> NeighbourHalfVects
    {
        get { return _neighHalfVects; }
        set { _neighHalfVects = value; }
    }


    void Awake()
    {
        CubeIsVisible = true;
        CubeSelected = false;
        CubeOccupied = false;
        CubeMoveable = false;
        CubePlatform = false;
        CubeIsSlope = false;

        CubeID = new Vector3Int(-1, -1, -1);
    }

    public void AssignCubeNeighbours()
    {
        if (!NeighboursSet)
        {
            if (CubeMoveable)
            {
                CubeConnections.SetCubeNeighbours(this);
            }
            else
            {
                CubeConnections.SetCubeHalfNeighbours(this);
            }
            NeighboursSet = true;
        }
    }



    public void CubeActive(bool onOff) {
		
		if (onOff) {
			CubeSelected = true;
		} else {
            CubeSelected = false;
		}
	}

	///////////////////////////////
	/// this is for when panel is clicked
	//public void CubeSelect(bool onOff, GameObject panelSelected = null) {

	//	if (onOff) {
	//		CubeActive (true);
	//		_activePanel = panelSelected;
 //           LocationManager.SetCubeActive_CLIENT(true, CubeID); // not sure if this should be here yet
 //       }
 //       else
 //       {
	//		CubeActive (false);
 //           LocationManager.SetCubeActive_CLIENT(false, Vector3Int.zero);
	//	}
	//}


    public void SetHalfNeighbourVects() {
        
        if(!NeighbourHalfVects.Any())
        {
            Vector3Int ownVect = new Vector3Int(CubeID.x, CubeID.y, CubeID.z);

            //NeighbourHalfVects.Add(new Vector3Int (ownVect.x - 1, ownVect.y - 1, ownVect.z - 1)); // 0
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y - 1, ownVect.z - 1)); // 1
            //NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 1, ownVect.y - 1, ownVect.z - 1)); // 2

            NeighbourHalfVects.Add(new Vector3Int (ownVect.x - 1, ownVect.y - 1, ownVect.z + 0)); // 3
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y - 1, ownVect.z + 0)); // 4 directly below
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 1, ownVect.y - 1, ownVect.z + 0)); // 5
                                                                                               
            //NeighbourHalfVects.Add(new Vector3Int (ownVect.x - 1, ownVect.y - 1, ownVect.z + 1)); // 6
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y - 1, ownVect.z + 1)); // 7
            //NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 1, ownVect.y - 1, ownVect.z + 1)); // 8

            /////////////////////////////////

            NeighbourHalfVects.Add(new Vector3Int (ownVect.x - 1, ownVect.y + 0, ownVect.z - 1)); // 9
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 0, ownVect.z - 1)); // 10 infront (south)
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 1, ownVect.y + 0, ownVect.z - 1)); // 11
    
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x - 1, ownVect.y + 0, ownVect.z + 0)); // 12 side (west)
            //NeighbourHalfVects.Add(ownVect);                                                  // 13 //// MIDDLE
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 1, ownVect.y + 0, ownVect.z + 0)); // 14 side (east)

            NeighbourHalfVects.Add(new Vector3Int (ownVect.x - 1, ownVect.y + 0, ownVect.z + 1)); // 15
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 0, ownVect.z + 1)); // 16 back (North)
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 1, ownVect.y + 0, ownVect.z + 1)); // 17 
           
            /////////////////////////////////

            //neighHalfVects.Add(new Vector3Int (ownVect.x - 1, ownVect.y + 1, ownVect.z - 1)); // 18
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 1, ownVect.z - 1)); // 19
            //neighHalfVects.Add(new Vector3Int (ownVect.x + 1, ownVect.y + 1, ownVect.z - 1)); // 20

            NeighbourHalfVects.Add(new Vector3Int (ownVect.x - 1, ownVect.y + 1, ownVect.z + 0)); // 21
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 1, ownVect.z + 0)); // 22 directly above
            NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 1, ownVect.y + 1, ownVect.z + 0)); // 23

    		//neighHalfVects.Add(new Vector3Int (ownVect.x - 1, ownVect.y + 1, ownVect.z + 1)); // 24
    		NeighbourHalfVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 1, ownVect.z + 1)); // 25
    		//neighHalfVects.Add(new Vector3Int (ownVect.x + 1, ownVect.y + 1, ownVect.z + 1)); // 26

        /////////////////////////////////
        }
    }

	public void SetNeighbourVects() {

        if (!NeighbourVects.Any())
        {
            Vector3Int ownVect = new Vector3Int(CubeID.x, CubeID.y, CubeID.z);
    
            //NeighbourVects.Add(new Vector3Int (ownVect.x - 2, ownVect.y - 2, ownVect.z - 2)); // 0
            NeighbourVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y - 2, ownVect.z - 2)); // 1
            //NeighbourVects.Add(new Vector3Int (ownVect.x + 2, ownVect.y - 2, ownVect.z - 2)); // 2
                                                                                       //
            NeighbourVects.Add(new Vector3Int (ownVect.x - 2, ownVect.y - 2, ownVect.z + 0)); // 3
    		NeighbourVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y - 2, ownVect.z + 0)); // 4 directly below
            NeighbourVects.Add(new Vector3Int (ownVect.x + 2, ownVect.y - 2, ownVect.z + 0)); // 5
                                                                                           //
            //NeighbourVects.Add(new Vector3Int (ownVect.x - 2, ownVect.y - 2, ownVect.z + 2)); // 6
            NeighbourVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y - 2, ownVect.z + 2)); // 7
            //NeighbourVects.Add(new Vector3Int (ownVect.x + 2, ownVect.y - 2, ownVect.z + 2)); // 8
    
            /////////////////////////////////
            NeighbourVects.Add(new Vector3Int (ownVect.x - 2, ownVect.y + 0, ownVect.z - 2)); // 9
            NeighbourVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 0, ownVect.z - 2)); // 10 infront (south)
            NeighbourVects.Add(new Vector3Int (ownVect.x + 2, ownVect.y + 0, ownVect.z - 2)); // 11
    
            NeighbourVects.Add(new Vector3Int (ownVect.x - 2, ownVect.y + 0, ownVect.z + 0)); // 12 side (west)
            //NeighbourVects.Add(ownVect);                                                   // 13 //// MIDDLE
            NeighbourVects.Add(new Vector3Int (ownVect.x + 2, ownVect.y + 0, ownVect.z + 0)); // 14 side (east)
    
            NeighbourVects.Add(new Vector3Int (ownVect.x - 2, ownVect.y + 0, ownVect.z + 2)); // 15
            NeighbourVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 0, ownVect.z + 2)); // 16 back (North)
            NeighbourVects.Add(new Vector3Int (ownVect.x + 2, ownVect.y + 0, ownVect.z + 2)); // 17 
            /////////////////////////////////
    
            //NeighbourVects.Add(new Vector3Int (ownVect.x - 2, ownVect.y + 2, ownVect.z - 2)); // 18
            NeighbourVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 2, ownVect.z - 2)); // 19
            //NeighbourVects.Add(new Vector3Int (ownVect.x + 2, ownVect.y + 2, ownVect.z - 2)); // 20
            //
            NeighbourVects.Add(new Vector3Int (ownVect.x - 2, ownVect.y + 2, ownVect.z + 0)); // 21
            NeighbourVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 2, ownVect.z + 0)); // 22 directly above
            NeighbourVects.Add(new Vector3Int (ownVect.x + 2, ownVect.y + 2, ownVect.z + 0)); // 23
                                                                                           //
            //NeighbourVects.Add(new Vector3Int (ownVect.x - 2, ownVect.y + 2, ownVect.z + 2)); // 24
            NeighbourVects.Add(new Vector3Int (ownVect.x + 0, ownVect.y + 2, ownVect.z + 2)); // 25
          
         /////////////////////////////////
        }
    }


    public void ResetPathFindingValues()
    {
        _pathFindingNode = null;
        _parentPathFinding = null;
        fCost = 0;
        hCost = 0;
        gCost = 0;
    }


    public void SetPanelSideToActiveWithPathFinding()
    {
        CubeLocationScript cubeScriptLeft = _platform_Panel_Cube._panelScriptChild.cubeScriptLeft;
        CubeLocationScript cubeScriptRight = _platform_Panel_Cube._panelScriptChild.cubeScriptRight;

        if (ReferenceEquals(this, cubeScriptLeft))
        {
            _platform_Panel_Cube._panelScriptChild._activeCubeScript = cubeScriptLeft;
        }
        else if (ReferenceEquals(this, cubeScriptRight))
        {
            _platform_Panel_Cube._panelScriptChild._activeCubeScript = cubeScriptRight;
        }
        else
        {
            _platform_Panel_Cube._panelScriptChild._activeCubeScript = null;
        }
    }


    public Vector3Int GetCubeMovementOffset()
    {
        SetPanelSideToActiveWithPathFinding();
        Vector3Int movementOffset = new Vector3Int(-1, -1, -1);

        if (_platform_Panel_Cube._panelScriptChild.ActivePanelSideIsLeft())
        {
            movementOffset = _movementOffset_Left;
        }
        else if (!_platform_Panel_Cube._panelScriptChild.ActivePanelSideIsLeft())
        {
            movementOffset = _movementOffset_Right;
        }

        //// For Slopes if both neighbouring sides are the same damn cube
        //if (ReferenceEquals(cubeScriptLeft, cubeScriptRight))
        //{
        //    Debug.LogError("fucken SLOPES");
        //    if(_platform_Panel_Cube._panelScriptChild._panelHitIndex == 1) // This is sooo dumb and annoying
        //    {
        //        movementOffset = _movementOffset_Right;
        //    }
        //    else if (_platform_Panel_Cube._panelScriptChild._panelHitIndex == 2)
        //    {
        //        movementOffset = _movementOffset_Left;
        //    }
        //}

        if (movementOffset == null || movementOffset == new Vector3Int(-1, -1, -1))
        {
            Debug.LogError("fuck ISSUE HERE movementOffset = " + movementOffset);
        }

        return movementOffset;
    }


    public void CreatePathFindingNodeInCube(Vector3Int unitID)
    {
        if (_platform_Panel_Cube != null && _platform_Panel_Cube._panelScriptChild != null)
        {
            _pathFindingNode = WorldBuilder._nodeBuilder.CreatePathFindingNode(transform, unitID);
            Vector3 moveOffset = (Vector3)GetCubeMovementOffset() * 0.5f;
            _pathFindingNode.transform.localPosition = new Vector3(moveOffset.x, moveOffset.y, moveOffset.z);
            _pathFindingNode.GetComponent<PathFindingNode>().CubeParentLoc = CubeID;

           // _platform_Panel_Cube._panelScriptChild.PanelPieceChangeColor("Yellow");
        }
    }

    public void DestroyPathFindingNode()
    {
        if (_pathFindingNode != null)
        {
            _pathFindingNode.GetComponent<PathFindingNode>().DestroyNode();
            if (CubeOccupied == false)
            {
                if (_platform_Panel_Cube != null && _platform_Panel_Cube._panelScriptChild != null)
                {
                    _platform_Panel_Cube._panelScriptChild.PanelIsDEActive();
                }
            }
        }
        ResetPathFindingValues();
    }


    public void DestroyCubeIfNotImportant()
    {
        if (CubeIsPanel)
            return;

        if (CubePlatform)
            return;

        if (CubeIsSlope)
            return;

        Destroy(this);
    }

}
