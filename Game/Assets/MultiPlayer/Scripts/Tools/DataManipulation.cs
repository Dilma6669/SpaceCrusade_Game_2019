using System.Collections.Generic;
using UnityEngine;

public class DataManipulation : MonoBehaviour {

    public static List<Vector3Int> GetLocVectorsFromCubeScript(List<CubeLocationScript> cubesScripts)
    {
        List<Vector3Int> vects = new List<Vector3Int>();
        foreach (CubeLocationScript cube in cubesScripts)
        {
            Vector3Int vect = cube.CubeID;
            vects.Add(vect);
        }
        return vects;
    }


    public static int[] ConvertVectorsIntoIntArray(List<Vector3Int> vects)
    {
        int[] intArray = new int[vects.Count * 3];
        int index = 0;
        foreach(Vector3Int vect in vects)
        {
            intArray[index] = vect.x;
            index += 1;
            intArray[index] = vect.y;
            index += 1;
            intArray[index] = vect.z;
            index += 1;
        }
        return intArray;
    }


    ///////// DUMB not being used leaving here incase for future
    public static int ConvertVectorIntoInt(Vector3Int vect)
    {
        return Mathf.Abs(int.Parse(vect.x.ToString("000") + vect.y.ToString("000"))); // + vect.z.ToString("000"))); -- commented this out because all 3 vectors created integer too long
    }



    public static List<Vector3Int> ConvertIntArrayIntoVectors(int[] intArray)
    {
        List<Vector3Int> vects = new List<Vector3Int>();

        int index = 0;
        for(int i = 0; i < intArray.Length/3; i++)
        {
            Vector3Int vect = new Vector3Int();

            vect.x = intArray[index];
            index += 1;
            vect.y = intArray[index];
            index += 1;
            vect.z = intArray[index];
            index += 1;

            vects.Add(vect);
        }
        return vects;
    }
}
