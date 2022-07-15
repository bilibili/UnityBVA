using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelfRandom : MonoBehaviour
{
    public float speed;
    public Vector3 minAxis, maxAxis;

    // Update is called once per frame
    void FixedUpdate()
    {
        float x= Random.Range(minAxis.x,maxAxis.x);
        float y= Random.Range(minAxis.y,maxAxis.y);
        float z= Random.Range(minAxis.z,maxAxis.z);
        transform.Rotate(new Vector3(x, y, z), speed/Application.targetFrameRate);
    }
}
