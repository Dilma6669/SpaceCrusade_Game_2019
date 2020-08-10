using UnityEngine;
using UnityEngine.Networking;

public class CameraAgent : NetworkBehaviour
{
    ////////////////////////////////////////////////

    private Camera _camera;
    private Transform _cameraContainer;
    private CameraPivot _cameraPivot;

    ////////////////////////////////////////////////


    public Camera MainCamera
    {
        get { return _camera; }
        set { _camera = value; }
    }

    public Transform CameraContainer
    {
        get { return _cameraContainer; }
        set { _cameraContainer = value; }
    }

    public CameraPivot Camera_Pivot
    {
        get { return _cameraPivot; }
        set { _cameraPivot = value; }
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    void Awake()
    {
        Camera_Pivot = transform.FindDeepChild("CameraPivot").GetComponent<CameraPivot>();
        if (Camera_Pivot == null) { Debug.LogError("We got a problem here");}

        CameraContainer = Camera_Pivot.transform.Find("CameraObject");
        if (CameraContainer == null) { Debug.LogError("We got a problem here");}

        MainCamera = CameraContainer.GetComponent<Camera>();
        if (_camera == null) { Debug.LogError("We got a problem here"); }

        _camera.enabled = false;
    }

    void Start()
    {
        if (!isLocalPlayer) return;

        _camera.enabled = true;

        PlayerManager.CameraAgent = this;
    }

    ////////////////////////////////////////////////

    public void ChangeCameraState()
    {
        if(CameraPivot.CameraStateIsGlobal)
        {
            CameraPivot.CameraState = CameraPivot.CAMERA_STATE_LOCAL;
        }
        else
        {
            CameraPivot.CameraState = CameraPivot.CAMERA_STATE_GLOBAL;
        }
    }

    ////////////////////////////////////////////////

    public void SetCamAgentToOrbitUnit(UnitScript unitScript)
    {
        if (!isLocalPlayer) return;

        if (unitScript.PlayerPivot == null)
        {
            Debug.LogError("ERROR UnitScript.PlayerPivot == null unitScript.NetID.Value: " + unitScript.UnitID);
        }
        Camera_Pivot.SetNewPivot(unitScript.PlayerPivot);
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////
}