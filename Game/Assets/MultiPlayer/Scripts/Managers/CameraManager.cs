using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static CameraManager _instance;

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

    public static void SetUpCameraAndLayers(int playerID, CameraAgent cameraAgent)
    {
        PlayerManager.CameraAgent = cameraAgent;

        KeyValuePair<Vector3Int, Vector3Int> camStartPos = GetCameraStartPosition(playerID);

        Debug.Log("fuck playerID " + playerID);
        Debug.Log("fuck camStartPos.key " + camStartPos.Key);

        Vector3 camPos = camStartPos.Key;
        Quaternion camRot = Quaternion.Euler(camStartPos.Value);

        PlayerManager.PlayerAgent.SetUpPlayerStartPosition(camPos, camRot);

        PlayerManager.CameraAgent._cameraPivotScript.angleH = camRot.eulerAngles.y;
        PlayerManager.CameraAgent._cameraPivotScript.angleV = -camRot.eulerAngles.x;

        /*

        // reveal layers up to current
        for (int i = 0; i <= _currLayer; i++) 
        {
            _camera.cullingMask |= 1 << LayerMask.NameToLayer("Floor" + i.ToString ());
        }
        */

        // units have already been put into correct layer now need to make camera see layer
        //string layerStr = "Player0" + playerID.ToString () + "Units";
        //_camera.cullingMask |= 1 << LayerMask.NameToLayer (layerStr);
    }

    public static KeyValuePair<Vector3Int, Vector3Int> GetCameraStartPosition(int playerID = -1)
    {
        return PlayerManager.GetPlayerStartPosition(playerID);
    }

    public static void SetCamToOrbitUnit(UnitScript unitScript)
    {
        //print("fuck SetCamToOrbitUnit unit " + unitScript.NetID.Value);
        PlayerManager.CameraAgent.SetCamAgentToOrbitUnit(unitScript);
    }

}
