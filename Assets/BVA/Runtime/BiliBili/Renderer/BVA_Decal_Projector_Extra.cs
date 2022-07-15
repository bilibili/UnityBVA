#if UNITY_2021_1_OR_NEWER
using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using BVA.Extensions;
namespace GLTF.Schema.BVA
{
    public class BVA_Decal_Projector_Extra : IExtra
    {
        public const string PROPERTY = "DecalProjector";
        public MaterialId material;
        public float drawDistance;
        public float fadeScale;
        public float startAngleFade;
        public float endAngleFade;
        public float fadeFactor;
        public DecalScaleMode scaleMode;
        public Vector2 uvScale, uvBias;
        public Vector3 pivot, size;
        public BVA_Decal_Projector_Extra(DecalProjector decal, MaterialId materialId)
        {
            material = materialId;
            drawDistance = decal.drawDistance;
            fadeScale = decal.fadeScale;
            startAngleFade = decal.startAngleFade;
            endAngleFade = decal.endAngleFade;
            fadeFactor = decal.fadeFactor;
            scaleMode = decal.scaleMode;
            uvScale = decal.uvScale;
            uvBias = decal.uvBias;
            pivot = decal.pivot;
            size = decal.size;
        }

        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(material), material.Id);
            jo.Add(nameof(drawDistance), drawDistance);
            jo.Add(nameof(fadeScale), fadeScale);
            if (startAngleFade != 180.0f) jo.Add(nameof(startAngleFade), startAngleFade);
            if (endAngleFade != 180.0f) jo.Add(nameof(endAngleFade), endAngleFade);
            if (fadeFactor != 1.0f) jo.Add(nameof(fadeFactor), fadeFactor);
            jo.Add(nameof(scaleMode), scaleMode.ToString());
            if (uvScale != Vector2.one) jo.Add(nameof(uvScale), uvScale.ToGltfVector2Raw().ToJArray());
            if (uvBias != Vector2.zero) jo.Add(nameof(uvBias), uvBias.ToGltfVector2Raw().ToJArray());
            jo.Add(nameof(pivot), pivot.ToGltfVector3Raw().ToJArray());
            jo.Add(nameof(size), size.ToGltfVector3Raw().ToJArray());
            return new JProperty(PROPERTY, jo);
        }
        public static async Task Deserialize(GLTFRoot _gltfRoot, JsonReader reader, DecalProjector decal, AsyncLoadMaterial loadMaterial)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(material):
                            int materialIndex = reader.ReadAsInt32().Value;
                            decal.material = await loadMaterial(new MaterialId() { Id = materialIndex, Root = _gltfRoot });
                            break;
                        case nameof(drawDistance):
                            decal.drawDistance = reader.ReadAsFloat();
                            break;
                        case nameof(fadeScale):
                            decal.fadeScale = reader.ReadAsFloat();
                            break;
                        case nameof(startAngleFade):
                            decal.startAngleFade = reader.ReadAsFloat();
                            break;
                        case nameof(endAngleFade):
                            decal.endAngleFade = reader.ReadAsFloat();
                            break;
                        case nameof(fadeFactor):
                            decal.fadeFactor = reader.ReadAsFloat();
                            break;
                        case nameof(scaleMode):
                            decal.scaleMode = reader.ReadStringEnum<DecalScaleMode>();
                            break;
                        case nameof(uvScale):
                            decal.uvScale = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                        case nameof(uvBias):
                            decal.uvBias = reader.ReadAsVector2().ToUnityVector2Raw();
                            break;
                        case nameof(pivot):
                            decal.pivot = reader.ReadAsVector3().ToUnityVector3Raw();
                            break;
                        case nameof(size):
                            decal.size = reader.ReadAsVector3().ToUnityVector3Raw();
                            break;
                    }
                }
            }
        }
    }
}
#endif