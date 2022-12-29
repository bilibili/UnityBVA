#if UNITY_EDITOR
using BVA.Loader;
using BVA;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using BVA.FileEncryptor;

public class EditorLoadCryptoZipMenu : Editor
{
    [MenuItem("BVA/Runtime Load/Load Crypto", priority = 93)]
    static async void LoadCrypto()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorApplication.EnterPlaymode();
        }
        else
        {
            var glbPath = EditorUtility.OpenFilePanel("bva file", "", "bva");
            if (string.IsNullOrEmpty(glbPath)) return;
            var importOptions = new ImportOptions
            {
                RuntimeImport = true,
            };

            string directoryPath = URIHelper.GetDirectoryName(glbPath);
            importOptions.DataLoader = new CryptoZipFileLoader(directoryPath);

            var Factory = ScriptableObject.CreateInstance<DefaultImporterFactory>();
            var sceneImporter = Factory.CreateSceneImporter(glbPath, importOptions);
            await sceneImporter.LoadAsync();
        }
    }
    static bool IsAvatarModel(GameObject root)
    {
        if (root.TryGetComponent<Animator>(out var animator))
        {
            if (animator != null && animator.avatar != null && animator.avatar.isHuman)
            {
                return true;
            }
        }
        return false;
    }
    [MenuItem("GLTF/Export Compressed BVA Selected")]
    static void ExportGLBSelected()
    {
        string name;
        if (Selection.transforms.Length > 1)
            name = SceneManager.GetActiveScene().name;
        else if (Selection.transforms.Length == 1)
        {
            name = Selection.activeGameObject.name;
        }
        else
            throw new Exception("No objects selected, cannot export.");

        var exportOptions = new ExportOptions { TexturePathRetriever = null,  ExportAvatar = IsAvatarModel(Selection.activeGameObject) };
        var exporter = new GLTFSceneExporter(Selection.transforms, exportOptions);

        var path = EditorUtility.SaveFilePanel("BVA Compressed File Export ", "", "Crypto", "bva");
        if (!string.IsNullOrEmpty(path))
        {
            exporter.SaveBVACompressed(path, name);
        }
    }
}
#endif