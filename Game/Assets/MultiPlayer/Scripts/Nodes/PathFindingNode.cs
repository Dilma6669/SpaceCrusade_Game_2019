using UnityEngine;

public class PathFindingNode : MonoBehaviour
{
    Vector3Int _unitControllerID;
    Vector3Int _cubeParentLoc;

    public Vector3Int UnitControllerID
    {
        get { return _unitControllerID; }
        set { _unitControllerID = value; }
    }

    public Vector3Int CubeParentLoc
    {
        get { return _cubeParentLoc; }
        set { _cubeParentLoc = value; }
    }


    private void OnTriggerEnter(Collider other)
    {
        //UnitScript unitScript = other.transform.parent.GetComponent<UnitScript>();
        //if (unitScript != null)
        //{
        //    Vector3Int unitID = other.gameObject.transform.parent.GetComponent<UnitScript>().UnitID;
        //    if (unitID == UnitControllerID)
        //    {
        //        //unitScript.SetUnitToNextLocation_CLIENT(CubeParentLoc);
        //        DestroyNode();
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {

    }

    public void DestroyNode()
    {
        Destroy(gameObject);
    }
}
