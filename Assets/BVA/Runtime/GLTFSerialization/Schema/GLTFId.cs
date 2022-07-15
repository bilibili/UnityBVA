using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema
{

    /// <summary>
    /// Abstract class that stores a reference to the root GLTF object and an id
    /// of an object of type T inside it.
    /// </summary>
    /// <typeparam name="T">The value type returned by the GLTFId reference.</typeparam>
    public abstract class GLTFId<T>
    {
        public int Id;
        public GLTFRoot Root;
        public abstract T Value { get; }

        protected GLTFId()
        {
        }

        public GLTFId(GLTFId<T> gltfId, GLTFRoot newRoot)
        {
            Id = gltfId.Id;
            Root = newRoot;
        }

        public void Serialize(JsonWriter writer)
        {
            writer.WriteValue(Id);
        }
    }

    public class AccessorId : GLTFId<Accessor>
    {
        public AccessorId()
        {
        }

        public AccessorId(AccessorId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override Accessor Value
        {
            get { return Root.Accessors[Id]; }
        }

        public static AccessorId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new AccessorId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class BufferId : GLTFId<GLTFBuffer>
    {
        public BufferId()
        {
        }

        public BufferId(BufferId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override GLTFBuffer Value
        {
            get { return Root.Buffers[Id]; }
        }

        public static BufferId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new BufferId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class BufferViewId : GLTFId<BufferView>
    {
        public BufferViewId()
        {
        }

        public BufferViewId(BufferViewId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override BufferView Value
        {
            get { return Root.BufferViews[Id]; }
        }

        public static BufferViewId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new BufferViewId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class CameraId : GLTFId<GLTFCamera>
    {
        public CameraId()
        {
        }

        public CameraId(CameraId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override GLTFCamera Value
        {
            get { return Root.Cameras[Id]; }
        }

        public static CameraId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new CameraId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class ImageId : GLTFId<GLTFImage>
    {
        public ImageId()
        {
        }

        public ImageId(ImageId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }


        public override GLTFImage Value
        {
            get { return Root.Images[Id]; }
        }

        public static ImageId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new ImageId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class MaterialId : GLTFId<GLTFMaterial>
    {
        public MaterialId()
        {
        }

        public MaterialId(MaterialId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override GLTFMaterial Value
        {
            get { return Root.Materials[Id]; }
        }

        public static MaterialId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new MaterialId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class MeshId : GLTFId<GLTFMesh>
    {
        public MeshId()
        {
        }

        public MeshId(MeshId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override GLTFMesh Value
        {
            get { return Root.Meshes[Id]; }
        }

        public static MeshId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new MeshId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class NodeId : GLTFId<Node>
    {
        public NodeId()
        {
        }

        public NodeId(NodeId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override Node Value
        {
            get { return Root.Nodes[Id]; }
        }

        public static NodeId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new NodeId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }

        public static List<NodeId> ReadList(GLTFRoot root, JsonReader reader)
        {
            if (reader.Read() && reader.TokenType != JsonToken.StartArray)
            {
                throw new Exception("Invalid array.");
            }

            var list = new List<NodeId>();

            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                var node = new NodeId
                {
                    Id = int.Parse(reader.Value.ToString()),
                    Root = root
                };

                list.Add(node);
            }

            return list;
        }
    }

    public class SamplerId : GLTFId<Sampler>
    {
        public SamplerId()
        {
        }

        public SamplerId(SamplerId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override Sampler Value
        {
            get { return Root.Samplers[Id]; }
        }

        public static SamplerId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new SamplerId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class SceneId : GLTFId<GLTFScene>
    {
        public SceneId()
        {
        }

        public SceneId(SceneId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }


        public override GLTFScene Value
        {
            get { return Root.Scenes[Id]; }
        }

        public static SceneId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new SceneId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class SkinId : GLTFId<Skin>
    {
        public SkinId()
        {
        }

        public SkinId(SkinId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override Skin Value
        {
            get { return Root.Skins[Id]; }
        }

        public static SkinId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new SkinId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    public class TextureId : GLTFId<GLTFTexture>
    {
        public TextureId()
        {
        }

        public TextureId(TextureId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override GLTFTexture Value
        {
            get { return Root.Textures[Id]; }
        }

        public static TextureId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new TextureId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
        public static TextureId Deserialize(GLTFRoot root, JProperty jProperty)
        {
            return new TextureId
            {
                Id = (int)jProperty.Value,
                Root = root
            };
        }
    }

    public class LightId : GLTFId<KHR_lights_punctualExtension>
    {
        public LightId()
        {
        }

        public LightId(LightId id, GLTFRoot newRoot) : base(id, newRoot)
        {
        }

        public override KHR_lights_punctualExtension Value
        {
            get { return Root.Extensions.Lights[Id]; }
        }

        public static LightId Deserialize(GLTFRoot root, JsonReader reader)
        {
            return new LightId
            {
                Id = reader.ReadAsInt32().Value,
                Root = root
            };
        }
    }

    namespace BVA
    {
        public class CollisionId : GLTFId<BVA_collisions_colliderExtension>
        {
            public CollisionId()
            {
            }

            public CollisionId(CollisionId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_collisions_colliderExtension Value
            {
                get { return Root.Extensions.Collisions[Id]; }
            }

            public static CollisionId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new CollisionId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class AudioId : GLTFId<BVA_audio_audioClipExtension>
        {
            public AudioId()
            {
            }

            public AudioId(AudioId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_audio_audioClipExtension Value
            {
                get { return Root.Extensions.Audios[Id]; }
            }

            public static AudioId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new AudioId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class AnimatorClipId : GLTFId<GLTFAnimation>
        {
            public AnimatorClipId()
            {
            }

            public AnimatorClipId(AnimatorClipId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override GLTFAnimation Value
            {
                get { return Root.Extensions.HumanoidAnimationClips[Id]; }
            }

            public static AnimatorClipId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new AnimatorClipId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class AvatarId : GLTFId<BVA_humanoid_avatarExtension>
        {
            public AvatarId() { }
            public AvatarId(AvatarId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_humanoid_avatarExtension Value
            {
                get { return Root.Extensions.Avatars[Id]; }
            }

            public static AvatarId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new AvatarId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class DressId : GLTFId<BVA_humanoid_dressExtension>
        {
            public DressId() { }
            public DressId(DressId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_humanoid_dressExtension Value
            {
                get { return Root.Extensions.Dresses[Id]; }
            }

            public static DressId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new DressId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class AccessoryId : GLTFId<BVA_humanoid_accessoryExtension>
        {
            public AccessoryId() { }
            public AccessoryId(AccessoryId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_humanoid_accessoryExtension Value
            {
                get { return Root.Extensions.Accessories[Id]; }
            }

            public static AccessoryId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new AccessoryId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class LightmapId : GLTFId<BVA_light_lightmapExtension>
        {
            public LightmapId() { }
            public LightmapId(LightmapId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_light_lightmapExtension Value
            {
                get { return Root.Extensions.Lightmaps[Id]; }
            }

            public static LightmapId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new LightmapId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }

        public class RenderSettingId : GLTFId<BVA_setting_renderSettingExtension>
        {
            public RenderSettingId() { }
            public RenderSettingId(RenderSettingId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_setting_renderSettingExtension Value
            {
                get { return Root.Extensions.RenderSettings[Id]; }
            }

            public static RenderSettingId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new RenderSettingId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class PlayableId : GLTFId<BVA_timeline_playableExtension>
        {
            public PlayableId()
            {
            }

            public PlayableId(PlayableId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_timeline_playableExtension Value
            {
                get { return Root.Extensions.Playables[Id]; }
            }

            public static PlayableId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new PlayableId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }

        public class UrlVideoId : GLTFId<BVA_url_videoExtension>
        {
            public UrlVideoId()
            {
            }

            public UrlVideoId(UrlVideoId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_url_videoExtension Value
            {
                get { return Root.Extensions.UrlVideos[Id]; }
            }

            public static UrlVideoId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new UrlVideoId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class MetaId : GLTFId<BVA_metaExtension>
        {
            public MetaId()
            {
            }

            public MetaId(MetaId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_metaExtension Value
            {
                get { return Root.Extensions.Metas[Id]; }
            }

            public static MetaId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new MetaId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class BlendshapeMixerId : GLTFId<BVA_blendShape_blendShapeMixerExtension>
        {
            public BlendshapeMixerId()
            {
            }

            public BlendshapeMixerId(BlendshapeMixerId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_blendShape_blendShapeMixerExtension Value
            {
                get { return Root.Extensions.BlendShapeMixers[Id]; }
            }

            public static BlendshapeMixerId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new BlendshapeMixerId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
        public class PostProcessId : GLTFId<BVA_postprocess_volumeExtension>
        {
            public PostProcessId()
            {
            }

            public PostProcessId(PostProcessId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_postprocess_volumeExtension Value
            {
                get { return Root.Extensions.PostProcessUrpVolumes[Id]; }
            }

            public static PostProcessId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new PostProcessId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }

        public class CubemapId : GLTFId<BVA_texture_cubemapExtension>
        {
            public CubemapId()
            {
            }

            public CubemapId(CubemapId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_texture_cubemapExtension Value
            {
                get { return Root.Extensions.Cubemaps[Id]; }
            }

            public static CubemapId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new CubemapId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
            public static CubemapId Deserialize(GLTFRoot root, JProperty jProperty)
            {
                return new CubemapId
                {
                    Id = (int)jProperty.Value,
                    Root = root
                };
            }
        }

        public class SpriteId : GLTFId<BVA_ui_spriteExtension>
        {
            public SpriteId()
            {
            }

            public SpriteId(SpriteId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_ui_spriteExtension Value
            {
                get { return Root.Extensions.Sprites[Id]; }
            }

            public static SpriteId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new SpriteId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
            public static SpriteId Deserialize(GLTFRoot root, JProperty jProperty)
            {
                return new SpriteId
                {
                    Id = (int)jProperty.Value,
                    Root = root
                };
            }
        }
        public class VaribleCollectionId : GLTFId<BVA_variable_collectionExtension>
        {
            public VaribleCollectionId()
            {
            }

            public VaribleCollectionId(VaribleCollectionId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_variable_collectionExtension Value
            {
                get { return Root.Extensions.VariableCollections[Id]; }
            }

            public static VaribleCollectionId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new VaribleCollectionId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }

        public class PuertsMonoId : GLTFId<BVA_variable_collectionExtension>
        {
            public PuertsMonoId()
            {
            }

            public PuertsMonoId(PuertsMonoId id, GLTFRoot newRoot) : base(id, newRoot)
            {
            }

            public override BVA_variable_collectionExtension Value
            {
                get { return Root.Extensions.VariableCollections[Id]; }
            }

            public static PuertsMonoId Deserialize(GLTFRoot root, JsonReader reader)
            {
                return new PuertsMonoId
                {
                    Id = reader.ReadAsInt32().Value,
                    Root = root
                };
            }
        }
    }
}
