using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class MirrorPlane : MonoBehaviour
{
    [Range(32, 8192)]
    public int TextureResolution = 512;
    public float HeightOffset = 0;
    public Camera ViewCamera;
    public string ReflectionTextureName = "_ReflectionTexture";

    RenderTexture reflTex;
    public RenderTexture ReflectionTexture => reflTex;
    [HideInInspector]
    public Camera reflCam;

    void OnEnable()
    {
        destroy();
        setupReflectionTexture(ReflectionTextureName);
        SetupCamera();
        if (ViewCamera == null)
        {
            ViewCamera = Camera.main;
        }
    }

    void OnValidate()
    {
        if (reflCam != null)
        {
            reflCam.targetTexture = null;
        }
        if (reflTex != null)
        {
            DestroyImmediate(reflTex);
        }
        setupReflectionTexture(ReflectionTextureName);
        if (reflCam != null)
        {
            reflCam.targetTexture = reflTex;
        }
    }

    void setupReflectionTexture(string texName)
    {
        reflTex = new RenderTexture(TextureResolution, TextureResolution, 16, RenderTextureFormat.ARGBHalf);
#if UNITY_EDITOR
        var material = GetComponent<Renderer>().sharedMaterial;
#else
        var material = GetComponent<Renderer>().material;
#endif
        material.SetTexture(ReflectionTextureName, reflTex);
    }

    void SetupCamera()
    {
        var camObj = new GameObject("Reflection Camera");
        camObj.transform.SetParent(transform, false);
        reflCam = camObj.AddComponent<Camera>();
        camObj.AddComponent<UniversalAdditionalCameraData>();
        reflCam.targetTexture = reflTex;
    }

    Matrix4x4 calculateReflectionMatrix(Vector3 normal, Vector3 pos)
    {
        var w = -Vector3.Dot(normal, pos);
        Matrix4x4 matrix;
        matrix.m00 = 1 - 2 * normal.x * normal.x;
        matrix.m01 = -2 * normal.x * normal.y;
        matrix.m02 = -2 * normal.x * normal.z;
        matrix.m03 = -2 * w * normal.x;

        matrix.m10 = -2 * normal.x * normal.y;
        matrix.m11 = 1 - 2 * normal.y * normal.y;
        matrix.m12 = -2 * normal.y * normal.z;
        matrix.m13 = -2 * w * normal.y;

        matrix.m20 = -2 * normal.x * normal.z;
        matrix.m21 = -2 * normal.y * normal.z;
        matrix.m22 = 1 - 2 * normal.z * normal.z;
        matrix.m23 = -2 * w * normal.z;

        matrix.m30 = 0;
        matrix.m31 = 0;
        matrix.m32 = 0;
        matrix.m33 = 1;

        return matrix;
    }

    Vector4 getClipPlane(Camera _cam, Vector3 normal, Vector3 pos)
    {
        var camNorm = _cam.worldToCameraMatrix.MultiplyVector(normal);
        var camPos = _cam.worldToCameraMatrix.MultiplyPoint(pos);
        return new Vector4(camNorm.x, camNorm.y, camNorm.z, -Vector3.Dot(camPos, camNorm));
    }

    void UpdateCamera(Camera viewCam)
    {
        if (reflCam == null)
        {
            Debug.LogWarning("\"Reflection camera\" is a null reference, which may be caused by manual deletion by the user.");
            return;
        }
        if (ViewCamera == null)
        {
            Debug.LogWarning("\"View camera\" is a null reference and reflection does not work properly.");
            return;
        }

        reflCam.orthographic = viewCam.orthographic;
        reflCam.fieldOfView = viewCam.fieldOfView;
        reflCam.nearClipPlane = viewCam.nearClipPlane;
        reflCam.farClipPlane = viewCam.farClipPlane;
        reflCam.depth = viewCam.depth;
        var viewCamData = viewCam.GetComponent<UniversalAdditionalCameraData>();
        if (viewCamData != null)
        {
            var reflCamData = reflCam.GetComponent<UniversalAdditionalCameraData>();
            if (reflCamData != null)
            {
                reflCamData.renderShadows = viewCamData.renderShadows;
            }
        }
        reflCam.clearFlags = viewCam.clearFlags;
        reflCam.backgroundColor = viewCam.backgroundColor;

        var normal = transform.up;
        var pos = transform.position + normal * HeightOffset;
        var viewMat = viewCam.worldToCameraMatrix * calculateReflectionMatrix(normal, pos);
        viewMat.SetRow(1, -viewMat.GetRow(1));
        reflCam.worldToCameraMatrix = viewMat;
        var clipPlane = getClipPlane(reflCam, normal, pos);
        var obliqueMatrix = viewCam.CalculateObliqueMatrix(clipPlane);
        reflCam.projectionMatrix = obliqueMatrix;
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UpdateCamera(SceneView.currentDrawingSceneView.camera);
#endif
    }

    void LateUpdate()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
#endif
            UpdateCamera(ViewCamera);
#if UNITY_EDITOR
        }
#endif
    }

    void destroy()
    {
        if (reflCam != null)
        {
            reflCam.targetTexture = null;
#if UNITY_EDITOR
            DestroyImmediate(reflCam.gameObject);
#else
            Destroy(reflCam.gameObject);
#endif
        }
        if (reflTex!=null)
        {
#if UNITY_EDITOR
            DestroyImmediate(reflTex);
#else
            Destroy(reflTex);
#endif
        }
    }

    void OnDisable()
    {
        destroy();
    }
}
