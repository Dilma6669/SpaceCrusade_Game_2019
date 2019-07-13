using UnityEngine;

public class PanelBuilder : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static PanelBuilder _instance;

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

    public static void CreatePanelForCube(int[] cubeData, CubeLocationScript cubeScript, int angle, int rotations)
    {

        Transform cubeTrans = cubeScript.GetComponent<Transform>();
        GameObject panelObject = WorldBuilder._nodeBuilder.CreatePanelObject(cubeTrans);
        PanelPieceScript panelScript = panelObject.GetComponent<PanelPieceScript>();

        int rotX = cubeData[3];
        int rotY = cubeData[4];
        int rotZ = cubeData[5];

        panelObject.transform.localPosition = new Vector3(0, 0, 0);
        panelObject.transform.localEulerAngles = new Vector3(rotX, rotY, rotZ);

        string name = "";

        if ((rotX == 90 || rotX == 270) && rotY == 0) // Floor
        {
            cubeScript.CubeIsSlope = false;
            //panelObject.transform.localScale = new Vector3(1, 1, 1);
            name = "Panel_Floor";
        }
        else if (rotX == 0 && rotZ == 0) // Wall
        {
            cubeScript.CubeIsSlope = false;
            //panelObject.transform.localScale = new Vector3(1, 1, 1);
            name = "Panel_Wall";
        }
        else
        {
            cubeScript.CubeIsSlope = true;
            panelObject.transform.localScale = new Vector3(20, 30, 1);
            name = "Panel_Angle";
        }

        panelObject.transform.tag = name;
        panelScript.name = name;

        panelScript.panelAngle = cubeData[4];
        cubeScript.PanelChildAngle = cubeData[4];

    }

}
