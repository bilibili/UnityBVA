using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using BVA;

namespace GLTF.Schema.BVA
{
    public class BVA_timeline_playableExtension : IExtension
    {
        public TrackAsset track;
        private NodeCache _cache;
        public BVA_timeline_playableExtension(TrackAsset track, NodeCache cache)
        {
            this.track = track;
            _cache = cache;
        }
        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_timeline_playableExtension(track, _cache);
        }

        public JProperty Serialize()
        {
            JProperty jProperty = new JProperty(BVA_timeline_playableExtensionFactory.EXTENSION_NAME, track.Serialize(_cache));
            return jProperty;
        }

        public static BVA_timeline_playableExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            var trackAsset = new TrackAsset();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(trackAsset.animationTrackGroup):
                        trackAsset.animationTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeAnimationTrack(root, reader));
                        break;
                    case nameof(trackAsset.audioTrackGroup):
                        trackAsset.audioTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeAudioTrack(root, reader));
                        break;
                    case nameof(trackAsset.blendShapeTrackGroup):
                        trackAsset.blendShapeTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeBlendShapeCurveTrack(root, reader));
                        break;
                    case nameof(trackAsset.materialCurveFloatTrackGroup):
                        trackAsset.materialCurveFloatTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeMaterialFloatCurveTrack(root, reader));
                        break;
                    case nameof(trackAsset.materialCurveColorTrackGroup):
                        trackAsset.materialCurveColorTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeMaterialColorCurveTrack(root, reader));
                        break;
                    case nameof(trackAsset.materialColorTrackGroup):
                        trackAsset.materialColorTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeMaterialColorTrack(root, reader));
                        break;
                    case nameof(trackAsset.materialVectorTrackGroup):
                        trackAsset.materialVectorTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeMaterialVectorTrack(root, reader));
                        break;
                    case nameof(trackAsset.materialFloatTrackGroup):
                        trackAsset.materialFloatTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeMaterialFloatTrack(root, reader));
                        break;
                    case nameof(trackAsset.materialIntTrackGroup):
                        trackAsset.materialIntTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeMaterialIntTrack(root, reader));
                        break;
                    case nameof(trackAsset.materialTextureTrackGroup):
                        trackAsset.materialTextureTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeMaterialTexture2DTrack(root, reader));
                        break;
                    case nameof(trackAsset.gameObjectActiveTrackGroup):
                        trackAsset.gameObjectActiveTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeGameObjectActiveTrack(root, reader));
                        break;
                    case nameof(trackAsset.componentEnableTrackGroup):
                        trackAsset.componentEnableTrackGroup.tracks = reader.ReadList(() => Playable.DeserializeComponentEnableTrack(root, reader));
                        break;
                }
            }

            return new BVA_timeline_playableExtension(trackAsset, null);
        }
    }

    public class BVA_timeline_playableExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_timeline_playable";
        public const string EXTENSION_ELEMENT_NAME = "playables";
        public PlayableProperty playable;
        public BVA_timeline_playableExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_timeline_playableExtensionFactory(PlayableProperty audioSourceProperty)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            playable = audioSourceProperty;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_timeline_playableExtensionFactory() { playable = playable };
        }

        public JProperty Serialize()
        {
            var pro = new JObject();
            pro.Add(new JProperty(nameof(playable.playable), playable.playable.Id));
            pro.Add(new JProperty(nameof(playable.playOnAwake), playable.playOnAwake));
            pro.Add(new JProperty(nameof(playable.loop), playable.loop));
            return new JProperty(EXTENSION_NAME, new JObject(new JProperty(nameof(playable), pro)));
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            playable = new PlayableProperty();
            if (extensionToken != null)
            {
                var playableToken = extensionToken.Value[nameof(playable.playable)];
                var collect = playableToken.Children();
                foreach (var v in collect)
                {
                    var jp = v as JProperty;
                    switch (jp.Name)
                    {
                        case nameof(playable.playable):
                            playable.playable = new PlayableId { Id = jp.Value.DeserializeAsInt(), Root = root };
                            break;
                        case nameof(playable.playOnAwake):
                            playable.playOnAwake = jp.Value.DeserializeAsBool();
                            break;
                        case nameof(playable.loop):
                            playable.loop = jp.Value.DeserializeAsBool();
                            break;
                    }
                }
            }
            return new BVA_timeline_playableExtensionFactory(playable);
        }
    }
}
