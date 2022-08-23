using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPositionRecorder : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform camera;

    private Vector3 selfPosition;
    private Quaternion selfRotation;

    private Vector3 cameraPosition;
    private Quaternion cameraRotation;

    void Start()
    {

        Record();
    }

    public void Record()
    {
        selfPosition = transform.position;
        selfRotation = transform.rotation;

        cameraPosition = camera.position;
        cameraRotation = camera.rotation;
    }

    public void ResetData()
    {
        transform.position = selfPosition;
        transform.rotation = selfRotation ;

        transform.position = cameraPosition ;
        transform.rotation = cameraRotation  ;
    }
}
