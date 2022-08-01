using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using BVA;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using BVA.Extensions;
using System;

namespace BVA
{
    public class ExportSceneMenu : EditorWindow
    {
        [MenuItem("BVA/Export/Export Scene")]
        static void Init()
        {
            ExportSceneMenu window = (ExportSceneMenu)GetWindow(typeof(ExportSceneMenu), false, "Export Scene Settings");
            window.Show();
        }

        public GameObject _rootGameObject;
        ExportInfo exportInfo;
        SceneAsset mainScene;
        List<SceneAsset> sceneAssets;
        int sceneCount = 0;
        string exportName;
        private Scene ActiveScene { get { return SceneManager.GetActiveScene(); } }
        int exportMode;
        static string[] EDIT_MODES = new[]{
            "Export GameObject",
            "Export Single Scene",
            "Export Multiply Scenes",
        };
        bool foldCommonProperty = true;
        ExportFileType exportType;
        //Collect material info which are not supported by exporter
        void CheckMaterialValidity()
        {
            
        }

        private void OnEnable()
        {
            if (sceneAssets == null) sceneAssets = new List<SceneAsset>();
            mainScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(ActiveScene.path);
        }

        private void OnGUI()
        {
            exportMode = GUILayout.Toolbar(exportMode, EDIT_MODES);
            EditorGUILayout.HelpBox($"version {BVAConst.FORMAT_VERSION}", MessageType.Info);
            EditorGUILayout.Space();
            foldCommonProperty = EditorGUILayout.Foldout(foldCommonProperty, "Common Export Settings");
            if (foldCommonProperty)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();
                GLTFExportMenu.CommonExportGUI();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            GLTFSceneExporter.DracoMeshCompression = EditorGUILayout.Toggle("Use Draco Compression", GLTFSceneExporter.DracoMeshCompression);
            GLTFSceneExporter.ExportLightmap = EditorGUILayout.Toggle("Export Lightmap", GLTFSceneExporter.ExportLightmap);
            GLTFSceneExporter.ExportRenderSetting = EditorGUILayout.Toggle("Export RenderSetting", GLTFSceneExporter.ExportRenderSetting);
            GLTFSceneExporter.ExportSkybox = EditorGUILayout.Toggle("Export Skybox", GLTFSceneExporter.ExportSkybox);

            exportType = (ExportFileType)EditorGUILayout.EnumPopup("Export Format", exportType);

            switch (exportMode)
            {
                case 0:
                    ExportGameObjects();
                    break;
                case 1:
                    ExportScene();
                    break;
                case 2:
                    ExportMultipleScenes();
                    break;
            }
            EditorGUILayout.Separator();
            if (exportInfo != null)
                ExportCommon.EditorGUICollectExportInfo(exportInfo);

            EditorGUILayout.Separator();
            ExportCommon.EditorGUIExportLog();

        }
        private void CollectExportInfo(GameObject root)
        {
            if (_rootGameObject != null)
                exportInfo = new ExportInfo(_rootGameObject);
        }
        private void CollectExportInfo()
        {
            var scene = ActiveScene;
            var gameObjects = scene.GetRootGameObjects();
            exportInfo = new ExportInfo(gameObjects);
        }
        private void CollectExportInfo(SceneAsset sceneAsset)
        {
            var scene = EditorSceneManager.GetSceneByPath(AssetDatabase.GetAssetPath(sceneAsset));
            var gameObjects = scene.GetRootGameObjects();
            exportInfo = new ExportInfo(gameObjects);
        }
        private void ExportScene(GLTFSceneExporter exporter, string sceneName, string ext = BVAConst.EXTENSION_BVA_SCENE, string title = "BVA Scene Export Path", string pref = "avatar_export_path")
        {
            var path = EditorUtility.OpenFolderPanel(title, EditorPrefs.GetString(pref), "");

            if (exportType == ExportFileType.GLTF) ext = BVAConst.EXTENSION_GLTF;
            if (exportType == ExportFileType.GLB) ext = BVAConst.EXTENSION_GLB;

            if (string.IsNullOrEmpty(sceneName))
            {
                EditorUtility.DisplayDialog("Scene doesn't have a valid name", $"Please try save scene before exporting!", "OK");
                return;
            }

            if (!string.IsNullOrEmpty(path))
            {
                EditorPrefs.SetString(pref, path);
                if (exportType == ExportFileType.GLTF)
                    exporter.SaveGLTFandBin(path, sceneName);
                else
                    exporter.SaveGLB(path, sceneName, ext);
                EditorUtility.DisplayDialog("export success", $"spend {exporter.ExportDuration.TotalSeconds}s finish export!", "OK");
            }
        }
        private void ExportGameObjects()
        {
            if (GLTFSceneExporter.ExportLightmap || GLTFSceneExporter.ExportSkybox)
            {
                EditorGUILayout.HelpBox("Export GameObjects usually do not require RenderSetting info, but you can still exort lightmap and skybox ", MessageType.Warning);
            }

            EditorGUILayout.BeginHorizontal();
            _rootGameObject = EditorGUILayout.ObjectField("GameObject Root", _rootGameObject, typeof(GameObject), true) as GameObject;
            if (GUILayout.Button("Select"))
            {
                _rootGameObject = Selection.activeGameObject;
            }
            if (GUILayout.Button("Collect Info"))
            {
                CollectExportInfo(_rootGameObject);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();

            CheckMaterialValidity();

            if (GUILayout.Button("Export Root"))
            {
                if (_rootGameObject == null)
                {
                    EditorUtility.DisplayDialog("Error", "export root must specify a valid GameObject as Root", "OK");
                    return;
                }
                var exportOptions = new ExportOptions { TexturePathRetriever = AssetDatabase.GetAssetPath };
                var exporter = new GLTFSceneExporter(new Transform[] { _rootGameObject.transform }, exportOptions);
                ExportScene(exporter, _rootGameObject.name);
            }
        }
        private void ExportScene()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Collect Info"))
            {
                CollectExportInfo();
            }

            if (GUILayout.Button("Export Current Scene"))
            {
                var scene = ActiveScene;
                var gameObjects = scene.GetRootGameObjects();
                var transforms = Array.ConvertAll(gameObjects, gameObject => gameObject.transform);

                var exportOptions = new ExportOptions { TexturePathRetriever = AssetDatabase.GetAssetPath };
                var exporter = new GLTFSceneExporter(transforms, exportOptions);

                ExportScene(exporter, scene.name);
            }
            EditorGUILayout.EndHorizontal();
        }
        private Scene GetScene(SceneAsset asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            return EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        }
        private void ExportMultipleScenes()
        {
            if (sceneAssets == null) sceneAssets = new List<SceneAsset>();
            EditorGUILayout.BeginHorizontal();
            {
                mainScene = EditorGUILayout.ObjectField("Main Scene : ", mainScene, typeof(SceneAsset), false) as SceneAsset;
                if (GUILayout.Button("Add Loaded Scenes"))
                {
                    mainScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorSceneManager.GetSceneAt(0).path);
                    sceneCount = EditorSceneManager.sceneCount - 1;
                    sceneAssets.ResetCount(sceneCount);
                    if (EditorSceneManager.sceneCount > 1)
                    {
                        for (int i = 0; i < EditorSceneManager.sceneCount - 1; i++)
                        {
                            sceneAssets.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorSceneManager.GetSceneAt(i).path));
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            sceneCount = EditorGUILayout.IntSlider("Extra Scene Count : ", sceneCount, 0, 10);
            if (sceneAssets == null) sceneAssets = new List<SceneAsset>();
            sceneAssets.ResetCount(sceneCount);
            for (int i = 0; i < sceneAssets.Count; i++)
            {
                EditorGUILayout.Space();
                sceneAssets[i] = EditorGUILayout.ObjectField(sceneAssets[i], typeof(SceneAsset), false) as SceneAsset;
            }
            EditorGUILayout.BeginHorizontal();
            {
                exportName = EditorGUILayout.TextField("Export Name: ", exportName);
                if (GUILayout.Button("Collect Info"))
                {
                    foreach (var asset in sceneAssets)
                        CollectExportInfo(asset);
                }
                Dictionary<string, Transform[]> transformsList = new Dictionary<string, Transform[]>();
                if (GUILayout.Button("Export Multiple Scenes"))
                {
                    List<Scene> loadedScene = new List<Scene>();
                    var scene = GetScene(mainScene);
                    var gameObjects = scene.GetRootGameObjects();
                    var transforms = Array.ConvertAll(gameObjects, gameObject => gameObject.transform);

                    foreach (var v in sceneAssets)
                    {
                        var extraScene = GetScene(v);
                        var rootGameObjects = extraScene.GetRootGameObjects();
                        var rootTransforms = Array.ConvertAll(rootGameObjects, rootGameObjects => rootGameObjects.transform);
                        transformsList.Add(v.name, rootTransforms);
                        loadedScene.Add(extraScene);
                    }

                    var exportOptions = new ExportOptions { TexturePathRetriever = AssetDatabase.GetAssetPath };
                    var exporter = new GLTFSceneExporter(transformsList, transforms, exportOptions);
                    ExportScene(exporter, string.IsNullOrEmpty(exportName) ? scene.name : exportName);

                    foreach (var v in loadedScene)
                        EditorSceneManager.UnloadSceneAsync(v);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
