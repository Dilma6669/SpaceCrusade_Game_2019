using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    ////////////////////////////////////////////////

    private static GridBuilder _instance;

    ////////////////////////////////////////////////

    public static bool _debugNodeSpheres = true;// debugging purposes

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

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    public static void BuildLocationGrid(Vector3 nodeLoc)
    {
        // these are the bottom left corner axis for EACH map node
        int startX = (int)(nodeLoc.x - (int)Mathf.Floor(MapSettings.sizeOfMapPiecesXZ / 2));
        int startY = (int)(nodeLoc.y - (int)Mathf.Floor(MapSettings.sizeOfMapPiecesY / 2));
        int startZ = (int)(nodeLoc.z - (int)Mathf.Floor(MapSettings.sizeOfMapPiecesXZ / 2));

        int finishX = startX + MapSettings.sizeOfMapPiecesXZ - 1;
        int finishY = startY + MapSettings.sizeOfMapPiecesY - 1;
        int finishZ = startZ + MapSettings.sizeOfMapPiecesXZ - 1;

        int gridLocX = startX;
        int gridLocY = startY;
        int gridLocZ = startZ;

        for (int y = startY; y <= finishY; y++) {

			gridLocX = startX;
			gridLocZ = startZ;

            for (int z = startZ; z <= finishZ; z++) {

				gridLocX = startX;

				for (int x = startX; x <= finishX; x++) {

                    Vector3 gridLoc = new Vector3(gridLocX, gridLocY, gridLocZ);

                    if (_debugNodeSpheres)
                    {
                        WorldBuilder._nodeBuilder.InstantiateNodeObject(gridLoc, Quaternion.identity, NodeTypes.GridNode, WorldManager._GridContainer.transform);
                    }

                    gridLocX += 1;
				}
				gridLocZ += 1;
			}
			gridLocY += 1;
		}
    }
}
	

