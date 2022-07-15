using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GLTF.Schema.BVA
{
    public class BVA_UI_RawImage_Extra : IExtra
    {
        public const string PROPERTY = "BVA_UI_RawImage_Extra";
        public TextureId texture;
        public BVA_UI_RawImage_Extra() { }

        public BVA_UI_RawImage_Extra(UnityEngine.UI.RawImage target, TextureId textureId = null)
        {
            this.texture = textureId;
        }
        public static async Task Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.UI.RawImage target, AsyncLoadTexture loadTexture = null, AsyncLoadMaterial loadMaterial = null, AsyncLoadSprite loadSprite = null)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_UI_RawImage_Extra.texture):
                            int textureIndex = reader.ReadAsInt32().Value;
                            target.texture = await loadTexture(new TextureId() { Id = textureIndex, Root = root });
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            if (texture != null) jo.Add(nameof(texture), texture.Id);
            return new JProperty(BVA_UI_RawImage_Extra.PROPERTY, jo);
        }
    }
}
