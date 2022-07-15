using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChangeLightmap))]
public class ChangeLightmapInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ChangeLightmap lightmapData = (ChangeLightmap)target;

        if (GUILayout.Button("Load"))
        {
            lightmapData.Load();
        }
        if (GUILayout.Button("Save"))
        {
            if (lightmapData.CheckResourcesDirectoryExists(lightmapData.resourceFolder))
            {
                if (!EditorUtility.DisplayDialog("Overwrite Lightmap Resources?", "Lighmap Resources folder with name: \"" + lightmapData.resourceFolder + "\" already exists.\n\nPress OK to overwrite existing lightmap data.", "OK", "Cancel"))
                {
                    return;
                }
            }
            else
            {
                if (!EditorUtility.DisplayDialog("Create Lightmap Resources?", "Create new lighmap Resources folder: \"" + lightmapData.resourceFolder + "?", "OK", "Cancel"))
                {
                    return;
                }
            }
            lightmapData.GenerateLightmapInfoStore();
        }
    }
}