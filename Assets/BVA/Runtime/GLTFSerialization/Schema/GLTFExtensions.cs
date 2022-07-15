using GLTF.Extensions;
using GLTF.Schema.BVA;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GLTF.Schema
{
    public class GLTFExtensions : GLTFChildOfRootProperty
    {
        public List<KHR_lights_punctualExtension> Lights { private set; get; }
        public List<BVA_collisions_colliderExtension> Collisions { private set; get; }
        public List<BVA_humanoid_avatarExtension> Avatars { private set; get; }
        public List<BVA_humanoid_dressExtension> Dresses { private set; get; }
        public List<BVA_humanoid_accessoryExtension> Accessories { private set; get; }
        public List<BVA_audio_audioClipExtension> Audios { private set; get; }
        public List<BVA_timeline_playableExtension> Playables { private set; get; }
        public List<BVA_url_videoExtension> UrlVideos { private set; get; }
        public List<BVA_metaExtension> Metas { private set; get; }
        public List<BVA_postprocess_volumeExtension> PostProcessUrpVolumes { private set; get; }
        public List<BVA_blendShape_blendShapeMixerExtension> BlendShapeMixers { private set; get; }
        public List<BVA_physics_dynamicBoneExtension> DynamicBones { private set; get; }
        public List<BVA_skybox_sixSidedExtension> Skyboxs { private set; get; }
        public List<BVA_variable_collectionExtension> VariableCollections { private set; get; }
        public List<BVA_texture_cubemapExtension> Cubemaps { private set; get; }
        public List<BVA_light_lightmapExtension> Lightmaps { private set; get; }
        public List<BVA_setting_renderSettingExtension> RenderSettings { private set; get; }
        public List<BVA_ui_spriteExtension> Sprites { private set; get; }

#if PUERTS_INTEGRATION
        public List<BVA_puerts_monoExtension> PuertsEvents { private set; get; }
#endif

        public const string ANIMATOR_CLIP_EXTENSION_NAME = "BVA_humanoid_animationExtension";
        public const string ANIMATOR_CLIP_ELEMENT_NAME = "humanoidAnimationClips";
        /// <summary>
        /// For export humanoid motions
        /// </summary>
        public List<GLTFAnimation> HumanoidAnimationClips;

        public bool Used => Lights.Count + Collisions.Count + Avatars.Count + HumanoidAnimationClips.Count + Audios.Count + Playables.Count + UrlVideos.Count + Metas.Count + PostProcessUrpVolumes.Count + BlendShapeMixers.Count + DynamicBones.Count + Skyboxs.Count + VariableCollections.Count + Cubemaps.Count + Sprites.Count + Lightmaps.Count + RenderSettings.Count
#if PUERTS_INTEGRATION
+ PuertsEvents.Count 
#endif
> 0;
        public void AddLight(KHR_lights_punctualExtension light)
        {
            Lights.Add(light);
        }
        public void AddLightmap(BVA_light_lightmapExtension lightmap)
        {
            Lightmaps.Add(lightmap);
        }
        public void AddRenderSetting(BVA_setting_renderSettingExtension setting)
        {
            RenderSettings.Add(setting);
        }
        public void AddCollision(BVA_collisions_colliderExtension collision)
        {
            Collisions.Add(collision);
        }
        public void AddAvatar(BVA_humanoid_avatarExtension avatar)
        {
            Avatars.Add(avatar);
        }
        public void AddDress(BVA_humanoid_dressExtension dress)
        {
            Dresses.Add(dress);
        }
        public void AddAccessory(BVA_humanoid_accessoryExtension accessory)
        {
            Accessories.Add(accessory);
        }
        public void AddAudioClip(BVA_audio_audioClipExtension audio)
        {
            Audios.Add(audio);
        }
        public void AddUrlVideo(BVA_url_videoExtension video)
        {
            UrlVideos.Add(video);
        }
        public void AddPlayable(BVA_timeline_playableExtension audio)
        {
            Playables.Add(audio);
        }
        public void AddMeta(BVA_metaExtension meta)
        {
            Metas.Add(meta);
        }
        public void AddBlendShapeMixer(BVA_blendShape_blendShapeMixerExtension blendshapeMixer)
        {
            BlendShapeMixers.Add(blendshapeMixer);
        }
        public void AddPostProcess(BVA_postprocess_volumeExtension postprocess)
        {
            PostProcessUrpVolumes.Add(postprocess);
        }
        public void AddDynamicBone(BVA_physics_dynamicBoneExtension dyanmicBone)
        {
            DynamicBones.Add(dyanmicBone);
        }
        public void AddSkybox(BVA_skybox_sixSidedExtension skybox)
        {
            Skyboxs.Add(skybox);
        }
        public void AddCubemap(BVA_texture_cubemapExtension cubemap)
        {
            Cubemaps.Add(cubemap);
        }
        public void AddVariableCollection(BVA_variable_collectionExtension variables)
        {
            VariableCollections.Add(variables);
        }
        public void AddSprite(BVA_ui_spriteExtension sprite)
        {
            Sprites.Add(sprite);
        }

#if PUERTS_INTEGRATION
        public void AddPuertsEvent(BVA_puerts_monoExtension puertsEvent)
        {
            PuertsEvents.Add(puertsEvent);
        }
#endif
        public GLTFExtensions()
        {
            Lights = new List<KHR_lights_punctualExtension>();
            Collisions = new List<BVA_collisions_colliderExtension>();
            Avatars = new List<BVA_humanoid_avatarExtension>();
            Dresses = new List<BVA_humanoid_dressExtension>();
            Accessories = new List<BVA_humanoid_accessoryExtension>();
            Audios = new List<BVA_audio_audioClipExtension>();
            Playables = new List<BVA_timeline_playableExtension>();
            UrlVideos = new List<BVA_url_videoExtension>();
            Metas = new List<BVA_metaExtension>();
            PostProcessUrpVolumes = new List<BVA_postprocess_volumeExtension>();
            BlendShapeMixers = new List<BVA_blendShape_blendShapeMixerExtension>();
            DynamicBones = new List<BVA_physics_dynamicBoneExtension>();
            Skyboxs = new List<BVA_skybox_sixSidedExtension>();
            VariableCollections = new List<BVA_variable_collectionExtension>();
            Cubemaps = new List<BVA_texture_cubemapExtension>();
            Lightmaps = new List<BVA_light_lightmapExtension>();
            RenderSettings = new List<BVA_setting_renderSettingExtension>();
            Sprites = new List<BVA_ui_spriteExtension>();

#if PUERTS_INTEGRATION
            PuertsEvents = new List<BVA_puerts_monoExtension>();
#endif
            HumanoidAnimationClips = new List<GLTFAnimation>();
        }

        public GLTFExtensions(GLTFExtensions ext, GLTFRoot gltfRoot) : base(ext, gltfRoot)
        {
            if (ext == null) return;
            if (ext.Extensions != null)
            {
                Extensions = new Dictionary<string, IExtension>();
                foreach (var v in ext.Extensions)
                {
                    Extensions.Add(v.Key, v.Value.Clone(gltfRoot));
                }
            }
        }

        public override void Serialize(JsonWriter writer)
        {
            writer.WriteStartObject();

            if (Lights != null && Lights.Count > 0)
            {
                writer.WritePropertyName(KHR_lights_punctualExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(KHR_lights_punctualExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Lights)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Collisions != null && Collisions.Count > 0)
            {
                writer.WritePropertyName(BVA_collisions_colliderExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_collisions_colliderExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Collisions)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Avatars != null && Avatars.Count > 0)
            {
                writer.WritePropertyName(BVA_humanoid_avatarExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_humanoid_avatarExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Avatars)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Dresses != null && Dresses.Count > 0)
            {
                writer.WritePropertyName(BVA_humanoid_dressExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_humanoid_dressExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Dresses)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Accessories != null && Accessories.Count > 0)
            {
                writer.WritePropertyName(BVA_humanoid_accessoryExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_humanoid_accessoryExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Accessories)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Audios != null && Audios.Count > 0)
            {
                writer.WritePropertyName(BVA_audio_audioClipExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_audio_audioClipExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Audios)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Playables != null && Playables.Count > 0)
            {
                writer.WritePropertyName(BVA_timeline_playableExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_timeline_playableExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Playables)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (UrlVideos != null && UrlVideos.Count > 0)
            {
                writer.WritePropertyName(BVA_url_videoExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_url_videoExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in UrlVideos)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Metas != null && Metas.Count > 0)
            {
                writer.WritePropertyName(BVA_metaExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_metaExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Metas)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (PostProcessUrpVolumes != null && PostProcessUrpVolumes.Count > 0)
            {
                writer.WritePropertyName(BVA_postprocess_volumeExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_postprocess_volumeExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in PostProcessUrpVolumes)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (BlendShapeMixers != null && BlendShapeMixers.Count > 0)
            {
                writer.WritePropertyName(BVA_blendShape_blendShapeMixerExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_blendShape_blendShapeMixerExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in BlendShapeMixers)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (HumanoidAnimationClips != null && HumanoidAnimationClips.Count > 0 && HumanoidAnimationClips[0].Channels.Count > 0 && HumanoidAnimationClips[0].Samplers.Count > 0)
            {
                writer.WritePropertyName(ANIMATOR_CLIP_EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(ANIMATOR_CLIP_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var animation in HumanoidAnimationClips)
                {
                    animation.Serialize(writer);
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            if (DynamicBones != null && DynamicBones.Count > 0)
            {
                writer.WritePropertyName(BVA_physics_dynamicBoneExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_physics_dynamicBoneExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in DynamicBones)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Skyboxs != null && Skyboxs.Count > 0)
            {
                writer.WritePropertyName(BVA_skybox_sixSidedExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_skybox_sixSidedExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Skyboxs)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Cubemaps != null && Cubemaps.Count > 0)
            {
                writer.WritePropertyName(BVA_texture_cubemapExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_texture_cubemapExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Cubemaps)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (VariableCollections != null && VariableCollections.Count > 0)
            {
                writer.WritePropertyName(BVA_variable_collectionExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_variable_collectionExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in VariableCollections)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Lightmaps != null && Lightmaps.Count > 0)
            {
                writer.WritePropertyName(BVA_light_lightmapExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_light_lightmapExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Lightmaps)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (RenderSettings != null && RenderSettings.Count > 0)
            {
                writer.WritePropertyName(BVA_setting_renderSettingExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_setting_renderSettingExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in RenderSettings)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            if (Sprites != null && Sprites.Count > 0)
            {
                writer.WritePropertyName(BVA_ui_spriteExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_ui_spriteExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in Sprites)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

#if PUERTS_INTEGRATION
            if (PuertsEvents != null && PuertsEvents.Count > 0)
            {
                writer.WritePropertyName(BVA_puerts_monoExtensionFactory.EXTENSION_NAME);
                writer.WriteStartObject();
                writer.WritePropertyName(BVA_puerts_monoExtensionFactory.EXTENSION_ELEMENT_NAME);
                writer.WriteStartArray();
                foreach (var v in PuertsEvents)
                {
                    var value = v.Serialize().Value;
                    writer.WriteToken(value.CreateReader());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
#endif
            writer.WriteEndObject();
            base.Serialize(writer);
        }

        public static GLTFExtensions Deserialize(GLTFRoot root, JsonReader reader)
        {
            var ext = new GLTFExtensions();
            if (reader.Read() && reader.TokenType != JsonToken.StartObject)
            {
                throw new GLTFParseException("GLTF extension must be an object");
            }

            JObject extras = (JObject)JToken.ReadFrom(reader);

            foreach (JToken child in extras.Children())
            {
                if (child.Type != JTokenType.Property)
                {
                    throw new GLTFParseException("Children token of extensions should be properties");
                }
                var innerReader = child.CreateReader();
                innerReader.Read();
                var curProp = innerReader.Value.ToString();
                switch (curProp)
                {
                    case KHR_lights_punctualExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == KHR_lights_punctualExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Lights = innerReader.ReadList(() => KHR_lights_punctualExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_collisions_colliderExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_collisions_colliderExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Collisions = innerReader.ReadList(() => BVA_collisions_colliderExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_humanoid_avatarExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_humanoid_avatarExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Avatars = innerReader.ReadList(() => BVA_humanoid_avatarExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_humanoid_dressExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_humanoid_dressExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Dresses = innerReader.ReadList(() => BVA_humanoid_dressExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_humanoid_accessoryExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_humanoid_accessoryExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Accessories = innerReader.ReadList(() => BVA_humanoid_accessoryExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_audio_audioClipExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_audio_audioClipExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Audios = innerReader.ReadList(() => BVA_audio_audioClipExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_url_videoExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_url_videoExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.UrlVideos = innerReader.ReadList(() => BVA_url_videoExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_metaExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_metaExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Metas = innerReader.ReadList(() => BVA_metaExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_postprocess_volumeExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_postprocess_volumeExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.PostProcessUrpVolumes = innerReader.ReadList(() => BVA_postprocess_volumeExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_timeline_playableExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_timeline_playableExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Playables = innerReader.ReadList(() => BVA_timeline_playableExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_blendShape_blendShapeMixerExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_blendShape_blendShapeMixerExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.BlendShapeMixers = innerReader.ReadList(() => BVA_blendShape_blendShapeMixerExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case ANIMATOR_CLIP_EXTENSION_NAME:
                        if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                        {
                            var prop = innerReader.Value.ToString();
                            if (prop == ANIMATOR_CLIP_ELEMENT_NAME)
                            {
                                ext.HumanoidAnimationClips = innerReader.ReadList(() => GLTFAnimation.Deserialize(root, innerReader));
                            }
                        }
                        break;
                    case BVA_physics_dynamicBoneExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_physics_dynamicBoneExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.DynamicBones = innerReader.ReadList(() => BVA_physics_dynamicBoneExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_skybox_sixSidedExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_skybox_sixSidedExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Skyboxs = innerReader.ReadList(() => BVA_skybox_sixSidedExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_texture_cubemapExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_texture_cubemapExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Cubemaps = innerReader.ReadList(() => BVA_texture_cubemapExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_variable_collectionExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_variable_collectionExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.VariableCollections = innerReader.ReadList(() => BVA_variable_collectionExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_light_lightmapExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_light_lightmapExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Lightmaps = innerReader.ReadList(() => BVA_light_lightmapExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_setting_renderSettingExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_setting_renderSettingExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.RenderSettings = innerReader.ReadList(() => BVA_setting_renderSettingExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }
                    case BVA_ui_spriteExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_ui_spriteExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.Sprites = innerReader.ReadList(() => BVA_ui_spriteExtension.Deserialize(root, innerReader));
                                }
                            }
                            break;
                        }

#if PUERTS_INTEGRATION
                    case BVA_puerts_monoExtensionFactory.EXTENSION_NAME:
                        {
                            if (innerReader.Read() && innerReader.TokenType == JsonToken.StartObject && innerReader.Read() && innerReader.TokenType == JsonToken.PropertyName)
                            {
                                var prop = innerReader.Value.ToString();
                                if (prop == BVA_puerts_monoExtensionFactory.EXTENSION_ELEMENT_NAME)
                                {
                                    ext.PuertsEvents = innerReader.ReadList(() => BVA_puerts_monoExtension.Deserialize(root, innerReader));
                                }
                            }
                        }
                        break;
#endif
                }
            }
            return ext;
        }
    }
}