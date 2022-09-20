using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleToolWindow : EditorWindow
{
    static readonly Dictionary<string, string[]> defaultShaders = new Dictionary<string, string[]>() { 
        { "UTS2", new string[] { "Universal Render Pipeline/Toon" } },
        { "MToon" ,new string[] { "VRM/URP/MToon" }}, 
        {"LiliumToon",new string[]{ "Shader Graphs/Toon", "Shader Graphs/Toon (GGX)", "Shader Graphs/Toon (Simple)", "Shader Graphs/Toon (Stylized Hair)", "Shader Graphs/Toon (Stylized Input)", "Shader Graphs/Toon (Texture Ramp)", "Shader Graphs/Toon (Transparent)", "Shader Graphs/Toon (TransparentCutout)", "Shader Graphs/Toon (Disolve)" } },
        {"ZeldaToon",new string[]{ "Shader Graphs/ZeldaToon" } },
    };
    static Dictionary<string, bool> packDefaultShaders;
    static string exportPath;
    public Shader customShader;
    static BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
    [MenuItem("BVA/Developer Tools/Pack Shader to Assetbundle")]
    static void Init()
    {
        AssetBundleToolWindow window = (AssetBundleToolWindow)GetWindow(typeof(AssetBundleToolWindow), false, "Pack Shader to Assetbundle");
        window.Show();
    }
    private void OnEnable()
    {
        packDefaultShaders = new Dictionary<string, bool>();
        foreach(var shader in defaultShaders)
        {
            packDefaultShaders.Add(shader.Key, true);
        }
    }
    private void OnGUI()
    {
        EditorGUILayout.PrefixLabel("Official Shaders:");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();
        foreach(var shader in defaultShaders)
        {
            packDefaultShaders[shader.Key] = EditorGUILayout.Toggle(shader.Key, packDefaultShaders[shader.Key]);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", buildTarget);

        EditorGUILayout.BeginHorizontal();
        exportPath = EditorGUILayout.TextField("Path: ", exportPath);
        if (GUILayout.Button("Choose", GUILayout.MaxWidth(100)))
        {
            exportPath = EditorUtility.SaveFolderPanel("Choose a location to save Assetbundle", UnityTools.GetAssetPath(), "save");
            if (string.IsNullOrEmpty(exportPath))
                return;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Build", GUILayout.MaxWidth(100)))
        {
            List<Shader> shaders = new List<Shader>();
            foreach (var shaderPath in defaultShaders)
            {
                if (packDefaultShaders[shaderPath.Key])
                {
                    foreach (var shaderName in shaderPath.Value)
                        shaders.Add(Shader.Find(shaderName));
                }
            }
            SaveAssetBundles(shaders, exportPath);
        }

        customShader = (Shader)EditorGUILayout.ObjectField("Custom Shader", customShader, typeof(Shader), false);

        if (GUILayout.Button("Build", GUILayout.MaxWidth(100)))
        {
            SaveAssetBundles(new List<Shader>() { customShader }, exportPath);
        }
    }
    private void SaveAssetBundles(List<Shader> shaders, string path)
    {
        AssetBundleBuild[] abbs = new AssetBundleBuild[shaders.Count];
        for (int i = 0; i < shaders.Count; i++)
        {
            abbs[i].assetNames = new string[1] { AssetDatabase.GetAssetPath(shaders[i]) };
            abbs[i].assetBundleName = Path.GetFileName(shaders[i].name.Replace('/','_'))+".assetbundle";
        }
        BuildPipeline.BuildAssetBundles(path, abbs, BuildAssetBundleOptions.None, buildTarget);
    }
}
