using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkNodeContainer : NetworkBehaviour
{
    private bool _moveNode;

    public Vector3 _posToMoveTo;
    public Vector3 _rotToMoveTo;

    private float _thrust;

    ////////////////////////////////////////////////
    ////////////////////////////////////////////////

    void Awake()
    {
        _moveNode = false;
        Transform parent = WorldManager._NetworkContainer.transform;
        transform.SetParent(parent);

        _thrust = 10;
    }

    void Update()
    {
        if (_moveNode)
        {
            // Moving
            transform.position = Vector3.MoveTowards(transform.position, _posToMoveTo, (Time.deltaTime * _thrust));

            // Rotation
            Vector3 targetDir = _posToMoveTo - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * 0.1f, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    public void MoveNetworkNode(KeyValuePair<Vector3, Vector3> posRot, float thrust)
    {
        _posToMoveTo = posRot.Key;
        _rotToMoveTo = posRot.Value;
        _thrust = thrust;
        _moveNode = true;
    }

}
