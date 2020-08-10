using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LayerManager : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static LayerManager _instance;

    ////////////////////////////////////////////////

    // Layer INfo
    //private static int _startLayer;
    //private static int _maxLayer; // This needs to change with the amout of y levels, basicly level*2 because of vents layer ontop of layer
    //private static int _minLayer;
    //private static int _currLayer;

    ////////////////////////////////////////////////

    //public static int LayerStart
    //{
    //    get { return _startLayer; }
    //    set { _startLayer = value; }
    //}
    //public static int LayerMax
    //{
    //    get { return _maxLayer; }
    //    set { _maxLayer = value; }
    //}
    //public static int LayerMin
    //{
    //    get { return _minLayer; }
    //    set { _minLayer = value; }
    //}
    //public static int LayerCurr
    //{
    //    get { return _currLayer; }
    //    set { _currLayer = value; }
    //}

    ////////////////////////////////////////////////

    public static Dictionary<int, List<CubeLocationScript>> _layerCubeList;
    public static Dictionary<int, List<BaseNode>> _layerNodeList;

    static int layerID;
    static BaseNode parentNode;
    static int parentLayerID;

    public static int LAYER_IGNORE_RAYCAST = 2;
    public static int LAYER_ENVIRONMENT = 8;

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
    }

    void Start()
    {
        _layerCubeList = new Dictionary<int, List<CubeLocationScript>>();
        _layerNodeList = new Dictionary<int, List<BaseNode>>();

        // change layer callback event
        //UIManager.OnChangeLayerClick += ChangeCameraLayerByHUD;

       // LayerCurr = -1;
    }

    ////////////////////////////////////
    ////////////////////////////////////

    //private static void AssignLayerToObject(GameObject obj, int visibilityLayer)
    //{
    //    obj.layer = visibilityLayer;

    //    if (visibilityLayer == LAYERVISIBLE)
    //    {
    //        if (obj.GetComponent<MeshCollider>())
    //            obj.GetComponent<MeshCollider>().enabled = true;

    //        if (obj.GetComponent<MeshRenderer>())
    //            obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
    //    }
    //    else if (visibilityLayer == LAYERNOTVISIBLE)
    //    {
    //        if (obj.GetComponent<MeshCollider>())
    //            obj.GetComponent<MeshCollider>().enabled = false;

    //        if (obj.gameObject.GetComponent<MeshRenderer>())
    //            obj.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
    //    }
    //}

    //////////////////////////////////////
    //////////////////////////////////////

    //public static void ChangeCameraLayerByHUD(int change)
    //{
    //    if (change == 1)
    //    {
    //        layerID++;
    //        ChangeSpecificCubesVisibility(LAYERVISIBLE);
    //    }
    //    if (change == -1)
    //    {
    //        ChangeSpecificCubesVisibility(LAYERNOTVISIBLE);
    //        layerID--;
    //    }
    //    LayerCurr = layerID;
    //}


    //public static void ChangeCameraLayer(CubeLocationScript cubeScript)
    //{
    //    BaseNode parentWorldNode = cubeScript.transform.parent.parent.GetComponent<BaseNode>();
    //    BaseNode parentMapNode = cubeScript.transform.parent.GetComponent<BaseNode>();

    //    if (parentWorldNode.entrance)
    //    {
    //        parentNode = parentWorldNode;
    //    }
    //    else
    //    {
    //        parentNode = parentMapNode;
    //    }
    //    parentLayerID = parentNode.NodeLayerCount;

    //    ChangeSpecificNodeVisibility(LAYERNOTVISIBLE); // set specfic world node to not visible, basicly clears all layers so correct layer can be set

    //    if (LayerCurr != -1)
    //    {
    //        //ChangeSpecificNodeVisibility(parentWorldNode, LAYERVISIBLE);
    //        //ChangeSpecificCubesVisibility(parentWorldNode, LAYERNOTVISIBLE);
    //    }
    
    //    layerID = cubeScript.CubeLayerID;

    //    for (int i = parentLayerID; i <= cubeScript.CubeLayerID; i++)
    //    {
    //        layerID = i;
    //        ChangeSpecificCubesVisibility(LAYERVISIBLE);
    //    }

    //    LayerCurr = layerID;


    //    // for the units
    //    UnitScript[] units = parentNode.GetComponentsInChildren<UnitScript>();
    //    foreach (UnitScript unit in units)
    //    {
    //       AssignUnitToLayer(unit.gameObject);
    //    }
    //}

    ////////////////////

    //public static void ChangeSpecificNodeVisibility(int visibilityLayer)
    //{
    //    AssignLayerToObject(parentNode.gameObject, visibilityLayer);

    //    List<GameObject> units = new List<GameObject>(); 

    //    Transform[] allChildren = parentNode.GetComponentsInChildren<Transform>();
    //    foreach (Transform child in allChildren)
    //    {
    //        AssignLayerToObject(child.gameObject, visibilityLayer);
    //    }
    //}

    ////////////////////

    //public static void ChangeSpecificCubesVisibility(int visibilityLayer)
    //{
    //    CubeLocationScript[] allChildren = parentNode.GetComponentsInChildren<CubeLocationScript>();

    //    foreach (CubeLocationScript child in allChildren)
    //    {
    //        if (child.CubeLayerID == layerID)
    //        {
    //            AssignLayerToObject(child.gameObject, visibilityLayer);

    //            Transform[] grandChildren = child.GetComponentsInChildren<Transform>();
    //            foreach (Transform grand in grandChildren)
    //            {
    //                AssignLayerToObject(grand.gameObject, visibilityLayer);
    //            }
    //        }
    //    }
    //}

    ////////////////////

    //public static void AddCubeToLayer(CubeLocationScript script)
    //{
    //    int cubeLayer = script.CubeLayerID;
    //    if (_layerCubeList.ContainsKey(cubeLayer))
    //    {
    //        _layerCubeList[cubeLayer].Add(script);
    //    }
    //    else
    //    {
    //        _layerCubeList.Add(cubeLayer, new List<CubeLocationScript> { script });
    //    }
    //}

    //public static void ChangeCubeLayerVisibility(int layerID, int visibilityLayer)
    //{
    //    if (_layerCubeList.ContainsKey(layerID))
    //    {
    //        List<CubeLocationScript> scripts = _layerCubeList[layerID];

    //        foreach (CubeLocationScript script in scripts)
    //        {
    //            AssignLayerToObject(script.gameObject, visibilityLayer);

    //            Transform[] allChildren = script.GetComponentsInChildren<Transform>();
    //            foreach (Transform child in allChildren)
    //            {
    //                AssignLayerToObject(child.gameObject, visibilityLayer);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Got an issue here NO layerID assigned or not in list: " + layerID);
    //    }
    //}


    //public static void MakeAllCubeLayersVisible()
    //{
    //    Debug.Log("MakeAllLayersVisible");
    //    foreach (int layerID in _layerCubeList.Keys)
    //    {
    //        ChangeCubeLayerVisibility(layerID, LAYERVISIBLE);
    //    }
    //}

    //public static void MakeAllCubeLayersNotVisible()
    //{
    //    Debug.Log("MakeAllLayersNotVisible");
    //    foreach (int layerID in _layerCubeList.Keys)
    //    {
    //        ChangeCubeLayerVisibility(layerID, LAYERNOTVISIBLE);
    //    }
    //}

    ////////////////////

    //public static void AddNodeToLayer(BaseNode node)
    //{
    //    int nodeLayer = node.NodeLayerCount;
    //    if (_layerNodeList.ContainsKey(nodeLayer))
    //    {
    //        _layerNodeList[nodeLayer].Add(node);
    //    }
    //    else
    //    {
    //        _layerNodeList.Add(nodeLayer, new List<BaseNode> { node });
    //    }
    //}

    //public static void ChangeNodeLayerVisibility(int layerID, int visibilityLayer)
    //{
    //    if (_layerNodeList.ContainsKey(layerID))
    //    {
    //        List<BaseNode> nodes = _layerNodeList[layerID];

    //        foreach (BaseNode node in nodes)
    //        {
    //            AssignLayerToObject(node.gameObject, visibilityLayer);

    //            Transform[] allChildren = node.GetComponentsInChildren<Transform>();
    //            foreach (Transform child in allChildren)
    //            {
    //                AssignLayerToObject(child.gameObject, visibilityLayer);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Got an issue here NO layerID assigned or not in list: " + layerID);
    //    }
    //}

    //public static void MakeAllNodeLayersVisible()
    //{
    //    foreach (int layerID in _layerNodeList.Keys)
    //    {
    //        ChangeNodeLayerVisibility(layerID, LAYERVISIBLE);
    //    }
    //}

    //public static void MakeAllNodeLayersNotVisible()
    //{
    //    Debug.Log("MakeAllLayersNotVisible");
    //    foreach (int layerID in _layerNodeList.Keys)
    //    {
    //        ChangeNodeLayerVisibility(layerID, LAYERNOTVISIBLE);
    //    }
    //}


    ////////////////////

    //public static void AssignUnitToLayer(GameObject unit)
    //{
    //    int playerID = PlayerManager.PlayerID;
    //    int unitControllerID = unit.GetComponent<UnitScript>().PlayerControllerID;

    //    //print("fuck AssignUnitToLayer playerID: " + playerID);
    //    //print("fuck AssignUnitToLayer unitControllerID: " + unitControllerID);

    //    if (playerID == unitControllerID)
    //    {
    //        AssignLayerToObject(unit, LAYERVISIBLE);

    //        Transform[] allChildren = unit.GetComponentsInChildren<Transform>();
    //        foreach (Transform child in allChildren)
    //        {
    //            AssignLayerToObject(child.gameObject, LAYERVISIBLE);
    //        }
    //    }
    //    else
    //    {
    //        AssignLayerToObject(unit, LAYERNOTVISIBLE);

    //        Transform[] allChildren = unit.GetComponentsInChildren<Transform>();
    //        foreach (Transform child in allChildren)
    //        {
    //            AssignLayerToObject(child.gameObject, LAYERNOTVISIBLE);
    //        }
    //    }
    //}



    //////////////////////////////////////////////
    //////////////////////////////////////////////


    //public static void AssignLayerCountsToWorldAndMapNodes(Dictionary<Vector3Int, WorldNode> worldNodes)
    //{
    //    int YAxisWorldNode = -1;
    //    int YAxisMapNode = -1;

    //    int mapLayerCounter = -1;
    //    int worldLayerCounter = -1;
    //    if (worldNodes.Count == 1)
    //    {
    //        foreach (WorldNode worldNode in worldNodes.Values)
    //        {
    //            worldNode.NodeLayerCount = 0; // worldNode Counter is the same as the most bottom map Node

    //            foreach (MapNode mapNode in worldNode.mapNodes)
    //            {
    //                if (YAxisMapNode != mapNode.NodeID.y)
    //                {
    //                    YAxisMapNode = mapNode.NodeID.y;
    //                    mapLayerCounter++;
    //                }

    //                mapNode.NodeLayerCount = mapLayerCounter;
    //                AddNodeToLayer(mapNode);
    //            }

    //            AddNodeToLayer(worldNode);
    //        }
    //    }
    //    else
    //    {
    //        foreach (WorldNode worldNode in worldNodes.Values)
    //        {
    //            if (YAxisWorldNode != worldNode.NodeID.y)
    //            {
    //                YAxisWorldNode = worldNode.NodeID.y;
    //                worldLayerCounter++;
    //            }

    //            worldNode.NodeLayerCount = worldLayerCounter * 3; // 3 is number layers in worldNode

    //            mapLayerCounter = worldNode.NodeLayerCount;

    //            foreach (MapNode mapNode in worldNode.mapNodes)
    //            {
    //                if (YAxisMapNode != mapNode.NodeID.y)
    //                {
    //                    YAxisMapNode = mapNode.NodeID.y;
    //                    mapLayerCounter++;
    //                }

    //                mapNode.NodeLayerCount = worldNode.NodeLayerCount + mapLayerCounter;
    //                AddNodeToLayer(mapNode);
    //            }

    //            AddNodeToLayer(worldNode);
    //        }
    //    }
    //}





}
