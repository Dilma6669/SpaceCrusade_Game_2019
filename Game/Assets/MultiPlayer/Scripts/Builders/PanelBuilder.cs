using UnityEngine;

public class PanelBuilder : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static PanelBuilder _instance;

    private ObjectBuilder objectbuilder;

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

        objectbuilder = FindObjectOfType<ObjectBuilder>();
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    public static void CreatePanelForCube(int[] cubeData, CubeLocationScript cubeScript)
    {
        GameObject panelObject = WorldBuilder._nodeBuilder.CreatePanelObject(cubeScript);

        panelObject.GetComponent<ObjectScript>().objectStyle = (CubeObjectStyles)cubeData[1];

        if (panelObject.GetComponent<Renderer>())
        {
            ObjectTextures texEnum = (ObjectTextures)cubeData[2];
            string matName = "MapTextures/Materials/" + texEnum.ToString();
            Material mat = (Material)Resources.Load(matName, typeof(Material));
            panelObject.GetComponent<Renderer>().material = mat;
        }


        PanelPieceScript panelScript = panelObject.GetComponent<PanelPieceScript>();

        int rotX = cubeData[4];
        int rotY = cubeData[5];
        int rotZ = cubeData[6];

        panelObject.GetComponent<ObjectScript>().forcedRotation = new Vector3Int(rotX, rotY, rotZ);

        panelObject.transform.localScale *= 200;

        if (cubeData[0] == (int)CubeObjectTypes.Panel_Floor) // Floor
        {
            panelObject.GetComponent<ObjectScript>().objectType = CubeObjectTypes.Panel_Floor;
            cubeScript.CubeIsSlope = false;
            panelScript._panelYAxis = 0;
        }
        else if (cubeData[0] == (int)CubeObjectTypes.Panel_Wall) // Floor
        {
            panelObject.GetComponent<ObjectScript>().objectType = CubeObjectTypes.Panel_Wall;
            cubeScript.CubeIsSlope = false;

            rotZ = 90;

            panelScript._panelYAxis = rotY;

        }
        else if (cubeData[0] == (int)CubeObjectTypes.Panel_Angle) // Floor
        {
            panelObject.GetComponent<ObjectScript>().objectType = CubeObjectTypes.Panel_Angle;
            cubeScript.CubeIsSlope = true;
        }
        else
        {
            Debug.LogError("GOT AN ISSUE HERE");
        }

        //Debug.Log("fuck TEST >>>>>>>>>> rotX " + rotX + " + rotY " + rotY + " + rotZ " + rotZ);

        panelObject.transform.localRotation = Quaternion.Euler(new Vector3Int(rotX, rotY, rotZ));


       // panelObject.transform.tag = panelName;
       // panelScript.name = panelName;
    }

}
