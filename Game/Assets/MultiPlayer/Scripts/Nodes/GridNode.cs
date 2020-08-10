using UnityEngine;

public class GridNode : MonoBehaviour
{
    public Vector3 NodeLoc;

    // Start is called before the first frame update
    void Start()
    {
        NodeLoc = transform.position;
    }

}
