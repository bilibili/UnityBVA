using UnityEngine;
namespace BVA.Component
{
    [ExecuteInEditMode]
    public class MirrorObject : MonoBehaviour
    {
        private Camera cam;
        public int RenderTextureSize = 512;
        [ContextMenu("Refresh Scale")]
        void Start()
        {
            cam = GetComponentInChildren<Camera>();
            RenderTexture rt = new RenderTexture((int)transform.localScale.x * RenderTextureSize, (int)transform.localScale.y * RenderTextureSize, 24, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;
#if UNITY_EDITOR
            GetComponent<MeshRenderer>().sharedMaterial.mainTexture = rt;
#else
            GetComponent<MeshRenderer>().material.mainTexture = rt;
#endif
        }
    }
}