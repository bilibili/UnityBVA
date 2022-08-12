using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreedomMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 axis;

    private void Start()
    {
        axis = (Random.insideUnitSphere+Vector3.up*2).normalized;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, Time.deltaTime);
        transform.RotateAround(Vector3.zero, axis, Time.deltaTime);
    }
}
