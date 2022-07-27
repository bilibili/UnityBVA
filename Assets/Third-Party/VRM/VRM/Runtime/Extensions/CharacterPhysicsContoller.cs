using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ADBRuntime.Mono;
using VRM;

public class CharacterPhysicsContoller : MonoBehaviour
{
    // Start is called before the first frame update
    public VRMSpringBone[] vrmControllers;
    public ADBRuntimeController ADBController;

    private float oldGrag;
    private float oldStiffness;
    private bool isUseBVAPhysics;
    private bool isFirstUpdate=true;
    private float iterationFloat;
    [SerializeField]
    [Range(0.001f,2)]
    private float m_drag=1;

    [SerializeField]
    [Range(0.001f, 2)]
    private float m_stiffness=1;

    [SerializeField]
    private bool m_UseBVAPhysics;

    public float Drag { get => m_drag; set => SetParam(value, m_stiffness); }

    public float Stiffness { get => m_stiffness; set => SetParam(m_drag, value); }

    public bool isClone = false;

    private void Start()
    {
        m_drag = oldGrag = 1;
        m_stiffness = oldStiffness = 1;
        m_UseBVAPhysics = isUseBVAPhysics = true;
        vrmControllers = gameObject.GetComponentsInChildren<VRMSpringBone>();
        for (int i = 0; i < vrmControllers.Length; i++)
        {
            vrmControllers[i].m_center = null;
        }

        ADBController = gameObject.GetComponent<ADBRuntimeController>();
        iterationFloat = ADBController.iteration;

        SwitchPhysics();
    }
    private void Update()
    {
    }

    public void EnableVRMPhysics()
    {
        for (int i = 0; i < vrmControllers.Length; i++)
        {
            vrmControllers[i].enabled = true;
        }
    }

    public void DisableVRMPhysics()
    {
        for (int i = 0; i < vrmControllers.Length; i++)
        {
            vrmControllers[i].enabled = false;
        }
    }

    public void EnableBVAPhysics()
    {
        ADBController.enabled = true;
    }

    public void DisableBVAPhysics()
    {
        ADBController.enabled = false;
    }

    public void SwitchPhysics()
    {
        if (isUseBVAPhysics)
        {
            EnableVRMPhysics();
            DisableBVAPhysics();
        }
        else
        {
            EnableBVAPhysics();
            DisableVRMPhysics();
        }
        m_UseBVAPhysics = isUseBVAPhysics = !isUseBVAPhysics;
    }

    public void SetParam(float drag, float stiffness)
    {
        drag = Mathf.Max(0.001f, drag);
        stiffness = Mathf.Max(0.001f, stiffness);

        float dragScale = drag / oldGrag;
        float stiffnessScale = stiffness / oldStiffness;
        iterationFloat = iterationFloat * stiffnessScale;

        ADBController.timeScale= 2-(2 - ADBController.timeScale)*dragScale ;
        ADBController.iteration = Mathf.CeilToInt(9- iterationFloat);

        for (int i = 0; i < vrmControllers.Length; i++)
        {
            var vrmController = vrmControllers[i];
            vrmController.m_dragForce *= dragScale;
            vrmController.m_stiffnessForce *= stiffnessScale;
        }

        m_drag = oldGrag = drag;
        m_stiffness = oldStiffness = stiffness;
    }

    public void OnValidate()
    {
        if (m_UseBVAPhysics != isUseBVAPhysics)
        {
            SwitchPhysics();
        }
/*        bool setParam = false;
        if (m_drag != oldGrag)
        {
            setParam = true;
        }
        if (m_stiffness != oldStiffness)
        {
            setParam = true;
        }
        if (setParam)
        {
            SetParam(m_drag, m_stiffness);
        }*/
    }
}
