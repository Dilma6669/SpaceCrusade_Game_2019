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

        Vector3Int camPos = camStartPos.Key;
        Quaternion camRot = Quaternion.Euler(camStartPos.Value);

        PlayerManager.PlayerAgent.SetUpPlayerStartPosition(camPos, camRot);
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
