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
    public class BVA_LookAt_Extra : IComponentExtra
    {
        public bool InverseLeftEyeVerticalDirection;
        public bool InverseRightEyeVerticalDirection;
        public string ComponentName => ComponentType.Name;
        public string ExtraName => GetType().Name;
        public System.Type ComponentType => typeof(LookAt);
        public void SetData(Component component)
        {
            var target = component as LookAt;
            this.InverseLeftEyeVerticalDirection = target.InverseLeftEyeVerticalDirection;
            this.InverseRightEyeVerticalDirection = target.InverseRightEyeVerticalDirection;
        }
        public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
        {
            var target = component as LookAt;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(target.InverseLeftEyeVerticalDirection):
                            target.InverseLeftEyeVerticalDirection = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(target.InverseRightEyeVerticalDirection):
                            target.InverseRightEyeVerticalDirection = reader.ReadAsBoolean().Value;
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(InverseLeftEyeVerticalDirection), InverseLeftEyeVerticalDirection);
            jo.Add(nameof(InverseRightEyeVerticalDirection), InverseRightEyeVerticalDirection);
            return new JProperty(ComponentName, jo);
        }

        public object Clone()
        {
            return new BVA_LookAt_Extra();
        }
    }
}
