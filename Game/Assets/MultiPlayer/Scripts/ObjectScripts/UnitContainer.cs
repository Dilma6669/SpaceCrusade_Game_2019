using UnityEngine;

public class UnitContainer : MonoBehaviour
{

    UnitScript _unitScript;


    private void Start()
    {
        _unitScript = GetComponentInParent<UnitScript>();
    }




    void OnMouseDown()
    {
        if (_unitScript.PlayerControllerID == PlayerManager.PlayerID)
        {
            if (!_unitScript.UnitAcive)
            {
                _unitScript.ActivateUnit();
                UnitsManager.SetUnitActive(true, _unitScript.PlayerControllerID, _unitScript.UnitID);
                UnitsManager.AssignCameraToActiveUnit();
            }
        }
    }

    void OnMouseOver()
    {
        if (!_unitScript.UnitAcive)
        {
            _unitScript.PanelPieceChangeColor("Green");
        }
    }
    void OnMouseExit()
    {
        if (!_unitScript.UnitAcive)
        {
            _unitScript.PanelPieceChangeColor("White");
        }
    }

}
