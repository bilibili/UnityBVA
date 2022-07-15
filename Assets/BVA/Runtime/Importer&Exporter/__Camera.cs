using GLTF.Schema;
using UnityEngine;
using GLTF.Schema.BVA;

namespace BVA
{
    public partial class GLTFSceneImporter
    {
        public void ImportCamera(GLTFCamera cam, GameObject nodeObj)
        {
            var camera = nodeObj.AddComponent<Camera>();
            camera.orthographic = cam.Type == GLTF.Schema.CameraType.orthographic;
            if (camera.orthographic)
            {
                camera.nearClipPlane = (float)cam.Orthographic.ZNear;
                camera.farClipPlane = (float)cam.Orthographic.ZFar;
                camera.orthographicSize = (float)cam.Orthographic.Size;
            }
            else
            {
                camera.nearClipPlane = (float)cam.Perspective.ZNear;
                camera.farClipPlane = (float)cam.Perspective.ZFar;
                camera.fieldOfView = (float)cam.Perspective.YFov * Mathf.Rad2Deg;
                camera.aspect = (float)cam.Perspective.AspectRatio;
            }
            if (cam.Extras != null && cam.Extras.Count > 0)
            {
                foreach(var extra in cam.Extras)
                {
                    var (propertyName, reader) = GetExtraProperty(extra);
                    if (propertyName == BVA_Camera_URP_Extra.PROPERTY)
                    {
                        BVA_Camera_URP_Extra.Deserialize(_gltfRoot, reader, camera);
                    }
                }
            }
        }
    }

    public partial class GLTFSceneExporter
    {
        private CameraId ExportCamera(Camera unityCamera)
        {
            GLTFCamera camera = new GLTFCamera();
            //name
            camera.Name = unityCamera.name;

            //type
            bool isOrthographic = unityCamera.orthographic;
            camera.Type = isOrthographic ? GLTF.Schema.CameraType.orthographic : GLTF.Schema.CameraType.perspective;
            Matrix4x4 matrix = unityCamera.projectionMatrix;

            var extra = new BVA_Camera_URP_Extra(unityCamera);
            camera.AddExtra(BVA_Camera_URP_Extra.PROPERTY, extra);

            //matrix properties: compute the fields from the projection matrix
            if (isOrthographic)
            {
                CameraOrthographic ortho = new CameraOrthographic();

                ortho.XMag = 1 / matrix[0, 0];
                ortho.YMag = 1 / matrix[1, 1];

                float farClip = (matrix[2, 3] / matrix[2, 2]) - (1 / matrix[2, 2]);
                float nearClip = farClip + (2 / matrix[2, 2]);
                ortho.ZFar = farClip;
                ortho.ZNear = nearClip;
                ortho.Size = unityCamera.orthographicSize;
                camera.Orthographic = ortho;
            }
            else
            {
                CameraPerspective perspective = new CameraPerspective();
                //float fov = 2 * Mathf.Atan(1 / matrix[1, 1]);
                //float aspectRatio = matrix[1, 1] / matrix[0, 0];
                perspective.YFov = unityCamera.fieldOfView * Mathf.Deg2Rad;
                perspective.AspectRatio = unityCamera.aspect;

                if (matrix[2, 2] == -1)
                {
                    //infinite projection matrix
                    float nearClip = matrix[2, 3] * -0.5f;
                    perspective.ZNear = nearClip;
                }
                else
                {
                    //finite projection matrix
                    float farClip = matrix[2, 3] / (matrix[2, 2] + 1);
                    float nearClip = farClip * (matrix[2, 2] + 1) / (matrix[2, 2] - 1);
                    perspective.ZFar = farClip;
                    perspective.ZNear = nearClip;
                }
                camera.Perspective = perspective;
            }

            var id = new CameraId
            {
                Id = _root.Cameras.Count,
                Root = _root
            };

            _root.Cameras.Add(camera);

            return id;
        }
    }
}