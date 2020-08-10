using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteractionsScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false;

    void Update()
    {
        if(isOver)
        {

            if(gameObject.name == "Panel_LeftSide")
            {
               // PlayerManager.CameraAgent._cameraPivotScript.CameraSideSpin(1);
            }
            else if(gameObject.name == "Panel_RightSide")
            {
               // PlayerManager.CameraAgent._cameraPivotScript.CameraSideSpin(-1);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) { isOver = true; }

    public void OnPointerExit(PointerEventData eventData) { isOver = false; }

}
