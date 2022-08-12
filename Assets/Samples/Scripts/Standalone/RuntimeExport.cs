using UnityEngine;
using BVA;
using System.IO;

namespace BVA.Sample
{
    public class RuntimeExport : SceneViewer
    {
        public Transform root;
        public void AddCube()
        {
            AddPrimitive(PrimitiveType.Cube);
        }
        public void AddSphere()
        {
            AddPrimitive(PrimitiveType.Sphere);
        }
        public void AddCapsule()
        {
            AddPrimitive(PrimitiveType.Capsule);
        }
        public void AddPrimitive(PrimitiveType primitiveType)
        {
            GameObject gameObject = GameObject.CreatePrimitive(primitiveType);
            gameObject.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(0.5f, 1.5f), Random.Range(-5.0f, 5.0f));
            gameObject.transform.SetParent(root, true);
        }
        public void AddModel()
        {
            OpenFile(AssetType.Common);
        }
        public override void OnLoaded(AssetType assetType, BVAScene scene)
        {
            base.OnLoaded(assetType, scene);
            LastLoadedScene.SetParent(root);
        }
        public void ExportAsGLB()
        {
            var exportOptions = new ExportOptions { TexturePathRetriever = null,ExportAvatar = true };
            var exporter = new GLTFSceneExporter(new Transform[] { root }, exportOptions);
            var path = SFB.StandaloneFileBrowser.SaveFilePanel("Runtime Export", "", "RuntimeExport", BVAConst.EXTENSION_BVA_AVATAR);
            if (!string.IsNullOrEmpty(path))
            {
                exporter.SaveGLB(Path.GetDirectoryName(path),Path.GetFileNameWithoutExtension(path));
            }
        }
    }
}