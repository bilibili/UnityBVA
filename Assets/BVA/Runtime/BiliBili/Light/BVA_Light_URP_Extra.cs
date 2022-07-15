using Newtonsoft.Json.Linq;
using UnityEngine;
using GLTF.Schema;
using Newtonsoft.Json;
using GLTF.Extensions;
using UnityEngine.Rendering;

namespace GLTF.Schema.BVA
{
    public class BVA_Light_URP_Extra : IExtra
    {
        public const string PROPERTY = "UniversalAdditionalLightData";
        public float shadowAngle;
        public float shadowBias;
        public float shadowStrength;
        public float shadowNearPlane;
        public float shadowNormalBias;
        public float shadowRadius;
        public LightShadowCasterMode lightShadowCasterMode;
        public LightShadowResolution shadowResolution;
        public LightShadows shadows;
        public LightShape shape;
        public int cullingMask;
        public int renderingLayerMask;

        public BVA_Light_URP_Extra(Light light)
        {
            lightShadowCasterMode = light.lightShadowCasterMode;
#if UNITY_EDITOR
            shadowAngle = light.shadowAngle;
#endif
            shadowBias = light.shadowBias;
            shadowStrength = light.shadowStrength;
            shadowNearPlane = light.shadowNearPlane;
            shadowNormalBias = light.shadowNormalBias;

#if UNITY_EDITOR
            shadowRadius = light.shadowRadius;
#endif
            shadowResolution = light.shadowResolution;
            shadows = light.shadows;
            shape = light.shape;

            cullingMask = light.cullingMask;
            renderingLayerMask = light.renderingLayerMask;
        }

        public JProperty Serialize()
        {
            JObject jo = new JObject();
            if (shadows != LightShadows.None)
            {
                jo.Add(nameof(shadowAngle), shadowAngle);
                jo.Add(nameof(shadowBias), shadowBias);
                jo.Add(nameof(shadowStrength), shadowStrength);
                jo.Add(nameof(shadowNearPlane), shadowNearPlane);
                jo.Add(nameof(shadowNormalBias), shadowNormalBias);
                jo.Add(nameof(shadowRadius), shadowRadius);

                jo.Add(nameof(lightShadowCasterMode), lightShadowCasterMode.ToString());
                jo.Add(nameof(shadowResolution), shadowResolution.ToString());
                jo.Add(nameof(shadows), shadows.ToString());
            }
            jo.Add(nameof(shape), shape.ToString());

            jo.Add(nameof(cullingMask), cullingMask);
            jo.Add(nameof(renderingLayerMask), renderingLayerMask);
            return new JProperty(PROPERTY, jo);
        }
        public static void Deserialize(GLTFRoot _gltfRoot, JsonReader reader, Light light)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
#if UNITY_EDITOR
                        case nameof(shadowAngle):
                            light.shadowAngle = reader.ReadAsFloat();
                            break;
#endif
                        case nameof(shadowBias):
                            light.shadowBias = reader.ReadAsFloat();
                            break;
                        case nameof(shadowStrength):
                            light.shadowStrength = reader.ReadAsFloat();
                            break;
                        case nameof(shadowNearPlane):
                            light.shadowNearPlane = reader.ReadAsFloat();
                            break;
                        case nameof(shadowNormalBias):
                            light.shadowNormalBias = reader.ReadAsFloat();
                            break;
#if UNITY_EDITOR
                        case nameof(shadowRadius):
                            light.shadowRadius = reader.ReadAsFloat();
                            break;
#endif
                        case nameof(lightShadowCasterMode):
                            light.lightShadowCasterMode = reader.ReadStringEnum<LightShadowCasterMode>();
                            break;
                        case nameof(shadowResolution):
                            light.shadowResolution = reader.ReadStringEnum<LightShadowResolution>();
                            break;
                        case nameof(shadows):
                            light.shadows = reader.ReadStringEnum<LightShadows>();
                            break;
                        case nameof(shape):
                            light.shape = reader.ReadStringEnum<LightShape>();
                            break;
                        case nameof(cullingMask):
                            light.cullingMask = reader.ReadAsInt32().Value;
                            break;
                        case nameof(renderingLayerMask):
                            light.renderingLayerMask = reader.ReadAsInt32().Value;
                            break;
                    }
                }
            }
        }
    }
}