using Newtonsoft.Json.Linq;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using BVA.Extensions;
using Newtonsoft.Json;
using GLTF.Extensions;

namespace GLTF.Schema.BVA
{
    [ComponentExtra]
    public class BVA_Renderer_URP_Extra : IComponentExtra
    {
        public bool staticShadowCaster;
        public ReflectionProbeUsage reflectionProbeUsage;
        public uint renderingLayerMask;
        public int rendererPriority;
        public string sortingLayerName;
        public int sortingLayerID;
        public int sortingOrder;
        public bool allowOcclusionWhenDynamic;
        public ShadowCastingMode shadowCastingMode;
        public bool receiveShadows;
        public bool isStatic;
        public string tag;
        public int layer;
        public int lightmapIndex;
        public Vector4 lightmapScaleOffset;

        public string ComponentName => ComponentType.Name;
        public string ExtraName => GetType().Name;
        public Type ComponentType => typeof(Renderer);

        public JProperty Serialize()
        {
            JObject jo = new JObject();
            if (isStatic) jo.Add(nameof(isStatic), isStatic);
            if (!string.IsNullOrEmpty(tag)) jo.Add(nameof(tag), tag);
            if (layer != 0) jo.Add(nameof(layer), layer);
            if (staticShadowCaster) jo.Add(nameof(staticShadowCaster), staticShadowCaster);
            if (reflectionProbeUsage != ReflectionProbeUsage.Off) jo.Add(nameof(reflectionProbeUsage), reflectionProbeUsage.ToString());
            jo.Add(nameof(renderingLayerMask), renderingLayerMask);
            if (rendererPriority != 0) jo.Add(nameof(rendererPriority), rendererPriority);
            jo.Add(nameof(sortingLayerName), sortingLayerName);
            if (sortingLayerID != 0) jo.Add(nameof(sortingLayerID), sortingLayerID);
            if (sortingOrder != 0) jo.Add(nameof(sortingOrder), sortingOrder);
            if (allowOcclusionWhenDynamic) jo.Add(nameof(allowOcclusionWhenDynamic), allowOcclusionWhenDynamic);
            jo.Add(nameof(shadowCastingMode), shadowCastingMode.ToString());
            if (receiveShadows) jo.Add(nameof(receiveShadows), receiveShadows);
            jo.Add(nameof(lightmapIndex), lightmapIndex);
            jo.Add(nameof(lightmapScaleOffset), lightmapScaleOffset.ToGltfVector4Raw().ToJArray());
            return new JProperty(ComponentName, jo);
        }
        public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
        {
            var renderer = component as Renderer;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
#if UNITY_2021_1_OR_NEWER
                        case nameof(staticShadowCaster):
                            renderer.staticShadowCaster = reader.ReadAsBoolean().Value;
                            break;
#endif
                        case nameof(reflectionProbeUsage):
                            renderer.reflectionProbeUsage = reader.ReadStringEnum<ReflectionProbeUsage>();
                            break;
                        case nameof(renderingLayerMask):
                            renderer.renderingLayerMask = (uint)reader.ReadAsDecimal().Value;
                            break;
                        case nameof(rendererPriority):
                            renderer.rendererPriority = reader.ReadAsInt32().Value;
                            break;
                        case nameof(sortingLayerName):
                            renderer.sortingLayerName = reader.ReadAsString();
                            break;
                        case nameof(sortingLayerID):
                            renderer.sortingLayerID = reader.ReadAsInt32().Value;
                            break;
                        case nameof(sortingOrder):
                            renderer.sortingOrder = reader.ReadAsInt32().Value;
                            break;
                        case nameof(allowOcclusionWhenDynamic):
                            renderer.allowOcclusionWhenDynamic = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(shadowCastingMode):
                            renderer.shadowCastingMode = reader.ReadStringEnum<ShadowCastingMode>();
                            break;
                        case nameof(receiveShadows):
                            renderer.receiveShadows = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(lightmapIndex):
                            renderer.lightmapIndex = reader.ReadAsInt32().Value;
                            break;
                        case nameof(lightmapScaleOffset):
                            renderer.lightmapScaleOffset = reader.ReadAsVector4().ToUnityVector4Raw();
                            break;
                        case nameof(isStatic):
                            renderer.gameObject.isStatic = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(tag):
                            renderer.gameObject.tag = reader.ReadAsString();
                            break;
                        case nameof(layer):
                            renderer.gameObject.layer = reader.ReadAsInt32().Value;
                            break;
                    }
                }
            }
        }
        public void SetData(Component component)
        {
            var renderer = component as Renderer;
            isStatic = renderer.gameObject.isStatic;
            tag = renderer.gameObject.tag;
            layer = renderer.gameObject.layer;
#if UNITY_2021_1_OR_NEWER
            staticShadowCaster = renderer.staticShadowCaster;
#endif
            reflectionProbeUsage = renderer.reflectionProbeUsage;
            renderingLayerMask = renderer.renderingLayerMask;
            rendererPriority = renderer.rendererPriority;
            sortingLayerName = renderer.sortingLayerName;
            sortingLayerID = renderer.sortingLayerID;
            sortingOrder = renderer.sortingOrder;
            allowOcclusionWhenDynamic = renderer.allowOcclusionWhenDynamic;
            shadowCastingMode = renderer.shadowCastingMode;
            receiveShadows = renderer.receiveShadows;
            lightmapIndex = renderer.lightmapIndex;
            lightmapScaleOffset = renderer.lightmapScaleOffset;
        }

        public object Clone()
        {
            return new BVA_Renderer_URP_Extra();
        }
    }
}