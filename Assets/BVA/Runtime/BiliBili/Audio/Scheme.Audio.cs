using UnityEngine;
using GLTF.Extensions;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema.BVA
{
    public class AudioAsset : GLTFProperty
    {
        public string name;
        public string uri;
        public string mimeType;
        public BufferViewId bufferView;
        public int lengthSamples;
        public int channels;
        public int frequency;
    }
    public class AudioSourceProperty
    {
        public AudioId audio;
        public bool playOnAwake;
        public bool loop;
        public float volume;
        public float pitch;
        public float panStereo;
        public float spatialBlend;
        public AudioRolloffMode rolloffMode;
        public float dopplerLevel;
        public float spread;
        public float minDistance;
        public float maxDistance;
        public AudioSourceProperty()
        {
            playOnAwake = true;
            volume = 1.0f;
            rolloffMode = AudioRolloffMode.Logarithmic;
            dopplerLevel = 1.0f;
            spread = 0.0f;
            minDistance = 1.0f;
            maxDistance = 500f;
        }
        public AudioSourceProperty(AudioSource audioSource)
        {
            playOnAwake = audioSource.playOnAwake;
            loop = audioSource.loop;
            volume = audioSource.volume;
            pitch = audioSource.pitch;
            panStereo = audioSource.panStereo;
            spatialBlend = audioSource.spatialBlend;
            rolloffMode = audioSource.rolloffMode;
            dopplerLevel = audioSource.dopplerLevel;
            spread = audioSource.spread;
            minDistance = audioSource.minDistance;
            maxDistance = audioSource.maxDistance;
        }
        public void Deserialize(AudioSource audioSource)
        {
            audioSource.playOnAwake = playOnAwake;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.panStereo = panStereo;
            audioSource.spatialBlend = spatialBlend;
            audioSource.rolloffMode = rolloffMode;
            audioSource.dopplerLevel = dopplerLevel;
            audioSource.spread = spread;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
        }
        public static AudioSourceProperty Deserialize(GLTFRoot root, JEnumerable<JToken> collect)
        {
            AudioSourceProperty pro = new AudioSourceProperty();
            foreach (var v in collect)
            {
                var jp = v as JProperty;
                switch (jp.Name)
                {
                    case nameof(pro.audio):
                        pro.audio = new AudioId() { Id = jp.Value.DeserializeAsInt(), Root = root };
                        break;
                    case nameof(pro.playOnAwake):
                        pro.playOnAwake = jp.Value.DeserializeAsBool();
                        break;
                    case nameof(pro.loop):
                        pro.loop = jp.Value.DeserializeAsBool();
                        break;
                    case nameof(pro.volume):
                        pro.volume = jp.Value.DeserializeAsFloat();
                        break;
                    case nameof(pro.pitch):
                        pro.pitch = jp.Value.DeserializeAsFloat();
                        break;
                    case nameof(pro.panStereo):
                        pro.panStereo = jp.Value.DeserializeAsFloat();
                        break;
                    case nameof(pro.spatialBlend):
                        pro.spatialBlend = jp.Value.DeserializeAsFloat();
                        break;
                    case nameof(pro.rolloffMode):
                        pro.rolloffMode = jp.Value.DeserializeAsEnum<AudioRolloffMode>();
                        break;
                    case nameof(pro.dopplerLevel):
                        pro.dopplerLevel = jp.Value.DeserializeAsFloat();
                        break;
                    case nameof(pro.spread):
                        pro.spread = jp.Value.DeserializeAsFloat();
                        break;
                    case nameof(pro.minDistance):
                        pro.minDistance = jp.Value.DeserializeAsFloat();
                        break;
                    case nameof(pro.maxDistance):
                        pro.maxDistance = jp.Value.DeserializeAsFloat();
                        break;
                }
            }
            return pro;
        }
        public JObject Serialize()
        {
            JObject pro = new JObject();
            if (audio != null) pro.Add(new JProperty(nameof(audio), audio.Id));
            pro.Add(new JProperty(nameof(playOnAwake), playOnAwake));
            pro.Add(new JProperty(nameof(loop), loop));
            pro.Add(new JProperty(nameof(volume), volume));
            pro.Add(new JProperty(nameof(pitch), pitch));
            pro.Add(new JProperty(nameof(panStereo), panStereo));
            pro.Add(new JProperty(nameof(spatialBlend), spatialBlend));
            pro.Add(new JProperty(nameof(rolloffMode), rolloffMode.ToString()));
            pro.Add(new JProperty(nameof(dopplerLevel), dopplerLevel));
            pro.Add(new JProperty(nameof(spread), spread));
            pro.Add(new JProperty(nameof(minDistance), minDistance));
            pro.Add(new JProperty(nameof(maxDistance), maxDistance));
            return pro;
        }
    }
}