using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GLTF.Schema.BVA
{
    public class BVA_audio_audioClipExtension : IExtension
    {
        public AudioAsset audio;

        public BVA_audio_audioClipExtension(AudioAsset audio)
        {
            this.audio = audio;
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_audio_audioClipExtension(audio);
        }

        public JProperty Serialize()
        {
            JObject propObj = new JObject();
            if (audio.name != null)
                propObj.Add(nameof(audio.name), audio.name);
            if (audio.uri != null)
                propObj.Add(nameof(audio.uri), audio.uri);
            if (audio.mimeType != null)
                propObj.Add(nameof(audio.mimeType), audio.mimeType);
            if (audio.bufferView != null)
                propObj.Add(nameof(audio.bufferView), audio.bufferView.Id);
            if (audio.channels != 0)
                propObj.Add(nameof(audio.channels), audio.channels);
            if (audio.frequency != 0)
                propObj.Add(nameof(audio.frequency), audio.frequency);
            if (audio.lengthSamples != 0)
                propObj.Add(nameof(audio.lengthSamples), audio.lengthSamples);
            JProperty jProperty = new JProperty(BVA_audio_audioClipExtensionFactory.EXTENSION_NAME, propObj);

            return jProperty;
        }

        public static BVA_audio_audioClipExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            string name = null;
            string uri = null;
            string mimeType = null;
            int bufferView = -1;
            int channels = 0;
            int lengthSamples = 0;
            int frequency = 0;
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(name):
                        name = reader.ReadAsString();
                        break;
                    case nameof(uri):
                        uri = reader.ReadAsString();
                        break;
                    case nameof(mimeType):
                        mimeType = reader.ReadAsString();
                        break;
                    case nameof(bufferView):
                        bufferView = reader.ReadAsInt32().Value;
                        break;
                    case nameof(channels):
                        channels = reader.ReadAsInt32().Value;
                        break;
                    case nameof(frequency):
                        frequency = reader.ReadAsInt32().Value;
                        break;
                    case nameof(lengthSamples):
                        lengthSamples = reader.ReadAsInt32().Value;
                        break;
                }
            }

            return new BVA_audio_audioClipExtension(new AudioAsset()
            {
                name = name,
                uri = uri,
                mimeType = mimeType,
                bufferView = bufferView >= 0 ? new BufferViewId() { Id = bufferView, Root = root } : null,
                channels = channels,
                lengthSamples = lengthSamples,
                frequency = frequency
            });
        }
    }

    public class BVA_audio_audioClipExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_audio_audioClip";
        public const string EXTENSION_ELEMENT_NAME = "audios";
        public List<AudioSourceProperty> components;
        public BVA_audio_audioClipExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_audio_audioClipExtensionFactory(List<AudioSourceProperty> audioSourceProperty)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            components = audioSourceProperty;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_audio_audioClipExtensionFactory() { components = components };
        }

        public JProperty Serialize()
        {
            var array = new JArray();
            foreach (var v in components)
            {
                array.Add(v.Serialize());
            }
            return new JProperty(EXTENSION_NAME, new JObject(new JProperty(EXTENSION_ELEMENT_NAME, array)));
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {

            components = new List<AudioSourceProperty>();
            if (extensionToken != null)
            {
                JToken token = extensionToken.Value[EXTENSION_ELEMENT_NAME];
                JArray vectorArray = token as JArray;
                if (vectorArray == null)
                {
                    throw new System.Exception("JToken used was not a JArray. It was a " + token.Type.ToString());
                }

                foreach (var extToken in vectorArray)
                {
                    var collect = extToken.Children();
                    AudioSourceProperty pro = AudioSourceProperty.Deserialize(root, collect);
                    components.Add(pro);
                }
            }
            return new BVA_audio_audioClipExtensionFactory(components);
        }
    }
}
