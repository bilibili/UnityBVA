using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
public class SwitchCamTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public AutoCam autoCam;
    public Transform pivot;
    public float minLength;
    public float maxLength;
    public Transform[] targets;
    private int index;
    void Start()
    {
        if (autoCam==null||targets == null || targets.Length == 0)
        {
            return;
        }
        index = 0;
        autoCam.SetTarget (targets[index]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            autoCam.SetTarget(targets[++index % (targets.Length)]);
            Debug.Log(index);
        }
        if (Input.GetMouseButtonDown(1))
        {
            autoCam.SetTarget(targets[--index % (targets.Length)]);
            Debug.Log(index);
        }

        if (Input.mouseScrollDelta.y!=0)
        {
            Vector3 direction = pivot.position - transform.position;
            if (Input.mouseScrollDelta.y>0&& direction.magnitude < maxLength)
            {
                float length = direction.magnitude;
                length *= (1 + Input.mouseScrollDelta.y * 0.01f);
                length = Mathf.Max(length, maxLength);

                direction = direction.normalized * length;

            }

            
        }

    }
}
