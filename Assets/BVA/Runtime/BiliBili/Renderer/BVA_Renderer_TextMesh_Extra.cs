using Newtonsoft.Json.Linq;
using UnityEngine;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System;

namespace GLTF.Schema.BVA
{
    [ComponentExtra]
    public class BVA_Renderer_TextMesh_Extra : IComponentExtra
    {
        public string text;
        public int fontSize;
        public FontStyle fontStyle;
        public float offsetZ;
        public TextAlignment alignment;
        public TextAnchor anchor;
        public float characterSize;
        public float lineSpacing;
        public float tabSize;
        public bool richText;
        public Color color;

        public string ComponentName => ComponentType.Name;
        public Type ComponentType => typeof(TextMesh);

        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(text), text);
            jo.Add(nameof(fontSize), fontSize);
            jo.Add(nameof(fontStyle), fontStyle.ToString());
            jo.Add(nameof(offsetZ), offsetZ);
            jo.Add(nameof(alignment), alignment.ToString());
            jo.Add(nameof(anchor), anchor.ToString());
            jo.Add(nameof(characterSize), characterSize);
            jo.Add(nameof(lineSpacing), lineSpacing);
            jo.Add(nameof(tabSize), tabSize);
            jo.Add(nameof(richText), richText);
            jo.Add(nameof(color), color.ToJArray());
            return new JProperty(ComponentName, jo);
        }

        public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
        {
            var textMesh = component as TextMesh;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(text):
                            textMesh.text = reader.ReadAsString();
                            break;
                        case nameof(fontSize):
                            textMesh.fontSize = reader.ReadAsInt32().Value;
                            break;
                        case nameof(fontStyle):
                            textMesh.fontStyle = reader.ReadStringEnum<FontStyle>();
                            break;
                        case nameof(offsetZ):
                            textMesh.offsetZ = reader.ReadAsFloat();
                            break;
                        case nameof(alignment):
                            textMesh.alignment = reader.ReadStringEnum<TextAlignment>();
                            break;
                        case nameof(anchor):
                            textMesh.anchor = reader.ReadStringEnum<TextAnchor>();
                            break;
                        case nameof(characterSize):
                            textMesh.characterSize = reader.ReadAsFloat();
                            break;
                        case nameof(lineSpacing):
                            textMesh.lineSpacing = reader.ReadAsFloat();
                            break;
                        case nameof(tabSize):
                            textMesh.tabSize = reader.ReadAsFloat();
                            break;
                        case nameof(richText):
                            textMesh.richText = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(color):
                            textMesh.color = reader.ReadAsRGBAColor();
                            break;
                    }
                }
            }
        }

        public void SetData(Component component)
        {
            var textMesh = component as TextMesh;
            text = textMesh.text;
            fontSize = textMesh.fontSize;
            fontStyle = textMesh.fontStyle;
            offsetZ = textMesh.offsetZ;
            alignment = textMesh.alignment;
            anchor = textMesh.anchor;
            characterSize = textMesh.characterSize;
            lineSpacing = textMesh.lineSpacing;
            tabSize = textMesh.tabSize;
            richText = textMesh.richText;
            color = textMesh.color;
        }

        public object Clone()
        {
            return new BVA_Renderer_TextMesh_Extra();
        }
    }
}
