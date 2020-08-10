using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Builder_Grid : MonoBehaviour
{
    public GameObject GridContainer;

    public TextAsset TextFile;

    public List<Vector3> gridNodeLocations;

    public Dictionary<Vector3, int[]> objectsData = new Dictionary<Vector3, int[]>();

    void Start()
    {
        GetLocationsFromGridNodes();

        CollectObjectsData();

        SaveDataArrayIntoDoc();

        print("FINISHED!!!");

    }

    //////////////////////////////////////////////

    private Vector3 GetRoundedVector3(Vector3 pos)
    {
        float x = (float)Math.Round(pos.x, 2);
        float y = (float)Math.Round(pos.y, 2);
        float z = (float)Math.Round(pos.z, 2);

        return new Vector3(x, y, z);
    }

    private Vector3Int GetFloorToIntVector3(Vector3 floorVect)
    {
        int x = Mathf.FloorToInt(floorVect.x);
        int y = Mathf.FloorToInt(floorVect.y);
        int z = Mathf.FloorToInt(floorVect.z);

        return new Vector3Int(x, y, z);
    }

    private void GetLocationsFromGridNodes()
    {
        gridNodeLocations = new List<Vector3>();

        GridNode[] gridNodes = GridContainer.GetComponentsInChildren<GridNode>();

        foreach(GridNode gridNode in gridNodes)
        {
            gridNodeLocations.Add(GetRoundedVector3(gridNode.transform.position));
            //Debug.Log("gridNode.transform.position == " + gridNode.transform.position);
        }

        Debug.Log("Total Grid Node Locations == " + gridNodeLocations.Count);
    }

    private void CollectObjectsData()
    {
        ObjectScript[] objs = FindObjectsOfType(typeof(ObjectScript)) as ObjectScript[];

        objectsData = new Dictionary<Vector3, int[]>();

        foreach (ObjectScript obj in objs)
        {
            Vector3 objPos = obj.transform.position;
            Vector3 globalRot = obj.transform.rotation.eulerAngles;
            Vector3 localRot = obj.transform.localRotation.eulerAngles;

            Vector3 objFace = obj.transform.forward;

            Vector3Int forceRotation = obj.GetComponent<ObjectScript>().forcedRotation;
            int parentYAxis = (int)obj.transform.parent.localEulerAngles.y;

            //Debug.Log("objFace == " + objFace);  //<< This is where we left off (need to combine both trans.forward & tras.euler, to get proper pos/rot data
            // Debug.Log("objRot == " + objRot);    //<< This is where we left off (need to combine both trans.forward & tras.euler, to get proper pos/rot data

            Vector3Int objRot = new Vector3Int(forceRotation.x, forceRotation.y + parentYAxis, forceRotation.z);

            if (objRot.x >= 360)
                objRot.x -= 360;

            if (objRot.y >= 360)
                objRot.y -= 360;

            if (objRot.z >= 360)
                objRot.z -= 360;

            if (objRot.z == 90) // Walls
            {
                if (objRot.y == 0 || objRot.y == 180)
                    objRot.y = 0;

                if (objRot.y == 90 || objRot.y == 270)
                    objRot.y = 90;

            }
            else
            {

            }


            //Debug.Log("globalRot.y == " + globalRot + " localRot.y == " + localRot);    //<< This is where we left off (need to combine both trans.forward & tras.euler, to get proper pos/rot data

            //Debug.Log("objRot >>>>>>>>> " + objRot);    //<< This is where we left off (need to combine both trans.forward & tras.euler, to get proper pos/rot data


            string name = obj.gameObject.GetComponent<Renderer>().material.name.Replace("(Instance)", "");
            //Debug.Log("fuck name >>>>>>>> " + name);
            ObjectTextures objTexture = (ObjectTextures)Enum.Parse(typeof(ObjectTextures), name);

            CubeStruct data = new CubeStruct()
            {
                objectType = obj.objectType,
                styleType = obj.objectStyle,
                objectTexture = objTexture,
                health = 100,
                rotation = objRot
            };

            int[] dataArray = ConvertCubeDataIntoIntArray(data);

            Vector3 roundedPos = GetRoundedVector3(objPos);

            if (!objectsData.ContainsKey(roundedPos))
                objectsData.Add(roundedPos, dataArray);

            // Debug.Log("Total Panel Objects Postion == " + objPos);
        }

        Debug.Log("Total Panel Objects in Scene == " + objectsData.Count);
    }

    //////////////////////////////////////////////

    private int[] ConvertCubeDataIntoIntArray(CubeStruct cubeData)
    {
        int[] dataArray = new int[7];

        dataArray[0] = (int)cubeData.objectType;
        dataArray[1] = (int)cubeData.styleType;
        dataArray[2] = (int)cubeData.objectTexture;
        dataArray[3] = (int)cubeData.health;
        dataArray[4] = (int)cubeData.rotation.x;
        dataArray[5] = (int)cubeData.rotation.y;
        dataArray[6] = (int)cubeData.rotation.z;


        return dataArray;

    }

    //////////////////////////////////////////////

    private void SaveDataArrayIntoDoc()
    {
        #if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(TextFile);

            File.WriteAllText(path, "");

        if (File.Exists(path))
        {
            int count = 1;
            int successfulData = 0;
            foreach (Vector3 loc in gridNodeLocations)
            {
                int[] dataArray;

                if (objectsData.ContainsKey(loc))
                {
                    dataArray = objectsData[loc];
                    successfulData++;
                    //Debug.Log("Recording data into file at loc == " + loc);
                }
                else
                {
                    // set a empty data shell
                    CubeStruct data = new CubeStruct()
                    {
                        objectType = CubeObjectTypes.Empty,
                        styleType = CubeObjectStyles.Default,
                        objectTexture = ObjectTextures.Nothing,
                        health = 0,
                        rotation = Vector3Int.zero
                    };
                    dataArray = ConvertCubeDataIntoIntArray(data);
                }

                string dataString = "";
                for(int i = 0; i < dataArray.Length; i++)
                {
                    int data = dataArray[i];
                    dataString += data.ToString();
                    if (i < dataArray.Length-1)
                    {
                        dataString += " ";
                    }
                    else if (count != gridNodeLocations.Count)
                    {
                        dataString += ",\n";
                    }
                }
                File.AppendAllText(path, dataString);

                count++;
            }
            Debug.Log("Recording data successfulData == " + successfulData);
        }
        else
        {
            Debug.Log("GOT AN ISSUE HERE");
        }
        #endif
    }

    //////////////////////////////////////////////

}