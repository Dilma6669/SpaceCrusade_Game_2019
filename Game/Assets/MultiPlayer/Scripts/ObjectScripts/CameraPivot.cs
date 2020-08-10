using System.Collections;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    ////////////////////////////////////////////////

    private Transform _cameraContainer;

    private Transform _cameraTransparentBox;
    private int _cameraTransparentPivot;  // the point in which when the camera moves vertically below makes panels and objects below camera transparent. Note its in reverse, negative is up, positive is down

    private float mouseRotationSpeed = 10f;                         // Horizontal turn speed.
    private float keysMovementSpeed = 1.0f;                           // Vertical turn speed.

    private float zoomMinFOV = 0f;
    private float zoomMaxFOV = -100f;
    private float zoomSensitivity = 5f;

    private float maxVerticalAngle = 50f;                               // Camera max clamp angle. 
    private float minVerticalAngle = -50f;                                 // Camera min clamp angle.

    private float h;  // key Hor
    private float v;  // key Vert
    private float x;  // Mouse Hor
    private float y;  // Mouse Vert
    private float d;  //scroll wheel

    public float angleH = 0;                                          // Float to store camera horizontal angle related to mouse movement.
    public float angleV = 0;                                          // Float to store camera vertical angle related to mouse movement.

    public float freeRoamAngleH = 0;                                           // Float to store camera horizontal angle related to mouse movement.
    public float freeRoamAngleV = 0;                                           // Float to store camera vertical angle related to mouse movement.

    private float targetFOV;                                           // Target camera FIeld of View.
    private float targetMaxVerticalAngle;                              // Custom camera max vertical clamp angle. 

    private float _cameraChangeUnitSpeed = 30f;

    public Transform CameraContainer
    {
        get { return _cameraContainer; }
        set { _cameraContainer = value; }
    }


    private bool freeRoamCamera = false;

    public bool CAMERA_MOVING = false;
    public static int CAMERA_STATE_GLOBAL = 0; // hopefully global rotations
    public static int CAMERA_STATE_LOCAL = 1; // hopefully local rotations

    public static int _cameraState;

    public static int CameraState
    {
        get { return _cameraState; }
        set { _cameraState = value; }
    }

    public static bool CameraStateIsLocal
    {
        get { return _cameraState == CAMERA_STATE_LOCAL; }
    }
    public static bool CameraStateIsGlobal
    {
        get { return _cameraState == CAMERA_STATE_GLOBAL; }
    }

    public float CameraVerticalAngle
    {
        get { return angleV; }
    }

    public float CameraTransparentPivot
    {
        get { return _cameraTransparentPivot; }
    }

    ////////////////////////////////////////////////

    private float _turnSpeed = 4.0f;

    private Vector3 _offset;

    private Transform playerPivotTrans;
    private Vector3 playerPivotRot;
    private Vector3 cameraPivotRot;

    ////////////////////////////////////////////////

    void Awake()
    {
        CameraContainer = transform.Find("CameraObject");
        if (CameraContainer == null) { Debug.LogError("We got a problem here"); }

        _cameraTransparentBox = transform.FindDeepChild("CameraTransparentBox");
        if (_cameraTransparentBox == null) { Debug.LogError("We got a problem here"); }

        _cameraTransparentPivot = 10;

    }

    void Start()
    {
        CameraState = CAMERA_STATE_GLOBAL;
    }

    ///////////////////////////////////////

    void FixedUpdate()
    {
        if (!PlayerManager.CameraAgent.isLocalPlayer) return;

        ///////////////////////////////////////

        // mouse look around
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        d = Input.GetAxis("Mouse ScrollWheel");

        // Basic Movement Player //
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (freeRoamCamera)
        {
            // For Free Roam
            KeyboardMovement();

            if (Input.GetMouseButton(2))
            {
                // Get mouse movement to orbit the camera.
                freeRoamAngleH += Mathf.Clamp(x, -1, 1) * mouseRotationSpeed;
                freeRoamAngleV += Mathf.Clamp(y, -1, 1) * mouseRotationSpeed;

                // Set camera orientation..
                Quaternion aimRotation = Quaternion.Euler(-freeRoamAngleV, freeRoamAngleH, 0);
                transform.rotation = aimRotation;
            }
        }
        else
        {
            if (Input.GetMouseButton(2))
            {
                CameraMove();
            }
            else
            {
                CameraStop();
            }

            CameraZoom();
        }
    }

    ///////////////////////////////////////
    void CameraMove()
    {
        CAMERA_MOVING = true;

        if (playerPivotTrans)
        {
            float cameraX = x;
            float cameraY = y;

            int unitAngle = UnitsManager.ActiveUnit.UnitAngle;

            if (CameraStateIsGlobal)
            {
                angleH += Mathf.Clamp(cameraX, -1, 1) * mouseRotationSpeed;
                angleV += Mathf.Clamp(cameraY, -1, 1) * mouseRotationSpeed;

                // couple max distance measures
                //if (angleV >= maxVerticalAngle)
                //    angleV = maxVerticalAngle;

                //if (angleV <= minVerticalAngle)
                //    angleV = minVerticalAngle;
                ///////

                float angH = angleH;
                float angV = angleV;

                // Set camera orientation..
                if (x != 0 || y != 0)
                {
                    //if (unitAngle == 0)
                    // angV = -angleV; //angV = 0; //Change this back if dont want x and y rotation

                    //if (unitAngle == 45)
                    //    angV = -angleV;

                    //if (unitAngle == 90)
                    //    angV = -angleV;

                    transform.localEulerAngles = new Vector3(-angV, angH, 0);
                }

                //adjusting the camera transparent box size depending on camera Y axis
                CameraSetTransparentBox();

            }
            else
            {
                // if (unitAngle == 0)
                // {
                angleH += Mathf.Clamp(cameraX, -1, 1) * mouseRotationSpeed;
                angleV += Mathf.Clamp(cameraY, -1, 1) * mouseRotationSpeed;

                // Set camera orientation..
                if (x != 0 || y != 0)
                {
                    Quaternion aimRotation = Quaternion.Euler(0, angleH, 0);
                    transform.rotation = aimRotation;
                }

                // To Stop camera always sticking to World Y Axis
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                // }
                //else if (unitAngle == 45)
                //{
                //    cameraX = x;
                //    cameraY = y;
                //    transform.localEulerAngles = new Vector3(0, 0, 0);
                //    CameraContainer.transform.localEulerAngles = new Vector3(cameraContainerAngle.x, cameraContainerAngle.y, -unitAngle);
                //}
                //else if (unitAngle == 90)
                //{
                //    cameraX = x;
                //    cameraY = y;

                //    angleH += Mathf.Clamp(cameraY, -1, 1) * mouseRotationSpeed;
                //    angleV += Mathf.Clamp(cameraX, -1, 1) * mouseRotationSpeed;

                //    // Set camera orientation..
                //    if (x != 0 || y != 0)
                //    {
                //        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
                //        transform.rotation = aimRotation;
                //    }

                //    // To Stop camera always sticking to World Y Axis
                //    transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

                //}
            }
        }
        UnitsManager.ActiveUnit.ClearPathFindingNodes();
    }

    void CameraStop()
    {
        CAMERA_MOVING = false;
    }

    void CameraZoom()
    {
        Vector3 cameraLocalPos = PlayerManager.CameraAgent.CameraContainer.transform.localPosition;
        if (d < 0f)
        {
            if (cameraLocalPos.z <= zoomMaxFOV)
                return;

            _offset = new Vector3(0, 0, -1);
        }
        else if (d > 0f)
        {
            if (cameraLocalPos.z >= zoomMinFOV)
                return;

            _offset = new Vector3(0, 0, 1);
        }
        PlayerManager.CameraAgent.CameraContainer.transform.localPosition += (_offset * zoomSensitivity);
        _offset = Vector3Int.zero;
    }

    ///////////////////////////////////////

    void CameraSetTransparentBox()
    {
        _cameraTransparentBox.localScale = Vector3Int.one;
        Vector3 localScale = _cameraTransparentBox.localScale;
        float absAngle = Mathf.Abs(angleV * 0.1f);

        if (angleV <= 0)
            _cameraTransparentBox.localScale = new Vector3(localScale.x + absAngle, localScale.y + absAngle, localScale.z);
    }

    ///////////////////////////////////////

    void KeyboardMovement()
    {
        if (freeRoamCamera)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                keysMovementSpeed = 5.0f;
            }
            else
            {
                keysMovementSpeed = 1.0f;
            }

            //Sets x and y basic movement
            transform.Translate(new Vector3(keysMovementSpeed * h, 0, 0));
            transform.Translate(new Vector3(0, 0, keysMovementSpeed * v));
        }
    }

    ///////////////////////////////////////

    // need a smooth transition when camera moves from unit to unit here, not just jump suddenly
    public void SetNewPivot(GameObject pivot)
    {
        StartCoroutine("MoveCameraTowards", pivot);

        if (freeRoamCamera)
        {
            SetFreeRoamCamera();
            return;
        }
    }

    IEnumerator MoveCameraTowards(GameObject pivot)
    {
        Vector3 targetPos = pivot.transform.position;

        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * _cameraChangeUnitSpeed);
            yield return null;
        }

        if (transform.position == targetPos)
        {
            transform.SetParent(pivot.transform);
            transform.localPosition = Vector3Int.zero;

            playerPivotTrans = pivot.transform;

            //PlayerManager.CameraAgent.CameraContainer.transform.localPosition = new Vector3Int(0, 0, -7); // y = 4
        }
    }


    public void SetFreeRoamCamera()
    {
        transform.SetParent(PlayerManager.PlayerAgent.gameObject.transform);
        transform.localPosition = Vector3Int.zero;
        transform.localEulerAngles = Vector3Int.zero;

        PlayerManager.CameraAgent.CameraContainer.transform.localPosition = Vector3Int.zero;
        PlayerManager.CameraAgent.CameraContainer.transform.localEulerAngles = Vector3Int.zero;
        CameraMove();
    }
}

