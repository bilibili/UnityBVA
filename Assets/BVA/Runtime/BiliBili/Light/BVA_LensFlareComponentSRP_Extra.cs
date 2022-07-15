#if UNITY_2021_1_OR_NEWER
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
namespace GLTF.Schema.BVA
{
    public class BVA_LensFlareComponentSRP_Extra : IExtra
    {
        public const string PROPERTY = "BVA_LensFlareComponentSRP_Extra";
        public float intensity;
        public float maxAttenuationDistance;
        public float maxAttenuationScale;
        public UnityEngine.AnimationCurve distanceAttenuationCurve;
        public UnityEngine.AnimationCurve scaleByDistanceCurve;
        public bool attenuationByLightShape;
        public UnityEngine.AnimationCurve radialScreenAttenuationCurve;
        public bool useOcclusion;
        public float occlusionRadius;
        public float occlusionOffset;
        public float scale;
        public bool allowOffScreen;
        public BVA_LensFlareComponentSRP_Extra() { }

        public BVA_LensFlareComponentSRP_Extra(UnityEngine.Rendering.LensFlareComponentSRP target)
        {
            this.intensity = target.intensity;
            this.maxAttenuationDistance = target.maxAttenuationDistance;
            this.maxAttenuationScale = target.maxAttenuationScale;
            this.distanceAttenuationCurve = target.distanceAttenuationCurve;
            this.scaleByDistanceCurve = target.scaleByDistanceCurve;
            this.attenuationByLightShape = target.attenuationByLightShape;
            this.radialScreenAttenuationCurve = target.radialScreenAttenuationCurve;
            this.useOcclusion = target.useOcclusion;
            this.occlusionRadius = target.occlusionRadius;
            this.occlusionOffset = target.occlusionOffset;
            this.scale = target.scale;
            this.allowOffScreen = target.allowOffScreen;
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.Rendering.LensFlareComponentSRP target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_LensFlareComponentSRP_Extra.intensity):
                            target.intensity = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.maxAttenuationDistance):
                            target.maxAttenuationDistance = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.maxAttenuationScale):
                            target.maxAttenuationScale = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.distanceAttenuationCurve):
                            target.distanceAttenuationCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.scaleByDistanceCurve):
                            target.scaleByDistanceCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.attenuationByLightShape):
                            target.attenuationByLightShape = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.radialScreenAttenuationCurve):
                            target.radialScreenAttenuationCurve = AnimationCurveExtension.DeserializeAnimationCurve(root, reader);
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.useOcclusion):
                            target.useOcclusion = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.occlusionRadius):
                            target.occlusionRadius = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.occlusionOffset):
                            target.occlusionOffset = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.scale):
                            target.scale = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_LensFlareComponentSRP_Extra.allowOffScreen):
                            target.allowOffScreen = reader.ReadAsBoolean().Value;
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(intensity), intensity);
            jo.Add(nameof(maxAttenuationDistance), maxAttenuationDistance);
            jo.Add(nameof(maxAttenuationScale), maxAttenuationScale);
            jo.Add(nameof(distanceAttenuationCurve), distanceAttenuationCurve.Serialize());
            jo.Add(nameof(scaleByDistanceCurve), distanceAttenuationCurve.Serialize());
            jo.Add(nameof(attenuationByLightShape), attenuationByLightShape);
            jo.Add(nameof(radialScreenAttenuationCurve), distanceAttenuationCurve.Serialize());
            jo.Add(nameof(useOcclusion), useOcclusion);
            jo.Add(nameof(occlusionRadius), occlusionRadius);
            jo.Add(nameof(occlusionOffset), occlusionOffset);
            jo.Add(nameof(scale), scale);
            jo.Add(nameof(allowOffScreen), allowOffScreen);
            return new JProperty(BVA_LensFlareComponentSRP_Extra.PROPERTY, jo);
        }
    }
}
#endif