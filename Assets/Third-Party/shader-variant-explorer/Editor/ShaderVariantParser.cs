using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditorInternal;
using UnityEngine;

public class ShaderVariantParser : MonoBehaviour
{
    [MenuItem("internal:Window/Needle/Shader Variant Explorer/Get Artifacts")]
    private static void GetPathsForSelected()
    {
        var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        StringBuilder assetPathInfo = new StringBuilder();

        var guidString = AssetDatabase.AssetPathToGUID(assetPath);
        //The ArtifactKey is needed here as there are plans to
        //allow importing for different platforms without switching
        //platform, thus ArtifactKeys will be parametrized in the future
        var artifactKey = new ArtifactKey(new GUID(guidString));
        var artifactID = AssetDatabaseExperimental.LookupArtifact(artifactKey);

        //Its possible for an Asset to have multiple import results,
        //if, for example, Sub-assets are present, so we need to iterate
        //over all the artifacts paths
        AssetDatabaseExperimental.GetArtifactPaths(artifactID, out var paths);

        assetPathInfo.Append($"Files associated with {assetPath}");
        assetPathInfo.AppendLine();

        foreach (var curVirtualPath in paths)
        {
            //The virtual path redirects somewhere, so we get the
            //actual path on disk (or on the in memory database, accordingly)
            var curPath = Path.GetFullPath(curVirtualPath);
            assetPathInfo.Append("\t" + curPath);
            assetPathInfo.AppendLine();
        }

        Debug.Log("Path info for asset:\n" + assetPathInfo.ToString());

        var artifactPath = paths.FirstOrDefault();
        if (artifactPath != null) ParseShaderArtifact(artifactPath);

        // InternalEditorUtility.OpenFileAtLineExternal(paths.First(), 0);
    }

    private static void ParseShaderArtifact(string artifactPath)
    {
        var bytes = File.ReadAllBytes(artifactPath);
        var lineCount = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] == '\n')
            {
                lineCount++;
            }
        }
        Debug.Log(lineCount);

        var allText = File.ReadAllText(artifactPath);
        var tmpFile = Application.dataPath + "/temp.txt";
        File.WriteAllText(tmpFile, allText);
        InternalEditorUtility.OpenFileAtLineExternal(tmpFile, 0);

        var lines0 = allText.Substring(allText.LastIndexOf('}'));
        Debug.Log(lines0);
        var lines = lines0.Split(new char[] { (char) 0}, StringSplitOptions.RemoveEmptyEntries);
        
        // Debug.Log(File.ReadAllText(artifactPath));
        // Debug.Log("Lines: " + File.ReadAllLines(artifactPath).Length);
        // var lines = File.ReadAllLines(artifactPath).Reverse().Take(10);
        Debug.Log($"End of file [{lines.Count()}]:\n" + string.Join(",", lines));
}
}
