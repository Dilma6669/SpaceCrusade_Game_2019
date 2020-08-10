using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public Vector3Int forcedRotation;

    public CubeObjectTypes objectType;
    public CubeObjectStyles objectStyle;

    private void Start()
    {
        transform.localRotation = Quaternion.Euler(forcedRotation);
    }
}
