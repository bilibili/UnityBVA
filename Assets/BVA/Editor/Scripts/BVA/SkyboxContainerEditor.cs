using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CustomEditor(typeof(SkyboxContainer))]
public class SkyboxContainerEditor : Editor
{
    SkyboxContainer _target;
    List<Material> incompatibleMaterials;
    int usedIndex;
    private void OnEnable()
    {
        _target = target as SkyboxContainer;
        incompatibleMaterials = new List<Material>(8);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (_target.materials == null) return;
        incompatibleMaterials.Clear();

        foreach (var v in _target.materials)
        {
            if (SkyboxContainer.IsValidMaterial(v))
                continue;
            incompatibleMaterials.Add(v);
        }
        if (incompatibleMaterials != null && incompatibleMaterials.Count > 0)
        {
            string materialNames = string.Join(",", incompatibleMaterials.Select((x) => { if (x == null) return "null"; else return x.name; }));
            string supportedNames = string.Join(",", SkyboxContainer.COMPATIBLE_SHADERS);
            EditorGUILayout.HelpBox($"{materialNames} is not compatible with exporter!\nsupported shaders are :{supportedNames}", MessageType.Error);
        }

        EditorGUILayout.BeginHorizontal();
        usedIndex = EditorGUILayout.IntField("Add scene skybox", usedIndex);
        if (GUILayout.Button("Set in scene"))
        {
            var skyboxMaterial = _target.Get(usedIndex);
            if (skyboxMaterial != null)
            {
                RenderSettings.skybox = skyboxMaterial;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add scene skybox"))
        {
            _target.Add(RenderSettings.skybox);
        }
        if (GUILayout.Button("Clear invalid materials"))
        {
            _target.RemoveInvalidOrNull();
        }
        EditorGUILayout.EndHorizontal();
    }
}
