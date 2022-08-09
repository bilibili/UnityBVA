using UnityEngine;
using System.Threading.Tasks;
using GLTF.Schema.BVA;
using System.IO;
using BVA.Component;
using OggVorbis;
using NAudio.Wave;
#if UNITY_EDITOR
using UnityEditor;
#endif

// All audio clips export as ogg at runtime, others will export original format
namespace BVA
{
    public partial class GLTFSceneImporter
    {
        private static AudioFormat GetAudioFormat(string audioMime)
        {
            if (audioMime == "audio/ogg")
                return AudioFormat.OGG;
            if (audioMime == "audio/mp3")
                return AudioFormat.MP3;
            return AudioFormat.WAV;
        }
        public async Task LoadAudio(GameObject sceneObj)
        {
            if (_gltfRoot.Extensions.Audios.Count == 0)
                return;
            _assetManager.AddContainer(sceneObj.AddComponent<AudioClipContainer>());

            foreach (var audio in _gltfRoot.Extensions.Audios)
            {
                AudioAsset gltfAudio = audio.audio;
                if (gltfAudio.bufferView != null)
                    await ConstructAudio(gltfAudio);
            }
        }

        protected async Task ConstructAudio(AudioAsset audio)
        {
            var bufferView = audio.bufferView.Value;
            var bufferContents = await GetBufferData(bufferView.Buffer);
            bufferContents.Stream.Position = bufferView.ByteOffset + bufferContents.ChunkOffset;
            var audioFormat = GetAudioFormat(audio.mimeType);
            switch (audioFormat)
            {
                case AudioFormat.WAV:
                    {
                        int sampleLength = (int)bufferView.ByteLength / sizeof(float);
                        float[] sampleData = new float[sampleLength];

                        BinaryReader br = new BinaryReader(bufferContents.Stream);
                        for (int i = 0; i < sampleLength; i++)
                        {
                            sampleData[i] = br.ReadSingle();
                        }

                        AssetManager.audioClipContainer.AddWavAudio(audio.name, sampleData, audio.lengthSamples, audio.channels, audio.frequency);
                    }
                    break;
                case AudioFormat.OGG:
                    {
                        byte[] content = new byte[bufferView.ByteLength];
                        bufferContents.Stream.Read(content, 0, (int)bufferView.ByteLength);
                        AssetManager.audioClipContainer.AddOggAudio(audio.name, content);
                    }
                    break;
                case AudioFormat.MP3:
                    {
                        byte[] content = new byte[bufferView.ByteLength];
                        bufferContents.Stream.Read(content, 0, (int)bufferView.ByteLength);
                        await AssetManager.audioClipContainer.AddMp3Audio(audio.name, content);
                    }
                    break;
            }
        }

        public void ImportAudio(AudioSourceProperty property, GameObject nodeObj)
        {
            var audioSource = nodeObj.AddComponent<AudioSource>();
            property.Deserialize(audioSource);
            audioSource.clip = AssetManager.audioClipContainer.Get(property.audio.Id);
        }
    }

    public partial class GLTFSceneExporter
    {
        public string GetExportPath(AudioClip clip)
        {
#if UNITY_EDITOR
            //export to Asset/... the path in Project
            var pathInProject = Path.ChangeExtension(AssetDatabase.GetAssetPath(clip), ".ogg");
            return pathInProject;
#else
            //export to Audio/,if name is conflict,rename it with (num)
            var exportWithoutExt = $"Audios/{clip.name}";
            var exportPath = $"{exportWithoutExt}.ogg";
            if (File.Exists(clip.name))
            {
                for (int i = 0; i < 1000; i++)
                {
                    var validName = $"{exportWithoutExt}({i}).ogg";
                    if (!File.Exists(exportWithoutExt))
                    {
                        return validName;
                    }
                }
            }
            return null;
#endif
        }

        private static string GetAudioMime(AudioFormat audioType)
        {
            return "audio/" + audioType.ToString().ToLower();
        }

        private AudioId ExportAudioExternalBuffer(AudioClip clip, AudioFormat audioType)
        {
            if (clip == null)
            {
                throw new System.Exception("audio can not be NULL.");
            }
            var url = GetExportPath(clip);
            AudioAsset audio = new AudioAsset() { name = clip.name, uri = url, mimeType = GetAudioMime(audioType) };

            BVA_audio_audioClipExtension ext = new BVA_audio_audioClipExtension(audio);
            var id = new AudioId
            {
                Id = _root.Extensions.Audios.Count,
                Root = _root
            };
            _root.Extensions.AddAudioClip(ext);
            _audios.Add(new AudioInfo() { audio = clip, url = url });
            return id;
        }

        private AudioId ExportAudioInternalBuffer(AudioClip clip)
        {
            if (clip == null)
            {
                return null;
            }
            var index = _audios.FindIndex((x) => x.audio == clip);
            if (index >= 0)
            {
                return new AudioId
                {
                    Id = index,
                    Root = _root
                };
            }
            AudioFormat audioFormat = GetAudioExportFormat(clip);
            bool exportOrigialFile = audioFormat == AudioFormat.MP3;

            AudioAsset audio = new AudioAsset() { name = clip.name, mimeType = GetAudioMime(audioFormat), channels = clip.channels, frequency = clip.frequency, lengthSamples = clip.samples };

            var byteOffset = _bufferWriter.BaseStream.Position;

#if UNITY_EDITOR
            if (exportOrigialFile)
            {
                var bytes = File.ReadAllBytes(AssetDatabase.GetAssetPath(clip));
                _bufferWriter.Write(bytes);
            }
            else
#endif
                try
                {
                    switch (audioFormat)
                    {
                        case AudioFormat.WAV:
                            {
                                var samples = new float[clip.samples * clip.channels];
                                clip.GetData(samples, 0);

                                foreach (var v in samples)
                                    _bufferWriter.Write(v);
                                break;
                            }
                        //case AudioFormat.MP3: //need fix runtime compression
                        //    {
                        //        WaveUtil.SaveToMP3File(clip, "E:\\output.mp3");

                        //        break;
                        //    }
                        case AudioFormat.OGG:
                            {
                                var bytes = VorbisPlugin.GetOggVorbis(clip, ExportAudioQuality);
                                _bufferWriter.Write(bytes);
                                break;
                            }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                    return null;
                }

            var byteLength = _bufferWriter.BaseStream.Position - byteOffset;
            byteLength = AppendToBufferMultiplyOf4(byteOffset, byteLength);

            audio.bufferView = ExportBufferView((uint)byteOffset, (uint)byteLength);

            var id = new AudioId
            {
                Id = _root.Extensions.Audios.Count,
                Root = _root
            };
            _root.Extensions.AddAudioClip(new BVA_audio_audioClipExtension(audio));
            _audios.Add(new AudioInfo() { audio = clip, url = null });
            _root.Extensions.AddExtension(_root, BVA_audio_audioClipExtensionFactory.EXTENSION_NAME, null, RequireExtensions);
            return id;
        }

        private void ExportAudios(string outputPath)
        {
            foreach (var v in _audios)
            {
                var absolutePath = outputPath + "/" + v.url;
                var file = new FileInfo(absolutePath);
                file.Directory.Create();
                LogPool.ExportLogger.Log(LogPart.Audio, $"save audio file {absolutePath}");
                VorbisPlugin.Save(absolutePath, v.audio);
            }
        }

        private AudioFormat GetAudioExportFormat(AudioClip clip)
        {
#if UNITY_EDITOR
            var assetPath = AssetDatabase.GetAssetPath(clip);
            var fileExtLower = assetPath.ToLower();
            if (fileExtLower.EndsWith(".mp3"))
                return AudioFormat.MP3;
            else if (fileExtLower.EndsWith(".ogg"))
                return AudioFormat.OGG;
            if (clip.length > ExportOggAudioClipLength)
                return AudioFormat.OGG;
            else
                return AudioFormat.WAV;
#else
                return AudioFormat.OGG;
#endif
        }

        /// <summary>
        /// export AudioSource
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private AudioId ExportAudioSource(AudioSource source)
        {
            var id = ExportAudioInternalBuffer(source.clip);
            return id;
        }

        /// <summary>
        /// Export audioClip in AudioClipContainer
        /// </summary>
        /// <param name="container"></param>
        private void ExportContainer(AudioClipContainer container)
        {
            if (!container.isActiveAndEnabled) return;
            container.RemoveInvalidOrNull();
            if (container.audioClips != null && container.audioClips.Count > 0)
            {
                foreach (var clip in container.audioClips)
                {
                    ExportAudioInternalBuffer(clip);
                }
            }
        }
    }
}