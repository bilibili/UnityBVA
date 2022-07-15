using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;

namespace GLTF.Schema.BVA
{
    public class BVA_UI_CanvasScaler_Extra : IExtra
    {
        public const string PROPERTY = "BVA_UI_CanvasScaler_Extra";
        public UnityEngine.UI.CanvasScaler.ScaleMode uiScaleMode;
        public float referencePixelsPerUnit;
        public float scaleFactor;
        public UnityEngine.Vector2 referenceResolution;
        public UnityEngine.UI.CanvasScaler.ScreenMatchMode screenMatchMode;
        public float matchWidthOrHeight;
        public UnityEngine.UI.CanvasScaler.Unit physicalUnit;
        public float fallbackScreenDPI;
        public float defaultSpriteDPI;
        public float dynamicPixelsPerUnit;
        public BVA_UI_CanvasScaler_Extra() { }

        public BVA_UI_CanvasScaler_Extra(UnityEngine.UI.CanvasScaler target)
        {
            this.uiScaleMode = target.uiScaleMode;
            this.referencePixelsPerUnit = target.referencePixelsPerUnit;
            this.scaleFactor = target.scaleFactor;
            this.referenceResolution = target.referenceResolution;
            this.screenMatchMode = target.screenMatchMode;
            this.matchWidthOrHeight = target.matchWidthOrHeight;
            this.physicalUnit = target.physicalUnit;
            this.fallbackScreenDPI = target.fallbackScreenDPI;
            this.defaultSpriteDPI = target.defaultSpriteDPI;
            this.dynamicPixelsPerUnit = target.dynamicPixelsPerUnit;
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.UI.CanvasScaler target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_UI_CanvasScaler_Extra.uiScaleMode):
                            target.uiScaleMode = reader.ReadStringEnum<UnityEngine.UI.CanvasScaler.ScaleMode>();
                            break;
                        case nameof(BVA_UI_CanvasScaler_Extra.referencePixelsPerUnit):
                            target.referencePixelsPerUnit = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_CanvasScaler_Extra.scaleFactor):
                            target.scaleFactor = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_CanvasScaler_Extra.referenceResolution):
                            target.referenceResolution = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                        case nameof(BVA_UI_CanvasScaler_Extra.screenMatchMode):
                            target.screenMatchMode = reader.ReadStringEnum<UnityEngine.UI.CanvasScaler.ScreenMatchMode>();
                            break;
                        case nameof(BVA_UI_CanvasScaler_Extra.matchWidthOrHeight):
                            target.matchWidthOrHeight = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_CanvasScaler_Extra.physicalUnit):
                            target.physicalUnit = reader.ReadStringEnum<UnityEngine.UI.CanvasScaler.Unit>();
                            break;
                        case nameof(BVA_UI_CanvasScaler_Extra.fallbackScreenDPI):
                            target.fallbackScreenDPI = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_CanvasScaler_Extra.defaultSpriteDPI):
                            target.defaultSpriteDPI = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_CanvasScaler_Extra.dynamicPixelsPerUnit):
                            target.dynamicPixelsPerUnit = reader.ReadAsFloat();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(uiScaleMode), uiScaleMode.ToString());
            jo.Add(nameof(referencePixelsPerUnit), referencePixelsPerUnit);
            jo.Add(nameof(scaleFactor), scaleFactor);
            jo.Add(nameof(referenceResolution), referenceResolution.ToGltfVector2Raw().ToJArray());
            jo.Add(nameof(screenMatchMode), screenMatchMode.ToString());
            jo.Add(nameof(matchWidthOrHeight), matchWidthOrHeight);
            jo.Add(nameof(physicalUnit), physicalUnit.ToString());
            jo.Add(nameof(fallbackScreenDPI), fallbackScreenDPI);
            jo.Add(nameof(defaultSpriteDPI), defaultSpriteDPI);
            jo.Add(nameof(dynamicPixelsPerUnit), dynamicPixelsPerUnit);
            return new JProperty(BVA_UI_CanvasScaler_Extra.PROPERTY, jo);
        }
    }
}
