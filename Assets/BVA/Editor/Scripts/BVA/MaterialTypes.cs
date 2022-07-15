using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BVA
{
    public enum MaterialPropertyType
    {
        Int,
        Float,
        Vector,
        Color,
        Texture2D,
        Cubemap
    }

    public struct MaterialExportProperty
        {
            public MaterialPropertyType type;
            public float defaultFloat;
            public Color defaultColor;
            public Vector4 defaultVector4;
            public MaterialExportProperty(float value)
            {
                type = MaterialPropertyType.Float;
                defaultFloat = value;
                defaultColor = Color.white;
                defaultVector4 = Vector4.one;
            }
            public MaterialExportProperty(Vector4 value)
            {
                type = MaterialPropertyType.Vector;
                defaultFloat = 0.0f;
                defaultColor = Color.white;
                defaultVector4 = value;
            }
            public MaterialExportProperty(Color value)
            {
                type = MaterialPropertyType.Color;
                defaultFloat = 0.0f;
                defaultColor = value;
                defaultVector4 = Vector4.one;
            }
        }
        public struct MaterialPropertyMap
        {
            public string MaterialName;
            public string[] PropertyName;
            public MaterialExportProperty[] Property;
        }
        public struct MaterialCache
        {
            public MaterialPropertyMap[] MaterialProperties;
            public int[] MaterialIndexes;
            public string[] MaterialNames;
        }
}