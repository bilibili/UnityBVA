using System.Collections.Generic;
using UnityEditor;
using BVA;
using UnityEngine;
using BVA.Component;
using System.Linq;

namespace BVA
{
    public class AvatarExportMenu : EditorWindow
    {
        [MenuItem("BVA/Export/Export Avatar")]
        static void Init()
        {
            AvatarExportMenu window = (AvatarExportMenu)EditorWindow.GetWindow(typeof(AvatarExportMenu), false, "Export Avatar");
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


        void CheckRootValidity()
        {
            if (Root == null) return;
            #region Animator Human Check
            var animator = Root.GetComponent<Animator>();
            if (animator == null || animator.avatar == null || !animator.isHuman)
            {
                EditorGUILayout.HelpBox($"Not a valid avatar gameObject, not find Animator component with human avatar", MessageType.Error);

                if (animator.avatar!=null)
                {
                    string avatarPath = AssetDatabase.GetAssetPath(animator.avatar);
                    ModelImporter importer = AssetImporter.GetAtPath(avatarPath) as ModelImporter;
                    if (importer != null && importer.animationType != ModelImporterAnimationType.Human)
                    {
                        if (GUILayout.Button("Fix it"))
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

            var meshs = Root.GetComponentsInChildren<SkinnedMeshRenderer>().Where(x=>!x.sharedMesh.isReadable).ToArray();

            if (meshs.Length!=0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox($"Model containning invalid mesh, mesh's read/write should be enabled", MessageType.Error);

                if (GUILayout.Button("Fix it"))
                {
                    for (int i = 0; i < meshs.Length; i++)
                    {
                        string meshPath = AssetDatabase.GetAssetPath(animator.avatar);
                        ModelImporter importer = AssetImporter.GetAtPath(meshPath) as ModelImporter;
                        if (importer.isReadable!=true)
                        {
                            importer.isReadable = true;
                            importer.SaveAndReimport();
                        }
                    }
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
                EditorGUILayout.HelpBox($"You need assign a {nameof(BVAMetaInfo)} to export avatar for legal use", MessageType.Error);
                if (GUILayout.Button("Fix it"))
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
                EditorGUILayout.HelpBox($"You need assign a {nameof(BlendShapeMixer)} to export avatar for facial blendshape", MessageType.Error);
                if (GUILayout.Button("Fix it"))
                {
                    ExportCommon.FixMissingBlendshapeMixer(Root);
                }
                EditorGUILayout.EndHorizontal();
                isValid = false;
            }
            if (!isValid) return;

            _folderAudioEditor = EditorGUILayout.BeginToggleGroup(" Export AudioClip", _folderAudioEditor);
            if (_folderAudioEditor)
            {
                audioClipContainer = Root.GetComponent<AudioClipContainer>();
                if (audioClipContainer == null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox($"Not find {nameof(AudioClipContainer)} on avatar, do you need to assign a new one", MessageType.Info);
                    if (GUILayout.Button("Add"))
                    {
                        audioClipContainer = Root.AddComponent<AudioClipContainer>();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    if (GUILayout.Button("Add AudioClip in AudioSource"))
                    {
                        ExportCommon.AddExistAudioClipToContainer(audioClipContainer, Root);
                    }
                }
                ShowContainerGUI(audioClipContainer);
            }
            EditorGUILayout.EndToggleGroup();

            _folderSkyboxEditor = EditorGUILayout.BeginToggleGroup(" Export Skybox", _folderSkyboxEditor);
            if (_folderSkyboxEditor)
            {
                skyboxContainer = Root.GetComponent<SkyboxContainer>();
                if (skyboxContainer == null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox($"Not find {nameof(SkyboxContainer)} on avatar, do you need to assign a new one", MessageType.Info);
                    if (GUILayout.Button("Add"))
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
            if (GUILayout.Button("Export"))
            {
                var exportOptions = new ExportOptions { TexturePathRetriever = AssetDatabase.GetAssetPath };
                var exporter = new GLTFSceneExporter(new Transform[] { Root.transform }, exportOptions);

                var path = EditorUtility.OpenFolderPanel("BVA Avatar Export Path", EditorPrefs.GetString("avatar_export_path"), "");
                if (!string.IsNullOrEmpty(path))
                {
                    EditorPrefs.SetString("avatar_export_path", path);
                    exporter.SaveGLB(path, Root.name, BVAConst.EXTENSION_BVA_AVATAR);
                    EditorUtility.DisplayDialog("export success", $"spend {exporter.ExportDuration.TotalSeconds}s finish export!", "OK");
                }
            }

            #endregion
        }
        void OnGUI()
        {
            EditorGUILayout.LabelField("Exporter", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            Root = EditorGUILayout.ObjectField("Export root", Root, typeof(GameObject), true) as GameObject;
            if (GUILayout.Button("Select"))
            {
                Root = Selection.activeGameObject;
            }

            if (GUILayout.Button("Collect Export Info"))
            {
                exportInfo = new ExportInfo(Root);
            }
            EditorGUILayout.EndHorizontal();
            //EditorGUILayout.Separator();
            if (exportInfo != null)
                ExportCommon.EditorGUICollectExportInfo(exportInfo);

            GLTFSceneExporter.ExportAnimationClips = EditorGUILayout.Toggle("Export Animations", GLTFSceneExporter.ExportAnimationClips);
            if(GLTFSceneExporter.ExportAnimationClips)
                EditorGUILayout.HelpBox($"Export legacy animation on Animation, as well convert mecanim animation to legacy skin animation and export it", MessageType.Info);

            EditorGUILayout.Separator();
            CheckRootValidity();
            EditorGUILayout.Separator();
            ExportCommon.EditorGUIExportLog();
            EditorGUILayout.HelpBox($"BVA version {BVAConst.FORMAT_VERSION}", MessageType.Info);
        }

    }
}