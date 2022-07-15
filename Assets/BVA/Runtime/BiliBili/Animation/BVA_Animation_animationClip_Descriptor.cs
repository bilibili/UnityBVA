using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using System.Collections.Generic;

namespace GLTF.Schema.BVA
{

    public class BVA_Animation_animationClip_Descriptor : IExtra
    {
        public const string NAME = "BVA_Animation_animationClip";

        public List<int> node;
        public UnityEngine.WrapMode wrapMode;
        public bool legacy;
        public static BVA_Animation_animationClip_Descriptor Deserialize(GLTFRoot _gltfRoot, JsonReader reader)
        {
            BVA_Animation_animationClip_Descriptor desc = new BVA_Animation_animationClip_Descriptor();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(node):
                            desc.node = reader.ReadInt32List();
                            break;
                        case nameof(wrapMode):
                            desc.wrapMode = reader.ReadStringEnum<UnityEngine.WrapMode>();
                            break;
                        case nameof(legacy):
                            desc.legacy = reader.ReadAsBoolean().Value;
                            break;
                    }
                }
            }
            return desc;
        }
        public JProperty Serialize()
        {
            var nodeArray = new JArray();
            foreach (var v in node)
                nodeArray.Add(v);
            return new JProperty(NAME, new JObject(
                new JProperty(nameof(node), nodeArray),
               new JProperty(nameof(wrapMode), wrapMode.ToString()),
               new JProperty(nameof(legacy), legacy)));
        }
    }
}