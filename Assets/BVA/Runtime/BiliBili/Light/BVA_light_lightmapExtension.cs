using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace GLTF.Schema.BVA
{
    public struct LightmapTextureInfo
    {
        public TextureId lightmapColor;
        public TextureId lightmapDir;
        public TextureId shadowMask;
    }
    public enum LightmapsEncoding
    {
        RGBM,
        DLDR,
        HDR,
    }
    public class BVA_light_lightmapExtension : IExtension
    {
        public const string LIGHTMAP_COLOR = nameof(LightmapTextureInfo.lightmapColor);
        public const string LIGHTMAP_DIR = nameof(LightmapTextureInfo.lightmapDir);
        public const string LIGHTMAP_SHADOWMASK = nameof(LightmapTextureInfo.shadowMask);
        public LightmapsMode lightmapsMode;
        public LightmapTextureInfo[] lightmaps;
        public LightmapsEncoding lightmapsEncoding;

        public BVA_light_lightmapExtension(LightmapsMode mode, LightmapTextureInfo[] lightmaps, LightmapsEncoding encoding)
        {
            lightmapsMode = mode;
            this.lightmaps = lightmaps;
            lightmapsEncoding = encoding;
            //lightmapEncoding = Shader.IsKeywordEnabled("UNITY_LIGHTMAP_FULL_HDR")?  LightmapEncoding.RGBM_HDR : LightmapEncoding.None;
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_light_lightmapExtension(lightmapsMode, lightmaps, LightmapsEncoding.RGBM);
        }

        public JProperty Serialize()
        {
            JObject propObj = new JObject();
            propObj.Add(nameof(lightmapsMode), lightmapsMode == LightmapsMode.NonDirectional ? nameof(LightmapsMode.NonDirectional) : nameof(LightmapsMode.CombinedDirectional));
            propObj.Add(nameof(lightmapsEncoding), lightmapsEncoding.ToString());
            JArray jarray = new JArray();
            foreach (var lightmap in lightmaps)
            {
                JObject lightmapObj = new JObject();
                if (lightmap.lightmapColor != null) lightmapObj.Add(LIGHTMAP_COLOR, lightmap.lightmapColor.Id);
                if (lightmap.lightmapDir != null) lightmapObj.Add(LIGHTMAP_DIR, lightmap.lightmapDir.Id);
                if (lightmap.shadowMask != null) lightmapObj.Add(LIGHTMAP_SHADOWMASK, lightmap.shadowMask.Id);
                jarray.Add(lightmapObj);
            }
            propObj.Add(nameof(lightmaps), jarray);

            JProperty jProperty = new JProperty(BVA_light_lightmapExtensionFactory.EXTENSION_NAME, propObj);

            return jProperty;
        }

        public static LightmapTextureInfo DeserializeLightmaps(GLTFRoot root, JsonReader reader)
        {
            var lightmap = new LightmapTextureInfo();

            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case LIGHTMAP_COLOR:
                        lightmap.lightmapColor = new TextureId() { Id = reader.ReadAsInt32().Value, Root = root };
                        break;
                    case LIGHTMAP_DIR:
                        lightmap.lightmapDir = new TextureId() { Id = reader.ReadAsInt32().Value, Root = root };
                        break;
                    case LIGHTMAP_SHADOWMASK:
                        lightmap.shadowMask = new TextureId() { Id = reader.ReadAsInt32().Value, Root = root };
                        break;
                }
            }

            return lightmap;
        }

        public static BVA_light_lightmapExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            LightmapsMode lightmapsMode = LightmapsMode.NonDirectional;
            List<LightmapTextureInfo> lightmaps = null;
            LightmapsEncoding lightmapsEncoding = LightmapsEncoding.RGBM;
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(BVA_light_lightmapExtension.lightmapsMode):
                        lightmapsMode = reader.ReadStringEnum<LightmapsMode>();
                        break;
                    case nameof(BVA_light_lightmapExtension.lightmaps):
                        lightmaps = reader.ReadList(() => DeserializeLightmaps(root, reader));
                        break;
                    case nameof(lightmapsEncoding):
                        lightmapsEncoding = reader.ReadStringEnum<LightmapsEncoding>();
                        break;
                }
            }
            return new BVA_light_lightmapExtension(lightmapsMode, lightmaps.ToArray(), lightmapsEncoding);
        }
    }

    public class BVA_light_lightmapExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_light_lightmapExtension";
        public const string EXTENSION_ELEMENT_NAME = "lightmaps";
        public BVA_light_lightmapExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }

        public BVA_light_lightmapExtensionFactory(LightmapId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public LightmapId id;
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_light_lightmapExtensionFactory(id);
        }

        public JProperty Serialize()
        {
            JProperty node = new JProperty(EXTENSION_ELEMENT_NAME, id.Id);
            return new JProperty(EXTENSION_NAME, new JObject(node));
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            if (extensionToken != null)
            {
                JToken indexToken = extensionToken.Value[EXTENSION_ELEMENT_NAME];
                int _id = indexToken != null ? indexToken.DeserializeAsInt() : -1;
                id = new LightmapId() { Id = _id, Root = root };
            }
            return new BVA_light_lightmapExtensionFactory(id);
        }
    }
}