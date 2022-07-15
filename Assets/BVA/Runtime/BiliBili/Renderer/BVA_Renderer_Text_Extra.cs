using Newtonsoft.Json.Linq;
using UnityEngine;
using GLTF.Schema;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;

namespace GLTF.Schema.BVA
{
    public class BVA_Renderer_Text_Extra : IExtra
    {
        public const string PROPERTY = "MeshText";
        public string text;
        //public Font font;
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
        public BVA_Renderer_Text_Extra(TextMesh textMesh)
        {
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
            jo.Add(nameof(color), color.ToNumericsColorRaw().ToJArray());
            return new JProperty(PROPERTY, jo);
        }
        public static void Deserialize(GLTFRoot _gltfRoot, JsonReader reader, TextMesh meshText)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(text):
                            meshText.text = reader.ReadAsString();
                            break;
                        case nameof(fontSize):
                            meshText.fontSize = reader.ReadAsInt32().Value;
                            break;
                        case nameof(fontStyle):
                            meshText.fontStyle = reader.ReadStringEnum<FontStyle>();
                            break;
                        case nameof(offsetZ):
                            meshText.offsetZ = reader.ReadAsFloat();
                            break;
                        case nameof(alignment):
                            meshText.alignment = reader.ReadStringEnum<TextAlignment>();
                            break;
                        case nameof(anchor):
                            meshText.anchor = reader.ReadStringEnum<TextAnchor>();
                            break;
                        case nameof(characterSize):
                            meshText.characterSize = reader.ReadAsFloat();
                            break;
                        case nameof(lineSpacing):
                            meshText.lineSpacing = reader.ReadAsFloat();
                            break;
                        case nameof(tabSize):
                            meshText.tabSize = reader.ReadAsFloat();
                            break;
                        case nameof(richText):
                            meshText.richText = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(color):
                            meshText.color = reader.ReadAsRGBAColor().ToUnityColorRaw();
                            break;
                    }
                }
            }
        }
    }
}
