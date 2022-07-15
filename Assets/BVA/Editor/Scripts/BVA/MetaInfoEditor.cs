using UnityEditor;
using BVA.Component;

namespace BVA
{
    [CustomEditor(typeof(BVAMetaInfo))]
    public class MetaInfoEditor : Editor
    {
        BVAMetaInfo meta;
        Editor inspector;
        SerializedProperty metaObjectProperty;
        private void OnEnable()
        {
            meta = target as BVAMetaInfo;

            metaObjectProperty = serializedObject.FindProperty(nameof(meta.metaInfo));
            if (meta.metaInfo != null)
            {
                inspector = CreateEditor(metaObjectProperty.objectReferenceValue);
            }
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(meta.metaInfo)));
            if (inspector != null)
            {
                inspector.OnInspectorGUI();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}