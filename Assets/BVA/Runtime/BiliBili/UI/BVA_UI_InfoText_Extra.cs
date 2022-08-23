using Newtonsoft.Json.Linq;
using GLTF.Math;
using GLTF.Schema;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using BVA.Component;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector4 = UnityEngine.Vector4;

namespace GLTF.Schema.BVA
{
    [ComponentExtra]
    public class BVA_UI_InfoText_Extra : IComponentExtra
    {
        public string info;
        public string ComponentName => ComponentType.Name;
        public string ExtraName => GetType().Name;
        public System.Type ComponentType => typeof(InfoText);
        public void SetData(Component component)
        {
            var target = component as InfoText;
            this.info = target.info;
        }
        public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
        {
            var target = component as InfoText;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(target.info):
                            target.info = reader.ReadAsString();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(info), info);
            return new JProperty(ComponentName, jo);
        }

        public object Clone()
        {
            return new BVA_UI_InfoText_Extra();
        }
    }
}
