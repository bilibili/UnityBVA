using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Vector3 point;
    public Vector3 axis;
    public float angle;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(point, axis, angle / Application.targetFrameRate);
    }
}
