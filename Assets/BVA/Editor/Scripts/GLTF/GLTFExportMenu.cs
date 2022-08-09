using System;
using UnityEditor;
using BVA;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GLTFExportMenu : EditorWindow
{
    public static string RetrieveTexturePath(UnityEngine.Texture texture)
    {
        return AssetDatabase.GetAssetPath(texture);
    }

    [MenuItem("BVA/Export/Settings")]
    static void Init()
    {
        GLTFExportMenu window = (GLTFExportMenu)EditorWindow.GetWindow(typeof(GLTFExportMenu), false, "Export Settings");
        window.Show();
    }
    private static readonly string[] SupportedExtensions = new string[] { "KHR_draco_mesh_compression", "KHR_material_pbrSpecularGlossiness", "KHR_lights_punctual", "KHR_texture_transform", "KHR_materials_unlit", "KHR_materials_clearcoat" };
    public static void CommonExportGUI()
    {
        var minWith = GUILayout.ExpandWidth(true);
        EditorGUIUtility.labelWidth = 400;
        GLTFSceneExporter.RequireExtensions = EditorGUILayout.Toggle(new GUIContent("Require extensions"), GLTFSceneExporter.RequireExtensions, minWith);

        GLTFSceneExporter.ExportFullPath = EditorGUILayout.Toggle("Export using original path", GLTFSceneExporter.ExportFullPath, minWith);
        GLTFSceneExporter.ExportNames = EditorGUILayout.Toggle("Export names of nodes", GLTFSceneExporter.ExportNames, minWith);
        GLTFSceneExporter.ExportTangent = EditorGUILayout.Toggle("Export mesh tangent", GLTFSceneExporter.ExportTangent, minWith);
        GLTFSceneExporter.ExportVertexColor = EditorGUILayout.Toggle("Export mesh vertex color", GLTFSceneExporter.ExportVertexColor, minWith);
        GLTFSceneExporter.ExportBlendShape = EditorGUILayout.Toggle("Export mesh blendshape", GLTFSceneExporter.ExportBlendShape, minWith);
        GLTFSceneExporter.ExportOriginalTextureFile = EditorGUILayout.Toggle("Export original texture files", GLTFSceneExporter.ExportOriginalTextureFile, minWith);
        GLTFSceneExporter.ExportUnlitWhenUsingCustomShader = EditorGUILayout.Toggle("Export unlit extension always", GLTFSceneExporter.ExportUnlitWhenUsingCustomShader, minWith);
        GLTFSceneExporter.ExportInActiveGameObject = EditorGUILayout.Toggle("Export node even the node is not active", GLTFSceneExporter.ExportInActiveGameObject, minWith);
        GLTFSceneExporter.ExportAudioQuality = EditorGUILayout.FloatField("Audio compression quality(ogg)", GLTFSceneExporter.ExportAudioQuality, minWith);
    }
    void OnGUI()
    {
        EditorGUILayout.LabelField("Exporter", EditorStyles.boldLabel);
        CommonExportGUI();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Importer", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("BVA version 0.1", MessageType.Info);
        EditorGUILayout.HelpBox($"Supported extensions: {string.Join(",", SupportedExtensions)}", MessageType.Info);
    }

    [MenuItem("GLTF/Export Selected")]
    static void ExportSelected()
    {
        ExportSelected(false);
        Debug.Log("Done!");
    }
    [MenuItem("GLTF/Export Selected To Zip")]
    static void ExportSelectedToZip()
    {
        ExportSelected(true);
        Debug.Log("Done!");
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

    static void SetExportType(GameObject root)
    {

        GLTFSceneExporter.ExportSceneType = IsAvatarModel(root) ? SceneType.Avatar : SceneType.Scene;
    }
    static void SetExportType(SceneType type)
    {
        GLTFSceneExporter.ExportSceneType = type;
    }
    static void ExportSelected(bool isCompressed)
    {
        string name;
        if (Selection.transforms.Length > 1)
            name = SceneManager.GetActiveScene().name;
        else if (Selection.transforms.Length == 1)
        {
            SetExportType(Selection.activeGameObject);
            name = Selection.activeGameObject.name;
        }
        else
            throw new Exception("No objects selected, cannot export.");

        var exportOptions = new ExportOptions { TexturePathRetriever = RetrieveTexturePath };
        var exporter = new GLTFSceneExporter(Selection.transforms, exportOptions);

        var path = EditorUtility.OpenFolderPanel("glTF Export Path", "", "");
        if (isCompressed)
        {
            string tempPath = GetTemporaryDirectory();
            exporter.SaveGLTFandBin(tempPath, name);
            string fullName = Path.Combine(path, name);
            System.IO.Compression.ZipFile.CreateFromDirectory(tempPath, fullName + ".zip");
        }
        else
        {
            if (!string.IsNullOrEmpty(path))
            {
                exporter.SaveGLTFandBin(path, name);
            }
        }
    }

    static string GetTemporaryDirectory()
    {
        string tempDirectory = Path.Combine(Application.temporaryCachePath, Path.GetRandomFileName());
        Directory.CreateDirectory(tempDirectory);
        return tempDirectory;
    }


    [MenuItem("GLTF/Export GLB Selected")]
    static void ExportGLBSelected()
    {
        string name;
        if (Selection.transforms.Length > 1)
            name = SceneManager.GetActiveScene().name;
        else if (Selection.transforms.Length == 1)
        {
            SetExportType(Selection.activeGameObject);
            name = Selection.activeGameObject.name;
        }
        else
            throw new Exception("No objects selected, cannot export.");

        var exportOptions = new ExportOptions { TexturePathRetriever = RetrieveTexturePath };
        var exporter = new GLTFSceneExporter(Selection.transforms, exportOptions);

        var path = EditorUtility.OpenFolderPanel("glTF Export Path", "", "");
        if (!string.IsNullOrEmpty(path))
        {
            exporter.SaveGLB(path, name);
        }
    }


    [MenuItem("GLTF/Export GLB&GLTF Selected")]
    static void ExportGLBAndGltfSelected()
    {
        string name;
        if (Selection.transforms.Length > 1)
            name = SceneManager.GetActiveScene().name;
        else if (Selection.transforms.Length == 1)
        {
            SetExportType(Selection.activeGameObject);
            name = Selection.activeGameObject.name;
        }
        else
            throw new Exception("No objects selected, cannot export.");

        var exportOptions = new ExportOptions { TexturePathRetriever = RetrieveTexturePath };
        var exporter = new GLTFSceneExporter(Selection.transforms, exportOptions);

        var path = EditorUtility.OpenFolderPanel("glTF Export Path", "", "");
        if (!string.IsNullOrEmpty(path))
        {
            exporter.SaveGLTFandBin(path, name);
            exporter = new GLTFSceneExporter(Selection.transforms, exportOptions);
            exporter.SaveGLB(path, name);
        }
    }


    [MenuItem("GLTF/Export Scene")]
    static void ExportScene()
    {
        var scene = SceneManager.GetActiveScene();
        var gameObjects = scene.GetRootGameObjects();
        var transforms = Array.ConvertAll(gameObjects, gameObject => gameObject.transform);
        SetExportType(SceneType.Scene);
        var exportOptions = new ExportOptions { TexturePathRetriever = RetrieveTexturePath };
        var exporter = new GLTFSceneExporter(transforms, exportOptions);
        var path = EditorUtility.OpenFolderPanel("glTF Export Path", "", "");
        if (path != "")
        {
            exporter.SaveGLB(path, scene.name);
        }
    }
}
