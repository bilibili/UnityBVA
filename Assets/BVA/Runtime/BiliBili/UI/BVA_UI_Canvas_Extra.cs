using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    public class BVA_UI_Canvas_Extra : IExtra
    {
        public const string PROPERTY = "BVA_UI_Canvas_Extra";
        public RenderMode renderMode;
        public float scaleFactor;
        public float referencePixelsPerUnit;
        public bool overridePixelPerfect;
        public bool pixelPerfect;
        public float planeDistance;
        public bool overrideSorting;
        public int sortingOrder;
        public int targetDisplay;
        public int sortingLayerID;
        public AdditionalCanvasShaderChannels additionalShaderChannels;
        public string sortingLayerName;
        public float normalizedSortingGridSize;
        public BVA_UI_Canvas_Extra() { }

        public BVA_UI_Canvas_Extra(Canvas target)
        {
            this.renderMode = target.renderMode;
            this.scaleFactor = target.scaleFactor;
            this.referencePixelsPerUnit = target.referencePixelsPerUnit;
            this.overridePixelPerfect = target.overridePixelPerfect;
            this.pixelPerfect = target.pixelPerfect;
            this.planeDistance = target.planeDistance;
            this.overrideSorting = target.overrideSorting;
            this.sortingOrder = target.sortingOrder;
            this.targetDisplay = target.targetDisplay;
            this.sortingLayerID = target.sortingLayerID;
            this.additionalShaderChannels = target.additionalShaderChannels;
            this.sortingLayerName = target.sortingLayerName;
            this.normalizedSortingGridSize = target.normalizedSortingGridSize;
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.Canvas target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_UI_Canvas_Extra.renderMode):
                            target.renderMode = reader.ReadStringEnum<UnityEngine.RenderMode>();
                            break;
                        case nameof(BVA_UI_Canvas_Extra.scaleFactor):
                            target.scaleFactor = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_Canvas_Extra.referencePixelsPerUnit):
                            target.referencePixelsPerUnit = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_Canvas_Extra.overridePixelPerfect):
                            target.overridePixelPerfect = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Canvas_Extra.pixelPerfect):
                            target.pixelPerfect = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Canvas_Extra.planeDistance):
                            target.planeDistance = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_Canvas_Extra.overrideSorting):
                            target.overrideSorting = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Canvas_Extra.sortingOrder):
                            target.sortingOrder = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_Canvas_Extra.targetDisplay):
                            target.targetDisplay = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_Canvas_Extra.sortingLayerID):
                            target.sortingLayerID = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_Canvas_Extra.additionalShaderChannels):
                            target.additionalShaderChannels = reader.ReadStringEnum<AdditionalCanvasShaderChannels>();
                            break;
                        case nameof(BVA_UI_Canvas_Extra.sortingLayerName):
                            target.sortingLayerName = reader.ReadAsString();
                            break;
                        case nameof(BVA_UI_Canvas_Extra.normalizedSortingGridSize):
                            target.normalizedSortingGridSize = reader.ReadAsFloat();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(renderMode), renderMode.ToString());
            jo.Add(nameof(scaleFactor), scaleFactor);
            jo.Add(nameof(referencePixelsPerUnit), referencePixelsPerUnit);
            jo.Add(nameof(overridePixelPerfect), overridePixelPerfect);
            jo.Add(nameof(pixelPerfect), pixelPerfect);
            jo.Add(nameof(planeDistance), planeDistance);
            jo.Add(nameof(overrideSorting), overrideSorting);
            jo.Add(nameof(sortingOrder), sortingOrder);
            jo.Add(nameof(targetDisplay), targetDisplay);
            jo.Add(nameof(sortingLayerID), sortingLayerID);
            jo.Add(nameof(additionalShaderChannels), additionalShaderChannels.ToString());
            jo.Add(nameof(sortingLayerName), sortingLayerName);
            jo.Add(nameof(normalizedSortingGridSize), normalizedSortingGridSize);
            return new JProperty(BVA_UI_Canvas_Extra.PROPERTY, jo);
        }
    }
}
