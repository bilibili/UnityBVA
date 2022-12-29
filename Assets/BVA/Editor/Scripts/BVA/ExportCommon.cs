using UnityEditor;
using UnityEngine;
using BVA.Component;
using GLTF.Schema.BVA;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using BVA.Extensions;

namespace BVA
{
    public enum EditorLanguage
    {
        Zh,
        En
    }
    public static class ExportCommon
    {
        const string PREFS_KEY = "BVA_LANG";
        const string ASSET_DIR = "Assets/";
        static bool _folderLog;
        static bool _folderExoprtInfo = true;
        static bool _folderExoprtInfo_Mesh = true;
        public static readonly HashSet<string> DEFAULT_SHADER_NAME = new HashSet<string>() { "Universal Render Pipeline/Lit", "Universal Render Pipeline/Complex Lit", "Universal Render Pipeline/Unlit" };
        public static EditorLanguage EditorLanguage
        {
            get { return (EditorLanguage)EditorPrefs.GetInt(PREFS_KEY); }
            set { EditorPrefs.SetInt(PREFS_KEY, (int)value); }
        }
        public static string Localization(string zh, string en) => EditorLanguage == EditorLanguage.Zh ? zh : en;

        public static void ShowLanguageSwitchButton(float positionLerp = 0.8f, float width = 60)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            Rect rect = EditorGUILayout.GetControlRect();
            Rect resultRect = new Rect(
                Mathf.Lerp(rect.xMin, rect.xMax, positionLerp),
                rect.yMin,
                width,
                rect.height
                );

            EditorLanguage = (EditorLanguage)GUI.Toolbar(resultRect, (int)EditorLanguage, new string[] { "CN", "EN" });
            EditorGUILayout.EndHorizontal();
        }

        public static void FixMissingMetaInfo(GameObject Root)
        {
            var metaComp = Root.GetOrAddComponent<BVAMetaInfo>();
            if (metaComp.metaInfo == null)
            {
                var meta = ScriptableObject.CreateInstance<BVAMetaInfoScriptableObject>();
                var savePath = $"{ASSET_DIR}{Root.name}_MetaInfo.asset";
                AssetDatabase.CreateAsset(meta, savePath);
                metaComp.metaInfo = AssetDatabase.LoadAssetAtPath<BVAMetaInfoScriptableObject>(savePath);
            }
        }

        public static void FixMissingBlendshapeMixer(GameObject Root)
        {
            var blendShape = Root.GetOrAddComponent<BlendShapeMixer>();
            Selection.activeGameObject = Root;
        }

        public static void EditorGUIExportLog()
        {
            _folderLog = EditorGUILayout.BeginToggleGroup(Localization("显示Log", "Show Logs"), _folderLog);
            if (_folderLog)
            {
                LogPool.ExportLogger.OnGUI();
            }
            EditorGUILayout.EndToggleGroup();
        }

        public static void EditorGUICollectExportInfo(ExportInfo info)
        {
            _folderExoprtInfo = EditorGUILayout.BeginToggleGroup(Localization("显示导出信息", "Show Export Information"), _folderExoprtInfo);
            if (_folderExoprtInfo)
            {
                //_folderExoprtInfo_Mesh = EditorGUILayout.BeginFoldoutHeaderGroup(_folderExoprtInfo_Mesh, "Mesh Information");
                if (_folderExoprtInfo_Mesh)
                {
                    EditorGUILayout.HelpBox(
                        $"Mesh Count : {info.meshInfo.MeshCount}   Vertex Count : {info.meshInfo.VertexCount}\n" +
                        $"Material Count : {info.materials.Count}\n" +
                        $"Texture Count : {info.textures.Count}\n" +
                        $"Avatar Count : {info.avatars.Count}\n" +
                        $"Audio Count : {info.audioInfo.AudioClipCount}   Size : {info.audioInfo.Size}", MessageType.Info);
                }
                //EditorGUILayout.EndFoldoutHeaderGroup();
            }
            EditorGUILayout.EndToggleGroup();
        }

        public static void EditorGUICheckBuildPlatform()
        {
            if (!EditorUserBuildSettings.activeBuildTarget.ToString().Contains("Standalone"))
            {
                EditorGUILayout.HelpBox(Localization("请在Build Settings里切换到Standalone(Windows,Mac,Linux)平台", "Please switch to Standalone(Windows,Mac,Linux) platform in Build Settings"), MessageType.Error);
            }
        }

        /// <summary>
        /// Automatic add AudioClip to AudioContainer in AudioSource
        /// </summary>
        /// <param name="root"></param>
        public static void AddExistAudioClipToContainer(AudioClipContainer container, GameObject root)
        {
            var sources = root.GetComponentsInChildren<AudioSource>();
            foreach (var source in sources)
            {
                if (source.clip != null && !container.audioClips.Contains(source.clip))
                {
                    container.audioClips.Add(source.clip);
                }
            }
        }

        public static List<Material> CheckModelShaderIsVaild(List<SceneAsset> sceneAssets)
        {
            var scenes = sceneAssets.Select(x => EditorSceneManager.GetSceneByPath(AssetDatabase.GetAssetPath(x))).ToArray();
            return CheckModelShaderIsVaild(scenes);
        }
        public static List<Material> CheckModelShaderIsVaild(params Scene[] scenes)
        {
            List<Material> result = new List<Material>();
            if (scenes == null || scenes.Length == 0)
            {
                return result;
            }

            for (int i = 0; i < scenes.Length; i++)
            {
                var scene = scenes[i];
                if (!scene.IsValid() || scene.rootCount == 0)
                {
                    continue;
                }
                var gameObjects = scene.GetRootGameObjects();
                foreach (var gameObject in gameObjects)
                {
                    var tempResult = CheckModelShaderIsVaild(gameObject);
                    result.AddRange(tempResult);
                }
            }


            return result;
        }
        public static List<Material> CheckModelShaderIsVaild(GameObject model)
        {
            List<Material> errorList = new List<Material>();
            if (model == null)
            {
                return errorList;
            }
            Renderer[] allRenderer = model.GetComponentsInChildren<Renderer>();
            bool isShow_EnvironmentReflectionsOnce = false;
            foreach (var renderer in allRenderer)
            {
                var materials = renderer.sharedMaterials;
                foreach (var material in materials)
                {
                    if (material == null)
                    {
                        continue;
                    }
                    Shader shader = material.shader;

                    if (DEFAULT_SHADER_NAME.Contains(shader.name))
                    {
                        if (material.GetFloat("_EnvironmentReflections") == 1)
                        {
                            isShow_EnvironmentReflectionsOnce = true;
                        }

                    }
                    else if (!(MaterialImporter.CustomShaders.ContainsKey(shader.name) || DEFAULT_SHADER_NAME.Contains(shader.name)))
                    {
                        errorList.Add(material);
                        string data = ExportCommon.Localization($"{shader.name} 不是支持的shader,但是在 {material.name}被使用!", $"{shader.name} is not vaild shader for exporter ,but is is been used by {material.name} !");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox(data, MessageType.Warning);
                        if (GUILayout.Button(ExportCommon.Localization("选择Renderer", "Select Renderer")))
                        {
                            Selection.activeObject = renderer;
                        }
                        if (GUILayout.Button(ExportCommon.Localization("选择Material", "Select Material")))
                        {
                            Selection.activeObject = material;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                }
            }

            if (isShow_EnvironmentReflectionsOnce)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(ExportCommon.Localization(" BVA导出URP材质时不支持EnvironmentReflections!", $"BVA does not support ElementReflections when exporting URP Materials !"), MessageType.Warning);
                if (GUILayout.Button(ExportCommon.Localization("修复", "Fix it")))
                {
                    foreach (var renderer in allRenderer)
                    {
                        var materials = renderer.sharedMaterials;
                        foreach (var material in materials)
                        {
                            material.SetFloat("_EnvironmentReflections", 0);
                        }
                    }

                }
                EditorGUILayout.EndHorizontal();
            }

            return errorList;
        }
    }
}