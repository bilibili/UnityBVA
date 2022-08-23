using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BVA.Component;
using System;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    [ComponentExtra]
    public class BVA_MirrorObject_Extra : IComponentExtra
    {
        public int RenderTextureSize;
        public BVA_MirrorObject_Extra() { }

        public BVA_MirrorObject_Extra(MirrorObject target)
        {
        }

        public string ComponentName =>ComponentType.Name;

        public Type ComponentType => typeof(MirrorObject);

        public object Clone()
        {
            return new BVA_MirrorObject_Extra();
        }

        public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
        {
            var target = component as MirrorObject;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_MirrorObject_Extra.RenderTextureSize):
                            target.RenderTextureSize = reader.ReadAsInt32().Value;
                            break;
                    }
                }
            }
        }

        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(RenderTextureSize), RenderTextureSize);
            return new JProperty(ComponentName, jo);
        }

        public void SetData(Component component)
        {
            var target = component as MirrorObject;
            this.RenderTextureSize = target.RenderTextureSize;
        }
    }
}
