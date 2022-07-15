using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using BVA;

namespace BVA
{
    [System.Serializable]
    public abstract class UrlAsset<T>
    {
        public string name;
        public string url;
        public string mimeType;
        public string[] alternate;
        public T resource { protected set; get; }
        public System.Action<T> onLoaded;
        public virtual JObject Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(name), name);
            jo.Add(nameof(url), url);
            jo.Add(nameof(mimeType), mimeType);
            JArray array = new JArray();
            if (alternate != null)
            {
                foreach (var v in alternate)
                    array.Add(v);
            }
            jo.Add(nameof(alternate), array);
            return jo;
        }
        public virtual IEnumerator Load() { yield return null; }
    }

    [System.Serializable]
    public class ImageUrlAsset : UrlAsset<Texture2D>
    {
        public override IEnumerator Load()
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else if (www.result == UnityWebRequest.Result.Success)
            {
                resource = DownloadHandlerTexture.GetContent(www);
                onLoaded(resource);
            }
        }

    }

    [System.Serializable]
    public class AudioUrlAsset : UrlAsset<AudioClip>
    {
        public override IEnumerator Load()
        {
            AudioType type = AudioType.MPEG;
            if (url.EndsWith(".ogg") || url.EndsWith(".OGG"))
                type = AudioType.OGGVORBIS;
            if (url.EndsWith(".wav") || url.EndsWith(".WAV"))
                type = AudioType.WAV;
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, type);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else if (www.result == UnityWebRequest.Result.Success)
            {
                resource = DownloadHandlerAudioClip.GetContent(www);
                onLoaded(resource);
            }
        }
    }

    [System.Serializable]
    public class VideoUrlAsset : UrlAsset<VideoClip>
    {
    }

}
