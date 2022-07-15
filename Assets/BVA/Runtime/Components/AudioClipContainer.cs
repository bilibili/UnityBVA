using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using GLTF.Schema.BVA;
using OggVorbis;
using NAudio.Wave;
using NLayer.NAudioSupport;
using System.Threading.Tasks;

namespace BVA.Component
{
    public class AudioClipContainer : MonoBehaviour, IContainer<AudioClip>
    {
        public List<AudioClip> audioClips;
        protected int currentFile;
        protected AudioClip currentAudioClip;
        public Dictionary<string, AudioClip> unloadedFiles;

        protected void Start()
        {
            if (unloadedFiles != null)
            {
                foreach (var v in unloadedFiles)
                    StartCoroutine(GetAudioClip(v.Key));
            }
        }
        public bool hasAudio => audioClips != null && audioClips.Count > 0;
        public void AddUrl(string url)
        {
            if (unloadedFiles == null)
                unloadedFiles = new Dictionary<string, AudioClip>();

            unloadedFiles.Add(url, null);
        }

        /// <summary>
        /// wav file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="wavData"></param>
        /// <param name="length"></param>
        /// <param name="channels"></param>
        /// <param name="frequency"></param>
        public void AddWavAudio(string name, float[] wavData, int length, int channels, int frequency)
        {
            var clip = AudioClip.Create(name, length, channels, frequency, false);
            clip.SetData(wavData, 0);
            if (audioClips == null) audioClips = new List<AudioClip>();
            audioClips.Add(clip);
        }
        public async Task AddMp3Audio(string name, byte[] mp3Data)
        {
            try
            {
                var (waveHeader, wavData) = await WaveUtil.ToWaveData(mp3Data);
                AddWavAudio(name, wavData, waveHeader.DataChunkSize / waveHeader.BlockSize, waveHeader.Channel, waveHeader.SampleRate);
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }
        }
        /// <summary>
        /// Ogg file might not show audio curve on inspector, but it is playable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vorbisData"></param>
        public void AddOggAudio(string name, byte[] vorbisData)
        {
            var clip = VorbisPlugin.ToAudioClip(vorbisData, name);
            if (audioClips == null) audioClips = new List<AudioClip>();
            audioClips.Add(clip);
        }

        protected AudioType GetAudioType(string extensoin)
        {
            if (extensoin == ".ogg")
                return AudioType.OGGVORBIS;
            if (extensoin == ".mp3")
                return AudioType.MPEG;
            if (extensoin == ".wav")
                return AudioType.WAV;
            if (extensoin == ".aiff" || extensoin == ".aif")
                return AudioType.AIFF;
            return AudioType.UNKNOWN;
        }

        protected IEnumerator GetAudioClip(string path)
        {
            var extension = Path.GetExtension(path);
            var name = Path.GetFileNameWithoutExtension(path);
            AudioType audioType = GetAudioType(extension);
            if (audioType == AudioType.UNKNOWN)
            {
                Debug.LogError($"Unsupported format {extension}");
                yield break;
            }
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(uwr.error);
                    yield break;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                clip.name = name;
                audioClips.Add(clip);
            }
        }

        public void SetWhenLoaded(AudioSource audioSource, AudioId audio)
        {
            if (audioSource == null) return;

        }

        public void LoadAudioFile(string pathToGltf, List<BVA_audio_audioClipExtension> audios)
        {
            foreach (var v in audios)
                AddUrl(Path.Combine(pathToGltf, v.audio.uri));
        }

        public bool Has(AudioClip obj)
        {
            return audioClips.Contains(obj);
        }

        public AudioClip Get(int index)
        {
            if (audioClips == null) return null;
            if (audioClips.Count <= index) return null;
            return audioClips[index];
        }

        public bool Remove(AudioClip obj)
        {
            return audioClips.Remove(obj);
        }

        public void RemoveAt(int index)
        {
            audioClips.RemoveAt(index);
        }

        public void RemoveInvalidOrNull()
        {
            audioClips.RemoveAll(x => x == null);
        }
    }
}
