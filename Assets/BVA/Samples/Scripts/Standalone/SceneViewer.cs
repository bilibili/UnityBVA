using UnityEngine;
using System.IO;

namespace BVA.Sample
{
    public class SceneViewer : MonoBehaviour
    {
        public string GLTFUri = null;
        public bool Multithreaded = true;
        public bool UseStream = false;
        public bool AppendStreamingAssets = true;
        public bool PlayAnimationOnLoad = true;
        public ImporterFactory Factory = null;
        public GLTFSceneImporter.ColliderType Collider = GLTFSceneImporter.ColliderType.None;
        public Transform LastLoadedScene;
        public virtual void OnLoaded(AssetType assetType, BVAScene scene)
        {
            LastLoadedScene = scene.mainScene;
        }
        public async void LoadModel(string path, AssetType assetType)
        {
            if (!File.Exists(path))
            {
                return;
            }

            Debug.LogFormat("{0}", path);
            BVASceneManager.Instance.onSceneLoaded = OnLoaded;
            if (assetType == AssetType.Avatar)
                await BVASceneManager.Instance.LoadAvatar(path);
            else
                await BVASceneManager.Instance.LoadSceneAsync(path);
        }

        public void OpenFile(AssetType assetType)
        {
            string[] paths;
            paths = SFB.StandaloneFileBrowser.OpenFilePanel("BVA/GLTF/GLB", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("GLTF Files", BVAConst.EXTENSION_BVA, BVAConst.EXTENSION_GLTF, BVAConst.EXTENSION_GLB) }, false);

            if (paths.Length == 0)
                return;

            string path = paths[0];
            UseStream = true;
            AppendStreamingAssets = false;
            GLTFUri = path;
            LoadModel(path, assetType);
        }
    }
}