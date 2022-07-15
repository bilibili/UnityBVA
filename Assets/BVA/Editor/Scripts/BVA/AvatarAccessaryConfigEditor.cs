using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BVA.Component
{
    [CustomEditor(typeof(AvatarAccessoryConfig))]
    public class AvatarAccessoryConfigEditor : Editor
    {
        AvatarAccessoryConfig _target;
        Animator animator;
        static string[] boneNames;
        bool isFolder = false;
        private void OnEnable()
        {
            _target = target as AvatarAccessoryConfig;
            animator = _target.GetComponent<Animator>();
            if (boneNames == null)
                boneNames = HumanTrait.BoneName;
        }
        public override void OnInspectorGUI()
        {
            if (_target.accessories == null)
                _target.accessories = new List<AccessoryConfig>();
            base.OnInspectorGUI();

            foreach (var v in _target.accessories)
            {
                if (v.gameObject != null && v.gameObject.GetComponentInChildren<Renderer>() == null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox($"GameObject {v.name} has no valid Renderer", MessageType.Error);
                    if (GUILayout.Button("Select"))
                    {
                        Selection.SetActiveObjectWithContext(v.gameObject, null);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            isFolder = EditorGUILayout.Foldout(isFolder, "Select Bone");
            if (isFolder)
            {
                for (int i = 0; i < boneNames.Length; i++)
                {
                    string bone = boneNames[i];
                    if (GUILayout.Button(bone))
                    {
                        Transform t = animator.GetBoneTransform((HumanBodyBones)i);
                        Selection.SetActiveObjectWithContext(t, null);
                    }
                }
            }
        }
    }
}