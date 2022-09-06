using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GLTF.Schema.BVA
{
    [AsyncComponentExtra]
    public class BVA_Renderer_DecalProjector_Extra : IAsyncComponentExtra
    {
        public MaterialId defaultMaterial;
        public MaterialId material;
        public float drawDistance;
        public float fadeScale;
        public float startAngleFade;
        public float endAngleFade;
        public UnityEngine.Vector2 uvScale;
        public UnityEngine.Vector2 uvBias;
        public UnityEngine.Rendering.Universal.DecalScaleMode scaleMode;
        public UnityEngine.Vector3 pivot;
        public UnityEngine.Vector3 size;
        public float fadeFactor;
        public string ComponentName => ComponentType.Name;
        public string ExtraName => GetType().Name;
        public System.Type ComponentType => typeof(DecalProjector);
        public void SetData(Component component, ExportTexture exportTexture, ExportMaterial exportMaterial, ExportSprite exportSprite, ExportCubemap exportCubemap)
        {
            var target = component as DecalProjector;
            this.material = exportMaterial(target.material);
            this.drawDistance = target.drawDistance;
            this.fadeScale = target.fadeScale;
            this.startAngleFade = target.startAngleFade;
            this.endAngleFade = target.endAngleFade;
            this.uvScale = target.uvScale;
            this.uvBias = target.uvBias;
            this.scaleMode = target.scaleMode;
            this.pivot = target.pivot;
            this.size = target.size;
            this.fadeFactor = target.fadeFactor;
        }
        public async Task Deserialize(GLTFRoot root, JsonReader reader, Component component, AsyncLoadTexture loadTexture, AsyncLoadMaterial loadMaterial, AsyncLoadSprite loadSprite, AsyncLoadCubemap loadCubemap)
        {
            var target = component as DecalProjector;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(target.material):
                            int materialIndex = reader.ReadAsInt32().Value;
                            target.material = await loadMaterial(new MaterialId() { Id = materialIndex, Root = root });
                            break;
                        case nameof(target.drawDistance):
                            target.drawDistance = reader.ReadAsFloat();
                            break;
                        case nameof(target.fadeScale):
                            target.fadeScale = reader.ReadAsFloat();
                            break;
                        case nameof(target.startAngleFade):
                            target.startAngleFade = reader.ReadAsFloat();
                            break;
                        case nameof(target.endAngleFade):
                            target.endAngleFade = reader.ReadAsFloat();
                            break;
                        case nameof(target.uvScale):
                            target.uvScale = reader.ReadAsVector2();
                            break;
                        case nameof(target.uvBias):
                            target.uvBias = reader.ReadAsVector2();
                            break;
                        case nameof(target.scaleMode):
                            target.scaleMode = reader.ReadStringEnum<UnityEngine.Rendering.Universal.DecalScaleMode>();
                            break;
                        case nameof(target.pivot):
                            target.pivot = reader.ReadAsVector3();
                            break;
                        case nameof(target.size):
                            target.size = reader.ReadAsVector3();
                            break;
                        case nameof(target.fadeFactor):
                            target.fadeFactor = reader.ReadAsFloat();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            if (material != null) jo.Add(nameof(material), material.Id);
            jo.Add(nameof(drawDistance), drawDistance);
            jo.Add(nameof(fadeScale), fadeScale);
            jo.Add(nameof(startAngleFade), startAngleFade);
            jo.Add(nameof(endAngleFade), endAngleFade);
            jo.Add(nameof(uvScale), uvScale.ToJArray());
            jo.Add(nameof(uvBias), uvBias.ToJArray());
            jo.Add(nameof(scaleMode), scaleMode.ToString());
            jo.Add(nameof(pivot), pivot.ToJArray());
            jo.Add(nameof(size), size.ToJArray());
            jo.Add(nameof(fadeFactor), fadeFactor);
            return new JProperty(ComponentName, jo);
        }

        public object Clone()
        {
            return new BVA_Renderer_DecalProjector_Extra();
        }
    }
}
