using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BVA
{
	[CustomEditor(typeof(GLTFImporter))]
	[CanEditMultipleObjects]
	public class GLTFImporterInspector : UnityEditor.AssetImporters.AssetImporterEditor
	{
		private string[] _importNormalsNames;

		public override void OnInspectorGUI()
		{
			if (_importNormalsNames == null)
			{
				_importNormalsNames = Enum.GetNames(typeof(GLTFImporterNormals))
					.Select(n => ObjectNames.NicifyVariableName(n))
					.ToArray();
			}
			EditorGUILayout.LabelField("Meshes", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_removeEmptyRootObjects"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_scaleFactor"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_maximumLod"), new GUIContent("Maximum LOD"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_readWriteEnabled"), new GUIContent("Read/Write Enabled"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_swapUvs"), new GUIContent("Swap UVs"));
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Normals", EditorStyles.boldLabel);
			EditorGUI.BeginChangeCheck();
			var importNormalsProp = serializedObject.FindProperty("_importNormals");
			var importNormals = EditorGUILayout.Popup(importNormalsProp.displayName, importNormalsProp.intValue, _importNormalsNames);
			if (EditorGUI.EndChangeCheck())
			{
				importNormalsProp.intValue = importNormals;
			}
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Materials", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_importMaterials"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_importAnimations"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_importAudio"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_importAvatars"));

			serializedObject.ApplyModifiedProperties();
			ApplyRevertGUI();
		}
	}
}
