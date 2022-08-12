using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using BVA;

public class GLTFExportMenu : EditorWindow
{
    public static string RetrieveTexturePath(Texture texture)
    {
        return AssetDatabase.GetAssetPath(texture);
    }

    [MenuItem("BVA/Export/Settings")]
    static void Init()
    {
        GLTFExportMenu window = (GLTFExportMenu)EditorWindow.GetWindow(typeof(GLTFExportMenu), false, ExportCommon.Localization("导出设置", "Export Settings"));
        window.Show();
    }
    private static readonly string[] SupportedExtensions = new string[] { "KHR_draco_mesh_compression", "KHR_material_pbrSpecularGlossiness", "KHR_lights_punctual", "KHR_texture_transform", "KHR_materials_unlit", "KHR_materials_clearcoat" };
    public static void CommonExportGUI()
    {

        var minWith = GUILayout.ExpandWidth(true);
        EditorGUIUtility.labelWidth = 400;
        GLTFSceneExporter.RequireExtensions = EditorGUILayout.Toggle(ExportCommon.Localization("强制需要扩展", "Require extensions"), GLTFSceneExporter.RequireExtensions, minWith);
        GLTFSceneExporter.ExportFullPath = EditorGUILayout.Toggle(ExportCommon.Localization("使用源路径导出(gltf)", "Export using original path(gltf)"), GLTFSceneExporter.ExportFullPath, minWith);
        GLTFSceneExporter.ExportNames = EditorGUILayout.Toggle(ExportCommon.Localization("导出节点名称", "Export names of nodes"), GLTFSceneExporter.ExportNames, minWith);
        GLTFSceneExporter.ExportTangent = EditorGUILayout.Toggle(ExportCommon.Localization("导出网格切线", "Export mesh tangent"), GLTFSceneExporter.ExportTangent, minWith);
        GLTFSceneExporter.ExportVertexColor = EditorGUILayout.Toggle(ExportCommon.Localization("导出网格定点色", "Export mesh vertex color"), GLTFSceneExporter.ExportVertexColor, minWith);
        GLTFSceneExporter.ExportBlendShape = EditorGUILayout.Toggle(ExportCommon.Localization("导出网格Blendshape", "Export mesh blendshape"), GLTFSceneExporter.ExportBlendShape, minWith);
        GLTFSceneExporter.ExportOriginalTextureFile = EditorGUILayout.Toggle(ExportCommon.Localization("导出源贴图文件(png/jpeg)", "Export original texture files(png/jpeg)"), GLTFSceneExporter.ExportOriginalTextureFile, minWith);
        GLTFSceneExporter.ExportUnlitWhenUsingCustomShader = EditorGUILayout.Toggle(ExportCommon.Localization("总是导出自发光材质扩展", "Export unlit extension always"), GLTFSceneExporter.ExportUnlitWhenUsingCustomShader, minWith);
        GLTFSceneExporter.ExportInActiveGameObject = EditorGUILayout.Toggle(ExportCommon.Localization("即使节点不激活也导出", "Export node even the node is not active"), GLTFSceneExporter.ExportInActiveGameObject, minWith);
        GLTFSceneExporter.ExportAudioQuality = EditorGUILayout.FloatField(ExportCommon.Localization("音频压缩质量(ogg)", "Audio compression quality(ogg)"), GLTFSceneExporter.ExportAudioQuality, minWith);
    }
    void OnGUI()
    {
        ExportCommon.ShowLanguageSwitchButton();
        EditorGUILayout.LabelField(ExportCommon.Localization("导出", "Export"), EditorStyles.boldLabel);
        CommonExportGUI();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(ExportCommon.Localization("导入", "Import"), EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        EditorGUILayout.HelpBox(ExportCommon.Localization($"BVA格式版本 {BVAConst.FORMAT_VERSION}", $"BVA version {BVAConst.FORMAT_VERSION}"), MessageType.Info);
        EditorGUILayout.HelpBox(ExportCommon.Localization($"支持的扩展 : {string.Join(",", SupportedExtensions)}", $"Supported extensions : {string.Join(",", SupportedExtensions)}"), MessageType.Info);
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

    static void ExportSelected(bool isCompressed)
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

        var exportOptions = new ExportOptions { TexturePathRetriever = RetrieveTexturePath, ExportAvatar= IsAvatarModel(Selection.activeGameObject) };
        var exporter = new GLTFSceneExporter(Selection.transforms, exportOptions);

        var path = EditorUtility.OpenFolderPanel(ExportCommon.Localization("glTF导出路径", "glTF Export Path"), "", "");
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
            name = Selection.activeGameObject.name;
        }
        else
            throw new Exception("No objects selected, cannot export.");

        var exportOptions = new ExportOptions { TexturePathRetriever = RetrieveTexturePath, ExportAvatar = IsAvatarModel(Selection.activeGameObject) };
        var exporter = new GLTFSceneExporter(Selection.transforms, exportOptions);

        var path = EditorUtility.OpenFolderPanel(ExportCommon.Localization("glTF导出路径", "glTF Export Path"), "", "");
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
            name = Selection.activeGameObject.name;
        }
        else
            throw new Exception("No objects selected, cannot export.");

        var exportOptions = new ExportOptions { TexturePathRetriever = RetrieveTexturePath, ExportAvatar = IsAvatarModel(Selection.activeGameObject) };
        var exporter = new GLTFSceneExporter(Selection.transforms, exportOptions);

        var path = EditorUtility.OpenFolderPanel(ExportCommon.Localization("glTF导出路径", "glTF Export Path"), "", "");
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
        var exportOptions = new ExportOptions { TexturePathRetriever = RetrieveTexturePath };
        var exporter = new GLTFSceneExporter(transforms, exportOptions);
        var path = EditorUtility.OpenFolderPanel(ExportCommon.Localization("glTF导出路径", "glTF Export Path"), "", "");
        if (path != "")
        {
            exporter.SaveGLB(path, scene.name);
        }
    }
}
