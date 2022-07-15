using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;

namespace GLTF.Schema.BVA
{
    public class BVA_UI_InputField_Extra : IExtra
    {
        public const string PROPERTY = "BVA_UI_InputField_Extra";
        public bool shouldHideMobileInput;
        public bool shouldActivateOnSelect;
        public string text;
        public float caretBlinkRate;
        public int caretWidth;
        public UnityEngine.Color caretColor;
        public bool customCaretColor;
        public UnityEngine.Color selectionColor;
        public int characterLimit;
        public UnityEngine.UI.InputField.ContentType contentType;
        public UnityEngine.UI.InputField.LineType lineType;
        public UnityEngine.UI.InputField.InputType inputType;
        public UnityEngine.TouchScreenKeyboardType keyboardType;
        public UnityEngine.UI.InputField.CharacterValidation characterValidation;
        public bool readOnly;
        public int caretPosition;
        public int selectionAnchorPosition;
        public int selectionFocusPosition;
        public BVA_UI_InputField_Extra() { }

        public BVA_UI_InputField_Extra(UnityEngine.UI.InputField target)
        {
            this.shouldHideMobileInput = target.shouldHideMobileInput;
            this.shouldActivateOnSelect = target.shouldActivateOnSelect;
            this.text = target.text;
            this.caretBlinkRate = target.caretBlinkRate;
            this.caretWidth = target.caretWidth;
            this.caretColor = target.caretColor;
            this.customCaretColor = target.customCaretColor;
            this.selectionColor = target.selectionColor;
            this.characterLimit = target.characterLimit;
            this.contentType = target.contentType;
            this.lineType = target.lineType;
            this.inputType = target.inputType;
            this.keyboardType = target.keyboardType;
            this.characterValidation = target.characterValidation;
            this.readOnly = target.readOnly;
            this.caretPosition = target.caretPosition;
            this.selectionAnchorPosition = target.selectionAnchorPosition;
            this.selectionFocusPosition = target.selectionFocusPosition;
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.UI.InputField target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_UI_InputField_Extra.shouldHideMobileInput):
                            target.shouldHideMobileInput = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_InputField_Extra.shouldActivateOnSelect):
                            target.shouldActivateOnSelect = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_InputField_Extra.text):
                            target.text = reader.ReadAsString();
                            break;
                        case nameof(BVA_UI_InputField_Extra.caretBlinkRate):
                            target.caretBlinkRate = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_InputField_Extra.caretWidth):
                            target.caretWidth = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_InputField_Extra.caretColor):
                            target.caretColor = reader.ReadAsRGBAColor().ToUnityColorRaw();
                            break;
                        case nameof(BVA_UI_InputField_Extra.customCaretColor):
                            target.customCaretColor = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_InputField_Extra.selectionColor):
                            target.selectionColor = reader.ReadAsRGBAColor().ToUnityColorRaw();
                            break;
                        case nameof(BVA_UI_InputField_Extra.characterLimit):
                            target.characterLimit = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_InputField_Extra.contentType):
                            target.contentType = reader.ReadStringEnum<UnityEngine.UI.InputField.ContentType>();
                            break;
                        case nameof(BVA_UI_InputField_Extra.lineType):
                            target.lineType = reader.ReadStringEnum<UnityEngine.UI.InputField.LineType>();
                            break;
                        case nameof(BVA_UI_InputField_Extra.inputType):
                            target.inputType = reader.ReadStringEnum<UnityEngine.UI.InputField.InputType>();
                            break;
                        case nameof(BVA_UI_InputField_Extra.keyboardType):
                            target.keyboardType = reader.ReadStringEnum<UnityEngine.TouchScreenKeyboardType>();
                            break;
                        case nameof(BVA_UI_InputField_Extra.characterValidation):
                            target.characterValidation = reader.ReadStringEnum<UnityEngine.UI.InputField.CharacterValidation>();
                            break;
                        case nameof(BVA_UI_InputField_Extra.readOnly):
                            target.readOnly = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_InputField_Extra.caretPosition):
                            target.caretPosition = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_InputField_Extra.selectionAnchorPosition):
                            target.selectionAnchorPosition = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_InputField_Extra.selectionFocusPosition):
                            target.selectionFocusPosition = reader.ReadAsInt32().Value;
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(shouldHideMobileInput), shouldHideMobileInput);
            jo.Add(nameof(shouldActivateOnSelect), shouldActivateOnSelect);
            jo.Add(nameof(text), text);
            jo.Add(nameof(caretBlinkRate), caretBlinkRate);
            jo.Add(nameof(caretWidth), caretWidth);
            jo.Add(nameof(caretColor), caretColor.ToNumericsColorRaw().ToJArray());
            jo.Add(nameof(customCaretColor), customCaretColor);
            jo.Add(nameof(selectionColor), selectionColor.ToNumericsColorRaw().ToJArray());
            jo.Add(nameof(characterLimit), characterLimit);
            jo.Add(nameof(contentType), contentType.ToString());
            jo.Add(nameof(lineType), lineType.ToString());
            jo.Add(nameof(inputType), inputType.ToString());
            jo.Add(nameof(keyboardType), keyboardType.ToString());
            jo.Add(nameof(characterValidation), characterValidation.ToString());
            jo.Add(nameof(readOnly), readOnly);
            jo.Add(nameof(caretPosition), caretPosition);
            jo.Add(nameof(selectionAnchorPosition), selectionAnchorPosition);
            jo.Add(nameof(selectionFocusPosition), selectionFocusPosition);
            return new JProperty(BVA_UI_InputField_Extra.PROPERTY, jo);
        }
    }
}
