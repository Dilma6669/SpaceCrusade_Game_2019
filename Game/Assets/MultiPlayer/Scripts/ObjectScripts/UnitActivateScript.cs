using UnityEngine;

public class UnitActivateScript : MonoBehaviour
{
    UnitScript _unitScript;

    // Start is called before the first frame update
    void Start()
    {
        _unitScript = transform.parent.GetComponent<UnitScript>();
    }


    void OnMouseDown()
    {
        UnitsManager.SetUnitActive(true, PlayerManager.PlayerID, _unitScript.UnitID);
    }
}
