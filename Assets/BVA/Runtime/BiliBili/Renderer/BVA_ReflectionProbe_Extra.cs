using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;

namespace GLTF.Schema.BVA
{
    public class BVA_ReflectionProbe_Extra : IExtra
    {
        public const string PROPERTY = "ReflectionProbe";
        public UnityEngine.Vector3 size;
        public UnityEngine.Vector3 center;
        public float nearClipPlane;
        public float farClipPlane;
        public float intensity;
        public bool hdr;
        public bool renderDynamicObjects;
        public float shadowDistance;
        public int resolution;
        public int cullingMask;
        public UnityEngine.Rendering.ReflectionProbeClearFlags clearFlags;
        public UnityEngine.Color backgroundColor;
        public float blendDistance;
        public bool boxProjection;
        public UnityEngine.Rendering.ReflectionProbeMode mode;
        public int importance;
        public UnityEngine.Rendering.ReflectionProbeRefreshMode refreshMode;
        public UnityEngine.Rendering.ReflectionProbeTimeSlicingMode timeSlicingMode;
        public CubemapId customBakedTexture;
        public BVA_ReflectionProbe_Extra() { }

        public BVA_ReflectionProbe_Extra(UnityEngine.ReflectionProbe target, CubemapId textureId = null)
        {
            this.size = target.size;
            this.center = target.center;
            this.nearClipPlane = target.nearClipPlane;
            this.farClipPlane = target.farClipPlane;
            this.intensity = target.intensity;
            this.hdr = target.hdr;
            this.renderDynamicObjects = target.renderDynamicObjects;
            this.shadowDistance = target.shadowDistance;
            this.resolution = target.resolution;
            this.cullingMask = target.cullingMask;
            this.clearFlags = target.clearFlags;
            this.backgroundColor = target.backgroundColor;
            this.blendDistance = target.blendDistance;
            this.boxProjection = target.boxProjection;
            this.mode = target.mode;// even baked, export as custom
            this.importance = target.importance;
            this.refreshMode = target.refreshMode;
            this.timeSlicingMode = target.timeSlicingMode;
            this.customBakedTexture = textureId;
        }
        public static async Task Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.ReflectionProbe target, AsyncLoadCubemap loadFunction)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_ReflectionProbe_Extra.size):
                            target.size = reader.ReadAsVector3().ToUnityVector3Raw();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.center):
                            target.center = reader.ReadAsVector3().ToUnityVector3Raw();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.nearClipPlane):
                            target.nearClipPlane = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.farClipPlane):
                            target.farClipPlane = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.intensity):
                            target.intensity = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.hdr):
                            target.hdr = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.renderDynamicObjects):
                            target.renderDynamicObjects = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.shadowDistance):
                            target.shadowDistance = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.resolution):
                            target.resolution = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.cullingMask):
                            target.cullingMask = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.clearFlags):
                            target.clearFlags = reader.ReadStringEnum<UnityEngine.Rendering.ReflectionProbeClearFlags>();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.backgroundColor):
                            target.backgroundColor = reader.ReadAsRGBAColor().ToUnityColorRaw();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.blendDistance):
                            target.blendDistance = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.boxProjection):
                            target.boxProjection = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.mode):
                            target.mode = reader.ReadStringEnum<UnityEngine.Rendering.ReflectionProbeMode>();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.importance):
                            target.importance = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.refreshMode):
                            target.refreshMode = reader.ReadStringEnum<UnityEngine.Rendering.ReflectionProbeRefreshMode>();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.timeSlicingMode):
                            target.timeSlicingMode = reader.ReadStringEnum<UnityEngine.Rendering.ReflectionProbeTimeSlicingMode>();
                            break;
                        case nameof(BVA_ReflectionProbe_Extra.customBakedTexture):
                            int textureIndex = reader.ReadAsInt32().Value;
                            if( target.mode == UnityEngine.Rendering.ReflectionProbeMode.Baked)
                                target.bakedTexture = await loadFunction(new CubemapId() { Id = textureIndex, Root = root });
                            else
                                target.customBakedTexture = await loadFunction(new CubemapId() { Id = textureIndex, Root = root });
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(size), size.ToGltfVector3Raw().ToJArray());
            jo.Add(nameof(center), center.ToGltfVector3Raw().ToJArray());
            jo.Add(nameof(nearClipPlane), nearClipPlane);
            jo.Add(nameof(farClipPlane), farClipPlane);
            jo.Add(nameof(intensity), intensity);
            jo.Add(nameof(hdr), hdr);
            jo.Add(nameof(renderDynamicObjects), renderDynamicObjects);
            jo.Add(nameof(shadowDistance), shadowDistance);
            jo.Add(nameof(resolution), resolution);
            jo.Add(nameof(cullingMask), cullingMask);
            jo.Add(nameof(clearFlags), clearFlags.ToString());
            jo.Add(nameof(backgroundColor), backgroundColor.ToNumericsColorRaw().ToJArray());
            jo.Add(nameof(blendDistance), blendDistance);
            jo.Add(nameof(boxProjection), boxProjection);
            jo.Add(nameof(mode), mode.ToString());
            jo.Add(nameof(importance), importance);
            jo.Add(nameof(refreshMode), refreshMode.ToString());
            jo.Add(nameof(timeSlicingMode), timeSlicingMode.ToString());
            if (customBakedTexture != null) jo.Add(nameof(customBakedTexture), customBakedTexture.Id);
            return new JProperty(BVA_ReflectionProbe_Extra.PROPERTY, jo);
        }
    }
}
