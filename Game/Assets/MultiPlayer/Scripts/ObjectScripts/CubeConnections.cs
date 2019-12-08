using UnityEngine;

public class CubeConnections : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static CubeConnections _instance;

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

    // an attempt to make cubes as platforms FROM the neighbouring cubes with panels
    // the Full cube with SLOPE panel in it will be passed into this (differnt to function underneath) --dont think they are similar, they are not
    public static void SetCubeNeighbours(CubeLocationScript cubeScript)
    {
        cubeScript.SetNeighbourVects();

        // an attempt to make all sourounding cubes of Slope into movable cubes
        foreach (Vector3 vect in cubeScript.NeighbourVects)
        {
            CubeLocationScript script = LocationManager.GetLocationScript_CLIENT(vect);

            if (script != null)
            {
                if (script.CubeMoveable && !script.CubeIsPanel)
                {
                    script.SetNeighbourVects();
                    script.CubePlatform = true;
                    //Debug.Log("fuck script.CubePlatform >> " + vect);
                }
            }
        }
        SetUpPanelInCube(cubeScript); // this call can only be made if slope panel inside full cube
    }

    // an attempt to make cubes as platforms FROM the neighbouring cubes with panels
    // the panel half cube will be passed into this
    public static void SetCubeHalfNeighbours(CubeLocationScript cubeHalfScript)
    {
        cubeHalfScript.SetHalfNeighbourVects(); // for the annoying slope issue

        // so this is essentially going through the proper neighbour cubes around the half panel cube
        foreach (Vector3 vect in cubeHalfScript.NeighbourHalfVects)
        {
            // this will only return valid full cubes, only 2 will come thru, above and below
            CubeLocationScript cubeScript = LocationManager.GetLocationScript_CLIENT(vect);

            if (cubeScript != null)
            {
                if (cubeScript.CubeMoveable && !cubeScript.CubeIsPanel)
                {
                    cubeScript.CubePlatform = true;
                }
                cubeScript.SetNeighbourVects(); // for the annoying slope issue
            }                                   // NO CALL TO SETUPPANELINCUBE //IMPORTANT
        }

        SetUpPanelInCube(cubeHalfScript);
    }

    ////////////////////////////////////////////////


    // If ANY kind of wall/floor/object make neighbour cubes walkable
    public static void SetUpPanelInCube(CubeLocationScript neighbourHalfScript) {

		PanelPieceScript panelScript = neighbourHalfScript._panelScriptChild;

        switch (panelScript.name) 
		{
		case "Panel_Floor":
			SetUpFloorPanel (neighbourHalfScript, panelScript);
			break;
		case "Panel_Wall":
			SetUpWallPanel (neighbourHalfScript, panelScript);
			break;
		case "Panel_Angle": // angles put in half points
			SetUpFloorAnglePanel (neighbourHalfScript, panelScript);
			break;
		default:
			Debug.Log ("fuck no issue:  " + panelScript.name);
			break;
		}
	}


    private static void SetHumanCubeRules(CubeLocationScript cubeScript, bool walkable, bool climable, bool jumpable)
    {
        cubeScript.IsHumanWalkable = walkable;
        cubeScript.IsHumanClimbable = climable;
        cubeScript.IsHumanJumpable = jumpable;
    }
    private static void SetAlienCubeRules(CubeLocationScript cubeScript, bool walkable, bool climable, bool jumpable)
    {
        cubeScript.IsAlienWalkable = walkable;
        cubeScript.IsAlienClimbable = climable;
        cubeScript.IsAlienJumpable = jumpable;
    }


    private static void SetUpFloorPanel(CubeLocationScript neighbourHalfScript, PanelPieceScript panelScript) {

        Vector3 cubeHalfLoc = neighbourHalfScript.CubeID;

        Vector3 leftVect = new Vector3 (cubeHalfLoc.x, cubeHalfLoc.y - 1, cubeHalfLoc.z);
        CubeLocationScript cubeScriptLeft = LocationManager.GetLocationScript_CLIENT(leftVect); // underneath panel
        if (cubeScriptLeft != null) {
            panelScript.cubeScriptLeft = cubeScriptLeft;
			panelScript.cubeLeftVector = leftVect;
            cubeScriptLeft._platform_Panel_Cube = neighbourHalfScript;

            SetHumanCubeRules(cubeScriptLeft, false, false, false);
            SetAlienCubeRules(cubeScriptLeft, true, true, true);
            cubeScriptLeft.IS_HUMAN_MOVABLE = false;
            cubeScriptLeft.IS_ALIEN_MOVABLE = true;
        }
        else
        {
            Debug.LogError("Got an issue here");
        }


        Vector3 rightVect = new Vector3(cubeHalfLoc.x, cubeHalfLoc.y + 1, cubeHalfLoc.z);
        CubeLocationScript cubeScriptRight = LocationManager.GetLocationScript_CLIENT(rightVect); // Ontop of panel
        if (cubeScriptRight != null)
        {
            panelScript.cubeScriptRight = cubeScriptRight;
            panelScript.cubeRightVector = rightVect;
            cubeScriptRight._platform_Panel_Cube = neighbourHalfScript;

            SetHumanCubeRules(cubeScriptRight, true, true, true);
            SetAlienCubeRules(cubeScriptRight, true, true, true);
            cubeScriptRight.IS_HUMAN_MOVABLE = true;
            cubeScriptRight.IS_ALIEN_MOVABLE = true;
        }
        else
        {
            Debug.LogError("Got an issue here");
        }

        if (cubeScriptLeft == null) {
			panelScript.cubeScriptLeft = panelScript.cubeScriptRight;
			panelScript.cubeLeftVector = panelScript.cubeRightVector;
            //Debug.LogWarning("cubeScript == null so making neighbours same cube");
        }
		if (cubeScriptRight == null) {
			panelScript.cubeScriptRight = panelScript.cubeScriptLeft;
			panelScript.cubeRightVector = panelScript.cubeLeftVector;
            //Debug.LogWarning("cubeScript == null so making neighbours same cube");
        }
	}


	private static void SetUpWallPanel(CubeLocationScript neighbourHalfScript, PanelPieceScript panelScript) {

        Vector3 cubeHalfLoc = neighbourHalfScript.CubeID;

        int cubeAngle = neighbourHalfScript.CubeAngle;
		int panelAngle = panelScript._panelYAxis;

		//panelScript._isLadder = true;

		int result = (cubeAngle - panelAngle);
		result = (((result + 180) + 360) % 360) - 180; //result = (((result + 180) % 360 + 360) % 360) - 180;

        CubeLocationScript cubeScriptLeft = null;
        CubeLocationScript cubeScriptRight = null;

        Vector3 leftVect = Vector3.zero;
        Vector3 rightVect = Vector3.zero;

        if (result == 180 || result == -180 || result == 0) // Down
        {
            leftVect = new Vector3(cubeHalfLoc.x, cubeHalfLoc.y, cubeHalfLoc.z - 1);
            rightVect = new Vector3(cubeHalfLoc.x, cubeHalfLoc.y, cubeHalfLoc.z + 1);
        }
        else if (result == 90 || result == -90)  //across 
        {
            leftVect = new Vector3(cubeHalfLoc.x - 1, cubeHalfLoc.y, cubeHalfLoc.z);
            rightVect = new Vector3(cubeHalfLoc.x + 1, cubeHalfLoc.y, cubeHalfLoc.z);
        }
        else
        {
			Debug.Log("SOMETHING weird: cubeAngle: " + cubeAngle + " panelAngle: " + panelAngle + " <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
		}

        //Debug.Log ("cubeAngle: " + cubeAngle + " panelAngle: " + panelAngle + " result: " + result);

        cubeScriptLeft = LocationManager.GetLocationScript_CLIENT(leftVect);
        if (cubeScriptLeft != null)
        {
            panelScript.cubeScriptLeft = cubeScriptLeft;
            panelScript.cubeLeftVector = leftVect;
            cubeScriptLeft._platform_Panel_Cube = neighbourHalfScript;

            if (panelScript._isLadder)
            {
                SetHumanCubeRules(cubeScriptLeft, true, true, true);
                cubeScriptLeft.IS_HUMAN_MOVABLE = true;
            }
            else
            {
                SetHumanCubeRules(cubeScriptLeft, false, false, false);
                cubeScriptLeft.IS_HUMAN_MOVABLE = false;
            }

            SetAlienCubeRules(cubeScriptLeft, true, true, true);
            cubeScriptLeft.IS_ALIEN_MOVABLE = true;
        }

         cubeScriptRight = LocationManager.GetLocationScript_CLIENT(rightVect);
        if (cubeScriptRight != null)
        {
            panelScript.cubeScriptRight = cubeScriptRight;
            panelScript.cubeRightVector = rightVect;
            cubeScriptRight._platform_Panel_Cube = neighbourHalfScript;

            if (panelScript._isLadder)
            {
                SetHumanCubeRules(cubeScriptRight, true, true, true);
                cubeScriptRight.IS_HUMAN_MOVABLE = true;
            }
            else
            {
                SetHumanCubeRules(cubeScriptRight, false, false, false);
                cubeScriptRight.IS_HUMAN_MOVABLE = false;
            }

            SetAlienCubeRules(cubeScriptRight, true, true, true);
            cubeScriptRight.IS_ALIEN_MOVABLE = true;
        }

		if (cubeScriptLeft == null) {
			panelScript.cubeScriptLeft = panelScript.cubeScriptRight;
			panelScript.cubeLeftVector = panelScript.cubeRightVector;
		}
		if (cubeScriptRight == null) {
			panelScript.cubeScriptRight = panelScript.cubeScriptLeft;
			panelScript.cubeRightVector = panelScript.cubeLeftVector;
		}
	}


    // this is a bit different, the actual MOVEABLE cube script gets passed in here coz slopes sit in the cube object not the half cube objects
    // THIS IS GOING TO CAUSE PROBLEMS IN FUTURE COZ THERES NO CHECKS IF THEY CAN MOVE ONTO SLOPE, ITS ALWAYS YES
    private static void SetUpFloorAnglePanel(CubeLocationScript cubeScript, PanelPieceScript panelScript)
    { 
        Vector3 cubeLoc = cubeScript.CubeID;

        Vector3 rightVect = new Vector3(cubeLoc.x, cubeLoc.y + 2, cubeLoc.z);
        Vector3 leftVect = new Vector3(cubeLoc.x, cubeLoc.y, cubeLoc.z); // Underneath ( I think)

        //int panelYAxis = panelScript._panelYAxis;

        //if (panelYAxis == 180 || panelYAxis == -180 || panelYAxis == 0) // Down
        //{
        //    rightVect = new Vector3(cubeLoc.x, cubeLoc.y + 1, cubeLoc.z + 1); // OnTop ( I think)
        //}
        //else if (panelYAxis == 90 || panelYAxis == -90 || panelYAxis == 270 )  //across 
        //{
        //    rightVect = new Vector3(cubeLoc.x + 1, cubeLoc.y + 1, cubeLoc.z); // OnTop ( I think)
        //}
        //else
        //{
        //    Debug.Log("SOMETHING weird: cubeAngle: " + panelYAxis + " panelAngle: " + panelYAxis + " <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        //}

        CubeLocationScript cubeScriptRight = LocationManager.GetLocationScript_CLIENT(rightVect);
        if (cubeScriptRight != null)
        {
            panelScript.cubeScriptRight = cubeScriptRight;
            panelScript.cubeRightVector = rightVect;
            cubeScriptRight._platform_Panel_Cube = cubeScript;

            SetHumanCubeRules(cubeScriptRight, true, true, true);
            SetAlienCubeRules(cubeScriptRight, true, true, true);
            cubeScriptRight.IS_HUMAN_MOVABLE = true;
            cubeScriptRight.IS_ALIEN_MOVABLE = true;
        }

        leftVect = new Vector3(cubeLoc.x, cubeLoc.y - 2, cubeLoc.z); // Underneath ( I think)
        CubeLocationScript cubeScriptLeft = LocationManager.GetLocationScript_CLIENT(leftVect);

        if (cubeScriptLeft != null)
        {
            panelScript.cubeScriptLeft = cubeScriptLeft;
            panelScript.cubeLeftVector = leftVect;
            cubeScriptLeft._platform_Panel_Cube = cubeScript;

            SetHumanCubeRules(cubeScriptLeft, true, true, true);
            SetAlienCubeRules(cubeScriptLeft, true, true, true);
            cubeScriptLeft.IS_HUMAN_MOVABLE = true;
            cubeScriptLeft.IS_ALIEN_MOVABLE = true;
        }

        if (cubeScriptLeft == null)
        {
            panelScript.cubeScriptLeft = panelScript.cubeScriptRight;
            panelScript.cubeLeftVector = panelScript.cubeRightVector;
            //Debug.LogWarning("cubeScript == null so making neighbours same cube");
        }
        if (cubeScriptRight == null)
        {
            panelScript.cubeScriptRight = panelScript.cubeScriptLeft;
            panelScript.cubeRightVector = panelScript.cubeLeftVector;
            //Debug.LogWarning("cubeScript == null so making neighbours same cube");
        }
    }

    // this is a bit different, the actual MOVEABLE cube script gets passed in here coz slopes sit in the cube object not the half cube objects
    // THIS IS GOING TO CAUSE PROBLEMS IN FUTURE COZ THERES NO CHECKS IF THEY CAN MOVE ONTO SLOPE, ITS ALWAYS YES
    private static void SetUpCeilingAnglePanel(CubeLocationScript cubeScript, PanelPieceScript panelScript)
    {
        Vector3 cubeLoc = cubeScript.CubeID;

        Vector3 TopHalfVect = new Vector3(cubeLoc.x, cubeLoc.y + 1, cubeLoc.z);
        Vector3 bottomHalfVect = new Vector3(cubeLoc.x, cubeLoc.y - 1, cubeLoc.z);
        CubeLocationScript cubeScriptHalfTop = LocationManager.GetHalfLocationScript_CLIENT(TopHalfVect); // ontop panel
        CubeLocationScript cubeScriptHalfBottom = LocationManager.GetHalfLocationScript_CLIENT(bottomHalfVect); // underneath panel

        if (cubeScriptHalfBottom != null) // ontop
        {
            panelScript.cubeScriptRight = cubeScript;
            panelScript.cubeRightVector = cubeLoc;
            cubeScriptHalfBottom._platform_Panel_Cube = cubeScript;

            SetHumanCubeRules(cubeScript, true, true, true);
            SetAlienCubeRules(cubeScript, true, true, true);
            cubeScriptHalfBottom.IS_HUMAN_MOVABLE = true;
            cubeScriptHalfBottom.IS_ALIEN_MOVABLE = true;
        }

        if (cubeScriptHalfTop != null) // underneath
        {
            panelScript.cubeScriptLeft = cubeScript;
            panelScript.cubeLeftVector = cubeLoc;
            cubeScriptHalfTop._platform_Panel_Cube = cubeScript;

            SetHumanCubeRules(cubeScript, true, true, true);
            SetAlienCubeRules(cubeScript, true, true, true);
            cubeScriptHalfTop.IS_HUMAN_MOVABLE = true;
            cubeScriptHalfTop.IS_ALIEN_MOVABLE = true;
        }
    }
}
