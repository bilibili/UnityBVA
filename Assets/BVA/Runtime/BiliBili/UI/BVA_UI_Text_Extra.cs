using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;

namespace GLTF.Schema.BVA
{
    public class BVA_UI_Text_Extra : IExtra
    {
        public const string PROPERTY = "BVA_UI_Text_Extra";
        public string text;
        public bool supportRichText;
        public bool resizeTextForBestFit;
        public int resizeTextMinSize;
        public int resizeTextMaxSize;
        public UnityEngine.TextAnchor alignment;
        public bool alignByGeometry;
        public int fontSize;
        public UnityEngine.HorizontalWrapMode horizontalOverflow;
        public UnityEngine.VerticalWrapMode verticalOverflow;
        public float lineSpacing;
        public UnityEngine.FontStyle fontStyle;
        public BVA_UI_Text_Extra() { }

        public BVA_UI_Text_Extra(UnityEngine.UI.Text target)
        {
            this.text = target.text;
            this.supportRichText = target.supportRichText;
            this.resizeTextForBestFit = target.resizeTextForBestFit;
            this.resizeTextMinSize = target.resizeTextMinSize;
            this.resizeTextMaxSize = target.resizeTextMaxSize;
            this.alignment = target.alignment;
            this.alignByGeometry = target.alignByGeometry;
            this.fontSize = target.fontSize;
            this.horizontalOverflow = target.horizontalOverflow;
            this.verticalOverflow = target.verticalOverflow;
            this.lineSpacing = target.lineSpacing;
            this.fontStyle = target.fontStyle;
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.UI.Text target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_UI_Text_Extra.text):
                            target.text = reader.ReadAsString();
                            break;
                        case nameof(BVA_UI_Text_Extra.supportRichText):
                            target.supportRichText = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Text_Extra.resizeTextForBestFit):
                            target.resizeTextForBestFit = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Text_Extra.resizeTextMinSize):
                            target.resizeTextMinSize = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_Text_Extra.resizeTextMaxSize):
                            target.resizeTextMaxSize = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_Text_Extra.alignment):
                            target.alignment = reader.ReadStringEnum<UnityEngine.TextAnchor>();
                            break;
                        case nameof(BVA_UI_Text_Extra.alignByGeometry):
                            target.alignByGeometry = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Text_Extra.fontSize):
                            target.fontSize = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_Text_Extra.horizontalOverflow):
                            target.horizontalOverflow = reader.ReadStringEnum<UnityEngine.HorizontalWrapMode>();
                            break;
                        case nameof(BVA_UI_Text_Extra.verticalOverflow):
                            target.verticalOverflow = reader.ReadStringEnum<UnityEngine.VerticalWrapMode>();
                            break;
                        case nameof(BVA_UI_Text_Extra.lineSpacing):
                            target.lineSpacing = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_Text_Extra.fontStyle):
                            target.fontStyle = reader.ReadStringEnum<UnityEngine.FontStyle>();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(text), text);
            jo.Add(nameof(supportRichText), supportRichText);
            jo.Add(nameof(resizeTextForBestFit), resizeTextForBestFit);
            jo.Add(nameof(resizeTextMinSize), resizeTextMinSize);
            jo.Add(nameof(resizeTextMaxSize), resizeTextMaxSize);
            jo.Add(nameof(alignment), alignment.ToString());
            jo.Add(nameof(alignByGeometry), alignByGeometry);
            jo.Add(nameof(fontSize), fontSize);
            jo.Add(nameof(horizontalOverflow), horizontalOverflow.ToString());
            jo.Add(nameof(verticalOverflow), verticalOverflow.ToString());
            jo.Add(nameof(lineSpacing), lineSpacing);
            jo.Add(nameof(fontStyle), fontStyle.ToString());
            return new JProperty(BVA_UI_Text_Extra.PROPERTY, jo);
        }
    }
}
