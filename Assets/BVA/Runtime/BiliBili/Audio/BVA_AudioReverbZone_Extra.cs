using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;

namespace GLTF.Schema.BVA
{
    public class BVA_AudioReverbZone_Extra : IExtra
    {
        public const string PROPERTY = "BVA_AudioReverbZone_Extra";
        public float minDistance;
        public float maxDistance;
        public UnityEngine.AudioReverbPreset reverbPreset;
        public int room;
        public int roomHF;
        public int roomLF;
        public float decayTime;
        public float decayHFRatio;
        public int reflections;
        public float reflectionsDelay;
        public int reverb;
        public float reverbDelay;
        public float HFReference;
        public float LFReference;
        public float diffusion;
        public float density;
        public BVA_AudioReverbZone_Extra() { }

        public BVA_AudioReverbZone_Extra(UnityEngine.AudioReverbZone target)
        {
            this.minDistance = target.minDistance;
            this.maxDistance = target.maxDistance;
            this.reverbPreset = target.reverbPreset;
            this.room = target.room;
            this.roomHF = target.roomHF;
            this.roomLF = target.roomLF;
            this.decayTime = target.decayTime;
            this.decayHFRatio = target.decayHFRatio;
            this.reflections = target.reflections;
            this.reflectionsDelay = target.reflectionsDelay;
            this.reverb = target.reverb;
            this.reverbDelay = target.reverbDelay;
            this.HFReference = target.HFReference;
            this.LFReference = target.LFReference;
            this.diffusion = target.diffusion;
            this.density = target.density;
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.AudioReverbZone target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_AudioReverbZone_Extra.minDistance):
                            target.minDistance = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.maxDistance):
                            target.maxDistance = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.reverbPreset):
                            target.reverbPreset = reader.ReadStringEnum<UnityEngine.AudioReverbPreset>();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.room):
                            target.room = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.roomHF):
                            target.roomHF = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.roomLF):
                            target.roomLF = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.decayTime):
                            target.decayTime = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.decayHFRatio):
                            target.decayHFRatio = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.reflections):
                            target.reflections = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.reflectionsDelay):
                            target.reflectionsDelay = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.reverb):
                            target.reverb = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.reverbDelay):
                            target.reverbDelay = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.HFReference):
                            target.HFReference = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.LFReference):
                            target.LFReference = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.diffusion):
                            target.diffusion = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_AudioReverbZone_Extra.density):
                            target.density = reader.ReadAsFloat();
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(minDistance), minDistance);
            jo.Add(nameof(maxDistance), maxDistance);
            jo.Add(nameof(reverbPreset), reverbPreset.ToString());
            jo.Add(nameof(room), room);
            jo.Add(nameof(roomHF), roomHF);
            jo.Add(nameof(roomLF), roomLF);
            jo.Add(nameof(decayTime), decayTime);
            jo.Add(nameof(decayHFRatio), decayHFRatio);
            jo.Add(nameof(reflections), reflections);
            jo.Add(nameof(reflectionsDelay), reflectionsDelay);
            jo.Add(nameof(reverb), reverb);
            jo.Add(nameof(reverbDelay), reverbDelay);
            jo.Add(nameof(HFReference), HFReference);
            jo.Add(nameof(LFReference), LFReference);
            jo.Add(nameof(diffusion), diffusion);
            jo.Add(nameof(density), density);
            return new JProperty(BVA_AudioReverbZone_Extra.PROPERTY, jo);
        }
    }
}
