using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    [ComponentExtra]
    public class BVA_MirrorPlane_Extra : IComponentExtra
    {
        public int TextureResolution;
        public float HeightOffset;
        public string ReflectionTextureName;
        public string ComponentName => ComponentType.Name;
        public System.Type ComponentType => typeof(MirrorPlane);
        public void SetData(Component component)
        {
            var target = component as MirrorPlane;
            this.TextureResolution = target.TextureResolution;
            this.HeightOffset = target.HeightOffset;
            this.ReflectionTextureName = target.ReflectionTextureName;
        }
        public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
        {
            var target = component as MirrorPlane;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(target.TextureResolution):
                            target.TextureResolution = reader.ReadAsInt32().Value;
                            break;
                        case nameof(target.HeightOffset):
                            target.HeightOffset = reader.ReadAsFloat();
                            break;
                        case nameof(target.ReflectionTextureName):
                            target.ReflectionTextureName = reader.ReadAsString();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(TextureResolution), TextureResolution);
            jo.Add(nameof(HeightOffset), HeightOffset);
            jo.Add(nameof(ReflectionTextureName), ReflectionTextureName);
            return new JProperty(ComponentName, jo);
        }

        public object Clone()
        {
            return new BVA_MirrorPlane_Extra();
        }
    }
}
