using UnityEngine;

public class CubeBuilder : MonoBehaviour {

    ////////////////////////////////////////////////

    private static CubeBuilder _instance;

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

    public static CubeLocationScript CreateCubeObject(Vector3 gridLoc, int[] cubeData, int rotations, int nodeLayerCount, Transform parent)
    {
        GameObject cubeObject = WorldBuilder._nodeBuilder.CreateDefaultCube(gridLoc, rotations, nodeLayerCount, parent);
        CubeLocationScript cubeScript = cubeObject.GetComponent<CubeLocationScript>();
        //cubeScript.CubeAngle = cubeType;

        if (cubeData[2] != 00)
        {
            PanelBuilder.CreatePanelForCube(cubeData, cubeScript, 0, rotations);
        }
      
        SortOutCubeScriptShit(gridLoc, cubeScript);

        return cubeScript;
    }


    private static void SortOutCubeScriptShit(Vector3 GridLoc, CubeLocationScript cubeScript)
    {
        // If cube is movable or not
        if (cubeScript.CubeMoveable)
        {
          LocationManager.SetCubeScriptToLocation_CLIENT(GridLoc, cubeScript);
        }
        else
        {
            LocationManager.SetCubeScriptToHalfLocation_CLIENT(GridLoc, cubeScript);
        }

        // for layering system
        LayerManager.AddCubeToLayer(cubeScript);
    }

    ////////////////////////////////////////////////

}
