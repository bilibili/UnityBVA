using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;

namespace BVA.Component
{
    [CustomEditor(typeof(AutoBlink))]
    public class AutoBlinkEditor : Editor
    {
        private void OnEnable()
        {
            _target = target as AutoBlink;
            if (_target.blendShapes == null)
                _target.blendShapes = new List<AutoBlink.BlendShapeInfo>();
            if (m_blendShapeValuesList == null)
                DrawBlendShapeBindingGUI();
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            //DrawBlendShapeBindingGUI();
            _target.blendTime = EditorGUILayout.Slider("Blend Time", _target.blendTime, 0f, 5f);
            _target.interval = EditorGUILayout.Slider("Interval", _target.interval, 1f, 30f);
            m_blendShapeValuesList?.DoLayoutList();
        }

        ReorderableList m_blendShapeValuesList;
        AutoBlink _target;
        List<AutoBlink.BlendShapeInfo> blendShapesInfo => _target.blendShapes;
        void DrawBlendShapeBindingGUI()
        {
            m_blendShapeValuesList = new ReorderableList(blendShapesInfo, typeof(AutoBlink.BlendShapeInfo));

            m_blendShapeValuesList.elementHeight = 96;

            m_blendShapeValuesList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = blendShapesInfo[index];
                rect.height = 20;
                rect.y += 4;
                element.skinnedMeshRenderer = EditorGUI.ObjectField(rect, "SkinnedMeshRenderer", element.skinnedMeshRenderer, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;

                if (element.skinnedMeshRenderer != null && element.skinnedMeshRenderer.sharedMesh.blendShapeCount > 0)
                {
                    var (blendShapePopupNames, blendShapePopupValues) = UnityTools.GetBlendshapesInfo(element.skinnedMeshRenderer.sharedMesh);
                    rect.y += 24;
                    element.index = EditorGUI.IntPopup(rect, element.index, blendShapePopupNames, blendShapePopupValues);
                    rect.y += 24;
                    element.minBlend = EditorGUI.IntSlider(rect, "Min Blend", element.minBlend, 0, 100);
                    rect.y += 24;
                    element.maxBlend = EditorGUI.IntSlider(rect, "Max Blend", element.maxBlend, 0, 100);
                }
                else
                {
                    rect.y += 24;
                    rect.height = 40;
                    EditorGUI.HelpBox(rect, "Please select a SkinnedMeshRenderer with BlendShapes", MessageType.Warning);
                }
            };
        }
    }
}