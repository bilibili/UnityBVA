using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BVA.Component
{
    [CustomEditor(typeof(AvatarDressUpConfig))]
    public class AvatarDressUpConfigEditor : Editor
    {
        AvatarDressUpConfig _target;
        private void OnEnable()
        {
            _target = target as AvatarDressUpConfig;
        }
        public override void OnInspectorGUI()
        {
            void CheckAnimatorNull()
            {
                if (_target.animator == null)
                {
                    EditorUtility.DisplayDialog("", "Assign avatar Animator first", "OK");
                    return;
                }
            }
            if (_target.dressUpConfigs == null)
            {
                _target.dressUpConfigs = new List<DressUpConfig>();
            }
                
            base.OnInspectorGUI();


            if (GUILayout.Button("Set As Default"))
            {
                CheckAnimatorNull();
                var config = _target.dressUpConfigs[_target.CurrentDressIndex];
                _target.dressUpConfigs.Remove(config);
                _target.dressUpConfigs.Insert(0, config);
                _target.SwitchDress(0);
            }
            if (GUILayout.Button("Add Dress"))
            {
                CheckAnimatorNull();
                var defaultConfig = _target.BuildConfig(_target.animator.transform);
                if (!_target.dressUpConfigs.Contains(defaultConfig))
                {
                    _target.dressUpConfigs.Add(defaultConfig);
                }

                _target.SwitchDress(_target.dressUpConfigs.Count - 1);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("  <  "))
            {
                _target.SwitchDress(false);
            }
            EditorGUILayout.TextArea($"  {_target.CurrentDressIndex}  ");
            if (GUILayout.Button("  >  "))
            {
                _target.SwitchDress(true);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}