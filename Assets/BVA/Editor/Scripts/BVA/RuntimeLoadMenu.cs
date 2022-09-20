using UnityEditor;
using UnityEngine;
using System.IO;
using BVA.Loader;

namespace BVA
{
    public class RuntimeLoadMenu
    {
        [MenuItem("BVA/Runtime Load/Load Scene", priority = 90)]
        static void Init()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.EnterPlaymode();
            }
            else
            {
                var glbPath = EditorUtility.OpenFilePanel("bva/glb/gltf/zip file", "", "bva,glb,gltf,zip");
                if (string.IsNullOrEmpty(glbPath)) return;

                Load(glbPath);
            }
        }

        [MenuItem("BVA/Runtime Load/Load Avatar", priority = 91)]
        static async void Init3()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.EnterPlaymode();
            }
            else
            {
                var glbPath = EditorUtility.OpenFilePanel("bva/glb/gltf file", "", "bva,glb,gltf");
                if (string.IsNullOrEmpty(glbPath)) return;

                var importOptions = new ImportOptions
                {
                    RuntimeImport = true,
                };

                string directoryPath = URIHelper.GetDirectoryName(glbPath);
                importOptions.DataLoader = new FileLoader(directoryPath);

                var Factory = ScriptableObject.CreateInstance<DefaultImporterFactory>();
                var sceneImporter = Factory.CreateSceneImporter(glbPath, importOptions);
                await sceneImporter.LoadAvatar();
            }
        }
        [MenuItem("BVA/Runtime Load/Load(Auto Detect)",priority = 92)]
        static async void Init4()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.EnterPlaymode();
            }
            else
            {
                var glbPath = EditorUtility.OpenFilePanel("bva/glb/gltf file", "", "bva,glb,gltf");
                if (string.IsNullOrEmpty(glbPath)) return;

                var importOptions = new ImportOptions
                {
                    RuntimeImport = true,
                };

                string directoryPath = URIHelper.GetDirectoryName(glbPath);
                importOptions.DataLoader = new FileLoader(directoryPath);

                var Factory = ScriptableObject.CreateInstance<DefaultImporterFactory>();
                var sceneImporter = Factory.CreateSceneImporter(glbPath, importOptions);
                await sceneImporter.LoadAsync();
            }
        }
        static async void Load(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            Debug.LogFormat("{0}", path);
            var ext = Path.GetExtension(path).ToLower();
            switch (ext)
            {
                case ".bva":
                case ".gltf":
                case ".glb":
                case ".zip":
                    BVASceneManager.Instance.onSceneLoaded = OnLoaded;
                    await BVASceneManager.Instance.LoadSceneAsync(path);
                    break;

                default:
                    Debug.LogWarningFormat("unknown file type: {0}", path);
                    break;
            }
        }

        private static void OnLoaded(AssetType arg1, BVAScene arg2)
        {
            Debug.Log(arg2.importer.LastLoadedScene.name);
        }
    }
}