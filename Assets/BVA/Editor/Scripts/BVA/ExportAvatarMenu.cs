using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BVA.Component;
using System.Linq;
using System.IO;

#if ENABLE_CRYPTO
using BVA.FileEncryptor;
#endif

namespace BVA
{
    public class ExportAvatarMenu : EditorWindow
    {
        [MenuItem("BVA/Export/Export Avatar")]
        static void Init()
        {
            ExportAvatarMenu window = (ExportAvatarMenu)EditorWindow.GetWindow(typeof(ExportAvatarMenu), false, ExportCommon.Localization("导出角色模型", "Export Avatar"));
            window.Show();
            window.Root = Selection.activeGameObject;
            GLTFSceneExporter.SetDefaultAvatarExportSetting();
        }
        public GameObject Root;

        ExportInfo exportInfo;
        #region Container Export
        AudioClipContainer audioClipContainer;
        SkyboxContainer skyboxContainer;
        Dictionary<System.Type, Editor> containerEditors;
        static bool _folderAudioEditor, _folderSkyboxEditor;
        private Vector2 scrollPosition;

        bool ExportAudios => _folderAudioEditor && audioClipContainer.audioClips.Count > 0;

        void ShowContainerGUI<T>(T container) where T : MonoBehaviour
        {
            if (container == null) return;
            if (containerEditors == null) containerEditors = new Dictionary<System.Type, Editor>();
            if (containerEditors.TryGetValue(container.GetType(), out Editor editor))
            {
                editor.OnInspectorGUI();
            }
            else
            {
                containerEditors.Add(container.GetType(), Editor.CreateEditor(container));
            }
        }
        #endregion
        string TextFixIt => ExportCommon.Localization("修复", "Fix it");
        string TextAdd => ExportCommon.Localization("添加", "Add");

        void CheckRootValidity()
        {
            if (Root == null) return;
            #region Animator Human Check
            var animator = Root.GetComponent<Animator>();
            if (animator == null)
            {
                EditorGUILayout.HelpBox(ExportCommon.Localization("找不到Animator组件", "not find Animator component"), MessageType.Error);
                return;
            }
            if (!animator.isHuman)
            {
                EditorGUILayout.HelpBox(ExportCommon.Localization("不是有效的Avatar物体，找不到包含有效Avatar的Animator组件", "Not a valid avatar gameObject, not find Animator component with human avatar"), MessageType.Error);

                if (animator.avatar != null)
                {
                    string avatarPath = AssetDatabase.GetAssetPath(animator.avatar);
                    ModelImporter importer = AssetImporter.GetAtPath(avatarPath) as ModelImporter;
                    if (importer != null && importer.animationType != ModelImporterAnimationType.Human)
                    {
                        if (GUILayout.Button(TextFixIt))
                        {
                            importer.animationType = ModelImporterAnimationType.Human;
                            importer.SaveAndReimport();
                        }
                    }
                }
                else
                {
                    return;
                }

            }
            #endregion

            #region Mesh isReadWrite Check

            var meshs = Root.GetComponentsInChildren<SkinnedMeshRenderer>().Where(x => !x.sharedMesh.isReadable).ToArray();

            if (meshs.Length != 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(ExportCommon.Localization("模型包含无效的网格，网格的read/write应该被勾选", "Model containning invalid mesh, mesh's read/write should be enabled"), MessageType.Error);

                if (GUILayout.Button(ExportCommon.Localization("修复", TextFixIt)))
                {
                    for (int i = 0; i < meshs.Length; i++)
                    {
                        string meshPath = AssetDatabase.GetAssetPath(animator.avatar);
                        ModelImporter importer = AssetImporter.GetAtPath(meshPath) as ModelImporter;
                        if (importer.isReadable != true)
                        {
                            importer.isReadable = true;
                            importer.SaveAndReimport();
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            #endregion

            #region Root Transform Check
            if (Root.transform.localRotation != Quaternion.identity || Root.transform.localPosition != Vector3.zero || Root.transform.localScale != Vector3.one)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(ExportCommon.Localization("需要重置根节点", "Please reset the root transform!"), MessageType.Error);
                if (GUILayout.Button(TextFixIt))
                {
                    Root.transform.localRotation = Quaternion.identity;
                    Root.transform.localPosition = Vector3.zero;
                    Root.transform.localScale = Vector3.one;
                }
                EditorGUILayout.EndHorizontal();
            }
            #endregion
            #region MetaInfo Check
            bool isValid = true;
            var meta = Root.GetComponent<BVAMetaInfo>();
            if (meta == null || meta.metaInfo == null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(ExportCommon.Localization($"你需要添加一个{nameof(BVAMetaInfo)}以便导出角色模型的法律信息", $"You need assign a {nameof(BVAMetaInfo)} to export avatar for legal use"), MessageType.Error);
                if (GUILayout.Button(TextFixIt))
                {
                    ExportCommon.FixMissingMetaInfo(Root);
                }
                EditorGUILayout.EndHorizontal();
                isValid = false;
            }
            var mixer = Root.GetComponent<BlendShapeMixer>();
            if (mixer == null || mixer.keys == null || mixer.keys.Count == 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(ExportCommon.Localization($"你需要添加一个{nameof(BlendShapeMixer)}组件以支持面捕", $"You need assign a {nameof(BlendShapeMixer)} to export avatar"), MessageType.Error);
                if (GUILayout.Button(TextFixIt))
                {
                    ExportCommon.FixMissingBlendshapeMixer(Root);
                }
                EditorGUILayout.EndHorizontal();
                isValid = false;
            }

            #endregion
            #region Material Check
            ExportCommon.CheckModelShaderIsVaild(Root);
            #endregion
            if (!isValid) return;

            _folderAudioEditor = EditorGUILayout.BeginToggleGroup(ExportCommon.Localization("导出音频", " Export AudioClip"), _folderAudioEditor);
            if (_folderAudioEditor)
            {
                audioClipContainer = Root.GetComponent<AudioClipContainer>();
                if (audioClipContainer == null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox(ExportCommon.Localization($"没有发现{nameof(AudioClipContainer)}，需要添加一个新的组件吗？", $"Not find {nameof(AudioClipContainer)} on avatar, do you need to assign a new one"), MessageType.Info);
                    if (GUILayout.Button(TextAdd))
                    {
                        audioClipContainer = Root.AddComponent<AudioClipContainer>();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    if (GUILayout.Button(ExportCommon.Localization("添加一个AudioClip到AudioSouce里", "Add AudioClip in AudioSource")))
                    {
                        ExportCommon.AddExistAudioClipToContainer(audioClipContainer, Root);
                    }
                }
                ShowContainerGUI(audioClipContainer);
            }
            EditorGUILayout.EndToggleGroup();

            _folderSkyboxEditor = EditorGUILayout.BeginToggleGroup(ExportCommon.Localization(" 导出天空盒", " Export Skybox"), _folderSkyboxEditor);
            if (_folderSkyboxEditor)
            {
                skyboxContainer = Root.GetComponent<SkyboxContainer>();
                if (skyboxContainer == null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox(ExportCommon.Localization($"没有发现{nameof(SkyboxContainer)}，需要添加一个新的组件吗？", $"Not find {nameof(SkyboxContainer)} on avatar, do you need to assign a new one"), MessageType.Info);
                    if (GUILayout.Button(TextAdd))
                    {
                        skyboxContainer = Root.AddComponent<SkyboxContainer>();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                ShowContainerGUI(skyboxContainer);
            }
            EditorGUILayout.EndToggleGroup();
            ////////////////////////////////////////////////////////////////////////////////
            EditorGUILayout.Separator();
#if ENABLE_CRYPTO
            bool encrypto = EditorConfidential.GUIPassword(out string password);
#endif
            if (GUILayout.Button(ExportCommon.Localization("导出", "Export")))
            {
                var exportOptions = new ExportOptions { TexturePathRetriever = AssetDatabase.GetAssetPath, ExportAvatar = true };
                var exporter = new GLTFSceneExporter(new Transform[] { Root.transform }, exportOptions);

                var path = EditorUtility.OpenFolderPanel(ExportCommon.Localization("BVA角色导出路径", "BVA Avatar Export Path"), EditorPrefs.GetString("avatar_export_path"), "");
                if (!string.IsNullOrEmpty(path))
                {
                    EditorPrefs.SetString("avatar_export_path", path);
#if ENABLE_CRYPTO
                    if (encrypto)
                        exporter.SaveBVACompressed(Path.Combine(path, Root.name) + $".{BVAConst.EXTENSION_BVA}", Root.name, password);
                    else
                        exporter.SaveGLB(path, Root.name);
#else
                    exporter.SaveGLB(path, Root.name);
#endif
                    EditorUtility.DisplayDialog(ExportCommon.Localization("导出成功", "export success"), ExportCommon.Localization($"花费{exporter.ExportDuration.TotalSeconds}秒完成导出", $"spend {exporter.ExportDuration.TotalSeconds}s finish export!"), "OK");
                }
            }
        }
        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            ExportCommon.ShowLanguageSwitchButton();
            EditorGUILayout.LabelField(ExportCommon.Localization("导出", "Export"), EditorStyles.boldLabel);

            ExportCommon.EditorGUICheckBuildPlatform();

            EditorGUILayout.BeginHorizontal();
            Root = EditorGUILayout.ObjectField(ExportCommon.Localization("根节点", "Export root"), Root, typeof(GameObject), true) as GameObject;
            if (GUILayout.Button(ExportCommon.Localization("选择", "Select")))
            {
                Root = Selection.activeGameObject;
            }

            if (GUILayout.Button(ExportCommon.Localization("收集导出信息", "Collect Export Info")))
            {
                exportInfo = new ExportInfo(Root);
            }
            EditorGUILayout.EndHorizontal();
            //EditorGUILayout.Separator();
            if (exportInfo != null)
                ExportCommon.EditorGUICollectExportInfo(exportInfo);

            GLTFSceneExporter.ExportAnimationClips = EditorGUILayout.Toggle(ExportCommon.Localization("导出动画", "Export Animations"), GLTFSceneExporter.ExportAnimationClips);
            if (GLTFSceneExporter.ExportAnimationClips)
                EditorGUILayout.HelpBox(ExportCommon.Localization("导出Animation组件上标记为legacy的动画，并转换Animator组件上的动画为骨骼蒙皮动画导出", $"Export legacy animation on Animation, as well convert mecanim animation to legacy skin animation and export it"), MessageType.Info);

            EditorGUILayout.Separator();
            CheckRootValidity();
            EditorGUILayout.Separator();
            ExportCommon.EditorGUIExportLog();
            EditorGUILayout.HelpBox(ExportCommon.Localization($"BVA 版本{BVAConst.FORMAT_VERSION}", $"BVA version {BVAConst.FORMAT_VERSION}"), MessageType.Info);

            EditorGUILayout.EndScrollView();
        }

    }
}