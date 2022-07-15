using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DrawNormal : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private MeshFilter _meshFilter = null;
    [SerializeField]
    private bool _displayWireframe = false;
    [SerializeField]
    private NormalsDrawData _faceNormals = new NormalsDrawData(new Color32(34, 221, 221, 155), true);
    [SerializeField]
    private NormalsDrawData _vertexNormals = new NormalsDrawData(new Color32(200, 255, 195, 127), false);

    [System.Serializable]
    private class NormalsDrawData
    {
        [SerializeField]
        protected DrawType _draw = DrawType.Selected;
        protected enum DrawType { Never, Selected, Always }
        [SerializeField]
        protected float _length = 0.3f;
        [SerializeField]
        protected Color _normalColor;
        private Color _baseColor = new Color32(255, 133, 0, 255);
        private const float _baseSize = 0.0125f;


        public NormalsDrawData(Color normalColor, bool draw)
        {
            _normalColor = normalColor;
            _draw = draw ? DrawType.Selected : DrawType.Never;
        }

        public bool CanDraw(bool isSelected)
        {
            return (_draw == DrawType.Always) || (_draw == DrawType.Selected && isSelected);
        }

        public void Draw(Vector3 from, Vector3 direction)
        {
            if (Camera.current.transform.InverseTransformDirection(direction).z < 0f)
            {
                Gizmos.color = _baseColor;
                Gizmos.DrawWireSphere(from, _baseSize);

                Gizmos.color = _normalColor;
                Gizmos.DrawRay(from, direction * _length);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        EditorUtility.SetSelectedRenderState(GetComponent<Renderer>(),_displayWireframe?  EditorSelectedRenderState.Wireframe: EditorSelectedRenderState.Hidden);
        OnDrawNormals(true);
    }

    void OnDrawGizmos()
    {
        if (!Selection.Contains(this))
            OnDrawNormals(false);
    }

    private void OnDrawNormals(bool isSelected)
    {
        if (_meshFilter == null)
        {
            _meshFilter = GetComponent<MeshFilter>();
            if (_meshFilter == null)
                return;
        }

        Mesh mesh = _meshFilter.sharedMesh;

        //Draw Face Normals
        if (_faceNormals.CanDraw(isSelected))
        {
            int[] triangles = mesh.triangles;
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v0 = transform.TransformPoint(vertices[triangles[i]]);
                Vector3 v1 = transform.TransformPoint(vertices[triangles[i + 1]]);
                Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 2]]);
                Vector3 center = (v0 + v1 + v2) / 3;

                Vector3 dir = Vector3.Cross(v1 - v0, v2 - v0);
                dir /= dir.magnitude;

                _faceNormals.Draw(center, dir);
            }
        }

        //Draw Vertex Normals
        if (_vertexNormals.CanDraw(isSelected))
        {
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            for (int i = 0; i < vertices.Length; i++)
            {
                _vertexNormals.Draw(transform.TransformPoint(vertices[i]), transform.TransformVector(normals[i]));
            }
        }
    }
#endif
}