using System.IO;
using UnityEditor;
using UnityEngine;
using BVA.Extensions;
using BVA.Component;

namespace BVA
{
    [CustomEditor(typeof(BVAMetaInfoScriptableObject))]
    public class MetaInfoScriptableObjectEditor : Editor
    {
        private static Texture2D TextureField(string name, Texture2D texture, int size)
        {
            GUILayout.BeginHorizontal();
            var style = new GUIStyle(GUI.skin.box);
            style.alignment = TextAnchor.UpperCenter;
            GUILayout.Label(name, style);
            var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(size), GUILayout.Height(size));
            GUILayout.EndVertical();
            return result;
        }

        BVAMetaInfoScriptableObject m_target;
        private void OnEnable()
        {
            m_target = target as BVAMetaInfoScriptableObject;
        }
        public override void OnInspectorGUI()
        {
            m_target.formatVersion = EditorGUILayout.TextField("Format Version", m_target.formatVersion);
            m_target.title = EditorGUILayout.TextField("Title", m_target.title);
            m_target.version = EditorGUILayout.TextField("Asset Version", m_target.version);
            m_target.author = EditorGUILayout.TextField("Author", m_target.author);
            m_target.contact = EditorGUILayout.TextField("Contact Information", m_target.contact);
            m_target.reference = EditorGUILayout.TextField("Reference", m_target.reference);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.HelpBox("To create a thumbnail image by Camera.main\nSelect a camera tag to MainCamera", MessageType.Info);
                    
                if (Camera.main)
                {
                    if (GUILayout.Button("Screenshot"))
                    {
                        TakeScreenShot();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            m_target.thumbnail = TextureField("Thumbnail", m_target.thumbnail, Screen.currentResolution.width>2560?256:128);
            m_target.contentType = (ContentType)EditorGUILayout.EnumPopup("Content Type", m_target.contentType);
            m_target.legalUser = (LegalUser)EditorGUILayout.EnumPopup("Legal user", m_target.legalUser);
            m_target.violentUsage = (UsageLicense)EditorGUILayout.EnumPopup("Allow violent usage", m_target.violentUsage);
            m_target.sexualUsage = (UsageLicense)EditorGUILayout.EnumPopup("Allow sexual usage", m_target.sexualUsage);
            m_target.commercialUsage = (UsageLicense)EditorGUILayout.EnumPopup("Allow commercial usage", m_target.commercialUsage);
            m_target.licenseType = (LicenseType)EditorGUILayout.EnumPopup("License type", m_target.licenseType);
            m_target.customLicenseUrl = EditorGUILayout.TextField("Custom license URL", m_target.customLicenseUrl);
        }
        private static string SaveDialog(BVAMetaInfoScriptableObject meta, string ext)
        {
            var assetPath = AssetDatabase.GetAssetPath(meta);
            var directory = Path.GetDirectoryName(assetPath);
            return EditorUtility.SaveFilePanel(
                    "Save thumbnail",
                    directory,
                    $"thumbnail.{ext}",
                    ext);
        }
        void TakeScreenShot()
        {
            string dstPath = SaveDialog(m_target, "png");
            if (string.IsNullOrEmpty(dstPath))
            {
                return;
            }

            var backup = RenderTexture.active;
            var backup2 = Camera.main.targetTexture;
            var rt = new RenderTexture(1024, 1024, 16, RenderTextureFormat.ARGB32);
            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            try
            {
                RenderTexture.active = rt;
                Camera.main.targetTexture = rt;
                Camera.main.Render();
                tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                tex.Apply();

                var ext = Path.GetExtension(dstPath).ToLower();
                switch (ext)
                {
                    case ".png":
                        File.WriteAllBytes(dstPath, tex.EncodeToPNG());
                        break;

                    case ".jpg":
                        File.WriteAllBytes(dstPath, tex.EncodeToJPG());
                        break;
                }

                var assetPath = dstPath.Substring(dstPath.LastIndexOf("Assets"));
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.ImportAsset(assetPath);
                    m_target.thumbnail = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                };
            }
            finally
            {
                RenderTexture.active = backup;
                Camera.main.targetTexture = backup2;
                Texture2D.DestroyImmediate(tex);
                RenderTexture.DestroyImmediate(rt);
            }
        }

    }
}