using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GLTF.Schema.BVA;

namespace GLTF.Schema
{
    public class GLTFProperty
    {
        private static Dictionary<string, ExtensionFactory> _extensionRegistry = new Dictionary<string, ExtensionFactory>()
        {
            { KHR_texture_transformExtensionFactory.EXTENSION_NAME, new KHR_texture_transformExtensionFactory() },
            { KHR_texture_basisuExtensionFactory.EXTENSION_NAME, new KHR_texture_basisuExtensionFactory() },
            { KHR_materials_emissive_strengthExtensionFactory.EXTENSION_NAME, new KHR_materials_emissive_strengthExtensionFactory() },
            { KHR_materials_pbrSpecularGlossinessExtensionFactory.EXTENSION_NAME, new KHR_materials_pbrSpecularGlossinessExtensionFactory() },
            { KHR_materials_clearcoatExtensionFactory.EXTENSION_NAME, new KHR_materials_clearcoatExtensionFactory() },
            { KHR_materials_unlitExtensionFactory.EXTENSION_NAME, new KHR_materials_unlitExtensionFactory() },
            { KHR_lights_punctualExtensionFactory.EXTENSION_NAME, new KHR_lights_punctualExtensionFactory() },
            { KHR_draco_mesh_compression_ExtensionFactory.EXTENSION_NAME, new KHR_draco_mesh_compression_ExtensionFactory() },
            { MSFT_LODExtensionFactory.EXTENSION_NAME, new MSFT_LODExtensionFactory() },
            { BVA_collisions_colliderExtensionFactory.EXTENSION_NAME, new BVA_collisions_colliderExtensionFactory() },
            { BVA_humanoid_avatarExtensionFactory.EXTENSION_NAME, new BVA_humanoid_avatarExtensionFactory() },
            { BVA_humanoid_accessoryExtensionFactory.EXTENSION_NAME, new BVA_humanoid_accessoryExtensionFactory() },
            { BVA_humanoid_dressExtensionFactory.EXTENSION_NAME, new BVA_humanoid_dressExtensionFactory() },
            { BVA_audio_audioClipExtensionFactory.EXTENSION_NAME, new BVA_audio_audioClipExtensionFactory() },
            { BVA_timeline_playableExtensionFactory.EXTENSION_NAME, new BVA_timeline_playableExtensionFactory() },
            { BVA_url_videoExtensionFactory.EXTENSION_NAME, new BVA_url_videoExtensionFactory() },
            { BVA_postprocess_volumeExtensionFactory.EXTENSION_NAME, new BVA_postprocess_volumeExtensionFactory() },
            { BVA_metaExtensionFactory.EXTENSION_NAME, new BVA_metaExtensionFactory() },
            { BVA_blendShape_blendShapeMixerExtensionFactory.EXTENSION_NAME, new BVA_blendShape_blendShapeMixerExtensionFactory() },
            { BVA_skybox_sixSidedExtensionFactory.EXTENSION_NAME, new BVA_skybox_sixSidedExtensionFactory() },
            { BVA_texture_cubemapExtensionFactory.EXTENSION_NAME, new BVA_texture_cubemapExtensionFactory() },
            { BVA_light_lightmapExtensionFactory.EXTENSION_NAME, new BVA_light_lightmapExtensionFactory() },
            { BVA_setting_renderSettingExtensionFactory.EXTENSION_NAME, new BVA_setting_renderSettingExtensionFactory() },
            { BVA_variable_collectionExtensionFactory.EXTENSION_NAME, new BVA_variable_collectionExtensionFactory() },
            { BVA_physics_dynamicBoneExtensionFactory.EXTENSION_NAME, new BVA_physics_dynamicBoneExtensionFactory() },
            { BVA_ui_spriteExtensionFactory.EXTENSION_NAME, new BVA_ui_spriteExtensionFactory() },

#if PUERTS_INTEGRATION
            { BVA_puerts_monoExtensionFactory.EXTENSION_NAME, new BVA_puerts_monoExtensionFactory() },
#endif
        };
        private static DefaultExtensionFactory _defaultExtensionFactory = new DefaultExtensionFactory();

        public static bool IsExtensionRegistered(string extensionName)
        {
            lock (_extensionRegistry)
            {
                return _extensionRegistry.ContainsKey(extensionName);
            }
        }

        public static void RegisterExtension(ExtensionFactory extensionFactory)
        {
            lock (_extensionRegistry)
            {
                _extensionRegistry[extensionFactory.ExtensionName] = extensionFactory;
            }
        }

        public static ExtensionFactory TryGetExtension(string extensionName)
        {
            lock (_extensionRegistry)
            {
                ExtensionFactory result;
                if (_extensionRegistry.TryGetValue(extensionName, out result))
                {
                    return result;
                }
                return null;
            }
        }

        public static bool TryRegisterExtension(ExtensionFactory extensionFactory)
        {
            lock (_extensionRegistry)
            {
                if (_extensionRegistry.ContainsKey(extensionFactory.ExtensionName))
                {
                    return false;
                }
                _extensionRegistry.Add(extensionFactory.ExtensionName, extensionFactory);
                return true;
            }
        }

        public Dictionary<string, IExtension> Extensions;
        public Dictionary<string, IExtra> Descriptors;
        public List<JToken> Extras;
        public void AddExtra(string extraName, IExtra extra)
        {
            if (Descriptors == null)
                Descriptors = new Dictionary<string, IExtra>();
            if (extra == null) throw new ArgumentNullException("Extra can't be null");
            if(!Descriptors.ContainsKey(extraName))
                Descriptors.Add(extraName, extra);
        }
        public void AddExtension(GLTFRoot _root, string extensionName, IExtension ext, bool RequireExtensions)
        {
            if (Extensions == null)
                Extensions = new Dictionary<string, IExtension>();

            if (_root.ExtensionsUsed == null)
            {
                _root.ExtensionsUsed = new List<string>(new[] { extensionName });
            }
            else if (!_root.ExtensionsUsed.Contains(extensionName))
            {
                _root.ExtensionsUsed.Add(extensionName);
            }

            if (RequireExtensions)
            {
                if (_root.ExtensionsRequired == null)
                {
                    _root.ExtensionsRequired = new List<string>(new[] { extensionName });
                }
                else if (!_root.ExtensionsRequired.Contains(extensionName))
                {
                    _root.ExtensionsRequired.Add(extensionName);
                }
            }
            if (ext != null)
                Extensions.Add(extensionName, ext);
        }
        public void CreateExtension()
        {
        }
        public GLTFProperty()
        {
        }

        public GLTFProperty(GLTFProperty property, GLTFRoot gltfRoot = null)
        {
            if (property == null) return;

            if (property.Extensions != null)
            {
                Extensions = new Dictionary<string, IExtension>(property.Extensions.Count);
                foreach (KeyValuePair<string, IExtension> extensionKeyValuePair in property.Extensions)
                {
                    Extensions.Add(extensionKeyValuePair.Key, extensionKeyValuePair.Value.Clone(gltfRoot));
                }
            }

            if (property.Extras != null)
            {
                Extras = new List<JToken>();
                foreach (var v in property.Extras)
                {
                    Extras.Add(v.DeepClone());
                }
            }
        }

        public void DefaultPropertyDeserializer(GLTFRoot root, JsonReader reader)
        {
            switch (reader.Value.ToString())
            {
                case "extensions":
                    Extensions = DeserializeExtensions(root, reader);
                    break;
                case "extras":
                    if (reader.Read() && reader.TokenType != JsonToken.StartObject)
                    {
                        throw new GLTFParseException("GLTF extra must be an object");
                    }

                    JObject extras = (JObject)JToken.ReadFrom(reader);
                    Extras = new List<JToken>();

                    foreach (JToken child in extras.Children())
                    {
                        if (child.Type != JTokenType.Property)
                        {
                            throw new GLTFParseException("Children token of extensions should be properties");
                        }

                        Extras.Add(child);
                    }
                    break;
                default:
                    SkipValue(reader);
                    break;
            }
        }

        private void SkipValue(JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new Exception("No value found.");
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                SkipObject(reader);
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                SkipArray(reader);
            }
        }

        private void SkipObject(JsonReader reader)
        {
            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                if (reader.TokenType == JsonToken.StartArray)
                {
                    SkipArray(reader);
                }
                else if (reader.TokenType == JsonToken.StartObject)
                {
                    SkipObject(reader);
                }
            }
        }

        private void SkipArray(JsonReader reader)
        {
            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                if (reader.TokenType == JsonToken.StartArray)
                {
                    SkipArray(reader);
                }
                else if (reader.TokenType == JsonToken.StartObject)
                {
                    SkipObject(reader);
                }
            }
        }

        private Dictionary<string, IExtension> DeserializeExtensions(GLTFRoot root, JsonReader reader)
        {
            if (reader.Read() && reader.TokenType != JsonToken.StartObject)
            {
                throw new GLTFParseException("GLTF extensions must be an object");
            }

            JObject extensions = (JObject)JToken.ReadFrom(reader);
            var extensionsCollection = new Dictionary<string, IExtension>();

            foreach (JToken child in extensions.Children())
            {
                if (child.Type != JTokenType.Property)
                {
                    throw new GLTFParseException("Children token of extensions should be properties");
                }

                JProperty childAsJProperty = (JProperty)child;
                string extensionName = childAsJProperty.Name;
                ExtensionFactory extensionFactory;

                lock (_extensionRegistry)
                {
                    if (!_extensionRegistry.TryGetValue(extensionName, out extensionFactory))
                    {
                        extensionFactory = _defaultExtensionFactory;
                    }
                }

                extensionsCollection.Add(extensionName, extensionFactory.Deserialize(root, childAsJProperty));
            }

            return extensionsCollection;
        }

        public virtual void Serialize(JsonWriter writer)
        {
            if (Extensions != null && Extensions.Count > 0)
            {
                writer.WritePropertyName("extensions");
                writer.WriteStartObject();
                foreach (var extension in Extensions)
                {
                    JToken extensionToken = extension.Value.Serialize();
                    extensionToken.WriteTo(writer);
                }
                writer.WriteEndObject();
            }

            if (Descriptors != null)
            {
                writer.WritePropertyName("extras");
                writer.WriteStartObject();
                foreach (var v in Descriptors)
                {
                    v.Value.Serialize().WriteTo(writer);
                }
                writer.WriteEndObject();
            }
        }
    }
}
