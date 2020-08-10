using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static UIManager _instance;

    ////////////////////////////////////////////////

    static Camera mainCamera;

    static Text playerIDText;
    static Text playerNameText;
    static Text totalPlayerText;
    static Text seedNumText;

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

        playerIDText = transform.FindDeepChild("PlayerNum").GetComponent<Text>();
        playerNameText = transform.FindDeepChild("PlayerName").GetComponent<Text>();
        totalPlayerText = transform.FindDeepChild("TotalPlayersNum").GetComponent<Text>();
        seedNumText = transform.FindDeepChild("SeedNum").GetComponent<Text>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        //GatherMouseHoverObjects();
    }

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    // The players personal GUI
    public static void SetUpPlayersGUI(int playerID)
    {
        GameManager._UIManager.GetComponent<Canvas>().enabled = true;

        playerIDText.text = playerID.ToString();
        playerNameText.text = PlayerManager.PlayerName;

        SyncedVars _syncedVars = GameObject.Find("SyncedVars").GetComponent<SyncedVars>(); // needs to be here, function runs before awake
        if (_syncedVars == null) { Debug.LogError("We got a problem here"); }

        seedNumText.text = _syncedVars.GlobalSeed.ToString();

        mainCamera = PlayerManager.CameraAgent.MainCamera;
    }

	public static void UpdateTotalPlayersGUI(int total) {

        totalPlayerText.text = total.ToString();
	}

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    //public void ChangeLayer(bool UpDown)
    //{
    //    if (UpDown)
    //    {
    //        if (OnChangeLayerClick != null)
    //            OnChangeLayerClick(1);
    //    }
    //    else
    //    {
    //        if (OnChangeLayerClick != null)
    //            OnChangeLayerClick(-1);
    //    }
    //}


    //public static void GatherMouseHoverObjects()
    //{
    //    if (EventSystem.current.IsPointerOverGameObject())
    //        return;

    //    GameObject gameObj;

    //    RaycastHit[] hits = Physics.RaycastAll(mainCamera.ScreenPointToRay(Input.mousePosition), 100.0F);

    //    System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

    //    gameObj = hits[0].transform.gameObject;
    //}

}
