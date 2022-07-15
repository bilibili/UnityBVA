using UnityEditor;
using UnityEngine;
using System.IO;
using BVA;
using BVA.Loader;
using System;

namespace BVA
{
    public class RuntimeLoadMenu
    {
        [MenuItem("BVA/Runtime Load/Load Avatar")]
        static async void Init3()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.EnterPlaymode();
            }
            else
            {
                var glbPath = EditorUtility.OpenFilePanel("glb or gltf file", "", "glb,gltf");

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


        [MenuItem("BVA/Runtime Load/Load (GLB&GLTF)")]
        static void Init()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.EnterPlaymode();
            }
            else
            {
                var glbPath = EditorUtility.OpenFilePanel("glb or gltf file", "", "glb,gltf");

                Load(glbPath);
            }
        }

        [MenuItem("BVA/Runtime Load/Load (Zip Archived)")]

        static void Init2()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.EnterPlaymode();
            }
            else
            {
                var glbPath = EditorUtility.OpenFilePanel("zip file", "", "zip");
                Load(glbPath);
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