using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Video;
using BVA;

namespace GLTF.Schema.BVA
{
    public class BVA_url_videoExtension : IExtension
    {
        public VideoUrlAsset asset;
        public BVA_url_videoExtension(VideoUrlAsset asset)
        {
            this.asset = asset;
        }
        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_url_videoExtension(asset);
        }

        public JProperty Serialize()
        {
            JProperty jProperty = new JProperty(BVA_url_videoExtensionFactory.EXTENSION_NAME, asset.Serialize());
            return jProperty;
        }

        public static BVA_url_videoExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            var asset = new VideoUrlAsset();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(asset.name):
                        asset.name = reader.ReadAsString();
                        break;
                    case nameof(asset.url):
                        asset.url = reader.ReadAsString();
                        break;
                    case nameof(asset.mimeType):
                        asset.mimeType = reader.ReadAsString();
                        break;
                    case nameof(asset.alternate):
                        asset.alternate = reader.ReadStringList().ToArray();
                        break;
                }
            }
            return new BVA_url_videoExtension(asset);
        }
    }
    public class BVA_url_videoExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_url_video";
        public const string EXTENSION_ELEMENT_NAME = "videos";
        public VideoPlayerProperty video;
        public BVA_url_videoExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_url_videoExtensionFactory(VideoPlayerProperty videoPlayerProperty)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            video = videoPlayerProperty;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_url_videoExtensionFactory() { video = video };
        }

        public JProperty Serialize()
        {
            var pro = new JObject();
            //pro.Add(new JProperty(nameof(video.video), video.video.Id));
            pro.Add(new JProperty(nameof(video.playOnAwake), video.playOnAwake));
            pro.Add(new JProperty(nameof(video.waitForFirstFrame), video.waitForFirstFrame));
            pro.Add(new JProperty(nameof(video.loop), video.loop));
            pro.Add(new JProperty(nameof(video.playbackSpeed), video.playbackSpeed));
            pro.Add(new JProperty(nameof(video.renderer), video.renderer));
            pro.Add(new JProperty(nameof(video.url), video.url));
            pro.Add(new JProperty(nameof(video.propertyName), video.propertyName));
            pro.Add(new JProperty(nameof(video.audioOutputMode), video.audioOutputMode.ToString()));
            JArray audioSources = new JArray();
            JArray audioSourceProperties = new JArray();
            foreach (var v in video.audioSourceProperties)
                audioSourceProperties.Add(v.Serialize());
            pro.Add(new JProperty(nameof(video.audioSourceProperties), audioSourceProperties));
            return new JProperty(EXTENSION_NAME, new JObject(new JProperty(nameof(video), pro)));
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            video = new VideoPlayerProperty();
            if (extensionToken != null)
            {
                var playableToken = extensionToken.Value[nameof(video)];
                var collect = playableToken.Children();
                foreach (var v in collect)
                {
                    var jp = v as JProperty;
                    switch (jp.Name)
                    {
                        /*case nameof(video.video):
                            video.video = new UrlVideoId { Id = jp.Value.DeserializeAsInt(), Root = root };
                            break;*/
                        case nameof(video.playOnAwake):
                            video.playOnAwake = jp.Value.DeserializeAsBool();
                            break;
                        case nameof(video.loop):
                            video.loop = jp.Value.DeserializeAsBool();
                            break;
                        case nameof(video.waitForFirstFrame):
                            video.waitForFirstFrame = jp.Value.DeserializeAsBool();
                            break;
                        case nameof(video.playbackSpeed):
                            video.playbackSpeed = jp.Value.DeserializeAsFloat();
                            break;
                        case nameof(video.renderer):
                            video.renderer = jp.Value.DeserializeAsInt();
                            break;
                        case nameof(video.url):
                            video.url = jp.Value.DeserializeAsString();
                            break;
                        case nameof(video.propertyName):
                            video.propertyName = jp.Value.DeserializeAsString();
                            break;
                        case nameof(video.audioOutputMode):
                            video.audioOutputMode = jp.Value.DeserializeAsEnum<VideoAudioOutputMode>();
                            break;
                        case nameof(video.audioSourceProperties):
                            video.audioSourceProperties = jp.Value.DeserializeAsList((x) => AudioSourceProperty.Deserialize(root, x));
                            break;
                    }
                }
            }
            return new BVA_url_videoExtensionFactory(video);
        }
    }
}