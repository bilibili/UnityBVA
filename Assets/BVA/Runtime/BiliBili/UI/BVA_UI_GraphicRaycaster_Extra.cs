using Newtonsoft.Json.Linq;
using GLTF.Math;
using GLTF.Schema;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector4 = UnityEngine.Vector4;

namespace GLTF.Schema.BVA
{
    public class BVA_UI_GraphicRaycaster_Extra : IExtra
    {
        public const string PROPERTY = "BVA_UI_GraphicRaycaster_Extra";
        public bool ignoreReversedGraphics;
        public UnityEngine.UI.GraphicRaycaster.BlockingObjects blockingObjects;
        public BVA_UI_GraphicRaycaster_Extra() { }

        public BVA_UI_GraphicRaycaster_Extra(UnityEngine.UI.GraphicRaycaster target)
        {
            this.ignoreReversedGraphics = target.ignoreReversedGraphics;
            this.blockingObjects = target.blockingObjects;
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.UI.GraphicRaycaster target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_UI_GraphicRaycaster_Extra.ignoreReversedGraphics):
                            target.ignoreReversedGraphics = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_UI_GraphicRaycaster_Extra.blockingObjects):
                            target.blockingObjects = reader.ReadStringEnum<UnityEngine.UI.GraphicRaycaster.BlockingObjects>();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(ignoreReversedGraphics), ignoreReversedGraphics);
            jo.Add(nameof(blockingObjects), blockingObjects.ToString());
            return new JProperty(BVA_UI_GraphicRaycaster_Extra.PROPERTY, jo);
        }
    }
}
