using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditorInternal;

namespace BVA.Component
{
    [CustomEditor(typeof(BlendShapeMixer))]
    public class BlendShapeMixerEditor : Editor
    {
        private struct BlendShapeCache
        {
            public string[] BlendShapeNames;
            public int[] BlendShapeIndexes;
        }
        BlendShapeMixer mixer;
        SkinnedMeshRenderer[] materialRenders;
        SkinnedMeshRenderer[] blendShapeRenders;
        Dictionary<string, BlendShapeCache> blendShapeDic;
        Dictionary<string, MaterialCache> materialDic;
        ReorderableList m_blendShapeValuesList;
        string[] presetNames;
        string[] renderNames;
        int[] renderIndexes;
        int m_mode;
        static string[] EDIT_MODES = new[]{
            "BlendShape",
            //"BlendShape List",
            "Material List"
        };

        private void refreshSMRComponent()
        {
            materialRenders = mixer.GetComponentsInChildren<SkinnedMeshRenderer>();
            blendShapeRenders = materialRenders.Where(x => x.sharedMesh.blendShapeCount > 0).ToArray();
            blendShapeDic = new Dictionary<string, BlendShapeCache>(blendShapeRenders.Length);
            foreach (var v in blendShapeRenders)
            {
                int blendShapeCount = v.sharedMesh.blendShapeCount;
                var (blendShapes, blendShapeIndexes) = UnityTools.GetBlendshapesInfo(v.sharedMesh);

                BlendShapeCache cache = new BlendShapeCache() { BlendShapeNames = blendShapes, BlendShapeIndexes = blendShapeIndexes };
                blendShapeDic.Add(v.name, cache);
            }
            materialDic = new Dictionary<string, MaterialCache>(materialRenders.Length);
            renderIndexes = new int[materialRenders.Length];
            renderNames = new string[materialRenders.Length];
            int c = 0;
            foreach (var v in materialRenders)
            {
                renderNames[c] = v.name;
                renderIndexes[c] = c++;
                List<Material> sharedMaterials = new List<Material>();
                v.GetSharedMaterials(sharedMaterials);
                var sharedMaterialCount = sharedMaterials.Count;
                int[] indexes = new int[sharedMaterialCount];
                string[] names = new string[sharedMaterialCount];
                MaterialPropertyMap[] properties = new MaterialPropertyMap[sharedMaterialCount];
                for (int i = 0; i < sharedMaterialCount; i++)
                {
                    var propertyMap = new Dictionary<string, MaterialExportProperty>();
                    Material material = sharedMaterials[i];
                    indexes[i] = i; names[i] = material.name;

                    List<string> propertyName = new List<string>();
                    List<MaterialExportProperty> property = new List<MaterialExportProperty>();
                    for (int j = 0; j < ShaderUtil.GetPropertyCount(material.shader); ++j)
                    {
                        var propType = ShaderUtil.GetPropertyType(material.shader, j);
                        var name = ShaderUtil.GetPropertyName(material.shader, j);
                        propertyName.Add(name);

                        switch (propType)
                        {
                            case ShaderUtil.ShaderPropertyType.Color:
                                property.Add(new MaterialExportProperty(material.GetColor(name)));
                                break;
                            case ShaderUtil.ShaderPropertyType.Vector:
                                property.Add(new MaterialExportProperty(material.GetVector(name)));
                                break;
                            case ShaderUtil.ShaderPropertyType.Float:
                            case ShaderUtil.ShaderPropertyType.Range:
                                property.Add(new MaterialExportProperty(material.GetFloat(name)));
                                break;
                        }
                    }
                    properties[i] = new MaterialPropertyMap() { MaterialName = material.name, PropertyName = propertyName.ToArray(), Property = property.ToArray() };
                }
                MaterialCache cache = new MaterialCache() { MaterialIndexes = indexes, MaterialNames = names, MaterialProperties = properties };
                materialDic.Add(v.name, cache);
            }

            renderNames = materialRenders.Select(smr => { return smr.name; }).ToArray();

        }

        private (string[], int[]) getBlendShapePopup(string rendererName)
        {
            var cache = blendShapeDic[rendererName];
            return (cache.BlendShapeNames, cache.BlendShapeIndexes);
        }

        private void OnEnable()
        {
            //presetNames = (System.Enum.GetValues(typeof(BlendShapeMixerPreset)) as BlendShapeMixerPreset[]).Select(x => x.ToString()).ToArray();
            mixer = target as BlendShapeMixer;
            if (mixer.keys == null || mixer.keys.Count < (int)BlendShapeMixerPreset.Custom)
            {
                mixer.CreateDefaultPreset();
            }
            refreshSMRComponent();
            SelectedIndex = 0;
        }
        List<bool> m_meshFolds = new List<bool>();

        int m_selectedIndex = -1;
        int SelectedIndex
        {
            get { return m_selectedIndex; }
            set
            {
                if (m_selectedIndex == value) return;
                m_selectedIndex = value;
                OnSelectGrid(m_selectedIndex);
            }
        }
        BlendShapeKey currentKey;
        void OnSelectGrid(int index)
        {
            currentKey = mixer.keys[index];

            //DrawBlendShapeBindingGUI();
            //DrawMaterialBindingGUI();
        }
        void DrawBlendShapeMixerSelect()
        {
            if (mixer != null && mixer.keys != null)
            {
                EditorGUILayout.LabelField("Select BlendShapeKey", EditorStyles.boldLabel);
                SelectedIndex = GUILayout.SelectionGrid(SelectedIndex, presetNames, 4);
            }
            if (currentKey == null) return;
            EditorGUILayout.Space();
            if (GUILayout.Button("Add New Mixer"))
            {
                mixer.keys.Add(new BlendShapeKey()
                {
                    keyName = "Custom",
                    preset = BlendShapeMixerPreset.Custom
                });
            }
            if (currentKey.preset == BlendShapeMixerPreset.Custom)
            {
                if (GUILayout.Button("Remove"))
                {
                    mixer.keys.Remove(currentKey);
                }
                currentKey.keyName = EditorGUILayout.TextField("Key Name", currentKey.keyName);
            }
            
            //currentKey.preset = (BlendShapeMixerPreset)EditorGUILayout.EnumPopup("Preset", currentKey.preset);
            currentKey.isBinary = EditorGUILayout.Toggle("Is Binary", currentKey.isBinary);
            BlendShapeConfirm();
        }

        void BlendShapeConfirm()
        {
            foreach (var mesh in blendShapeRenders)
            {
                for (int i = 0; i < mesh.sharedMesh.blendShapeCount; i++)
                {
                    mesh.SetBlendShapeWeight(i, 0);
                }
            }

            foreach (BlendShapeValueBinding value in currentKey.blendShapeValues)
            {
                value.Set(1);
                //value.node.SetBlendShapeWeight(value.index, value.weight);
            }
        }
        bool AllBlendShapeBindsGUI()
        {
            bool changed = false;
            int foldIndex = 0;
            foreach (var renderer in materialRenders)
            {
                var mesh = renderer.sharedMesh;
                if (mesh != null && mesh.blendShapeCount > 0)
                {
                    if (foldIndex >= m_meshFolds.Count)
                    {
                        m_meshFolds.Add(false);
                    }
                    m_meshFolds[foldIndex] = EditorGUILayout.Foldout(m_meshFolds[foldIndex], renderer.name);
                    if (m_meshFolds[foldIndex])
                    {
                        EditorGUI.indentLevel += 1;
                        for (int i = 0; i < mesh.blendShapeCount; ++i)
                        {
                            var src = renderer.GetBlendShapeWeight(i);
                            var dst = EditorGUILayout.Slider(mesh.GetBlendShapeName(i), src, 0, 100.0f);
                            if (dst != src)
                            {
                                renderer.SetBlendShapeWeight(i, dst);
                                changed = true;

                                if (dst == 0)
                                {
                                    RemoveBlendShapeValue(renderer, i);
                                }
                                else
                                {
                                    UpdateBlendShapeValue(renderer, i, dst);
                                }
                            }
                        }
                        EditorGUI.indentLevel -= 1;
                    }
                    ++foldIndex;
                }
            }
            return changed;
        }

        void RemoveBlendShapeValue(SkinnedMeshRenderer mesh, int index)
        {
            BlendShapeValueBinding removeValue = null;

            foreach (BlendShapeValueBinding value in currentKey.blendShapeValues)
            {
                if (value.node == mesh)
                {
                    if (value.index == index)
                    {
                        removeValue = value;
                        break;
                    }
                }
            }

            currentKey.blendShapeValues.Remove(removeValue);
        }

        void UpdateBlendShapeValue(SkinnedMeshRenderer mesh, int index, float weight)
        {
            foreach (BlendShapeValueBinding value in currentKey.blendShapeValues)
            {
                if (value.node == mesh)
                {
                    if (value.index == index)
                    {
                        value.weight = weight;
                        return;
                    }
                }
            }

            currentKey.blendShapeValues.Add(new BlendShapeValueBinding()
            {
                node = mesh,
                index = index,
                weight = weight
            });
        }

        void DrawBlendShapeBindingGUI()
        {
            m_blendShapeValuesList = new ReorderableList(currentKey.blendShapeValues, typeof(BlendShapeValueBinding));

            m_blendShapeValuesList.elementHeight = 70;
            m_blendShapeValuesList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) =>
            {
                if (EditorApplication.isPlaying)
                {
                    GUI.backgroundColor = Color.red;
                }
            };

            m_blendShapeValuesList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var element = currentKey.blendShapeValues[index];
                rect.height = 20;
                rect.y += 4;
                element.node = EditorGUI.ObjectField(rect, "SkinnedMeshRenderer", element.node, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;

                if (element.node != null && element.node.sharedMesh.blendShapeCount > 0)
                {
                    var (blendShapePopupNames, blendShapePopupValues) = getBlendShapePopup(element.node.name);
                    rect.y += 24;
                    element.index = EditorGUI.IntPopup(rect, element.index, blendShapePopupNames, blendShapePopupValues);
                    rect.y += 24;
                    element.weight = EditorGUI.Slider(rect, "Weight", element.weight, 0f, 1.0f);
                    if (EditorApplication.isPlaying)
                    {
                        element.node.SetBlendShapeWeight(element.index, element.weight * 100);
                    }
                }
                else
                {
                    rect.y += 24;
                    rect.height = 40;
                    EditorGUI.HelpBox(rect, "Please select a SkinnedMeshRenderer with BlendShapes", MessageType.Warning);
                }
            };
        }

        int currentRendererIndex, currentMaterialIndex, currentPropertyIndex;
        void DrawMaterialBindingGUI()
        {
            if (renderNames == null || renderIndexes == null)
                return;
            currentRendererIndex = EditorGUILayout.IntPopup("SkinnedMeshRenderer", currentRendererIndex, renderNames, renderIndexes);
            string rendererName = renderNames[currentRendererIndex];
            var cache = materialDic[rendererName];
            currentMaterialIndex = EditorGUILayout.IntPopup("Material", currentMaterialIndex, cache.MaterialNames, cache.MaterialIndexes);
            var propertyMap = cache.MaterialProperties[currentMaterialIndex];
            currentPropertyIndex = EditorGUILayout.Popup("Property", currentPropertyIndex, propertyMap.PropertyName);
            if (GUILayout.Button("Add Material Property"))
            {
                if (propertyMap.Property[currentPropertyIndex].type == MaterialPropertyType.Float)
                {
                    currentKey.materialFloatValues.Add(new MaterialFloatValueBinding()
                    {
                        node = materialRenders[currentRendererIndex],
                        index = currentMaterialIndex,
                        propertyName = propertyMap.PropertyName[currentPropertyIndex],
                        baseValue = propertyMap.Property[currentPropertyIndex].defaultFloat,
                        targetValue = propertyMap.Property[currentPropertyIndex].defaultFloat
                    });
                }
                if (propertyMap.Property[currentPropertyIndex].type == MaterialPropertyType.Color)
                {
                    currentKey.materialColorValues.Add(new MaterialColorValueBinding()
                    {
                        node = materialRenders[currentRendererIndex],
                        index = currentMaterialIndex,
                        propertyName = propertyMap.PropertyName[currentPropertyIndex],
                        baseValue = propertyMap.Property[currentPropertyIndex].defaultColor,
                        targetValue = propertyMap.Property[currentPropertyIndex].defaultColor
                    });
                }
                if (propertyMap.Property[currentPropertyIndex].type == MaterialPropertyType.Vector)
                {
                    currentKey.materialVector4Values.Add(new MaterialVector4ValueBinding()
                    {
                        node = materialRenders[currentRendererIndex],
                        index = currentMaterialIndex,
                        propertyName = propertyMap.PropertyName[currentPropertyIndex],
                        baseValue = propertyMap.Property[currentPropertyIndex].defaultVector4,
                        targetValue = propertyMap.Property[currentPropertyIndex].defaultVector4
                    });
                }
            }
            for (int i = 0; i < currentKey.materialFloatValues.Count; i++)
            {
                var v = currentKey.materialFloatValues[i];
                v.node = EditorGUILayout.ObjectField("SkinnedMeshRenderer", v.node, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
                v.index = EditorGUILayout.IntField("Index", v.index);
                v.propertyName = EditorGUILayout.TextField("Index", v.propertyName);
                v.baseValue = EditorGUILayout.FloatField("Float From", v.baseValue);
                v.targetValue = EditorGUILayout.FloatField("Float To", v.targetValue);
                if (GUILayout.Button("X"))
                {
                    currentKey.materialFloatValues.RemoveAt(i);
                    return;
                }
            }
            for (int i = 0; i < currentKey.materialColorValues.Count; i++)
            {
                var v = currentKey.materialColorValues[i];
                v.node = EditorGUILayout.ObjectField("SkinnedMeshRenderer", v.node, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
                v.index = EditorGUILayout.IntField("Index", v.index);
                v.propertyName = EditorGUILayout.TextField("Index", v.propertyName);
                v.baseValue = EditorGUILayout.ColorField("Color From", v.baseValue);
                v.targetValue = EditorGUILayout.ColorField("Color To", v.targetValue);
                if (GUILayout.Button("X"))
                {
                    currentKey.materialColorValues.RemoveAt(i);
                    return;
                }
            }
            for (int i = 0; i < currentKey.materialVector4Values.Count; i++)
            {
                var v = currentKey.materialVector4Values[i];
                v.node = EditorGUILayout.ObjectField("SkinnedMeshRenderer", v.node, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
                v.index = EditorGUILayout.IntField("Index", v.index);
                v.propertyName = EditorGUILayout.TextField("Index", v.propertyName);
                v.baseValue = EditorGUILayout.Vector4Field("Vector From", v.baseValue);
                v.targetValue = EditorGUILayout.Vector4Field("Vector To", v.targetValue);
                if (GUILayout.Button("X"))
                {
                    currentKey.materialVector4Values.RemoveAt(i);
                    return;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying)
            {
                presetNames = mixer.keys.Select(x => x.keyName).ToArray();
                DrawBlendShapeMixerSelect();
                EditorGUILayout.Space();
                m_mode = GUILayout.Toolbar(m_mode, EDIT_MODES);
                switch (m_mode)
                {
                    case 0:
                        {
                            AllBlendShapeBindsGUI(); ;
                        }
                        break;
                    case 1:
                        {
                            if (GUILayout.Button("Clear"))
                            {
                                currentKey.materialFloatValues?.Clear();
                                currentKey.materialColorValues?.Clear();
                                currentKey.materialVector4Values?.Clear();
                            }
                            DrawMaterialBindingGUI();
                        }
                        break;
                }
            }
            else
            {
                foreach (var key in mixer.keys)
                {
                    var oValue = mixer.RuntimeWeight[key.keyName];
                    var value = EditorGUILayout.Slider(key.keyName, mixer.RuntimeWeight[key.keyName], 0, 1);
                    if (oValue != value)
                    {
                        mixer.RuntimeWeight[key.keyName] = value;

                        foreach (var blendshapeValue in key.blendShapeValues)
                        {
                            blendshapeValue.node.SetBlendShapeWeight(blendshapeValue.index, blendshapeValue.weight * mixer.RuntimeWeight[key.keyName]);
                        }

                        foreach (var materialValue in key.materialVector4Values)
                        {
                            Vector4 targetValue = materialValue.baseValue + (materialValue.targetValue - materialValue.baseValue) * mixer.RuntimeWeight[key.keyName];
                            materialValue.node.materials[materialValue.index].SetVector(materialValue.propertyName, targetValue);
                        }
                        foreach (var materialValue in key.materialColorValues)
                        {
                            Color targetValue = materialValue.baseValue + (materialValue.targetValue - materialValue.baseValue) * mixer.RuntimeWeight[key.keyName];
                            materialValue.node.materials[materialValue.index].SetColor(materialValue.propertyName, targetValue);
                        }
                        foreach (var materialValue in key.materialFloatValues)
                        {
                            float targetValue = materialValue.baseValue + (materialValue.targetValue - materialValue.baseValue) * mixer.RuntimeWeight[key.keyName];
                            materialValue.node.materials[materialValue.index].SetFloat(materialValue.propertyName, targetValue);
                        }
                    }
                }
            }
        }
    }
}