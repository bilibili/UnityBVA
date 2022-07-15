using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityTools;
using BVA.Extensions;

public class TextureToolWindow : EditorWindow
{
    int m_mode;
    static string[] EDIT_MODES = new[]{
            "Texture Format Converter"
        };
    readonly string[] sizesText = new string[] { "1", "1\\2", "1\\4", "1\\8", "1\\16" };
    readonly int[] sizes = new int[] { 1, 2, 4, 8, 16 };
    [MenuItem("BVA/Developer Tools/Texture Tool")]
    static void Init()
    {
        TextureToolWindow window = (TextureToolWindow)GetWindow(typeof(TextureToolWindow), false, "Texture Tool");
        window.Show();
    }
    private void OnEnable()
    {
    }
    private void OnGUI()
    {
        m_mode = GUILayout.Toolbar(m_mode, EDIT_MODES);
        switch (m_mode)
        {
            case 0:
                FormatConvertGUI();
                break;
                case 1:
                ConvertLightmapGUI();
                break;
        }
    }
    Texture2D texture;
    int descale;
    TextureFileFormat format;
    private void FormatConvertGUI()
    {
        EditorGUILayout.Space();
        texture = EditorGUILayout.ObjectField(texture, typeof(Texture2D), false) as Texture2D;
        descale = EditorGUILayout.IntPopup("Downsampler", descale, sizesText, sizes);
        format = (TextureFileFormat)EditorGUILayout.EnumPopup("Foramt", format);
        if (GUILayout.Button("Convert"))
        {
            string path = EditorUtility.SaveFilePanel("", UnityTools.GetAssetPath(), "", format.ToString());
            if (!string.IsNullOrEmpty(path))
            {
                Convert(texture, descale, format, path);
            }
        }
    }
    private void Convert(Texture2D texture, int descale, TextureFileFormat format, string path)
    {
        Texture2D texture2D = RenderExportTextureSize(ref texture, texture.width / descale, texture.height / descale);
        File.WriteAllBytes(path, UnityTools.EncodeTexture(texture2D, format));
    }
    Texture2D lightmapColor;
    private void ConvertLightmapGUI()
    {
        lightmapColor = EditorGUILayout.ObjectField(lightmapColor, typeof(Texture2D), false) as Texture2D;
        if (GUILayout.Button("Convert"))
        {
            string path = EditorUtility.SaveFilePanel("", UnityTools.GetAssetPath(), "", format.ToString());
            if (!string.IsNullOrEmpty(path))
            {
                Texture2D export = UnityTools.RenderExportLightmap(texture);
                File.WriteAllBytes(path, UnityTools.EncodeTexture(export, TextureFileFormat.png));
            }
        }
    }
}