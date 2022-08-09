using UnityEditor;
using UnityEngine;
using BVA.Component;

namespace BVA
{
    public static class ExportCommon
    {
        const string ASSET_DIR = "Assets/";
        static bool _folderLog;
        static bool _folderExoprtInfo = true;
        static bool _folderExoprtInfo_Mesh = true;

        public static void FixMissingMetaInfo(GameObject Root)
        {
            var metaComp = Root.AddComponent<BVAMetaInfo>();
            var meta = ScriptableObject.CreateInstance<BVAMetaInfoScriptableObject>();
            var savePath = $"{ASSET_DIR}{Root.name}_MetaInfo.asset";
            AssetDatabase.CreateAsset(meta, savePath);
            metaComp.metaInfo = AssetDatabase.LoadAssetAtPath<BVAMetaInfoScriptableObject>(savePath);
        }

        public static void FixMissingBlendshapeMixer(GameObject Root)
        {
            var metaComp = Root.AddComponent<BlendShapeMixer>();
        }

        public static void EditorGUIExportLog()
        {
            _folderLog = EditorGUILayout.BeginToggleGroup("Show Logs", _folderLog);
            if (_folderLog)
            {
                LogPool.ExportLogger.OnGUI();
            }
            EditorGUILayout.EndToggleGroup();
        }

        public static void EditorGUICollectExportInfo(ExportInfo info)
        {
            _folderExoprtInfo = EditorGUILayout.BeginToggleGroup("Show Export Information", _folderExoprtInfo);
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
                EditorGUILayout.HelpBox("Please switch to Standalone(Windows,Mac,Linux) platform in Build Settings", MessageType.Error);
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
    }
}