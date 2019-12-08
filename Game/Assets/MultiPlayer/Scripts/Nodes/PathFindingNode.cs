using UnityEngine;

public class PathFindingNode : MonoBehaviour
{
    Vector3 _unitControllerID;
    Vector3 _cubeParentLoc;

    public Vector3 UnitControllerID
    {
        get { return _unitControllerID; }
        set { _unitControllerID = value; }
    }

    public Vector3 CubeParentLoc
    {
        get { return _cubeParentLoc; }
        set { _cubeParentLoc = value; }
    }


    private void OnTriggerEnter(Collider other)
    {
        UnitScript unitScript = other.transform.parent.GetComponent<UnitScript>();
        if (unitScript != null)
        {
            Vector3 unitID = other.gameObject.transform.parent.GetComponent<UnitScript>().UnitID;
            if (unitID == UnitControllerID)
            {
                unitScript.SetUnitToNextLocation_CLIENT(CubeParentLoc);
                DestroyNode();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    public void DestroyNode()
    {
        Destroy(gameObject);
    }
}
