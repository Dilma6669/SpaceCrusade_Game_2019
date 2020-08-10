using UnityEngine;

public class RotationalNode : MonoBehaviour
{
    public bool _rotate = false;

    // Update is called once per frame
    void Update()
    {
        if(_rotate)
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 90f);
    }
}
