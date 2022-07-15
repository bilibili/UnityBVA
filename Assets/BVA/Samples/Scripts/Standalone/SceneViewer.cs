using UnityEngine;
using System.IO;
using BVA;

namespace BVA.Sampler
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
            var ext = Path.GetExtension(path).ToLower();
            switch (ext)
            {
                case ".gltf":
                case ".glb":
                    BVASceneManager.Instance.onSceneLoaded = OnLoaded;
                    await BVASceneManager.Instance.LoadSceneAsync(path);
                    break;
                default:
                    Debug.LogWarningFormat("unknown file type: {0}", path);
                    break;
            }
        }
        public void LoadMotion(string path)
        {

        }
        public void OpenFile(AssetType assetType)
        {
            string extension = BVAConst.EXTENSION_GLB;
            //if (assetType == AssetType.Scene)
            //    extension = BVAConst.EXTENSION_BVA_SCENE_GLB;
            //if (assetType == AssetType.Avatar)
            //    extension = BVAConst.EXTENSION_BVA_AVATAR_GLB;

            string[] paths;
            if (assetType == AssetType.Common || assetType == AssetType.StandardGLTF)
                paths = SFB.StandaloneFileBrowser.OpenFilePanel("BVA", "", new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("GLTF Files", "gltf", "glb") }, false);
            else
                paths = SFB.StandaloneFileBrowser.OpenFilePanel("BVA", "", extension, false);

            if (paths.Length == 0)
                return;

            string path = paths[0];
            UseStream = true;
            AppendStreamingAssets = false;
            GLTFUri = path;

            var ext = Path.GetExtension(path).ToLower();
            switch (ext)
            {
                case ".gltf":
                case ".glb":
                    LoadModel(path, assetType);
                    break;
                case ".bvh":
                    LoadMotion(path);
                    break;
            }
        }
    }
}