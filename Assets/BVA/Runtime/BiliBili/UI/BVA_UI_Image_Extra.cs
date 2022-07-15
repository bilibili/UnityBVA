using Newtonsoft.Json.Linq;
using GLTF.Math;
using GLTF.Schema;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;

namespace GLTF.Schema.BVA
{
    public class BVA_UI_Image_Extra : IExtra
    {
        public const string PROPERTY = "BVA_UI_Image_Extra";
        public SpriteId sprite;
        public UnityEngine.UI.Image.Type type;
        public bool preserveAspect;
        public bool fillCenter;
        public UnityEngine.UI.Image.FillMethod fillMethod;
        public float fillAmount;
        public bool fillClockwise;
        public int fillOrigin;
        public float alphaHitTestMinimumThreshold;
        public bool useSpriteMesh;
        public float pixelsPerUnitMultiplier;
        public BVA_UI_Image_Extra() { }

        public BVA_UI_Image_Extra(UnityEngine.UI.Image target, SpriteId spriteId)
        {
            this.sprite = spriteId;
            this.type = target.type;
            this.preserveAspect = target.preserveAspect;
            this.fillCenter = target.fillCenter;
            this.fillMethod = target.fillMethod;
            this.fillAmount = target.fillAmount;
            this.fillClockwise = target.fillClockwise;
            this.fillOrigin = target.fillOrigin;
            this.alphaHitTestMinimumThreshold = target.alphaHitTestMinimumThreshold;
            this.useSpriteMesh = target.useSpriteMesh;
            this.pixelsPerUnitMultiplier = target.pixelsPerUnitMultiplier;
        }
        public static async Task Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.UI.Image target, AsyncLoadTexture loadTexture, AsyncLoadMaterial loadMaterial, AsyncLoadSprite loadSprite)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_UI_Image_Extra.sprite):
                            int spriteIndex = reader.ReadAsInt32().Value;
                            target.sprite = await loadSprite(new SpriteId() { Id = spriteIndex, Root = root });
                            break;
                        case nameof(BVA_UI_Image_Extra.type):
                            target.type = reader.ReadStringEnum<UnityEngine.UI.Image.Type>();
                            break;
                        case nameof(BVA_UI_Image_Extra.preserveAspect):
                            target.preserveAspect = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Image_Extra.fillCenter):
                            target.fillCenter = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Image_Extra.fillMethod):
                            target.fillMethod = reader.ReadStringEnum<UnityEngine.UI.Image.FillMethod>();
                            break;
                        case nameof(BVA_UI_Image_Extra.fillAmount):
                            target.fillAmount = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_Image_Extra.fillClockwise):
                            target.fillClockwise = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Image_Extra.fillOrigin):
                            target.fillOrigin = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_UI_Image_Extra.alphaHitTestMinimumThreshold):
                            target.alphaHitTestMinimumThreshold = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_UI_Image_Extra.useSpriteMesh):
                            target.useSpriteMesh = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_Image_Extra.pixelsPerUnitMultiplier):
                            target.pixelsPerUnitMultiplier = reader.ReadAsFloat();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(sprite), sprite.Id);
            jo.Add(nameof(type), type.ToString());
            jo.Add(nameof(preserveAspect), preserveAspect);
            jo.Add(nameof(fillCenter), fillCenter);
            jo.Add(nameof(fillMethod), fillMethod.ToString());
            jo.Add(nameof(fillAmount), fillAmount);
            jo.Add(nameof(fillClockwise), fillClockwise);
            jo.Add(nameof(fillOrigin), fillOrigin);
            jo.Add(nameof(alphaHitTestMinimumThreshold), alphaHitTestMinimumThreshold);
            jo.Add(nameof(useSpriteMesh), useSpriteMesh);
            jo.Add(nameof(pixelsPerUnitMultiplier), pixelsPerUnitMultiplier);
            return new JProperty(BVA_UI_Image_Extra.PROPERTY, jo);
        }
    }
}
