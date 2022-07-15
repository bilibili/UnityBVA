using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    public Vector3 axis;
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(axis, speed/Application.targetFrameRate);
    }
}
