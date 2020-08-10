using UnityEngine.EventSystems;
using UnityEngine;

public class PanelPieceScript : MonoBehaviour {

    Renderer _rend;

    static Camera mainCamera;

    public bool PANEL_HAS_ERROR = false;

    public bool _panelActive = false;

	public int _panelYAxis = 0;

    public Vector3Int cubeLeftVector;
	public Vector3Int cubeRightVector;

	public CubeLocationScript cubeScriptParent = null;
	public CubeLocationScript cubeScriptRight = null; // Ontop (Floor)
    public CubeLocationScript cubeScriptLeft = null; // Underneath (Floor)

    public CubeLocationScript _activeCubeScript = null;

    public int _panelHitIndex = 0;

    private bool _movementPossible = false;
    private bool _panelVisible = true;


    public bool panelBelowUnit;
    public bool cameraAboveUnitPivot;

    private Material normalMat;
    private Material diffuseMat;

    private MeshCollider meshCollider;

    // Use this for initialization
    void Start () {
		_rend = GetComponent<Renderer> ();
        transform.gameObject.layer = LayerManager.LAYER_ENVIRONMENT;

        if (PlayerManager.CameraAgent)
            mainCamera = PlayerManager.CameraAgent.MainCamera;

        if (_rend)
            normalMat = _rend.material;

        diffuseMat = (Material)Resources.Load("MapTextures/Materials/Grate_Diffuse", typeof(Material));

        meshCollider = GetComponent<MeshCollider>();
        meshCollider.enabled = false;
    }

    void Update()
    {
        if (PlayerManager.CameraAgent)
        {
            float camVertAngle = PlayerManager.CameraAgent.Camera_Pivot.CameraVerticalAngle;
            cameraAboveUnitPivot = camVertAngle < PlayerManager.CameraAgent.Camera_Pivot.CameraTransparentPivot;
            panelBelowUnit = DetermineIfPanelIsLowerThanUnit();
        }
    }

    //////////////////////////////////////////////
    public void PanelPieceChangeColor(string color)
    {
        if(_rend == null)
            _rend = GetComponent<Renderer>();

        if (PANEL_HAS_ERROR)
            color = "Red";

        if (_rend == null)
            return;

        Color origColour = _rend.material.color;
        Color tempColor = Color.white;

        switch (color)
        {
            case "Red":
                tempColor = Color.red;
                break;
            case "Black":
                tempColor = Color.black;
                break;
            case "White":
                tempColor = Color.white;
                break;
            case "Green":
                tempColor = Color.green;
                break;
            case "Pink":
                tempColor = Color.magenta;
                break;
            case "Yellow":
                tempColor = Color.yellow;
                break;
            default:
                break;
        }

        tempColor.a = origColour.a;
        _rend.material.color = tempColor;
    }
    //////////////////////////////////////////////

    //////////////////////////////////////////////
    public void PanelIsActive()
    {
        return;
        //PanelPieceChangeColor("Green");
    }

    public void PanelIsDEActive()
    {
        return;
           //PanelPieceChangeColor("White");
    }
    //////////////////////////////////////////////

    public bool ActivePanelSideIsLeft()
    {
        if (ReferenceEquals(_activeCubeScript, cubeScriptLeft))
        {
            return true;
        }
        else if (ReferenceEquals(_activeCubeScript, cubeScriptRight))
        {
            return false;
        }
        else
        {
            Debug.LogError("fuck ISSUE HERE _activeCubeScript = " + _activeCubeScript);
        }
        return false;
    }


    //////////////////////////////////////////////
    private bool DetermineIfPanelIsLowerThanUnit() // Compares cube local locations 
    {
        if (UnitsManager.ActiveUnit == null)
            return false;

        CubeLocationScript unitCube = UnitsManager.ActiveUnit.CubeUnitIsOn;

        //Debug.Log("fuck unitCube = " + unitCube);
        //Debug.Log("fuck unitCube.MapNodeParent = " + unitCube.MapNodeParent);

        float unitMapNodeYLocPos = unitCube.MapNodeParent.transform.localPosition.y;
        float thisMapNodeYLocPos = cubeScriptParent.MapNodeParent.transform.localPosition.y;

        if (thisMapNodeYLocPos > unitMapNodeYLocPos) // Mapnodes above unit always turn transpaarent
            return false;

        float unitCubeYLocPos = unitCube.transform.localPosition.y;
        float thisCubeYLocPos = cubeScriptParent.transform.localPosition.y;

        return unitCubeYLocPos > thisCubeYLocPos;
    }

    private bool PanelIsAllowedToGoInvisible()
    {
        //if (cameraAboveUnitPivot)
        //{
        //    if (panelBelowUnit)
        //        return false;
        //}
        return true;
    }
    //////////////////////////////////////////////

    //////////////////////////////////////////////
    public void GoHalfTransparent()
    {
        if (_rend)
        {
            _rend.material = diffuseMat;

            _panelVisible = false;
            gameObject.layer = LayerManager.LAYER_IGNORE_RAYCAST;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    //public void GoFullTransparent()
    //{
    //    _rend.material = diffuseMat;
    //    _panelVisible = false;
    //    gameObject.layer = LayerManager.LAYER_IGNORE_RAYCAST;
    //    GetComponent<MeshRenderer>().enabled = false;
    //}

    public void GoNotTransparent()
    {
        if (_rend)
        {
            _rend.material = normalMat;
            _panelVisible = true;
            gameObject.layer = LayerManager.LAYER_ENVIRONMENT;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
    //////////////////////////////////////////////

    //////////////////////////////////////////////
    void OnMouseDown()
    {
        if (_movementPossible)
        {
            MovementManager.MakeActiveUnitMove_CLIENT(); // this just checks is a unit is currently selected and then does the moving shit
        }
    }

    //////////////////////////////////////////////
    void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit hitPoint;

        if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hitPoint, 100.0F))
        {
            //Debug.Log("fuck hitPoint.gameObject " + hitPoint.transform.gameObject.name);

            if (!PlayerManager.CameraAgent.Camera_Pivot.CAMERA_MOVING)
            {
                _panelHitIndex = 0;

                int[] surface1 = new int[2] { 2, 3 }; // << Put surface triangle Indexs in here
                int[] surface2 = new int[2] { 6, 7 }; // << Put surface triangle Indexs in here

                int triIndex = hitPoint.triangleIndex;
                //Debug.Log("Hit Triangle index : " + hitPoint.triangleIndex);

                if (triIndex != surface1[0] && triIndex != surface1[1] && triIndex != surface2[0] && triIndex != surface2[1])
                {
                    _activeCubeScript = null;
                    Debug.Log("Hit Triangle index NOT REGISTERED: " + triIndex);
                    return;
                }

                if (hitPoint.collider.gameObject.GetComponent<PanelPieceScript>() != null)
                {
                    CubeObjectTypes panelType = hitPoint.collider.gameObject.GetComponent<ObjectScript>().objectType;

                    if (panelType == CubeObjectTypes.Panel_Floor)
                    {
                        if (triIndex == surface1[0] || triIndex == surface1[1]) // (if floor) To sit BELOW of panels
                            _activeCubeScript = cubeScriptRight;

                        if (triIndex == surface2[0] || triIndex == surface2[1]) // (if floor) To sit OnTop of panels
                            _activeCubeScript = cubeScriptLeft;
                    }

                    if (panelType == CubeObjectTypes.Panel_Wall)
                    {
                        if (triIndex == surface1[0] || triIndex == surface1[1]) // either 0 or 90 angle
                            _activeCubeScript = cubeScriptRight;

                        if (triIndex == surface2[0] || triIndex == surface2[1]) // either 0 or 90 angle
                            _activeCubeScript = cubeScriptLeft;
                    }

                    if (panelType == CubeObjectTypes.Panel_Angle)
                    {
                        if (triIndex == surface1[0] || triIndex == surface1[1]) // (if floor) To sit OnTop of panels
                            _activeCubeScript = cubeScriptRight;

                        if (triIndex == surface2[0] || triIndex == surface2[1]) // (if floor) To sit OnTop of panels
                            _activeCubeScript = cubeScriptLeft;
                    }


                    if (MovementManager.BuildMovementPath(_activeCubeScript))
                    {
                        //Debug.Log("fuck _movementPossible ");
                        //PanelPieceChangeColor("Pink");
                        _movementPossible = true;
                    }
                    else
                    {
                        //PanelPieceChangeColor("White");
                        _movementPossible = false;
                    }
                }
            }
        }
    }
    //////////////////////////////////////////////

    private void OnTriggerStay(Collider other)
    {
        meshCollider.enabled = true;

        if (!PanelIsAllowedToGoInvisible())
        {
            GoNotTransparent();
            return;
        }

        if (other.name == "PanelTrigger")
            GoHalfTransparent();
    }

    private void OnTriggerExit(Collider other)
    {
        meshCollider.enabled = false;

        if (!PanelIsAllowedToGoInvisible())
            return;

        if (other.name == "PanelTrigger")
            GoNotTransparent();
    }
}
