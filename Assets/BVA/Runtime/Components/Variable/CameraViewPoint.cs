using UnityEngine;

/// <summary>
/// export camera Anchors, all the child transform will be regard as camera view point
/// </summary>
public class CameraViewPoint : MonoBehaviour
{
    public Transform[] anchors;
    [ContextMenu("Set Values")]
    public void OnEnable()
    {
        anchors = GetComponentsInChildren<Transform>();
    }
}
